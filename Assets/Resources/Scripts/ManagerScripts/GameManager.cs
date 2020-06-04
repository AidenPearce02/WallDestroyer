using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float Record { get; private set; }
    public int Money { get; private set; }
    public int LastOpenedLevel { get; private set; }
    public int CurrentLevel { get; private set; }
    private string fileGlobalData = "GlobalData";
    public int CountLevels { get; private set; }
    public string Perks { get; private set; } = null;
    public string Skins { get; private set; } = null;
    public int CountSessions { get; private set; } = 0;
    public void SetCountSession(int countSessions) {
        CountSessions = countSessions;
    }


    private void Awake()
    {
        if (Instance == null){
            Instance = this;
            LoadData(); 
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void SaveData() {
        if (Perks == null) PerkManager.Instance.SaveData();
        else if (Skins == null) SkinManager.Instance.SaveData();
        else SaveSystem.SaveData();
    }

    public void LoadData() {
        GameData gameData = SaveSystem.LoadData();
        if (gameData!=null) {
            Money = gameData.money;
            LastOpenedLevel = gameData.lastOpenedLevel;
            Perks = gameData.perks;
            Record = gameData.record;
            Skins = gameData.skins;
        }
        else
        {
            Money = 10000000;
            LastOpenedLevel = 0;
            Record = 0;
        }
        GlobalData globalData = XmlIO.LoadXml<GlobalData>(fileGlobalData);
        CountLevels = globalData.CountLevels;
    }

    public void SetCurrentLevel(int level) {
        CurrentLevel = level;
    }
    public void SetLastOpenedLevel() {
        LastOpenedLevel++;
        SaveData();
    }

    public void SetMoney(int money) {
        Money += money;
        SaveData();
    }

    public void SetRecord(float record) {
        Record = record; 
        SaveData();
    }

    public void SetPerks(string perks) {
        Perks = perks;
        SaveData();
    }
    public void SetSkins(string skins) {
        Skins = skins;
        SaveData();
    }
}
