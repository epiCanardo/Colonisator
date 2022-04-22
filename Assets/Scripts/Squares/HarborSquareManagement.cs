using ColanderSource;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;

namespace Colfront.GamePlay
{
    public class HarborSquareManagement : SquareManagement
    {
        [Header("Prefab pnj")]
        public List<GameObject> npcsPrefabs;
        public List<Material> npcsMaterials;
        //public AnimatorController defaultController;
        public GameObject defaultPath;

        private bool toolTipActive = false;
        private Island island;

        public void SetIsland()
        {
            island = ServiceGame.GetIsland(coordinates);

            // parcours des pnj pour l'île et instanciation
            // en évitant de bousiller les prefab hein !!
            foreach (string npc in island.npcs)
            {                
                var npcToInstanciate = npcsPrefabs[Random.Range(0, npcsPrefabs.Count)];
                var instance = Instantiate(npcToInstanciate, transform.position, transform.rotation);

                instance.GetComponentInChildren<SkinnedMeshRenderer>(false).material = npcsMaterials[Random.Range(0, npcsMaterials.Count)];

                if (defaultPath != null)
                {
                    var path = instance.AddComponent<PathManager>();
                    path.GlobalPath = defaultPath;
                    //instance.GetComponent<Animator>().runtimeAnimatorController = defaultController;
                }

                
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        // Update is called once per frame
        void Update()
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

        private void OnMouseDown()
        {
            if (!toolTipActive)
            {
                GameManager.Instance.ToggleHarborTooltip(true, island);
                toolTipActive = true;
            }
        }

        void OnMouseOver()
        {
            //Debug.Log($"({coordinates.x}, {coordinates.y})");

            //if (!toolTipActive)
            //{
            //    // récup de l'island en fonction des coordonnées
            //    var island = gameManager.game.GetIsland(coordinates);

            //    gameManager.ToggleHarborTooltip(true, island);
            //    toolTipActive = true;                              
            //}
        }

        private void OnMouseExit()
        {
            StartCoroutine(TooltipDesactivation());  
        }

        IEnumerator TooltipDesactivation()
        {
            yield return new WaitForSeconds(1);
            GameManager.Instance.ToggleHarborTooltip(false);
            toolTipActive = false;
        }
    }
}
