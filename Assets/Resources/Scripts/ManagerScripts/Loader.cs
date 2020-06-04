using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject perkManager;
    public GameObject skinManager;
    public GameObject adManager;
    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.Instance == null)
            Instantiate(gameManager);
        if (PerkManager.Instance == null)
            Instantiate(perkManager);
        if (SkinManager.Instance == null)
            Instantiate(skinManager);
        if (AdManager.Instance == null)
            Instantiate(adManager);
    }

}
