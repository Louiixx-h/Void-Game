using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LuisHenriqueLab.GameMecanics.Gameplay
{
    public class PlayerController : GameManager<PlayerController>, IDamageable
    {
        [SerializeField] PlayerInput playerInput;
        [SerializeField] AttackController attackController;
        [SerializeField] MovementController movementController;
        [SerializeField] AnimationController animationController;
        [SerializeField] AudioSource audioSource;

        float maxLife = 100f;
        float maxEnergy = 100f;
        float currentLife = 0;
        float currentEnergy = 0;
        Vector2 direction = Vector2.zero;
        PlayerState currentState;

        public bool isHitted = false;
        public bool isDeath = false;
        public AudioClipData audioClipData;
        
        public bool IsJump
        {
            get => currentState == PlayerState.JUMP;
        }

        public bool IsFall
        {
            get => currentState == PlayerState.FALL;
        }

        public bool IsDeath
        {
            get => currentState == PlayerState.DEATH;
        }

        public bool IsAttack
        {
            get => attackController.IsAttacking;
        }

        public bool IsGrounded
        {
            get => movementController.IsGrounded;
        }

        void Start()
        {
            currentLife = maxLife;
            currentEnergy = maxEnergy;
            playerInput.onActionTriggered += OnAction;
            UIManager.Instance.uIStatusPlayer.SetMaxLife(maxLife);
            UIManager.Instance.uIStatusPlayer.SetMaxEnergy(maxEnergy);
        }

        void IDamageable.TakeDamage(float value)
        {
            if (isDeath) return;
            currentLife -= value;
            UIManager.Instance.uIStatusPlayer.TakeDamage(value);
            
            if (currentLife <= 0) ChangeState(PlayerState.DEATH);
            else HittedState();
        }

        void OnAction(InputAction.CallbackContext context)
        {
            switch(context.action.name)
            {
                case "Move":
                    direction = context.ReadValue<Vector2>();
                    ChangeState(PlayerState.RUN);
                    break;
                case "Jump":
                    ChangeState(PlayerState.JUMP);
                    break;
                case "Fire":
                    ChangeState(PlayerState.ATTACK);
                    break;
            }
        }

        public void ChangeState(PlayerState state)
        {
            if (currentState == PlayerState.DEATH) return;
            switch (state)
            {
                case PlayerState.IDLE:
                    IdleState();
                    break;
                case PlayerState.RUN:
                    RunState(direction);
                    break;
                case PlayerState.ROLL:
                    break;
                case PlayerState.JUMP:
                    JumpState();
                    break;
                case PlayerState.FALL:
                    FallState();
                    break;
                case PlayerState.ATTACK:
                    AttackState(state);
                    break;
                case PlayerState.DEATH:
                    DeathState();
                    break;
            }
        }

        public void IdleState() 
        {
            bool isGrounded = movementController.IsGrounded;
            bool isAttacking = attackController.IsAttacking;

            if (isDeath || isHitted) return;

            if (isGrounded && isAttacking && IsFall)
            {
                attackController.EndAttack();
                isAttacking = false;
            }

            if (isGrounded && !isAttacking)
            {
                animationController.ChangeStateAnimation(AnimationState.IDLE);
                currentState = PlayerState.IDLE;
            }
        }

        public void RunState(Vector2 direction)
        {
            bool isGrounded = movementController.IsGrounded;
            bool isAttacking = attackController.IsAttacking;

            if (isDeath || isHitted) return;

            if (isGrounded && !isAttacking)
            {
                currentState = PlayerState.RUN;
                movementController.Run(direction);
                animationController.ChangeStateAnimation(AnimationState.RUN);
            }
        }

        public void JumpState()
        {
            if (!IsGrounded || IsAttack || isDeath || isHitted) return;
            movementController.Jump();
            currentState = PlayerState.RUN;
            animationController.ChangeStateAnimation(AnimationState.JUMP);
        }

        public void FallState()
        {
            if (isDeath || isHitted) return;
            currentState = PlayerState.FALL;
            animationController.ChangeStateAnimation(AnimationState.FALL);
        }

        public void AttackState(PlayerState state)
        {
            if (!IsGrounded || IsJump || isDeath || isHitted || IsAttack) return;
            currentState = PlayerState.ATTACK;
            switch (attackController.CurrentAttack) {
                case 0:
                    UIManager.Instance.uIStatusPlayer.SpendEnergy(30);
                    animationController.ChangeStateAnimation(AnimationState.ATTACK_1);
                    break;
                case 1:
                    UIManager.Instance.uIStatusPlayer.SpendEnergy(40);
                    animationController.ChangeStateAnimation(AnimationState.ATTACK_2);
                    break;
            }
            attackController.Fire();
        }

        public void HittedState()
        {
            PlaySound("damage");
            isHitted = true;
            attackController.IsAttacking = false;
            animationController.ChangeStateAnimation(AnimationState.HITTED);
        }

        public void EndHittedState()
        {
            isHitted = false;
            ChangeState(PlayerState.RUN);
        }

        public void DeathState()
        {
            if (isDeath) return;
            
            isDeath = true;
            animationController.ChangeStateAnimation(AnimationState.DEATH);
            UIManager.Instance.uIGameOver.Show();
        }

        public void EndDeathState()
        {
            isDeath = false;
            ChangeState(PlayerState.IDLE);
            UIManager.Instance.uIGameOver.Hide();
        }

        public void PlaySound(string nameEffect)
        {
            SoundManager.Instance.PlaySoundEffect(nameEffect, audioClipData, audioSource);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == DataLayers.LIMBO)
            {
                ((IDamageable)this).TakeDamage(maxLife);
                animationController.ChangeStateAnimation(AnimationState.DEATH);
            }
        }
    }

    public enum PlayerState
    {
        IDLE,
        RUN,
        ROLL,
        JUMP,
        FALL,
        ATTACK,
        DEATH,
    }
}