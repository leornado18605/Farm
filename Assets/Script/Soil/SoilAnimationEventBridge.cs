using UnityEngine;

public class SoilAnimationEventBridge : MonoBehaviour
{
    public ToolController toolController;
    
    [SerializeField] private Transform bagSpawnPoint;
    public void OnHoeStage1() => toolController.OnHoeStage(1);
    public void OnHoeStage2() => toolController.OnHoeStage(2);
    public void OnHoeStage3() => toolController.OnHoeStage(3);
    
    public void OnWaterStage1() => toolController.OnWaterStage(1);
    public void OnWaterStage2() => toolController.OnWaterStage(2);


    public void OnSeedStage1()
    {
        toolController.OnSeedStage(1);
    }

    public void ShowBag()
    {
        toolController.ShowBag(true);

        if (SeedParticlePool.Instance != null)
        {
            SeedParticlePool.Instance.SpawnSeed(bagSpawnPoint.position, Random.Range(2, 4));
        }
    }

    public void HideBag() => toolController.ShowBag(false);

}