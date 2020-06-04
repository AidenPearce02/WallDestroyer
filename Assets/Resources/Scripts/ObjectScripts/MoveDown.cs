using UnityEngine;

public class MoveDown : MonoBehaviour
{
    private Rigidbody2D rb2d;
    /*private float time = 0.0f;
    private int tenSecondDelay = 5;
    private float speed = -1.5f;
    private float lastSpeed = -1.5f;
    private bool isSlow = false;
    private GameObject Player;*/
    // Start is called before the first frame update
    private void Start()
    {
        //Player = GameObject.Find("Doll");
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb2d.velocity = new Vector2(0, MoveDownManager.Instance.speed);
        /*if (LevelManager.Instance.IsGame&&!LevelManager.Instance.GameOver) {
            time += Time.deltaTime;
            if (time >= tenSecondDelay) {
                time = 0.0f;
                speed *= 1.2f;
            }
            if (Player)
            {
                if (LevelManager.Instance.Immortal)
                {
                    if(speed!=0f) lastSpeed = speed;
                    speed = 0f;
                    rb2d.velocity = Vector2.zero;
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
                    Debug.Log(speed);
                    rb2d.velocity = new Vector2(0, speed);
                }
            }
        }
        if (LevelManager.Instance.GameOver||!LevelManager.Instance.IsGame)
        {
            rb2d.velocity = Vector2.zero;
        }*/
    }
}
