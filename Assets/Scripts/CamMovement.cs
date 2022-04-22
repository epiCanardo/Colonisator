using UnityEngine;

namespace Colfront.GamePlay
{

    public class CamMovement : MonoBehaviour
    {
        [SerializeField] float camspeed = 1500;
        [SerializeField] float zommspeed = 20;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }


        private void FixedUpdate()
        {
            var hMovement = Input.GetAxis("Horizontal");
            var vMovement = Input.GetAxis("Vertical");
            var zoom = 0;

            if (Input.GetKey(KeyCode.PageDown))
                zoom = -1;
            else if (Input.GetKey(KeyCode.PageUp))
                zoom = 1;


            transform.Translate(hMovement * Time.deltaTime * camspeed, 0, Time.deltaTime * zoom * zommspeed);
            transform.Translate(0, vMovement * Time.deltaTime * camspeed, Time.deltaTime * zoom * zommspeed);
        }
    }
}