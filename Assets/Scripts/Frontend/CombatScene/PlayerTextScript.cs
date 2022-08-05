using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Front.CombatScene
{
    public class PlayerTextScript : UnityEngine.MonoBehaviour
    {
        //public Slider bar;

        [Header("Canvas")]
        public GameObject HealthBarPreab;

        private Slider bar;

        // Start is called before the first frame update
        void Start()
        {
            var HealthBar = Instantiate(HealthBarPreab, transform.position, HealthBarPreab.transform.rotation, transform);
            bar = HealthBar.transform.Find("Slider").GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            var screen = Camera.main.WorldToScreenPoint(transform.position);
            bar.transform.position = new Vector3(screen.x, screen.y, 0);
            bar.transform.LookAt(Camera.main.WorldToScreenPoint(transform.position));
        }
    }
}