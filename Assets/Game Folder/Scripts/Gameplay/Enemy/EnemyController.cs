using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] protected Image spriteLife;
    protected IEnemy enemy;

    float maxLife = 32;
    float currentLife = 0;
    protected bool isDeath = false;

    private void Start()
    {
        enemy.InitState();
        currentLife = maxLife;
        spriteLife.fillAmount = currentLife / maxLife;
    }

    void IDamageable.TakeDamage(float value)
    {
        if (isDeath) return;
        currentLife -= value;
        spriteLife.fillAmount -= value / maxLife;
        if (currentLife <= 0)
        {
            isDeath = true;
            enemy.ChangeState(EnemyState.DEATH);
        }
        else enemy.ChangeState(EnemyState.HITTED);
    }

    public void InitState()
    {
        enemy.ChangeState(EnemyState.START);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}