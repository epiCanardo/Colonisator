using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using Assets.Scripts.Service;
using DG.Tweening;
using TMPro;
using UnityEngine;
using static Assets.Scripts.ModsDTO.MapDefinitionDTO;

namespace Assets.Scripts.Front.MainManagers
{
    public class ShipManager : UnityEngine.MonoBehaviour
    {
        public Ship ship { get; set; }
        public GameObject CaptainSpot;
        public GameObject FlagLeftSpot;
        public GameObject FlagRightSpot;
        public GameObject FlagLeftTextile;
        public GameObject FlagRightTextile;
        public Shader FlagShader;
        public TextMeshProUGUI ActionText; // texte des actions / intentions du navire
        public TextMeshProUGUI CardConsequencesText; // texte des conséquences de cartes
        
        [Header("Pour le calcul des couleurs des voiles")]
        public List<GameObject> SailsToColor;

        [Header("Pour le calcul des couleurs de la coque")]
        public GameObject HullToColor;

        [Header("Pour le calcul des couleurs de la minimap")]
        public SpriteRenderer MiniMapRenderer;
        //public GameObject MinimapSprite;

        public GameObject selectionCircle;

        [SerializeField]
        private Vector3 iconForward;

        private bool isSwinging = true;
        private Vector3 cardConsequencesPosition;

        public void AssignCrew()
        {
            NpcsManager.Instance.InstanciateNPC(NpcType.Captain, CaptainSpot, gameObject);
        }

        public void AssignColors()
        {
            Faction faction = ServiceGame.GetFactionFromId(ship.owner);
            FactionManager factionManager = FactionsManager.Instance.GetFactionManager(faction);

            MeshRenderer rend = FlagLeftTextile.GetComponent<MeshRenderer>();
            rend.material = new Material(FlagShader);
            rend.material.SetTexture("flagTexture", factionManager.Flag);
            rend.material.SetFloat("amplitude", Random.Range(3, 8));

            rend = FlagRightTextile.GetComponent<MeshRenderer>();
            rend.material = new Material(FlagShader);
            rend.material.SetTexture("flagTexture", factionManager.Flag);
            rend.material.SetFloat("amplitude", Random.Range(3, 8));

            // couleurs du navire
            var colors = factionManager.Colors;

            MeshRenderer hullMesh;
            hullMesh = HullToColor.GetComponent<MeshRenderer>();
            hullMesh.material = new Material(hullMesh.material);
            hullMesh.material.color = colors[0]; // la teinte de la coque est donnée par la première couleur du drapeau

            MeshRenderer sailMesh;
            for (int i = 0; i < colors.Count; i++)
            {
                sailMesh = SailsToColor[i].GetComponent<MeshRenderer>();
                sailMesh.material = new Material(sailMesh.material);
                sailMesh.material.color = colors[0]; // la teinte des voiles est donnée par la première couleur du joueur
            }

            // couleur du sprite pour la minimap            
            MiniMapRenderer.sprite = Resources.Load<Sprite>($"Textures/Icons/Ships/{factionManager.MainColor}");
            MiniMapRenderer.color = colors[0];

            // couleur de l'anneau de sélection
            MeshRenderer ringMesh = selectionCircle.GetComponent<MeshRenderer>();
            ringMesh.material = new Material(ringMesh.material);
            ringMesh.material.color = colors[0]; // la teinte des l'anneau est donné par la couleur du joueur
            ringMesh.material.SetColor("_EmissionColor", colors[0]);
        }

        public void Move(Vector3 direction)
        {
            transform.Translate(direction);
            RotateShipIcon();
        }

        public void RotateShipIcon()
        {
            MiniMapRenderer.transform.forward = iconForward;
        }
       

        private void OnMouseDown()
        {
            SelectShip();
        }

        private void Start()
        {
            iconForward = MiniMapRenderer.transform.forward;
            
            // if (selectionCircle != null)
            //    selectionCircle.transform.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetRelative(true).SetEase(Ease.Linear);
        }

        public void SelectShip()
        {           
            Faction faction = ServiceGame.GetFactionFromId(ship.owner);
            FactionManager factionManager = FactionsManager.Instance.GetFactionManager(faction);
            GameManager.Instance.SetInfoPanelBorderColor(factionManager.Colors[0]);
            GameManager.Instance.SetInfoPanelFlag(factionManager.Flag);
            GameManager.Instance.SetInfoPanelTitle(ship);
        }

        /// <summary>
        /// affiche du texte d'action en cours du navire
        /// </summary>
        /// <param name="text"></param>
        public void PrintActionText(string text)
        { 
            ActionText.text = text;
            ActionText.transform.SetAsLastSibling();
        }

        public IEnumerator PrintCardConsequencesText(string text)
        {
            cardConsequencesPosition = CardConsequencesText.transform.position;

            CardConsequencesText.text = text;
            ActionText.transform.SetAsLastSibling();
            yield return CardConsequencesText.transform.DOMoveY(150, 3).WaitForCompletion();

            // retour à la position de départ
            CardConsequencesText.text = string.Empty;
            CardConsequencesText.transform.position = cardConsequencesPosition;
        }
    }
}
