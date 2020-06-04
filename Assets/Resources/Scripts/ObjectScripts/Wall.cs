using UnityEngine;
public class Wall : MonoBehaviour
{
    private void Start()
    {
        if (gameObject.name.Contains("Brick"))
            GetComponent<Health>().SetLife(2);
        else if (gameObject.name.Contains("Iron"))
            GetComponent<Health>().SetLife(3);
    }
}
