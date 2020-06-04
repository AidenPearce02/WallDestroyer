using UnityEngine;
public class RightLeg : MonoBehaviour
{
    private void Start()
    {
        if (gameObject.name.Contains("Lightning"))
            GetComponent<Health>().SetLife(2);
    }
}
