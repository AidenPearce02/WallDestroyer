using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour,IPointerDownHandler
{
    public static bool GameIsPaused = false;
    public GameObject pausePanel;
    public GameObject countDown;
    // Start is called before the first frame update
    void Start()
    {
        pausePanel.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(Resume);
        pausePanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(MainMenu);
    }
    public void OnPointerDown(PointerEventData data)
    {
        if (GameIsPaused) Resume();
        else Pause();
    }

    public void Resume() {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        countDown.GetComponent<CountDown>().StartCountDown();
    }

    void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    public void MainMenu()
    {
        SceneManagerWithParameters.Load("MainMenu");
    }
}
