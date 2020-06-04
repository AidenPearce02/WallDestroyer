using UnityEngine;
using UnityEngine.UI;

public class CountDownAd : MonoBehaviour
{
    // Start is called before the first frame update
    float timeLeft = 4f;
    private Text text;
    void Start()
    {

    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft >= 2 && timeLeft < 3)
        {
            text.text = "2";
        }
        else if (timeLeft >= 1 && timeLeft < 2)
        {
            text.text = "1";
        }
        else if (timeLeft >= 0 && timeLeft < 1)
        {
            text.text = "0";
        }
        else if (timeLeft < 0)
        {
            EndGameMenu.Instance.Skip();
        }
    }

    public void StartCountDown()
    {
        text = GetComponent<Text>();
        text.text = "3";
        timeLeft = 4f;
    }
}
