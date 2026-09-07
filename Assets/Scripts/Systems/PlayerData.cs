using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData : Singleton<PlayerData>
{
    public float CurrentHealth = 50;
    public float MaxHealth = 50;
    public float Gold;

    public int CurrentActIndex = 0;
    public int CurrentFloorIndex = 0;

    public MapGraph CurrentMap = null;

    private void Start()
    {
        print(Application.persistentDataPath);
    }

    public void Save()
    {
        SaveData saveData = new SaveData();
        saveData.CurrentHealth = CurrentHealth;
        saveData.MaxHealth = MaxHealth;
        saveData.Gold = Gold;

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load() {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

             CurrentHealth = saveData.CurrentHealth;
             MaxHealth = saveData.MaxHealth;
             Gold = saveData.Gold;
        }
    }
}

[Serializable]
public class SaveData
{
    public float CurrentHealth;
    public float MaxHealth;
    public float Gold;
}