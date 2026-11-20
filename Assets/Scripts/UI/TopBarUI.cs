using CardBattlerCourse.DeckBuilder;
using CardBattlerCourse.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace CardBattlerCourse.UI
{
    public class TopBarUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI deckCountText;
        private UIBubbleManager UIBubbleManager => UIBubbleManager.Instance;

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

        private void UpdateHealthText(int currentHealth, int changedAmount = 0)
        {
            if(changedAmount > 0)
            {
                UIBubbleManager.DisplayePopupText(TextType.None, changedAmount.ToString(), healthText.transform.position);
            }
            healthText.text = $"{currentHealth}/{(int)PlayerData.Instance.MaxHealth}";
        }

        private void UpdateGoldText(int amount, int changedAmount = 0)
        {
            if (changedAmount > 0)
            {
                UIBubbleManager.DisplayePopupText(TextType.None, changedAmount.ToString(), goldText.transform.position);
            }
            goldText.text = $"{amount}";
        }

        private void UpdateDeckCountText()
        {
            deckCountText.text = $"{DeckManager.Instance.GetDeck().Count}";
        }
    }
}
