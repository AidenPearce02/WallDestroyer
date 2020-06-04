using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizationScene : MonoBehaviour
{
    public Button back, next;
    public Text money;
    public GameObject prefabSkin, prefabSkins, SkinPanel, MessagePanel;
    public Sprite used, bought;
    private SkinManager.Skin skin;
    private GameObject pressedSkin;
    private List<GameObject> pages = new List<GameObject>();
    private int min = 0, max = 0, currentPage = 0, price = 0, iMoney = 0;
    private bool isMessage;
    private float timeMessage;

    public SwipeType SwipeType { get; private set; } = SwipeType.NONE;
    public static CustomizationScene Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            int countSkins = (SkinManager.Instance == null) ? 30 : SkinManager.Instance.countSkins,
            maxOnPage = 15;
            max = countSkins / maxOnPage;
            for (int n = 0; n <= max; n++)
            {
                int temp = ((countSkins - (n + 1) * maxOnPage) < 0) ? (countSkins - n * maxOnPage) : maxOnPage;
                if (temp > 0)
                {
                    GameObject skins = Instantiate(prefabSkins, gameObject.transform) as GameObject;
                    for (int i = n * maxOnPage; i < n * maxOnPage + temp; i++)
                    {
                        SkinManager.Skin skin = SkinManager.Instance.Skins[i];
                        GameObject skinObject = Instantiate(prefabSkin) as GameObject;
                        skinObject.GetComponent<Image>().sprite = skin.skinHead;
                        skinObject.name = skin.name;
                        if (skin.state == SkinManager.State.AVAILABLE)
                        {
                            skinObject.transform.GetChild(0).gameObject.SetActive(false);
                        }
                        else if(skin.state == SkinManager.State.BOUGHT)
                        {
                            skinObject.transform.GetChild(0).gameObject.SetActive(true);
                            skinObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = bought;
                        }
                        else if(skin.state == SkinManager.State.USED)
                        {
                            skinObject.transform.GetChild(0).gameObject.SetActive(true);
                            skinObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = used;
                        }
                        skinObject.transform.SetParent(skins.transform);
                        skinObject.GetComponent<Button>().onClick.AddListener(ShowSkin);
                    }
                    skins.SetActive(false);
                    pages.Add(skins);
                }
            }
            SkinPanel.transform.GetChild(4).gameObject.GetComponent<Button>().onClick.AddListener(BackToSkins);
        }
        else if (Instance != this)
            Destroy(gameObject);
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

    private void ChangedSkin() {
        MessagePanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Success";
        MessagePanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = "The skin was changed succesful";
    }

    private void BackToSkins()
    {
        SkinPanel.SetActive(false);
        pages.ToArray()[currentPage].SetActive(true);
    }

    void ShowSkin()
    {
        pressedSkin = EventSystem.current.currentSelectedGameObject;
        string name = pressedSkin.name;
        skin = SkinManager.Instance.GetSkinByName(name);
        if (skin != null) {
            SkinPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = skin.name;
            SkinPanel.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = skin.skinFull;
            price = skin.price;
            SkinPanel.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Price: "+skin.price;
            SkinPanel.transform.GetChild(2).gameObject.SetActive(false);
            if (skin.state == SkinManager.State.USED)
            {
                SkinPanel.transform.GetChild(3).gameObject.SetActive(false);
                Vector3 pos = SkinPanel.transform.GetChild(4).transform.localPosition;
                pos.x = 0f;
                SkinPanel.transform.GetChild(4).localPosition = pos;
            }
            else{
                SkinPanel.transform.GetChild(3).gameObject.SetActive(true);
                Vector3 posBuy = SkinPanel.transform.GetChild(3).transform.localPosition;
                posBuy.x = -110f;
                SkinPanel.transform.GetChild(3).transform.localPosition = posBuy;
                Vector3 posBack = SkinPanel.transform.GetChild(4).transform.localPosition;
                posBack.x = 110f;
                SkinPanel.transform.GetChild(4).transform.localPosition = posBack;
                if (skin.state == SkinManager.State.AVAILABLE) {
                    SkinPanel.transform.GetChild(2).gameObject.SetActive(true);
                    SkinPanel.transform.GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = "Buy";
                    SkinPanel.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(Buy);
                }
                else
                {
                    SkinPanel.transform.GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = "Use";
                    SkinPanel.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(Use);
                }
            }

        }
        pages.ToArray()[currentPage].SetActive(false);
        SkinPanel.SetActive(true);
    }

    private void Use()
    {
        SkinPanel.SetActive(false);
        string nameSkinUsed = SkinManager.Instance.SetCurrentSkin(skin);
        GameObject skinUsed = null;
        foreach (GameObject page in pages)
        {
            skinUsed = page.transform.Find(nameSkinUsed).gameObject;
            if (skinUsed != null) break;
        }
        skinUsed.transform.GetChild(0).gameObject.SetActive(true);
        skinUsed.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = bought;
        pressedSkin.transform.GetChild(0).gameObject.SetActive(true);
        pressedSkin.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = used;
        ChangedSkin();
        MessagePanel.SetActive(true);
        isMessage = true;
    }

    private void Buy()
    {
        SkinPanel.SetActive(false);
        if (GameManager.Instance.Money >= price)
        {
            SkinManager.Instance.ChangeStateBySkin(skin,SkinManager.State.BOUGHT);
            GameManager.Instance.SetMoney(-price);
            pressedSkin.transform.GetChild(0).gameObject.SetActive(true);
            pressedSkin.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = bought;
            ShowSuccess();
            SkinManager.Instance.SaveData();
        }
        else
        {
            ShowError();
        }
        MessagePanel.SetActive(true);
        isMessage = true;
    }

    void Start()
    {
        iMoney = GameManager.Instance.Money;
        money.text = "Money: " + iMoney;
        SkinPanel.SetActive(false);
        MessagePanel.SetActive(false);
        pages.ToArray()[currentPage].SetActive(true);
        back.onClick.AddListener(Back);
        next.onClick.AddListener(Next);
    }

    void Back()
    {
        pages.ToArray()[currentPage--].SetActive(false);
        pages.ToArray()[currentPage].SetActive(true);
    }

    void Next()
    {
        pages.ToArray()[currentPage++].SetActive(false);
        pages.ToArray()[currentPage].SetActive(true);
    }

    public void SetSwipe(SwipeType temp)
    {
        SwipeType = temp;
    }

    void Update()
    {
        if (iMoney != GameManager.Instance.Money) {
            iMoney = GameManager.Instance.Money;
            money.GetComponent<Text>().text = "Money: " + iMoney;
        }
        if (isMessage)
        {
            timeMessage += Time.deltaTime;
            if (timeMessage >= 2)
            {
                timeMessage = 0.0f;
                isMessage = false;
                MessagePanel.SetActive(false);
                pages.ToArray()[currentPage].SetActive(true);
            }
        }
        else
        {
            if (SwipeType == SwipeType.RIGHT && back.gameObject.activeInHierarchy)
            {
                Back();
                SwipeType = SwipeType.NONE;
            }
            else if (SwipeType == SwipeType.LEFT && next.gameObject.activeInHierarchy)
            {
                Next();
                SwipeType = SwipeType.NONE;
            }
            if (min == max)
            {
                back.gameObject.SetActive(false);
                next.gameObject.SetActive(false);
            }
            else
            {
                if (currentPage == min)
                {
                    back.gameObject.SetActive(false);
                }
                else
                {
                    back.gameObject.SetActive(true);
                }
                if (currentPage == max - 1)
                {
                    next.gameObject.SetActive(false);
                }
                else
                {
                    next.gameObject.SetActive(true);
                }
            }
        }
    }
}
