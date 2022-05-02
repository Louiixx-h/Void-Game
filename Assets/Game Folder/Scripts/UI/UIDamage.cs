using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDamage : MonoBehaviour
{
    [SerializeField] GameObject damage;
    TextMeshProUGUI damageText;

    float m_InitialTextSize = 1.3f;
    float m_TweenTime = 0.7f;
    float m_LifeTime = 1f;

    public void SetDamage(float value, Vector3 position)
    {
        var damageInstantied = Instantiate(damage, position, Quaternion.identity);
        damageInstantied.transform.SetParent(gameObject.transform);
        damageText = damageInstantied.GetComponent<TextMeshProUGUI>();
        damageText.text = value.ToString();
        StartAnimation(damageInstantied);
    }

    void StartAnimation(GameObject damageInstantied)
    {
        damageText.gameObject.transform.localScale = Vector2.one * m_InitialTextSize;
        var targetPosition = new Vector3(
            damageText.transform.position.x, 
            damageText.transform.position.y + 5,
            damageText.transform.position.z
        );


        LeanTween.scale(damageText.gameObject, Vector2.one, m_TweenTime);
        LeanTween.move(damageText.gameObject, targetPosition, 1);

        Destroy(damageInstantied, m_LifeTime);
    }
}
