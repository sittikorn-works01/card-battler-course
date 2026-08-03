using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    private float currentHealth;

    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private Flash flash;

    private void Start()
    {
        currentHealth = maxHealth;
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

    public bool IsAlive() => currentHealth > 0;
}
