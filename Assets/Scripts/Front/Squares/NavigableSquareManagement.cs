using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.Squares
{
    public class NavigableSquareManagement : SquareManagement
    {     
        protected override bool navigable => true;

        void OnMouseDown()
        {
            //// si on est en mode navigation
            //if (gameManager.IsNavigationModeActive())
            //{
            //    gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            //    navMode.SquareActivation(true);
            //}
            SquareSelection();
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

    }
}
