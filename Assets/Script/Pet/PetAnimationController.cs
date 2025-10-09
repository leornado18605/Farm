using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class PetAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private IAnimationState currentState;

    private Dictionary<PetAnimationState, IAnimationState> states;

    void Awake()
    {
        states = new Dictionary<PetAnimationState, IAnimationState>
        {
            { PetAnimationState.Idle, new PetIdleState() },
            { PetAnimationState.WalkUp, new PetWalkUpState() },
            { PetAnimationState.WalkDown, new PetWalkDownState() },
            { PetAnimationState.WalkLeft, new PetWalkLeftState() },
            { PetAnimationState.WalkRight, new PetWalkRightState() },
            { PetAnimationState.Sit, new PetSitState() },
            { PetAnimationState.Lay, new PetLayState() },
            { PetAnimationState.Sleep, new PetSleepState() },
            { PetAnimationState.Bath, new PetBathState() }
        };

        ChangeState(PetAnimationState.Idle);
    }

    public void ChangeState(PetAnimationState newState)
    {
        if (!states.ContainsKey(newState)) return;
        if (currentState == states[newState]) return;

        currentState?.Exit(animator);
        currentState = states[newState];
        currentState.Enter(animator);
    }
}