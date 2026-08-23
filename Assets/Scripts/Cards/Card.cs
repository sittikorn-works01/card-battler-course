using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer illustrationRender;
    [SerializeField] private TextMeshPro cardNameText;
    [SerializeField] private TextMeshPro descriptionText;
    [SerializeField] private TextMeshPro actionText;

    [SerializeField] private SortingGroup sortingGroup;
    private int originalSortingOrder;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    [SerializeField] private float hoverScale = 2f;
    [SerializeField] private float hoverOffset = 2f;


    private static bool isBeingDragged = false;
    private bool isPlaying = false;

    private CardData cardData;

    [SerializeField] private Collider2D cardCollider;
    [SerializeField] private SpriteRenderer glowOverlay;
    [SerializeField] private int glowDuration = 300;
    [SerializeField] private SpriteRenderer disableOverlay;

    private void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;
        originalSortingOrder = sortingGroup.sortingOrder;
    }

    public void LoadCardData(CardData cardData)
    {
        this.cardData = cardData;
        illustrationRender.sprite = cardData.Illustration;
        cardNameText.text = cardData.CardName;
        descriptionText.text = cardData.CardDescription;
        actionText.text = cardData.actionCost.ToString();
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.IsBattleActive()) return;

        RewardManager.Instance.SelectCard(cardData);        
    }

    private void OnMouseEnter()
    {
        if (isBeingDragged) return;

        transform.localScale = originalScale * hoverScale;
        transform.localPosition += new Vector3(0, hoverOffset, 0);
        sortingGroup.sortingOrder += 1;

    }

    private void OnMouseExit()
    {
        if (isBeingDragged) return;

        transform.localScale = originalScale;
        transform.localPosition = originalPosition;
        sortingGroup.sortingOrder = originalSortingOrder;
    }

    private void OnMouseDrag()
    {
        isBeingDragged = true;
        gameObject.transform.position = GetMousePosition();
    }

    public void SetIsPlaying(bool isPlaying)
    {
        this.isPlaying = isPlaying;
    }

    private void OnMouseUp()
    {
        isBeingDragged = false;
        if (isPlaying) return;

        transform.localScale = originalScale;
        transform.localPosition = originalPosition;
        sortingGroup.sortingOrder = originalSortingOrder;
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = transform.position.z - Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    public CardData GetCardData() => cardData;

    private void OnDestroy()
    {
        isBeingDragged = false;
    }

    public void SetInteractable(bool interactable)
    {
        cardCollider.enabled = interactable;
        disableOverlay.gameObject.SetActive(!interactable);
    }

    public void Glow()
    {
        GlowCoroutine().Forget();
    }

    private async UniTask GlowCoroutine()
    {
        glowOverlay.gameObject.SetActive(true);
        await UniTask.Delay(glowDuration);
        glowOverlay.gameObject.SetActive(false);
    }

}
