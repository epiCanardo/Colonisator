using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Front.Paths;
using Assets.Scripts.Model;
using Assets.Scripts.Service;
using UnityEngine;

//using UnityEditor.Animations;

namespace Assets.Scripts.Front.Squares
{
    public class HarborSquareManagement : SquareManagement
    {
        protected override bool navigable => true;

        [Header("Prefab pnj")]
        public List<GameObject> npcsPrefabs;
        public List<Material> npcsMaterials;
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

                // modification de la couleur du material en fonction de la couleur de la faction
                // todo : faire !!
                Material material = new Material(npcsMaterials[Random.Range(0, npcsMaterials.Count)]);           
                instance.GetComponentInChildren<SkinnedMeshRenderer>(false).material = material;

                // ajout du path pour le mouvement par défaut des npc
                if (defaultPath != null)
                {
                    var path = instance.AddComponent<PathManager>();
                    path.GlobalPath = defaultPath;
                }                
            }
        }

        public void SetHarborSquareColor()
        {
            // affectation de la couleur du spriterenderer en fonction de la faction
            Faction faction = ServiceGame.GetFactionFromId(island.owner);
            if (faction != null)
                SetSquareColor(FactionsManager.Instance.GetFactionManager(faction).Colors[0]);
            else
                SetSquareColor(ColorTools.NameToColor("silver"));
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
