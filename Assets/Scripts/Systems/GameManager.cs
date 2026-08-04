using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float transitionTime = 2f; 

    private bool isGameActive = true;

    private void OnEnable()
    {
        PlayerEvents.OnPlayerDeath += OnGameEnded;
        BossEvents.OnBossDeath += OnGameEnded;
    }
    private void OnDisable()
    {
        PlayerEvents.OnPlayerDeath -= OnGameEnded;
        BossEvents.OnBossDeath -= OnGameEnded;
    }

    private void OnGameEnded()
    {
        isGameActive = false;
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("Gameplay");
    }

    public bool IsGameActive() => isGameActive;


}
