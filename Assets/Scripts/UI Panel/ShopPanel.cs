using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : BasePanel
{
    [SerializeField] private Button exitShopButton;
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable()
    {
        exitShopButton.onClick.AddListener(ExitShop);
        goldText.text = PlayerData.Instance.Gold.ToString();
    }

    private void OnDisable()
    {
        exitShopButton.onClick.RemoveAllListeners();
    }

    private void ExitShop()
    {
        GameManager.Instance.EnterState(GameState.Map);
    }
}
