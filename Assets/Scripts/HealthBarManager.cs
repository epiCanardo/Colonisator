using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.WorldToScreenPoint(transform.position));
    }
}
