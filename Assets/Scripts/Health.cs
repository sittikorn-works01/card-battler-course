using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private float maxHealth;
    private float currentHealth;
    private int currentBlock;

    public float CurrentHealth => currentHealth;

    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI blockText;

    [SerializeField] private Flash flash;

    public void Init(float currentHealth, float maxHealth)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
        currentBlock = 0;
        UpdateHealthBarUI();
        UpdateBlockUI();
    }

    private void UpdateHealthBarUI()
    {
        healthBarFill.fillAmount = (currentHealth / maxHealth);
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    private void UpdateBlockUI()
    {
        blockText.gameObject.SetActive(currentBlock > 0);
        blockText.text = currentBlock.ToString();
    }

    public void Heal(int healAmount)
    {
        if(healAmount <= 0)
        {
            return;
        }

        currentHealth += healAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthBarUI();
    }

    public void AddBlock(int blockAmount)
    {
        if (blockAmount <= 0)
        {
            return;
        }

        currentBlock += blockAmount;
        UpdateBlockUI();
    }

    public void ClearBlock()
    {
        currentBlock = 0;
        UpdateBlockUI();
    }

    public void TakeDamage(int damageAmount)
    {
        StartCoroutine(flash.FlashRoutine());

        int blocked = Mathf.Min(currentBlock, damageAmount);
        currentBlock -= blocked;

        currentHealth -= damageAmount - blocked;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthBarUI();
        UpdateBlockUI();
    }

    //public void GetHealthData(out float currentHealth, out float maxHealth)
    //{
    //    currentHealth = this.currentHealth; 
    //    maxHealth = this.maxHealth;
    //}

    public bool IsAlive() => currentHealth > 0;
}
