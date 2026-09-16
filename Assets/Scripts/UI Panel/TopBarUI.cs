using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI deckCountText;
    [SerializeField] private Button pauseButton;

    private void OnEnable()
    {
        PlayerData.OnHealthChanged += UpdateHealthText;
        PlayerData.OnGoldChanged += UpdateGoldText;
        DeckEvents.OnDeckProcessed += UpdateDeckCountText;
    }

    private void Start()
    {
        UpdateHealthText((int)PlayerData.Instance.CurrentHealth);
        UpdateGoldText(PlayerData.Instance.Gold);
        UpdateDeckCountText();
    }

    private void OnDisable()
    {
        PlayerData.OnHealthChanged -= UpdateHealthText;
        PlayerData.OnGoldChanged -= UpdateGoldText;
        DeckEvents.OnDeckProcessed -= UpdateDeckCountText;
    }

    private void UpdateHealthText(int currentHealth)
    {
        healthText.text = $"HP: {currentHealth}/{(int)PlayerData.Instance.MaxHealth}";
    }

    private void UpdateGoldText(int amount)
    {
        goldText.text = $"{amount} Gold";
    }

    private void UpdateDeckCountText()
    {
        deckCountText.text = $"Deck: {DeckManager.Instance.GetDeck().Count}";
    }
}
