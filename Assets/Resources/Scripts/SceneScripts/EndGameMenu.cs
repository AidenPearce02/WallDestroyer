using System;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    public static EndGameMenu Instance { get; private set; }
    public GameObject endGamePanel, secondLifePanel, countDown;
    public Button mainMenu;
    public Button tempButton;
    private bool end = false;
    private bool endSession = true;
    public bool secondLife = false;
    private bool timer = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        secondLifePanel.SetActive(false);
        endGamePanel.SetActive(false);
        secondLifePanel.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(WatchAd);
        secondLifePanel.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(Skip);
        mainMenu.onClick.AddListener(MainMenu);
    }

    public void HideSecondLife() {
        secondLife = false;
        secondLifePanel.SetActive(false);
    }

    public void Skip()
    {
        secondLife = false;
        LevelManager.Instance.isSecondLife = true;
    }

    private void WatchAd()
    {
        secondLifePanel.SetActive(false);
        AdManager.Instance.PlaySecondLife();
    }

    void MainMenu() {
        if (end){
            if (GameManager.Instance.CurrentLevel!=0 && GameManager.Instance.CurrentLevel == GameManager.Instance.LastOpenedLevel + 1&&!LevelManager.Instance.GameOver)
            {
                GameManager.Instance.SetLastOpenedLevel();
            }
            GameManager.Instance.SetMoney(LevelManager.Instance.Money);
            SceneManagerWithParameters.Load("MainMenu");
            end = false;
        }
    }

    void Restart() {
        if (end){
            SceneManagerWithParameters.Load("TemplateScene");
            endGamePanel.transform.GetChild(0).gameObject.SetActive(false);
            GameManager.Instance.SetMoney(LevelManager.Instance.Money);
            end = false;
        }
    }

    void NextLevel() {
        if (end){
            if (GameManager.Instance.CurrentLevel == GameManager.Instance.LastOpenedLevel + 1)
            {
                GameManager.Instance.SetLastOpenedLevel();
            }
            GameManager.Instance.SetCurrentLevel(GameManager.Instance.CurrentLevel + 1);
            if (GameManager.Instance.CurrentLevel >= GameManager.Instance.CountLevels) {
                endGamePanel.transform.GetChild(3).gameObject.SetActive(false);
            }
            GameManager.Instance.SetMoney(LevelManager.Instance.Money);
            SceneManagerWithParameters.Load("TemplateScene");
            end = false;
        }
    }

    private void Update()
    {
        if (!end&&!secondLife) { 
            if(!LevelManager.Instance.FreeMode)
            {
                if (LevelManager.Instance.GameOver)
                {
                    tempButton.GetComponentInChildren<Text>().text = "Try Again";
                    tempButton.onClick.AddListener(Restart);
                    end = true;
                }
                else if (LevelManager.Instance.GetObjects().Count == 0) {
                    tempButton.GetComponentInChildren<Text>().text = "Next Level";
                    tempButton.onClick.AddListener(NextLevel);
                    end = true;
                }
            }
            else{
                if (LevelManager.Instance.GameOver){
                    if (!LevelManager.Instance.isSecondLife)
                    {
                        secondLife = true;
                    }
                    else
                    {
                        float score = LevelManager.Instance.Score;
                        if (PerkManager.Instance.checkPerk("Perk 9", 3))
                        {
                            score += 2 * LevelManager.Instance.Money;
                        }
                        if (PerkManager.Instance.checkPerk("Perk 10", 4))
                        {
                            score += 1000;
                        }
                        if (score > 1.5f * GameManager.Instance.Record && PerkManager.Instance.checkPerk("Perk 6", 2))
                        {
                            GameManager.Instance.SetMoney(300);
                        }
                        if (score > GameManager.Instance.Record)
                        {
                            GameManager.Instance.SetRecord(score);
                        }
                        tempButton.GetComponentInChildren<Text>().text = "Try Again";
                        tempButton.onClick.AddListener(Restart);
                        end = true;
                    }
                }
            }
        }
        if (secondLife) ShowSecondLife();
        if (end) ShowMenu();
    }

    private void ShowSecondLife()
    {
        if (timer)
        {
            secondLifePanel.SetActive(true);
            countDown = GameObject.Find("CountDown");
            countDown.GetComponent<CountDownAd>().StartCountDown();
            timer = false;
        }
    }

    void ShowMenu()
    {
        secondLifePanel.SetActive(false);
        if (endSession)
        {
            int countSessions = GameManager.Instance.CountSessions + 1;
            GameManager.Instance.SetCountSession(countSessions);
            if (countSessions == 2)
            {
                AdManager.Instance.PlaySecondSessionEnd();
                GameManager.Instance.SetCountSession(0);
            }
            endSession = false;
        }
        endGamePanel.SetActive(true);
        if (LevelManager.Instance.FreeMode) {
            endGamePanel.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
