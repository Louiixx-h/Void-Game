using UnityEngine;

public class DoDamage : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider;

    LayerMask target;
    float damage = 0;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Attack(float damage, LayerMask target)
    {
        this.target = target;
        this.damage = damage;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        IDamageable damageable = col.gameObject.GetComponent<IDamageable>();
        if (damageable == null || col.gameObject.layer != target) return;
        damageable.TakeDamage(damage);
        UIManager.Instance.uIDamage.SetDamage(damage, transform.position);
    }
}
