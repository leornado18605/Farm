using UnityEngine;

public class SoilAnimationEventBridge : MonoBehaviour
{
    public ToolController toolController;

    public void OnHoeStage1() => toolController.OnHoeStage(1);
    public void OnHoeStage2() => toolController.OnHoeStage(2);
    public void OnHoeStage3() => toolController.OnHoeStage(3);
}