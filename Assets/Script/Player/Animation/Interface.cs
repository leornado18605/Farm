using UnityEngine;

public interface IAnimationState
{
    void Enter(Animator animator);
    void Exit(Animator animator);
}
public interface IHoeable
{
    void OnHoe();
}