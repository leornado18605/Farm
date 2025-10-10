using UnityEngine;

/// <summary>
/// Quản lý animation gieo hạt (Seed) cho Player.
/// Hướng gieo được lấy từ PlayerController (_lastMoveDir).
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerSeedHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private ToolController toolController;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerController playerController;

    private bool isSeeding = false;

    // Animator parameters
    private static readonly int IsSeeding = Animator.StringToHash("IsSeeding");
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");

    /// <summary>
    /// Gọi khi bắt đầu gieo hạt — đất đã cuốc xong (Hoed).
    /// </summary>
    public void StartSeeding()
    {
        if (isSeeding) return;
        isSeeding = true;

        // Dừng chuyển động player
        rb.linearVelocity = Vector2.zero;

        // Lấy hướng hiện tại của player từ PlayerController
        Vector2 dir = playerController.LastDirection;

        // Cập nhật hướng anim để gieo đúng hướng
        animator.SetFloat(MoveX, dir.x);
        animator.SetFloat(MoveY, dir.y);

        animator.SetBool(IsSeeding, true);
        Debug.Log($"🎬 Start seeding with direction: {dir}");
    }

    /// <summary>
    /// Gọi từ Animation Event khi hạt được gieo xuống đất.
    /// </summary>
    public void OnPlantSeedEvent()
    {
        //toolController.OnPlantSeed();
    }

    /// <summary>
    /// Gọi từ Animation Event cuối clip (khi gieo hạt xong).
    /// </summary>
    public void EndSeeding()
    {
        if (!isSeeding) return;
        isSeeding = false;
        animator.SetBool(IsSeeding, false);
        Debug.Log("✅ Seeding finished, back to idle.");
    }
}