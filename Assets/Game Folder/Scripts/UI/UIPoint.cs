using TMPro;
using UnityEngine;

public class UIPoint : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextPoint;
    
    float points = 0;

    private void Start()
    {
        SetPoint(points);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetPoint(float value)
    {
        points += value;
        m_TextPoint.text = points.ToString();
    }
}
