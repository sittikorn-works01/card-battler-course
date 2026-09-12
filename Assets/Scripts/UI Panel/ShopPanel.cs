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
        PlayerData.OnGoldChanged += UpdateGoldTextUI;
    }

    private void UpdateGoldTextUI(int amount)
    {
        goldText.text = amount.ToString();
    }

    private void OnDisable()
    {
        exitShopButton.onClick.RemoveAllListeners();
        PlayerData.OnGoldChanged -= UpdateGoldTextUI;
    }

    

    private void ExitShop()
    {
        GameManager.Instance.EnterState(GameState.Map);
    }
}
