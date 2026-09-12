using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI pauseButtonText;

    private bool isPaused = false;

    private void OnEnable()
    {
        PlayerData.OnHealthChanged += UpdateHealthText;
        PlayerData.OnGoldChanged += UpdateGoldText;
        //pauseButton.onClick.AddListener(TogglePause);
    }

    private void Start()
    {
        UpdateHealthText((int)PlayerData.Instance.CurrentHealth);
        UpdateGoldText(PlayerData.Instance.Gold);
    }

    private void OnDisable()
    {
        PlayerData.OnHealthChanged -= UpdateHealthText;
        PlayerData.OnGoldChanged -= UpdateGoldText;
        //pauseButton.onClick.RemoveListener(TogglePause);
    }

    private void UpdateHealthText(int currentHealth)
    {
        healthText.text = $"HP: {currentHealth}/{(int)PlayerData.Instance.MaxHealth}";
    }

    private void UpdateGoldText(int amount)
    {
        goldText.text = $"{amount} Gold";
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pauseButtonText.text = isPaused ? "Resume" : "Pause";
    }
}
