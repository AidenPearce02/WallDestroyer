using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    public static PerkManager Instance { get; private set; }
    public Perks PerksData { get; private set; }
    public string OpenPerks { get; private set; } = "";
    public class OpenPerk
    {
        public string name;
        public bool isOpen;
        public int level;
        public OpenPerk(string name,int level, bool isOpen)
        {
            this.name = name;
            this.isOpen = isOpen;
            this.level = level;
        }
    }
    public List<OpenPerk> Perks { get; private set; }
    private string filePerksData = "Perks";
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        LoadData();
    }

    public int LastOpenLevel { get; private set; } = 0;
    public void AddLastOpenLevel() {
        LastOpenLevel++;
    }

    void LoadData()
    {
        PerksData = XmlIO.LoadXml<Perks>(filePerksData);
        int count = PerksData.perks.Length;
        Perks = new List<OpenPerk>();
        int i = 0;
        if (GameManager.Instance) {
            if (GameManager.Instance.Perks == null) {
                for (int index = 0; index < count; index++) {
                    OpenPerks += "0";
                }
            }
            else OpenPerks = GameManager.Instance.Perks;
            int temp = 0;
            foreach (char state in OpenPerks)
            {
                if (state == '1')
                {
                    Perks.Add(new OpenPerk(PerksData.perks[i].name, PerksData.perks[i].level, true));
                    temp++;
                }
                else Perks.Add(new OpenPerk(PerksData.perks[i].name, PerksData.perks[i].level, false));
                if (i % 3 == 2) {
                    if (temp >= 2) {
                        LastOpenLevel++;
                    }
                    temp = 0;
                }
                i++;
            }
            LastOpenLevel++;
        }
    }

    public void SaveData() {
        OpenPerks = "";
        foreach (var perk in Perks)
        {
            if (perk.isOpen) OpenPerks += "1";
            else OpenPerks += "0";
        }
        GameManager.Instance.SetPerks(OpenPerks);
    }

    public void AddPerk(string name, int level) {
        foreach (var item in Perks)
        {
            if (item.name == name && item.level == level)
            {
                item.isOpen = true;
                break;
            }
        }
    }

    public bool checkPerk(string name, int level) {
        foreach (var item in Perks)
        {
            if (item.name == name && item.level == level && item.isOpen)
            {
                return true;
            }
        }
        return false;
    }
}
