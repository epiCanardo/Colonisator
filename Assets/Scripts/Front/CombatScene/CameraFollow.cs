using UnityEngine;

namespace Assets.Scripts.Front.CombatScene
{
    class CameraFollow : UnityEngine.MonoBehaviour
    {
        public GameObject playerShip;
        public GameObject ennemyShip;

        private Vector3 offset = new Vector3(0f, 393f, -250f);

        private void Start()
        {
            
        }

        private void Update()
        {
            // calcul de la distance entre les deux navires
            float dist = Mathf.Max(400, Vector3.Distance(ennemyShip.transform.position, playerShip.transform.position));
            

            // la caméra doit être centrée entre les deux navires
            transform.position = (ennemyShip.transform.position + playerShip.transform.position) / 2 + Vector3.up * dist;

            //transform.position = playerShip.transform.position + offset;
        }
    }
}
