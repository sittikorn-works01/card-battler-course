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
        PlayerEvents.OnPlayerDeath += OnPlayerDeath;
        BossEvents.OnBossDeath += OnBossDeath;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerDeath -= OnPlayerDeath;
        BossEvents.OnBossDeath -= OnBossDeath;
    }

    public override void Open()
    {
        base.Open();

        restartButton.onClick.AddListener(RestartButton);
        nextButton.onClick.AddListener(RandomDropCards);

        resultText.gameObject.SetActive(true);

        nextButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
    }

    public override void Close()
    {
        base.Close();

        restartButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();
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
        resultText.gameObject.SetActive(false);

        nextButton.gameObject.SetActive(false);        
        RewardManager.Instance.SpawnRewardCards();
    }

    public void OnSelectRewardCard()
    {
        restartButton.gameObject.SetActive(true);
        RewardManager.Instance.HideDropCards();
    }

    private void RestartButton()
    {
        Close();
        GameManager.Instance.EnterState(GameState.Map);
    }
}
