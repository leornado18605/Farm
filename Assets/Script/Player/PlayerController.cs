using System;
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
    private Tween moveTween;
    [SerializeField] private GameObject seedBag;

    private bool isMovingToTile = false;
    private Vector3 targetPosition;
    
    private Crop currentHarvestTarget;
    
    private ItemPickup currentPickupTarget;
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

    public void StartWatering()
    {
        _animator.SetBool("IsWatering", true);
        Debug.Log("Start Watering");
    }

    public void StopWatering()
    {
        _animator.SetBool("IsWatering", false);
        Debug.Log("Stop Watering");
    }
    #region Fishing System

    public bool IsFishingReady { get; private set; } = false;

    public void StartFishing()
    {
        _animator.SetTrigger("Cast");
        IsFishingReady = false;
        Debug.Log("🎣 Casting...");
    }

// Gọi từ AnimationEvent cuối clip “Casting”
    public void OnCastingComplete()
    {
        IsFishingReady = true;
        Debug.Log("✅ Casting done, waiting for fish...");
    }

    public void SetFishingWaitIdle()
    {
        _animator.SetTrigger("WaitIdle");
        Debug.Log("⏳ Waiting for fish...");
    }

    public void OnFishHooked()
    {
        _animator.SetTrigger("Hooked");
        Debug.Log("🐟 Fish is biting!");
    }

    public void OnReelFish(bool success)
    {
        _animator.SetTrigger("Roll");

        if (success)
        {
            _animator.SetTrigger("CapturedFish");
            Debug.Log("✅ Caught a fish!");
        }
        else
        {
            _animator.SetTrigger("CapturedNothing");
            Debug.Log("❌ No fish caught...");
        }

        // Quay lại idle sau 2s
        Invoke(nameof(StopFishing), 20f);
    }

    public void StopFishing()
    {
        _animator.ResetTrigger("Cast");
        _animator.ResetTrigger("WaitIdle");
        _animator.ResetTrigger("Hooked");
        _animator.ResetTrigger("Roll");
        _animator.ResetTrigger("CapturedFish");
        _animator.ResetTrigger("CapturedNothing");
        IsFishingReady = false;
        Debug.Log("🎣 Fishing ended");
    }

    #endregion


    public void StartHarvestAnim(Crop target)
    {
        currentHarvestTarget = target;
        _animator.SetTrigger("Harvest");
    }
    
    public void OnHarvestEnd()
    {
        _animator.ResetTrigger("Harvest");
        currentHarvestTarget = null;
    }

    public void OnHarvestHit()
    {
        if (currentHarvestTarget != null)
        {
            currentHarvestTarget.StartHarvestAnim(this); 
        }
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
    
    public void MoveToAndAct(Vector2 targetPos, System.Action onArrive)
    {
        if (IsBusy()) return;

        if (moveTween != null && moveTween.IsActive())
            moveTween.Kill();

        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;

        FaceDirection(dir);

        float dist = Vector2.Distance(transform.position, targetPos);
        float travelTime = dist / moveSpeed;

        _animator.SetFloat("Speed", 1f);

        moveTween = transform.DOMove(targetPos, travelTime)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                FaceDirection(dir);
                _animator.SetFloat("Speed", 1f);
            })
            .OnComplete(() =>
            {
                _animator.SetFloat("Speed", 0f);
                onArrive?.Invoke();
            });
    }


    private bool IsBusy()
    {
        return _animator.GetBool("IsHoeing") || 
               _animator.GetBool("IsSeeding") || 
               _animator.GetBool("IsWatering");
    }
    
    public void StartPickupAnim(ItemPickup item)
    {
        currentPickupTarget = item;
        _animator.SetTrigger("Pickup"); // ⚙️ trigger anim "Pickup"
    }

    public void OnPickupAnimEnd()
    {
        if (currentPickupTarget != null)
        {
            currentPickupTarget.OnPickedByPlayer();
            currentPickupTarget = null;
        }
    }
}
