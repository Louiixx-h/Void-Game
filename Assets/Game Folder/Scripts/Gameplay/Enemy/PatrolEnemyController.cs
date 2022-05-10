using UnityEngine;

public class PatrolEnemyController : EnemyController, IEnemy
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteCharacter;
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] DoDamage doDamage;
    [SerializeField] CheckPlayer checkPlayer;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClipData audioClipData;

    GameObject target;
    EnemyState currentState;
    float moveSpeed = 3;
    float damageDefault = 24;
    bool isAttacking = false;

    private void Awake()
    {
        enemy = this;
        checkPlayer.onEnter += OnEnter;
        checkPlayer.onExit += OnExit;
    }

    void IEnemy.InitState()
    {
        ((IEnemy)this).ChangeState(EnemyState.START);
    }

    public void PatrolState()
    {
        isAttacking = false;
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
            ((IEnemy)this).ChangeState(EnemyState.ATTACK);
            return;
        }

        animator.Play("follow");
        var direction = (currentPosition - targetPosition).normalized * -1;

        rigidBody.MovePosition(
            rigidBody.transform.position + 
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
            ((IEnemy)this).ChangeState(EnemyState.FOLLOW);
            return;
        }

        animator.Play("attack_1");
        currentState = EnemyState.ATTACK;
        isAttacking = true;
    }

    void HittedState()
    {
        isAttacking = false;
        animator.Play("hitted");
        currentState = EnemyState.HITTED;
    }

    void DeathState()
    {
        PlaySound("game_over");
        isAttacking = false;
        animator.Play("death");
        currentState = EnemyState.DEATH;
        UIManager.Instance.uIPoints.SetPoint(120);
    }

    private void FixedUpdate()
    {
        FollowState();
    }

    void IEnemy.ChangeState(EnemyState state)
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
        doDamage.Attack(damageDefault);
    }

    public void EndAttack()
    {
        if (currentState == EnemyState.START) return;
        isAttacking = false;
        var currentPosition = transform.position;
        var targetPosition = target.transform.position;
        if (Vector2.Distance(currentPosition, targetPosition) > 2)
            ((IEnemy)this).ChangeState(EnemyState.ATTACK);
        else
            ((IEnemy)this).ChangeState(EnemyState.FOLLOW);
    }

    public void EndHitted()
    {
        if (currentState == EnemyState.START) return;
        ((IEnemy)this).ChangeState(EnemyState.FOLLOW);
    }

    public void OnEnter(Collider2D collision)
    {
        if (currentState == EnemyState.DEATH) return;
        if (collision.gameObject.layer == DataLayers.PLAYER)
        {
            target = collision.gameObject;
            currentState = EnemyState.FOLLOW;
        }
    }

    public void OnExit(Collider2D collision)
    {
        if (currentState == EnemyState.DEATH) return;
        if (collision.gameObject.layer == DataLayers.PLAYER)
        {
            ((IEnemy)this).ChangeState(EnemyState.START);
        }
    }

    public void PlaySound(string nameEffect)
    {
        SoundManager.Instance.PlaySoundEffect(nameEffect, audioClipData, audioSource);
    }
}
