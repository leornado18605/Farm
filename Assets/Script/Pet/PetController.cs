using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PetController : MonoBehaviour
{
    [Header("Follow Settings")] [SerializeField]
    private Transform player;

    [SerializeField] private float followDistance = 1.5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopThreshold = 0.1f;

    [Header("Components")] [SerializeField]
    private Rigidbody2D rb;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;

    private Vector2 moveDir;
    private Vector2 lastMoveDir = Vector2.down;

    void FixedUpdate()
    {
        if (player == null) return;

        FollowPlayer();
        UpdateAnimator();
        UpdateSpriteFlip();
    }

    private void FollowPlayer()
    {
        Vector2 dir = (player.position - transform.position);
        float distance = dir.magnitude;

        if (distance > followDistance)
        {
            moveDir = dir.normalized;
            rb.linearVelocity = moveDir * moveSpeed;
            lastMoveDir = moveDir;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            moveDir = Vector2.zero;
        }
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("MoveX", lastMoveDir.x);
        animator.SetFloat("MoveY", lastMoveDir.y);
        animator.SetFloat("Speed", moveDir.magnitude);
    }

    private void UpdateSpriteFlip()
    {
        if (lastMoveDir.x < -0.1f)
            sprite.flipX = false;
        else if (lastMoveDir.x > 0.1f)
            sprite.flipX = true;

    }

}