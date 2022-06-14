using UnityEngine;

namespace Assets.Scripts.Front.HUD
{
    public class LookAtCam : UnityEngine.MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            //Vector3 v = Camera.main.transform.position - transform.position;
            //v.x = v.z = 0.0f;
            //transform.LookAt(Camera.main.transform.position - v);


            //transform.LookAt(Camera.main.transform);
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }
}
