using ColanderSource;
using UnityEngine;

namespace Colfront.GamePlay
{
    public abstract class SquareManagement : MonoBehaviour
    {
        public Square coordinates;
        //protected GameManager gameManager;

        // Start is called before the first frame update
        void Start()
        {
            //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            coordinates = new Square(0, 0);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void RAZNavigationMode()
        {
            //var material = gameObject.GetComponent<MeshRenderer>().material;
            //material.color = Color.black;

            var outline = gameObject.GetComponent<Outline>();
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 1f;
        }
    }
}
