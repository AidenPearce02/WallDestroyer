using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    float timeLeft = 4f;
    private Text text;
    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft >= 2 && timeLeft < 3) {
            text.text = "2";
        }
        else if (timeLeft >= 1 && timeLeft < 2) {
            text.text = "1";
        }
        else if (timeLeft >= 0 && timeLeft < 1) {
            text.text = "Destroy!";
        }
        else if (timeLeft < 0)
        {
            LevelManager.Instance.IsGame = true;
            gameObject.SetActive(false);
        }
    }

    public void StartCountDown()
    {
        LevelManager.Instance.IsGame = false;
        gameObject.SetActive(true);
        text.text = "3";
        timeLeft = 4f;
    }
}
