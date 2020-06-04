using UnityEngine;

public class MoveDownManager : MonoBehaviour
{
    public static MoveDownManager Instance { get; private set; }
    private float time = 0.0f;
    private int fiveSecondDelay = 5;
    public float speed { get; private set; } = -1.5f;
    private float basicSpeed = -1.5f;
    private float lastSpeed = -1.5f;
    private int counterAccelerationSpeed = 1;
    private bool isSlow = false;
    private GameObject Player;

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
    private void Start()
    {
        Player = GameObject.Find("Doll");
    }

    private void Update()
    {
        if (LevelManager.Instance.IsGame&&!LevelManager.Instance.GameOver) {
            if (!isSlow)
            {
                time += Time.deltaTime;
                if (time >= fiveSecondDelay)
                {
                    time = 0.0f;
                    speed = basicSpeed + basicSpeed * 0.2f * counterAccelerationSpeed;
                    counterAccelerationSpeed++;
                }
            }
            if (Player)
            {
                if (LevelManager.Instance.Immortal)
                {
                    if(speed!=0f) lastSpeed = speed;
                    speed = 0f;
                }
                else
                {
                    if(speed==0f)
                    speed = lastSpeed;
                    if (Player.GetComponent<Effect>().CurrentEffect == Effect.EffectType.PURPLE && !isSlow)
                    {
                        if (PerkManager.Instance.checkPerk("Perk 12", 4)) speed = 0;
                        else speed /= 2;
                        isSlow = true;
                    }
                    else if (Player.GetComponent<Effect>().CurrentEffect == Effect.EffectType.NONE && isSlow) 
                    {
                        if (PerkManager.Instance.checkPerk("Perk 12", 4)) speed = lastSpeed;
                        else speed *= 2;
                        isSlow = false;
                    }
                }
            }
        }
        if (LevelManager.Instance.GameOver||!LevelManager.Instance.IsGame)
        {
            if (speed != 0f) lastSpeed = speed;
            speed = 0f;
        }
    }
}
