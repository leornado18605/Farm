using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private IAnimationState currentState;

    private Dictionary<AnimationState, IAnimationState> states;

    void Awake()
    {
        states = new Dictionary<AnimationState, IAnimationState>
        {
            { AnimationState.IdleDown, new IdleDownState() },
            { AnimationState.IdleUp, new IdleUpState() },
            { AnimationState.IdleRight, new IdleRightState() },

            { AnimationState.WalkBehind, new WalkBehindState() },
            { AnimationState.WalkLeft, new WalkLeftState() },
            { AnimationState.WalkThrough, new WalkThroughState() },

            { AnimationState.Attack, new AttackState() },

            { AnimationState.Dead, new DeadState() }
        };


        ChangeState(AnimationState.IdleDown);
    }

    public void ChangeState(AnimationState newState)
    {
        if (!states.ContainsKey(newState)) return;
        if (currentState == states[newState]) return;

        currentState?.Exit(animator);
        currentState = states[newState];
        currentState.Enter(animator);
    }
}
