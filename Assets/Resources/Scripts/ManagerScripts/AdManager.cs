using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    public static AdManager Instance { get; private set; }
    private const string gameId = "3516819";
    private const bool testMode = true;
    private const string SecondLife = "SecondLife";
    private const string SecondSessionEnd = "SecondSessionEnd";
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
    void Start()
    {
        #if UNITY_ADS
        Advertisement.Initialize(gameId, testMode);
        Advertisement.AddListener(this);
        #endif
    }

    public void PlaySecondLife() {
        #if UNITY_ADS
        if (!Advertisement.IsReady(SecondLife)) return;
        Advertisement.Show(SecondLife);
        #endif
    }

    public void PlaySecondSessionEnd() {
        #if UNITY_ADS
        if (!Advertisement.IsReady(SecondSessionEnd)) return;
        Advertisement.Show(SecondSessionEnd);
        #endif   
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Failed:
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Finished:
                if (placementId == SecondLife)
                {
                    EndGameMenu.Instance.HideSecondLife();
                    LevelManager.Instance.StartGame();
                }
                break;
            default:
                break;
        }
    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    public void OnUnityAdsDidError(string message)
    {

    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }
}
