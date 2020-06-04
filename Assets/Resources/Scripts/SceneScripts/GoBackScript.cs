using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoBackScript : MonoBehaviour
{
    private Button backToMainMenu;
    private void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    void ChangedActiveScene(Scene current, Scene next) {
        if (GameObject.Find("BackToMainMenu"))
        {
            backToMainMenu = GameObject.Find("BackToMainMenu").GetComponent<Button>();
            backToMainMenu.onClick.AddListener(Back);
        }
    }

    void Back() {
        if (!SceneManagerWithParameters.GetActiveSceneName().Equals("MainMenu"))
        {
            SceneManagerWithParameters.Load("MainMenu");
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();    
        }
    }
}
