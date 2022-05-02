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

    private void Start()
    {
        enemy.InitState();
        currentLife = maxLife;
        spriteLife.fillAmount = currentLife / maxLife;
    }

    public void TakeDamage(float value)
    {
        currentLife -= value;
        spriteLife.fillAmount -= value / maxLife;
        if (currentLife <= 0)
            enemy.ChangeState(EnemyState.DEATH);
        else
            enemy.ChangeState(EnemyState.HITTED);
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