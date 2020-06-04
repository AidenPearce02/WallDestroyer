using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LevelingScene : MonoBehaviour
{
    private int Level = 0;
    public GameObject UpgradePanel, Upgrade, Content, PerkPanel, MessagePanel;
    public Text tMoney;
    private GameObject upgradePanel, pressedUpgrade;
    private int price, pLevel, money=0;
    private string pName;
    private bool isMessage = false;
    private float timeMessage = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        money = GameManager.Instance.Money;
        tMoney.text = "Money: " + money;
        if (PerkManager.Instance)
        {
            int i = 0;
            foreach (var perk in PerkManager.Instance.PerksData.perks)
            {
                if (perk.level != Level)
                {
                    upgradePanel = Instantiate(UpgradePanel, Content.transform);
                    upgradePanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Level " + perk.level;
                    Level++;
                }
                GameObject upgrade = Instantiate(Upgrade, upgradePanel.transform.GetChild(1));
                upgrade.transform.GetChild(0).gameObject.GetComponent<Text>().text = perk.name;
                if(PerkManager.Instance.Perks.ToArray()[i].isOpen)
                {
                    Button bUpgrade = upgrade.GetComponent<Button>();
                    ColorBlock cb = bUpgrade.colors;
                    cb.normalColor = Color.green;
                    bUpgrade.colors = cb;
                }
                upgrade.GetComponent<Button>().onClick.AddListener(ShowPerk);
                if (perk.level > PerkManager.Instance.LastOpenLevel) {
                    upgrade.GetComponent<Button>().interactable = false;
                }
                i++;
            }
        }
    }

    private void ShowPerk()
    {
        pressedUpgrade = EventSystem.current.currentSelectedGameObject;
        pName = pressedUpgrade.transform.GetChild(0).gameObject.GetComponent<Text>().text;
        pLevel = int.Parse(pressedUpgrade.transform.parent.parent.GetChild(0).gameObject.GetComponent<Text>().text.Replace("Level ",""));
        int i = 0;
        foreach (var perk in PerkManager.Instance.PerksData.perks)
        {
            if (perk.level == pLevel && perk.name==pName)
            {
                PerkPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = perk.name;
                PerkPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = perk.description;
                price = 1000;
                switch (perk.level)
                {
                    case 2: price = 2000; break;
                    case 3: price = 4000; break;
                    case 4: price = 8000; break;
                    case 5: price = 10000; break;
                    case 6: price = 15000; break;
                }
                PerkPanel.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Price: " + price;
                if (PerkManager.Instance.Perks.ToArray()[i].isOpen)
                {
                    PerkPanel.transform.GetChild(2).gameObject.SetActive(false);
                    PerkPanel.transform.GetChild(3).gameObject.SetActive(false);
                    Vector3 pos = PerkPanel.transform.GetChild(4).transform.localPosition;
                    pos.x = 0f;
                    PerkPanel.transform.GetChild(4).localPosition = pos;
                }
                else
                {
                    PerkPanel.transform.GetChild(2).gameObject.SetActive(true);
                    PerkPanel.transform.GetChild(3).gameObject.SetActive(true);
                    Vector3 posBuy = PerkPanel.transform.GetChild(3).transform.localPosition;
                    posBuy.x = -110f;
                    PerkPanel.transform.GetChild(3).transform.localPosition = posBuy;
                    PerkPanel.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(Buy);
                    Vector3 posBack = PerkPanel.transform.GetChild(4).transform.localPosition;
                    posBack.x = 110f;
                    PerkPanel.transform.GetChild(4).transform.localPosition = posBack;
                }
                PerkPanel.transform.GetChild(4).gameObject.GetComponent<Button>().onClick.AddListener(Back);
                break;
            }
            i++;
        }
        Content.SetActive(false);
        PerkPanel.SetActive(true);
    }

    private void Buy()
    {
        PerkPanel.SetActive(false);
        if (GameManager.Instance.Money >= price)
        {
            PerkManager.Instance.AddPerk(pName,pLevel);
            GameManager.Instance.SetMoney(-price);
            Button bPressedUpgrade = pressedUpgrade.GetComponent<Button>();
            ColorBlock cb = bPressedUpgrade.colors;
            cb.normalColor = Color.green;
            bPressedUpgrade.colors = cb; 
            int countOpenUpgrades = GetCountOpenUpgrades();
            if (countOpenUpgrades >= 2) {
                if (!IsOpenNextLevel()) {
                    OpenNextLevel();
                    PerkManager.Instance.AddLastOpenLevel();
                }
            }
            PerkManager.Instance.SaveData();
            ShowSuccess();
        }
        else
        {
            ShowError();
        }
        MessagePanel.SetActive(true);
        isMessage = true;
    }

    private void OpenNextLevel()
    {
        Transform upgradePanel = Content.transform.GetChild(index).GetChild(1);
        for (int i = 0; i < upgradePanel.childCount; i++) {
            upgradePanel.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
        }
    }
    private int index;
    private bool IsOpenNextLevel()
    {
        Transform upgradePanel = pressedUpgrade.transform.parent.parent;
        for (index = 0; index < Content.transform.childCount; index++) {
            if (Content.transform.GetChild(index) == upgradePanel) {
                index++;
                break;
            }
        }
        if (index >= Content.transform.childCount)
        {
            return true;
        }
        return Content.transform.GetChild(index).GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable;
    }

    private int GetCountOpenUpgrades()
    {
        Transform upgradePanel = pressedUpgrade.transform.parent;
        int count = 0;
        for(int i = 0;i<upgradePanel.childCount;i++)
        {
            if (upgradePanel.GetChild(i).gameObject.GetComponent<Button>().colors.normalColor == Color.green) {
                count++;
            }
        }
        return count;
    }

    private void ShowError()
    {
        MessagePanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Error";
        MessagePanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Not enough money";
    }

    private void ShowSuccess()
    {
        MessagePanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Success";
        MessagePanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = "The purchase was successful";
    }

    private void Back()
    {
        PerkPanel.SetActive(false);
        Content.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (money != GameManager.Instance.Money) {
            money = GameManager.Instance.Money;
            tMoney.text = "Money: " + money;
        }
        if (isMessage)
        {
            timeMessage += Time.deltaTime;
            if (timeMessage >= 2)
            {
                timeMessage = 0.0f;
                isMessage = false;
                MessagePanel.SetActive(false);
                Content.SetActive(true);
            }
        }
    }
}
