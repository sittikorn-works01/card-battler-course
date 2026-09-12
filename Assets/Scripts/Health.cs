using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private float maxHealth;
    private float currentHealth;

    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private Flash flash;

    public void Init(float currentHealth, float maxHealth)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
        UpdateHealthBarUI();
    }

    private void UpdateHealthBarUI()
    {
        healthBarFill.fillAmount = (currentHealth / maxHealth);
        healthText.text = $"{currentHealth} / {maxHealth}";
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

    public void TakeDamage(int damageAmount)
    {
        StartCoroutine(flash.FlashRoutine());
        currentHealth -= damageAmount;
        if (currentHealth < 0) 
        { 
            currentHealth = 0;
        }
        UpdateHealthBarUI();
    }

    //public void GetHealthData(out float currentHealth, out float maxHealth)
    //{
    //    currentHealth = this.currentHealth; 
    //    maxHealth = this.maxHealth;
    //}

    public bool IsAlive() => currentHealth > 0;
}
