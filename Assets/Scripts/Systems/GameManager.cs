using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float transitionTime = 2f;
    [SerializeField] private TextMeshProUGUI resultText;

    private bool isGameActive = true;

    private void OnEnable()
    {
        PlayerEvents.OnPlayerDeath += PlayerEvents_OnPlayerDeath;
        BossEvents.OnBossDeath += BossEvents_OnBossDeath;
    }
    private void OnDisable()
    {
        PlayerEvents.OnPlayerDeath -= PlayerEvents_OnPlayerDeath;
        BossEvents.OnBossDeath -= BossEvents_OnBossDeath;
    }

    private void PlayerEvents_OnPlayerDeath()
    {
        isGameActive = false;
        resultText.text = "You lose";
        StartCoroutine(RestartGame());
    }

    private void BossEvents_OnBossDeath()
    {
        isGameActive = false;
        resultText.text = "You defeated the boss!";
        StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("Gameplay");
    }

    public bool IsGameActive() => isGameActive;


}
