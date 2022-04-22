using System.Collections;
using UnityEngine;

namespace Assets.Scripts.CombatScene
{
    class CannonBallManager : MonoBehaviour
    {
        private Rigidbody rb;

        private TrailRenderer trail;
        private bool isTrailActive;

        public ParticleSystem OceanImpact;
        public ParticleSystem WoodImpact;

        AudioSource audioSource;

        private void Awake()
        {           
            audioSource = GetComponent<AudioSource>();

            rb = GetComponent<Rigidbody>();
            trail = GetComponent<TrailRenderer>();
        }

        private void Start()
        {

        }

        public void Fire(float force, float elevation, bool withTrail)
        {
            rb.AddRelativeForce(Vector3.left * force + ((elevation * Vector3.up) * force), ForceMode.Impulse);
            isTrailActive = withTrail;
        }

        private void Update()
        {
            if (transform.position.y <= -30f /*&& !OceanImpact.isPlaying*/)
                Destroy(gameObject);

            // if (transform.position.y <= 1f /*&& !OceanImpact.isPlaying*/)
            // StartCoroutine(FallinOcean());

            if (isTrailActive)
            {
                trail.emitting = true;
            }

        }

        IEnumerator FallinOcean()
        {
            audioSource.PlayOneShot(CombatManager.Instance.waterHitSounds[CombatManager.Instance.NextInt(0, 5)]);
            OceanImpact.Play();
            yield return null;
        }

        private void OnTriggerEnter(Collider other)
        {
            //if (other.tag.Equals("EnnemyShip"))
            //{
            //    double rnd = pm.rnd.NextDouble();

            //    // ça ne touche pas
            //    if (rnd < 0.5)
            //        audioSource.PlayOneShot(pm.flybySounds[pm.rnd.Next(0, 10)]);
            //    else
            //    {
            //        // ça touche + destruction
            //        audioSource.PlayOneShot(pm.hittingSounds[pm.rnd.Next(0, 5)]);
            //        WoodImpact.Play();
            //        //Destroy(gameObject);
            //    }
            //}
        }
    }
}
