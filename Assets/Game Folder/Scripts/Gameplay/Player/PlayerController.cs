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

        float maxLife = 100f;
        float currentLife = 0;
        Vector2 direction = Vector2.zero;

        PlayerState currentState;

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
            UIManager.Instance.uIStatusPlayer.SetMaxLife(maxLife);
            playerInput.onActionTriggered += OnAction;
        }

        public void TakeDamage(float value)
        {
            currentLife -= value;
            UIManager.Instance.uIStatusPlayer.TakeDamage(currentLife);
            if (currentLife <= 0) ChangeState(PlayerState.DEATH);
            else
            {
                print("hitted");
                HittedState();
            }
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
                    ChangeState(PlayerState.ATTACK_1);
                    break;
            }
        }

        public void ChangeState(PlayerState state)
        {
            if (state == PlayerState.DEATH) return;
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
                case PlayerState.ATTACK_1:
                    AttackState(state);
                    break;
                case PlayerState.ATTACK_2:
                    AttackState(state);
                    break;
                case PlayerState.ATTACK_3:
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

            if (isGrounded && !isAttacking)
            {
                currentState = PlayerState.RUN;
                movementController.Run(direction);
                animationController.ChangeStateAnimation(AnimationState.RUN);
            }
        }

        public void JumpState()
        {
            if (!IsGrounded || IsAttack) return;
            movementController.Jump();
            currentState = PlayerState.RUN;
            animationController.ChangeStateAnimation(AnimationState.JUMP);
        }

        public void FallState()
        {
            currentState = PlayerState.FALL;
            animationController.ChangeStateAnimation(AnimationState.FALL);
        }

        public void AttackState(PlayerState state)
        {
            if (!IsGrounded || IsJump) return;
            switch (state) {
                case PlayerState.ATTACK_1:
                    attackController.Fire();
                    currentState = PlayerState.ATTACK_1;
                    animationController.ChangeStateAnimation(AnimationState.ATTACK_1);
                    break;
                case PlayerState.ATTACK_2:
                    attackController.Fire();
                    currentState = PlayerState.ATTACK_2;
                    animationController.ChangeStateAnimation(AnimationState.ATTACK_2);
                    break;
            }
        }

        public void HittedState()
        {
            animationController.ChangeStateAnimation(AnimationState.HITTED);
             
        }

        public void DeathState()
        {
            animationController.ChangeStateAnimation(AnimationState.DEATH);
            UIManager.Instance.uIGameOver.Show();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == DataLayers.LIMBO)
            {
                TakeDamage(maxLife);
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
        ATTACK_1,
        ATTACK_2,
        ATTACK_3,
        DEATH,
    }
}