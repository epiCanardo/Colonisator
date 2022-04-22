using UnityEngine;

namespace Assets.Scripts.CombatScene
{
    class EnnemyShipManager : MonoBehaviour
    {
        public ParticleSystem WoodImpact;

        AudioSource audioSource;

        private void Start()
        { 
            audioSource = GetComponent<AudioSource>();            
        }

        private void Update()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("CannonBall"))
            {
                double rnd = CombatManager.Instance.NextDouble;

                // ça ne touche pas
                if (rnd < 0.2)
                    audioSource.PlayOneShot(CombatManager.Instance.flybySounds[CombatManager.Instance.NextInt(0, 10)]);
                else
                {
                    // ça touche + destruction
                    audioSource.PlayOneShot(CombatManager.Instance.hittingSounds[CombatManager.Instance.NextInt(0, 5)]);
                    Destroy(other.gameObject);
                    WoodImpact.Play();
                }
            }
        }
    }
}
