using UnityEngine;
public class Health : MonoBehaviour
{
    public int Life { get; private set; } = 1;
    // Start is called before the first frame update

    public void SetLife(int life) {
        Life = life;
    }

    public void DropLife() {
        Life--;
    }
}
