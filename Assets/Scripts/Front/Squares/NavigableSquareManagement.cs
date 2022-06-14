using System.Linq;
using Assets.Scripts.Front.MainManagers;
using Assets.Store.QuickOutline.Scripts;
using UnityEngine;

namespace Assets.Scripts.Front.Squares
{
    public class NavigableSquareManagement : SquareManagement
    {
        private Vector3 scale;
        private NavigationModeManager navMode;
        private Outline outline;

        // Start is called before the first frame update
        void Start()
        {
            //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            navMode = GameObject.Find("NavigationModeManager").GetComponent<NavigationModeManager>();
            outline = gameObject.GetComponent<Outline>();
            outline.OutlineColor = Color.yellow;
            scale = gameObject.transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.IsNavigationModeActive())
            {
                // clic gauche : sélection de la case
                if (navMode.squaresRemaning > 0 && Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider == gameObject.GetComponent<Collider>())
                        {
                            var square = gameObject.GetComponent<NavigableSquareManagement>().coordinates;
                            if (navMode.NextPossibleSquare().Contains(square))
                            {
                                navMode.SquareActivation(true, square);
                                //gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                                outline.OutlineColor = Color.green;
                                outline.OutlineWidth = 2f;
                            }
                        }
                    }
                }
                // clic droit : désactivation de la sélection
                else if (Input.GetMouseButtonDown(2) /*&& Input.GetKeyDown(KeyCode.LeftControl)*/)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider == gameObject.GetComponent<Collider>())
                        {
                            var square = gameObject.GetComponent<NavigableSquareManagement>().coordinates;
                            //var material = gameObject.GetComponent<MeshRenderer>().material;
                            var outline = gameObject.GetComponent<Outline>();
                            //if (material.color == Color.blue)
                            if (outline.OutlineColor == Color.green)
                            {
                               // material.color = Color.black;
                                navMode.SquareActivation(false, square);
                                outline.OutlineColor = Color.yellow;
                                outline.OutlineWidth = 1f;
                            }
                        }
                    }
                }
                else
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider == gameObject.GetComponent<Collider>())
                        {
                            // survol : augmentation de la visiblité
                            //gameObject.transform.localScale = 3 * scale;
                            outline.OutlineWidth = 5f;
                        }
                    }
                }
            }
            else
            {
                // clic droit sur une case pour les options rapides
                if (Input.GetMouseButtonDown(1))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider == gameObject.GetComponent<Collider>())
                        {
                            var ship = GameManager.Instance.GetActualPlayinghipObject;
                            ship.transform.position = transform.position + (Vector3.down * 10);
                            ship.GetComponent<ShipManager>().ship.coordinates = coordinates;
                        }
                    }
                }
            }
        }

        void OnMouseDown()
        {
            //// si on est en mode navigation
            //if (gameManager.IsNavigationModeActive())
            //{
            //    gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            //    navMode.SquareActivation(true);
            //}
        }

        void OnMouseOver()
        {
            //// si on est en mode navigation
            //if (gameManager.IsNavigationModeActive())
            //{
            //    // survol : augmentation de la visiblité
            //    gameObject.transform.localScale = 3 * scale;

            //    // clic droit : désactivation de la sélection
            //    if (Input.GetMouseButton(1))
            //    {
            //        var material = gameObject.GetComponent<MeshRenderer>().material;
            //        if (material.color == Color.blue)
            //        {
            //            material.color = Color.white;
            //            navMode.SquareActivation(false);
            //        }
            //    }
            //}

            // Debug.Log($"({coordinates.x}, {coordinates.y})");
        }

        private void OnMouseExit()
        {
            // si on est en mode navigation
            if (GameManager.Instance.IsNavigationModeActive())
            {
                // retour à la visibilité normale
                //gameObject.transform.localScale = scale;
                if (outline.OutlineColor == Color.yellow)
                    outline.OutlineWidth = 1f;
            }
        }
    }
}
