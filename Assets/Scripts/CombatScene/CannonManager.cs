using UnityEngine;

namespace Assets.Scripts.CombatScene
{
    class CannonManager : MonoBehaviour
    {
        public GameObject cannonBallGO;
        public GameObject shotAnimation;
        public int Force;
        public GameObject maxScope;

        AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
        }

        public void FireBall()
        {            
            var orientation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 180, transform.rotation.eulerAngles.z);
            var cannonBall = Instantiate(cannonBallGO, transform.position + Vector3.forward * 3 + Vector3.up, orientation);
            cannonBall.GetComponent<CannonBallManager>().Fire(Force, 0.05f, false);

            // animation de tir
            if (shotAnimation != null)
                shotAnimation.GetComponent<ParticleSystem>().Play();
        }
    }
}
