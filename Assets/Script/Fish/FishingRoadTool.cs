using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Tools/Fishing Rod Tool")]
public class FishingRodTool : Tool
{
    [Header("Fishing Settings")]
    [SerializeField] private float minWaitTime = 1.5f;
    [SerializeField] private float maxWaitTime = 4f;
    [SerializeField] private float catchChance = 0.6f; // tỉ lệ bắt được cá
    public override void Use(GroundTile tile, PlayerController player)
    {
        // Nếu ô đất có nước (ví dụ tag = "Water") thì mới câu được
        if (tile == null || !tile.CompareTag("Water"))
        {
            Debug.Log("❌ Không thể câu ở đây!");
            return;
        }

        // Lấy vị trí ô nước để cast cần
        Vector2 waterPos = tile.transform.position;
        TryFish(waterPos, player);
    }
    public void TryFish(Vector2 waterPos, PlayerController player)
    {
        // Giai đoạn 1: vung cần
        player.StartFishing();

        // Sau khi casting xong (AnimationEvent gọi OnCastingComplete)
        player.StartCoroutine(FishingRoutine(player));
    }

    private IEnumerator FishingRoutine(PlayerController player)
    {
        // Giai đoạn 2: chờ cá cắn
        yield return new WaitUntil(() => player.IsFishingReady); // Đợi casting xong
        player.SetFishingWaitIdle();

        float wait = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(wait);

        // Giai đoạn 3: cá cắn
        player.OnFishHooked();

        // Chờ 1 chút trước khi kéo
        yield return new WaitForSeconds(0.5f);

        // Giai đoạn 4: roll (kéo cá)
        bool success = Random.value < catchChance;
        player.OnReelFish(success);
    }
}