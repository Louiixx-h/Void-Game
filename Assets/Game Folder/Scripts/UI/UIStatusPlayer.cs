using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusPlayer : MonoBehaviour
{
    [SerializeField] Image imageLife;

    float maxLife = 0;
    float currentLife = 0;

    public void SetMaxLife(float value)
    {
        maxLife = value;
        TakeLife(maxLife);
    }

    public void TakeLife(float value)
    {
        currentLife += value;
        imageLife.fillAmount = currentLife / maxLife;
    }

    public void TakeDamage(float value)
    {
        currentLife -= value;
        imageLife.fillAmount = currentLife / maxLife;
    }
}
