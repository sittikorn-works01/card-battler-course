using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIntentUI : MonoBehaviour
{
    [SerializeField] private Image intentIcon;
    [SerializeField] private TextMeshProUGUI valueText;

    [SerializeField] private Sprite attackIcon;
    [SerializeField] private Sprite buffIcon;

    public void SetIntent(EnemyIntent intent)
    {
        gameObject.SetActive(true);
        valueText.text = intent.Value.ToString();

        intentIcon.sprite = intent.Type switch
        {
            IntentType.Attack => attackIcon,
            IntentType.Buff => buffIcon,
            _ => intentIcon.sprite
        };
    }

    public void Clear()
    {
        gameObject.SetActive(false);
    }
}
