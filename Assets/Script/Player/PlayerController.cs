using UnityEngine;

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

    void FixedUpdate()
    {
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
        _rb2D.linearVelocity = moveInput * moveSpeed;   // dùng velocity, không phải linearVelocity
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
}
