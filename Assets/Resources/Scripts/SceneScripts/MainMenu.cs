using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button bFreeMode;
    public Button bWalkthrough;
    public Button bCustomizer;
    public Button bLeveling;
    public Text tMoney;
    void Start()
    {
        bFreeMode.onClick.AddListener(FreeMode);
        bWalkthrough.onClick.AddListener(Walkthrough);
        bCustomizer.onClick.AddListener(Customizer);
        bLeveling.onClick.AddListener(Leveling);
    }
    private void Update()
    {
        tMoney.text = "Money: " + GameManager.Instance.Money;
    }
    void Walkthrough() {
        SceneManagerWithParameters.Load("СhooseLevel");
    }
    void Customizer() {
        SceneManagerWithParameters.Load("CustomizationScene");
    }
    void Leveling() {
        SceneManagerWithParameters.Load("LevelingScene");
    }

    void FreeMode() {
        GameManager.Instance.SetCurrentLevel(0);
        SceneManagerWithParameters.Load("TemplateScene");
    }
}
