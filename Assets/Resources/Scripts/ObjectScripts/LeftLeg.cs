using UnityEngine;
public class LeftLeg : MonoBehaviour
{
    private void Start()
    {
        if (gameObject.name.Contains("Lightning"))
            GetComponent<Health>().SetLife(2);
        
    }
}
