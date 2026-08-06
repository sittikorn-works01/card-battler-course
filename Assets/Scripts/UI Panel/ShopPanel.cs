using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : BasePanel
{
    [SerializeField] private Button exitShopButton;

    private void OnEnable()
    {
        exitShopButton.onClick.AddListener(ExitShop);
    }

    private void OnDisable()
    {
        exitShopButton.onClick.RemoveAllListeners();
    }

    private void ExitShop()
    {
        GameManager.Instance.EnterState(GameState.Battle);
    }
}
