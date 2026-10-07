using TMPro;
using UnityEngine;

public class TopBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI deckCountText;

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
        healthText.text = $"{currentHealth}/{(int)PlayerData.Instance.MaxHealth}";
    }

    private void UpdateGoldText(int amount)
    {
        goldText.text = $"{amount}";
    }

    private void UpdateDeckCountText()
    {
        deckCountText.text = $"{DeckManager.Instance.GetDeck().Count}";
    }
}
