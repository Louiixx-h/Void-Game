using UnityEngine;

public class PatrolEnemyController : EnemyController, IEnemy
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteCharacter;
    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] DoDamage doDamage;

    GameObject target;
    EnemyState currentState;
    float moveSpeed = 2;
    float damageDefault = 24;
    bool isAttacking = false;

    private void Awake()
    {
        enemy = this;
    }

    public void InitState()
    {
        ChangeState(EnemyState.START);
    }

    public void PatrolState()
    {
        animator.Play("patrol");
        currentState = EnemyState.START;
    }

    void FollowState()
    {
        if (currentState != EnemyState.FOLLOW) return;
        var currentPosition = transform.position;
        var targetPosition = target.transform.position;

        if (Vector2.Distance(currentPosition, targetPosition) <= 2)
        {
            ChangeState(EnemyState.ATTACK);
            return;
        }

        animator.Play("follow");
        var direction = (currentPosition - targetPosition).normalized * -1;
        
        rigidbody.MovePosition(
            rigidbody.transform.position + 
            direction * 
            moveSpeed * 
            Time.deltaTime
        );

        if (direction.x > 0) transform.localScale = new Vector3(1, 1, 1);
        if (direction.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    void AttackState()
    {
        if (isAttacking) return;
        var currentPosition = transform.position;
        var targetPosition = target.transform.position;

        if (Vector2.Distance(currentPosition, targetPosition) > 2)
        {
            ChangeState(EnemyState.FOLLOW);
            return;
        }

        animator.Play("attack_1");
        currentState = EnemyState.ATTACK;
        isAttacking = true;
    }

    void HittedState()
    {
        animator.Play("hitted");
        currentState = EnemyState.HITTED;
    }

    void DeathState()
    {
        animator.Play("death");
        currentState = EnemyState.DEATH;
        UIManager.Instance.uIPoints.SetPoint(120);
    }

    private void FixedUpdate()
    {
        FollowState();
    }

    public void ChangeState(EnemyState state)
    {
        switch(state)
        {
            case EnemyState.FOLLOW:
                currentState = EnemyState.FOLLOW;
                break;
            case EnemyState.START:
                PatrolState();
                break;
            case EnemyState.ATTACK:
                AttackState();
                break;
            case EnemyState.HITTED:
                HittedState();
                break;
            case EnemyState.DEATH:
                DeathState();
                break;
        }
    }

    public void Fire()
    {
        doDamage.Attack(damageDefault, DataLayers.PLAYER);
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    public void EndHitted()
    {
        if (currentState == EnemyState.START) return;
        ChangeState(EnemyState.FOLLOW);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState == EnemyState.DEATH) return;
        if (collision.gameObject.layer == DataLayers.PLAYER)
        {
            target = collision.gameObject;
            currentState = EnemyState.FOLLOW;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentState == EnemyState.DEATH) return;
        if (collision.gameObject.layer == DataLayers.PLAYER)
        {
            ChangeState(EnemyState.START);
        }
    }
}
