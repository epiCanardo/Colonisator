using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Model;
using Assets.Store.QuickOutline.Scripts;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Front.Squares
{
    public abstract class SquareManagement : UnityEngine.MonoBehaviour
    {
        public Square coordinates { get; set; }
        protected abstract bool navigable { get; }
        public bool IsNavigable => navigable;

        private NavigationModeManager navMode;
        private Outline outline;
        private Vector3 scale;
        private Color outLineColor = Color.yellow;

        // Start is called before the first frame update
        void Start()
        {
            //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            //coordinates = new Square(0, 0);

            navMode = NavigationModeManager.Instance;
            outline = gameObject.GetComponent<Outline>();
            outline.OutlineColor = outLineColor;
            scale = gameObject.transform.localScale;
        }

        public void SetOutlineColor(Color color)
        {
            outLineColor = color;
        }

        private void OnMouseUpAsButton()
        {
            var squareManager = gameObject.GetComponent<SquareManagement>();
            GameManager.Instance.coordinatesTextObject.text = $"Coordonnées : {squareManager.coordinates.ToString()}";

            using (StreamWriter sW = new StreamWriter(@"D:\Unity\Colonisator\Mods\Core\Values\Gameplay\draft.json",true))
            {
                sW.Write($"[{squareManager.coordinates.ToString()}],");
                sW.Close();
            }

            // clic gauche : sélection de la case
            if (navMode.squaresRemaning > 0)
            {                
                if (squareManager.IsNavigable)
                {
                    var square = squareManager.coordinates;
                    if (navMode.NextPossibleSquare().Contains(square))
                    {
                        navMode.SquareActivation(true, square);
                        outline.OutlineColor = Color.green;
                        outline.OutlineWidth = 2f;
                    }
                }
            }
        }

        // Update is called once per frame
        //void Update()
        //{
        //    if (IsNavigable && GameManager.Instance.IsNavigationModeActive())
        //    {
        //        // clic gauche : sélection de la case
        //        if (navMode.squaresRemaning > 0 && Input.GetMouseButtonDown(0))
        //        {
        //            RaycastHit hit;
        //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //            if (Physics.Raycast(ray, out hit))
        //            {
        //                if (hit.collider == gameObject.GetComponent<Collider>())
        //                {
        //                    var squareManager = gameObject.GetComponent<SquareManagement>();
        //                    if (squareManager.IsNavigable)
        //                    {
        //                        var square = squareManager.coordinates;
        //                        if (navMode.NextPossibleSquare().Contains(square))
        //                        {
        //                            navMode.SquareActivation(true, square);
        //                            outline.OutlineColor = Color.green;
        //                            outline.OutlineWidth = 2f;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        // clic droit : désactivation de la sélection
        //        else if (Input.GetMouseButtonDown(2) /*&& Input.GetKeyDown(KeyCode.LeftControl)*/)
        //        {
        //            RaycastHit hit;
        //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //            if (Physics.Raycast(ray, out hit))
        //            {
        //                if (hit.collider == gameObject.GetComponent<Collider>())
        //                {
        //                    var square = gameObject.GetComponent<NavigableSquareManagement>().coordinates;
        //                    //var material = gameObject.GetComponent<MeshRenderer>().material;
        //                    var outline = gameObject.GetComponent<Outline>();
        //                    //if (material.color == Color.blue)
        //                    if (outline.OutlineColor == Color.green)
        //                    {
        //                        // material.color = Color.black;
        //                        navMode.SquareActivation(false, square);
        //                        outline.OutlineColor = Color.yellow;
        //                        outline.OutlineWidth = 1f;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            RaycastHit hit;
        //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //            if (Physics.Raycast(ray, out hit))
        //            {
        //                if (hit.collider == gameObject.GetComponent<Collider>())
        //                {
        //                    // survol : augmentation de la visiblité
        //                    //gameObject.transform.localScale = 3 * scale;
        //                    outline.OutlineWidth = 5f;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // clic droit sur une case pour les options rapides
        //        if (Input.GetMouseButtonDown(1))
        //        {
        //            RaycastHit hit;
        //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //            if (Physics.Raycast(ray, out hit))
        //            {
        //                if (hit.collider == gameObject.GetComponent<Collider>())
        //                {
        //                    var ship = GameManager.Instance.GetActualPlayinghipObject;
        //                    ship.transform.position = transform.position + (Vector3.down * 10);
        //                    ship.GetComponent<ShipManager>().ship.coordinates = coordinates;
        //                }
        //            }
        //        }
        //    }
        //}
        public void RAZNavigationMode()
        {
            //var material = gameObject.GetComponent<MeshRenderer>().material;
            //material.color = Color.black;

            var outline = gameObject.GetComponent<Outline>();
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 1f;
        }

        private void OnMouseExit()
        {
            // si on est en mode navigation
            if (GameManager.Instance.IsNavigationModeActive())
            {
                // retour à la visibilité normale                
                if (outline != null && outline.OutlineColor == Color.yellow)
                    outline.OutlineWidth = 1f;
            }
        }

        //void OnMouseOver()
        //{
        //    var squareManager = gameObject.GetComponent<SquareManagement>();
        //    GameManager.Instance.coordinatesTextObject.text = $"Coordonnées : {squareManager.coordinates.ToString()}";
        //}
    }
}
