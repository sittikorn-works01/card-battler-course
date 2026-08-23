using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopCanvas;

    private void OnEnable()
    {
        shopCanvas.SetActive(true);
    }

    private void OnDisable()
    {
        shopCanvas.SetActive(false);
    }
}
