using Devflowbr.GameMecanics.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Devflowbr.GameMecanics.Gameplay
{
    public class PlayerController : MonoBehaviour, IPlayerState
    {
        [SerializeField] private BoxCollider2D boxCollider2D;
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private SpriteRenderer spritePlayer;

        Vector2 movePlayer = Vector2.zero;

        bool isGrounded = true;
        float circleRadius = 0.1f;

        float maxLife = 100f;
        float currentLife = 100f;

        float damageDefault = 12;

        float runvelocity   = 8f;
        float jumpHeight    = 300f;

        int isRunHash        = 0;
        int isJumpHash       = 0;
        int isJumpToFallHash = 0;
        int isDeadHash       = 0;
        int isFallHash       = 0;
        int isIdleHash       = 0;


        void Start()
        {
            isRunHash = Animator.StringToHash("isRun");
            isJumpHash = Animator.StringToHash("isJump");
            isFallHash = Animator.StringToHash("isFall");
            isIdleHash = Animator.StringToHash("isIdle");
            playerInput.onActionTriggered += OnAction;
        }

        private void FixedUpdate()
        {
            movePlayer.y = rigidbody.velocity.y;
            rigidbody.velocity = movePlayer;
            
            CheckGround();
        }

        void OnAction(InputAction.CallbackContext context)
        {
            switch(context.action.name)
            {
                case "Move":
                    var direction = context.ReadValue<Vector2>();
                    Run(direction);
                    break;
                case "Jump":
                    Jump();
                    break;
            }
        }

        public void Dead()
        {
        }

        public void Idle()
        {
        }

        public void Jump()
        {
            if (!isGrounded) return;
            JumpAnim();
            rigidbody.AddForce(new Vector2(0, jumpHeight));
        }

        public void Run(Vector2 direction)
        {
            movePlayer = direction * runvelocity;
            Flip(movePlayer.x);

            if (direction.magnitude > 0.1) {
                RunAnim(true);
                return;
            }
            movePlayer.x = 0;
            RunAnim(false);
        }

        public void DeadAnim(bool isActive)
        {
            animator.SetBool(isDeadHash, isActive);
        }

        public void JumpAnim()
        {
            animator.SetBool(isJumpHash, true);
        }

        public void RunAnim(bool isActive)
        {
            animator.SetBool(isRunHash, isActive);
        }

        public void IdleAnim(bool isActive)
        {
            animator.SetBool(isIdleHash, isActive);
        }

        public void FallAnim(bool isActive)
        {
            animator.SetBool(isFallHash, isActive);
        }

        public void JumpToFallAnim()
        {
            animator.SetBool(isJumpHash, false);
            animator.SetBool(isFallHash, true);
        }

        public void FallToIdleAnim()
        {
            if (!isGrounded) return;
            animator.SetBool(isFallHash, false);
            animator.SetBool(isIdleHash, true);
        }

        void Flip(float value) {
            if (value > 0)
            {
                spritePlayer.flipX = false;
                return;
            }
            if (value < 0)
            {
                spritePlayer.flipX = true;
            }
        }

        void CheckGround()
        {
            Vector2 point = boxCollider2D.bounds.center;
            Vector2 size = boxCollider2D.bounds.size;
            float distance = 0.5f;

            RaycastHit2D raycastHit2D = Physics2D.BoxCast(
                point,
                size,
                0f,
                Vector2.down,
                distance,
                DataLayers.GROUND
            );

            if (raycastHit2D.collider == null)
            {
                DrawLine(point, Vector2.down, Color.yellow);
                isGrounded = false;
            }
            else
            {
                DrawLine(point, Vector2.down, Color.red);
                isGrounded = true;
            }
        }

        void DrawLine(Vector2 start, Vector2 dir, Color color)
        {
            Debug.DrawRay(start, dir, color);
        }
    }
}