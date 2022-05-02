using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [HideInInspector] public string currentState;
    
    public void ChangeStateAnimation(string state)
    {
        if (state == currentState) return;
        animator.Play(state);
    }
}

struct AnimationState
{
    public const string IDLE = "idle";
    public const string RUN = "run";
    public const string JUMP = "jump";
    public const string FALL = "fall";
    public const string ATTACK_1 = "attack_1";
    public const string ATTACK_2 = "attack_2";
    public const string DEATH = "death";
    public const string HITTED = "hitted";
}