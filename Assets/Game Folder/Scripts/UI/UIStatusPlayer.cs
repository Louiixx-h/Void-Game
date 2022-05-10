using UnityEngine;
using UnityEngine.UI;

public class UIStatusPlayer : MonoBehaviour
{
    [SerializeField] Image imageLife;
    [SerializeField] Image imageEnergy;

    float maxLife = 0;
    float currentLife = 0;

    float maxEnergy = 0;
    float currentEnergy = 0;

    private void FixedUpdate()
    {
        if (imageEnergy.fillAmount < 1)
        {
            imageEnergy.fillAmount += 0.2f * Time.deltaTime;
        }
    }

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
        currentLife = currentLife - value;
        imageLife.fillAmount = currentLife / maxLife;
    }

    public void SetMaxEnergy(float value)
    {
        maxEnergy = value;
        currentEnergy = maxEnergy;
        imageEnergy.fillAmount = currentEnergy / maxEnergy;
    }

    public void SpendEnergy(float value)
    {
        currentEnergy -= value;
        if (currentEnergy < 0) currentEnergy = 0;
        imageEnergy.fillAmount = currentEnergy / maxEnergy;
    }
}
