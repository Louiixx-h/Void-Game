using LuisHenriqueLab.GameMecanics.Gameplay;
using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] SpriteRenderer spritePlayer;
    [SerializeField] GameObject hitBox;

    float runvelocity = 8f;
    float jumpHeight = 300f;
    bool isGrounded = true;
    Vector2 movePlayer = Vector2.zero;

    public bool IsGrounded
    {
        get => isGrounded;
    }

    private void FixedUpdate()
    {
        CheckGround();
        GravityAndMovement();
    }

    void GravityAndMovement()
    {
        if (PlayerController.Instance.IsAttack)
        {
            movePlayer = Vector2.zero;
            rigidBody.velocity = Vector2.zero;
            return;
        }
        movePlayer.y = rigidBody.velocity.y;
        rigidBody.velocity = movePlayer;

        if(rigidBody.velocity.x != 0) PlayerController.Instance.ChangeState(PlayerState.RUN);
        else PlayerController.Instance.ChangeState(PlayerState.IDLE);
    }

    public void Run(Vector2 direction)
    {
        movePlayer = direction * runvelocity;
        movePlayer.y = 0;
        Flip(movePlayer.x);
    }

    public void Jump()
    {
        rigidBody.AddForce(new Vector2(0, jumpHeight));
    }

    void Flip(float value)
    {
        if (value > 0)
        {
            hitBox.transform.localScale = new Vector3(1, 1, 1);
            spritePlayer.flipX = false;
        }
        if (value < 0)
        {
            hitBox.transform.localScale = new Vector3(-1, 1, 1);
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
            isGrounded = false;
            DrawLine(point, Vector2.down, Color.yellow);
            PlayerController.Instance.ChangeState(PlayerState.FALL);
        }
        else
        {
            isGrounded = true;
            DrawLine(point, Vector2.down, Color.red);
        }
    }

    void DrawLine(Vector2 start, Vector2 dir, Color color)
    {
        Debug.DrawRay(start, dir, color);
    }
}
