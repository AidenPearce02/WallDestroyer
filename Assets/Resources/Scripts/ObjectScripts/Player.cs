using UnityEngine;

public class Player : MonoBehaviour
{
    private Effect effect;
    private void Start()
    {
        effect = GetComponent<Effect>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Roof"))
        {
            if (effect.CurrentEffect == Effect.EffectType.YELLOW)
            {
                effect.Default();
                LevelManager.Instance.SetImmortal(true);
                LevelManager.Instance.SetDurationImmortal(3);
            }
            else
            {
                LevelManager.Instance.EndGame();
            }
        }
            
    }
}
