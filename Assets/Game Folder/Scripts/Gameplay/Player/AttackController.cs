using LuisHenriqueLab.GameMecanics.Gameplay;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] DoDamage doDamage;

    int currentAttack = 0;
    float damageDefault = 12;
    bool isAttacking = false;
    bool canAttack = true;

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
            doDamage.Attack(DamageDefault, DataLayers.ENEMY);
            DoAttack();
            currentAttack++;
        }
    }

    void DoAttack()
    {
        switch (currentAttack)
        {
            case 0:
                PlayerController.Instance.ChangeState(PlayerState.ATTACK_1);
                break;
            case 1:
                PlayerController.Instance.ChangeState(PlayerState.ATTACK_2);
                break;
        }
    }

    public void EndAttack()
    {
        if (currentAttack == 2)
        {
            currentAttack = 0;
        }
        isAttacking = false;
        canAttack = true;
        PlayerController.Instance.ChangeState(PlayerState.IDLE);
    }
}
