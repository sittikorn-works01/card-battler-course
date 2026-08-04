using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : BasePanel
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI resultText;

    private void OnEnable()
    {
        restartButton.onClick.AddListener(RestartButton);
        nextButton.onClick.AddListener(RandomDropCards);

        PlayerEvents.OnPlayerDeath += OnPlayerDeath;
        BossEvents.OnBossDeath += OnBossDeath;
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();

        PlayerEvents.OnPlayerDeath -= OnPlayerDeath;
        BossEvents.OnBossDeath -= OnBossDeath;
    } 

    private void OnPlayerDeath()
    {
        resultText.text = "You lose";
        Open();
    }

    private void OnBossDeath()
    {
        resultText.text = "You defeated the boss!";
        Open();
    }


    private void RandomDropCards()
    {
        nextButton.gameObject.SetActive(false);
        RewardManager.Instance.SpawnRewardCards();
    }

    public void OnSelectRewardCard()
    {
        restartButton.gameObject.SetActive(true);
    }

    private void RestartButton()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
