using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public enum EffectType
    {
        NONE,
        YELLOW,
        GREEN,
        RED,
        PURPLE,
        BLUE,
        BLACK,
        RAINBOW
    }
    private float time = 0.0f;
    public int Duration { get; private set; } = 0;
    public EffectType CurrentEffect { get; private set; } = EffectType.NONE;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentEffect)
        {
            case EffectType.NONE:
                GetComponent<Renderer>().material.color = Color.white;
                break;
            case EffectType.YELLOW:
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case EffectType.GREEN:
                GetComponent<Renderer>().material.color = Color.green;
                break;
            case EffectType.RED:
                GetComponent<Renderer>().material.color = Color.red;
                break;
            case EffectType.PURPLE:
                GetComponent<Renderer>().material.color = new Color(0.5f, 0.0f, 0.75f, 1.0f);
                break;
            case EffectType.BLUE:
                GetComponent<Renderer>().material.color = Color.blue;
                break;
            case EffectType.BLACK:
                GetComponent<Renderer>().material.color = Color.black;
                break;
            case EffectType.RAINBOW:
                GetComponent<Renderer>().material.color = new Color(0.3f, 0.0f, 0.51f, 1.0f);
                break;
        }
        if (CurrentEffect != EffectType.NONE)
        {
            if(CurrentEffect != EffectType.RED && CurrentEffect != EffectType.YELLOW && Duration != 0)
            {
                time += Time.deltaTime;
                if (time >= Duration)
                {
                    time = 0.0f;
                    Duration = 0;
                    Default();
                }
            }
            
        }
    }

    public void Default()
    {
        if (CurrentEffect == EffectType.GREEN) LevelManager.Instance.timeGreenBooster = 0.0f;
        if (CurrentEffect == EffectType.RED) LevelManager.Instance.timeRedBooster = 0.0f;
        CurrentEffect = EffectType.NONE;
        LevelManager.Instance.SetIsEffectedWall(false);
    }

    public void SetCurrentEffect(EffectType effectType) {
        if (gameObject.GetComponent<Player>() && PerkManager.Instance.checkPerk("Perk 8", 3)) LevelManager.Instance.AddMoney(10);
        CurrentEffect = effectType;
    }

    public void SetDuration(int duration) {
        Duration = duration;
    }
}
