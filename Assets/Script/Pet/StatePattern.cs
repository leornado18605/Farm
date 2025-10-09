using UnityEngine;

public class PetIdleState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("Idle", true);
    public void Exit(Animator animator) => animator.SetBool("Idle", false);
}

// ==== WALK ====
public class PetWalkUpState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("WalkUp", true);
    public void Exit(Animator animator) => animator.SetBool("WalkUp", false);
}

public class PetWalkDownState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("WalkDown", true);
    public void Exit(Animator animator) => animator.SetBool("WalkDown", false);
}

public class PetWalkLeftState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("WalkLeft", true);
    public void Exit(Animator animator) => animator.SetBool("WalkLeft", false);
}

public class PetWalkRightState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("WalkRight", true);
    public void Exit(Animator animator) => animator.SetBool("WalkRight", false);
}

// ==== RELAX ====
public class PetSitState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("Sit", true);
    public void Exit(Animator animator) => animator.SetBool("Sit", false);
}

public class PetLayState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("Lay", true);
    public void Exit(Animator animator) => animator.SetBool("Lay", false);
}

public class PetSleepState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("Sleep", true);
    public void Exit(Animator animator) => animator.SetBool("Sleep", false);
}

// ==== BATH ====
public class PetBathState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetTrigger("Bath");
    public void Exit(Animator animator) {}
}