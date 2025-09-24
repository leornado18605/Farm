using UnityEngine;

public class IdleDownState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("IdleDown", true);
    public void Exit(Animator animator) => animator.SetBool("IdleDown", false);
}

public class IdleUpState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("IdleUp", true);
    public void Exit(Animator animator) => animator.SetBool("IdleUp", false);
}

public class IdleRightState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("IdleRight", true);
    public void Exit(Animator animator) => animator.SetBool("IdleRight", false);
}


public class WalkBehindState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("WalkBehind", true);
    public void Exit(Animator animator) => animator.SetBool("WalkBehind", false);
}

public class WalkLeftState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("WalkLeft", true);
    public void Exit(Animator animator) => animator.SetBool("WalkLeft", false);
}

public class WalkThroughState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("WalkThrough", true);
    public void Exit(Animator animator) => animator.SetBool("WalkThrough", false);
}

public class AttackState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("Attack", true);
    public void Exit(Animator animator) => animator.SetBool("Attack", false);
}

public class DeadState : IAnimationState
{
    public void Enter(Animator animator) => animator.SetBool("Dead", true);
    public void Exit(Animator animator) => animator.SetBool("Dead", false);
}
