using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuStart : MonoBehaviour
{
    [SerializeField] Button startGame;
    [SerializeField] Button exitGame;

    private void Start()
    {
        startGame.onClick.AddListener(() =>
        {
            OpenStartGame();
        });
        exitGame.onClick.AddListener(() =>
        {
            ExitGame();
        });
    }

    void OpenStartGame()
    {
        SceneManager.LoadScene("Fase_1");
    }

    void ExitGame()
    {
        Application.Quit();
    }
}
