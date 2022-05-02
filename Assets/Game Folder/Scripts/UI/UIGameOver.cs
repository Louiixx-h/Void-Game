using TMPro;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextGameOver;

    float m_InitialTextSize = 1.3f;
    float m_TweenTime = 2f;

    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void ShowAnimation()
    {
        LeanTween.cancel(m_TextGameOver.gameObject);
        m_TextGameOver.gameObject.transform.localScale = Vector2.one /  m_InitialTextSize;
        LeanTween.scale(m_TextGameOver.gameObject, Vector2.one, m_TweenTime);
    }

    private void OnEnable()
    {
        ShowAnimation();
    }
}
