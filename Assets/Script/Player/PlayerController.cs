using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [SerializeField] private Rigidbody2D _rb2D;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sprite;

    private Vector2 _lastMoveDir = Vector2.down;
    private bool isHoeing = false;
    
    [SerializeField] private GameObject seedBag;
    public Vector2 LastDirection => _lastMoveDir;

    void FixedUpdate()
    {
        if (isHoeing)
        {
            _rb2D.linearVelocity = Vector2.zero;
            return;
        }
        
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        Vector2 moveInput = ReadInput();
        moveInput = ClampToCardinal(moveInput);

        ApplyMovement(moveInput);
        UpdateLastDirection(moveInput);
        UpdateAnimator(moveInput);
        UpdateSpriteFlip();
    }

    private Vector2 ReadInput()
    {
        return new Vector2(JoystickControl.direct.x, JoystickControl.direct.y);
    }
    private void ApplyMovement(Vector2 moveInput)
    {
        _rb2D.linearVelocity = moveInput * moveSpeed;   
    }

    private Vector2 ClampToCardinal(Vector2 input)
    {
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            return new Vector2(Mathf.Sign(input.x), 0f);
        else if (Mathf.Abs(input.y) > 0f)
            return new Vector2(0f, Mathf.Sign(input.y));
        return Vector2.zero;
    }

    private void UpdateLastDirection(Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
            _lastMoveDir = moveInput;
    }

    private void UpdateAnimator(Vector2 moveInput)
    {
        _animator.SetFloat("MoveX", _lastMoveDir.x);
        _animator.SetFloat("MoveY", _lastMoveDir.y);
        _animator.SetFloat("Speed", moveInput.magnitude);
    }

    private void UpdateSpriteFlip()
    {
        if (_lastMoveDir.x < 0)
            _sprite.flipX = true;
        else if (_lastMoveDir.x > 0)
            _sprite.flipX = false;
    }
    public void StartHoeing()
    {
        if (isHoeing) return; 
        isHoeing = true;

        _rb2D.linearVelocity = Vector2.zero;

        _animator.SetFloat("Speed", 0f);
        _animator.SetBool("IsHoeing", true);

    }

    public void EndHoeing()
    {
        if (!isHoeing) return;
        isHoeing = false;

        _animator.SetBool("IsHoeing", false);
    }
    
    public void FaceDirection(Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            dir = new Vector2(Mathf.Sign(dir.x), 0);
        else
            dir = new Vector2(0, Mathf.Sign(dir.y));

        _animator.SetFloat("MoveX", dir.x);
        _animator.SetFloat("MoveY", dir.y);
        _lastMoveDir = dir;

        if (dir.x < 0)
            _sprite.flipX = true;
        else if (dir.x > 0)
            _sprite.flipX = false;
    }

    public void StartSeeding()
    {
        _animator.SetBool("IsSeeding", true);
        Debug.Log("StartSeeding");
    }

    public void StopSeeding()
    {
        _animator.SetBool("IsSeeding", false);
        Debug.Log("StopSeeding");
    }

    public void ShowSeedBag(bool show)
    {
        if (seedBag == null) return;

        if (show)
        {
            seedBag.SetActive(true);
            seedBag.transform.DOScale(1f, 0.15f).From(0.5f);
        }
        else
        {
            seedBag.transform.DOScale(0.5f, 0.1f)
                .OnComplete(() => seedBag.SetActive(false));
        }
    }

}
