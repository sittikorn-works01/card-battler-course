using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData : Singleton<PlayerData>
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private int gold;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public int Gold => gold;

    //private int CurrentActIndex = 0;
    //private int CurrentFloorIndex = 0;

    private string savePath; 
    public MapGraph CurrentMap = null;

    private void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savefile.json");
    }
    public bool TrySpendingGold(int price)
    {
        if (gold < price) return false;
        gold -= price;
        return true;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }

    public void Save()
    {
        SaveData saveData = new SaveData();
        saveData.CurrentHealth = CurrentHealth;
        saveData.MaxHealth = MaxHealth;
        saveData.Gold = Gold;

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);
    }

    public void Load() 
    {
        if (!File.Exists(savePath)) return;

        try
        {
            string json = File.ReadAllText(savePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            currentHealth = saveData.CurrentHealth;
            maxHealth = saveData.MaxHealth;
            gold = saveData.Gold;

            print("LOAD COMPLETED");
        }
        catch (Exception e) {
            Debug.LogWarning($"Save file corrupted or unreadable, starting fresh: {e.Message}");
        }        
    }
}

[Serializable]
public class SaveData
{
    public float CurrentHealth;
    public float MaxHealth;
    public int Gold;
}