using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace CardBattlerCourse.Systems
{
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

        public static event Action<int> OnGoldChanged;
        public static event Action<int> OnHealthChanged;

        private void Start()
        {
            savePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        }
        public bool TrySpendingGold(int price)
        {
            if (gold < price) return false;
            gold -= price;
            OnGoldChanged?.Invoke(gold);
            return true;
        }

        public void AddGold(int amount)
        {
            gold += amount;
            OnGoldChanged?.Invoke(gold);
        }

        public void TakeDamage(float amount)
        {
            currentHealth = Mathf.Max(0, currentHealth - amount);
            OnHealthChanged?.Invoke((int)currentHealth);
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            OnHealthChanged?.Invoke((int)currentHealth);
        }

        // Mirrors an already-computed health value (e.g. from the in-battle
        // Health component, which accounts for block) instead of applying its
        // own delta - use this when something else already did the math.
        public void SetHealth(float value)
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChanged?.Invoke((int)currentHealth);
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

                OnGoldChanged?.Invoke(gold);
                OnHealthChanged?.Invoke((int)currentHealth);

            }
            catch (Exception e)
            {
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
}
