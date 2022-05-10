using LuisHenriqueLab.GameMecanics.Gameplay;
using System.Collections;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] DoDamage doDamage;

    int currentAttack = 0;
    float damageDefault = 12;
    float resetAttackTime = 1;
    bool isAttacking = false;
    bool canAttack = true;
    Coroutine coroutine;

    public int CurrentAttack
    {
        get { return currentAttack; }
    }

    public bool IsAttacking
    {
        get => isAttacking;
        set => isAttacking = value;
    }

    public float DamageDefault
    {
        get => damageDefault;
        set => damageDefault = value;
    }

    public bool CanAttack
    {
        get => canAttack
            && !PlayerController.Instance.IsFall
            && !PlayerController.Instance.IsJump;
        set => canAttack = value;
    }

    public void Fire()
    {
        if (CanAttack)
        {
            canAttack = false;
            isAttacking = true;
            switch (currentAttack)
            {
                case 0:
                    PlayerController.Instance.PlaySound("attack_1");
                    break;
                case 1:
                    PlayerController.Instance.PlaySound("attack_2");
                    break;
            }

            currentAttack++;
            
            if(currentAttack == 1)
            {
                coroutine = StartCoroutine(ResetAttack(resetAttackTime));
            }
        }
    }

    public void DoDamage()
    {
        doDamage.Attack(DamageDefault);
    }

    IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds(time);
        currentAttack = 0;
        yield return null;
    }

    public void EndAttack()
    {
        if (currentAttack == 2)
        {
            StopCoroutine(coroutine);
            currentAttack = 0;
        }
        isAttacking = false;
        canAttack = true;
        PlayerController.Instance.ChangeState(PlayerState.RUN);
    }
}
