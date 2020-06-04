using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseLevel : MonoBehaviour
{
    public Button back, next;
    public GameObject prefabLevel, prefabLevels;
    public Sprite locked, completed;
    private List<GameObject> pages = new List<GameObject>();
    private int min = 0, max = 0, currentPage = 0;
    public SwipeType SwipeType { get; private set; } = SwipeType.NONE;
    public static ChooseLevel Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            int countLevels = (GameManager.Instance == null) ? 30 : GameManager.Instance.CountLevels,
            lastOpenedLevel = (GameManager.Instance == null) ? 0 : GameManager.Instance.LastOpenedLevel,
            maxOnPage = 15;
            max = countLevels / maxOnPage;
            for (int n = 0; n <= max; n++)
            {
                int temp = ((countLevels - (n + 1) * maxOnPage) < 0) ? (countLevels - n * maxOnPage) : maxOnPage;
                if (temp > 0)
                {
                    GameObject levels = Instantiate(prefabLevels, gameObject.transform) as GameObject;
                    for (int i = n * maxOnPage; i < n * maxOnPage + temp; i++)
                    {
                        GameObject level = Instantiate(prefabLevel) as GameObject;
                        level.GetComponentInChildren<Text>().text = (i + 1).ToString();
                        if (i < lastOpenedLevel)
                        {
                            level.transform.GetChild(1).GetComponent<Image>().sprite = completed;
                        }
                        else if (i > lastOpenedLevel)
                        {
                            level.GetComponent<Button>().interactable = false;
                            level.transform.GetChild(1).GetComponent<Image>().sprite = locked;
                        }
                        else {
                            level.transform.GetChild(1).gameObject.SetActive(false);
                        }
                        if (level.GetComponent<Button>().interactable) {
                            level.GetComponent<Button>().onClick.AddListener(LoadLevel);
                        }
                        level.transform.SetParent(levels.transform);
                    }
                    levels.SetActive(false);
                    pages.Add(levels);
                }
            }
        }
        else if (Instance != this)
            Destroy(gameObject);
    }
    void LoadLevel() {
        if (GameManager.Instance) {
            GameManager.Instance.SetCurrentLevel(int.Parse(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text));
            SceneManagerWithParameters.Load("TemplateScene");
        }
    }

    void Start()
    {
       
        pages.ToArray()[currentPage].SetActive(true);
        back.onClick.AddListener(Back);
        next.onClick.AddListener(Next);
    }

    void Back() {
        pages.ToArray()[currentPage--].SetActive(false);
        pages.ToArray()[currentPage].SetActive(true);
    }

    void Next() {
        pages.ToArray()[currentPage++].SetActive(false);
        pages.ToArray()[currentPage].SetActive(true);
    }

    public void SetSwipe(SwipeType temp)
    {
        SwipeType = temp;
    }

    void Update()
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
        if (min == max) {
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
