using UnityEngine;
using ColanderSource;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using System.Linq;

namespace Colfront.GamePlay
{
    public class ShipManager : MonoBehaviour
    {
        public Ship ship;
        public GameObject CaptainSpot;
        public GameObject FlagLeftSpot;
        public GameObject FlagRightSpot;
        public GameObject FlagLeftTextile;
        public GameObject FlagRightTextile;
        public Shader FlagShader;
        public TextMeshProUGUI ActionText;

        [Header("Pour le calcul des couleurs des voiles")]
        public List<GameObject> SailsToColor;

        [Header("Pour le calcul des couleurs de la coque")]
        public GameObject HullToColor;

        [Header("Pour le calcul des couleurs de la minimap")]
        public GameObject MinimapSprite;

        private bool isSwinging = true;

        // Start is called before the first frame update
        void Start()
        {
            //StartSwing(2);
        }

        public void StartSwing(float halfArc)
        {
            if (isSwinging)
                transform.DORotate(new Vector3(0, 0, halfArc), 3f).SetEase(Ease.InOutFlash)
                    .OnComplete(() => StartSwing(-halfArc));
        }

        public void PauseSwing()
        {
            isSwinging = false;
        }

        public void ResumeSwing()
        {
            isSwinging = true;
            StartSwing(2);
        }

        public void AssignCrew()
        {
            NpcsManager.Instance.InstanciateNPC(NpcType.Captain, CaptainSpot, gameObject);
        }

        public void AssignFlag()
        {
            Faction faction = ServiceGame.GetFactionFromId(ship.owner);

            MeshRenderer rend = FlagLeftTextile.GetComponent<MeshRenderer>();
            rend.material = new Material(FlagShader);
            rend.material.SetTexture("flagTexture", FactionsManager.Instance.Factions.First(x => x.Faction.Equals(faction)).Flag);
            rend.material.SetFloat("amplitude", Random.Range(3, 8));

            rend = FlagRightTextile.GetComponent<MeshRenderer>();
            rend.material = new Material(FlagShader);
            rend.material.SetTexture("flagTexture", FactionsManager.Instance.Factions.First(x => x.Faction.Equals(faction)).Flag);
            rend.material.SetFloat("amplitude", Random.Range(3, 8));

            // couleurs du navire
            var colors = FlagsManager.Instance.GetMainColorsFromTexture(FactionsManager.Instance.Factions.First(x => x.Faction.Equals(faction)).Flag);

            MeshRenderer hullMesh;
            hullMesh = HullToColor.GetComponent<MeshRenderer>();
            hullMesh.material = new Material(hullMesh.material);
            hullMesh.material.color = colors[0];

            MeshRenderer sailMesh;
            for (int i = 0; i < colors.Count; i++)
            {
                sailMesh = SailsToColor[i].GetComponent<MeshRenderer>();
                sailMesh.material = new Material(sailMesh.material);
                sailMesh.material.color = colors[1];
            }

            MinimapSprite.GetComponent<SpriteRenderer>().color = new Color32(colors[0].r, colors[0].g, colors[0].b, 255);
        }

        public void Move(Vector3 direction)
        {
            transform.Translate(direction);
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnMouseDown()
        {
            SelectShip();
            //GameManager.Instance.ToggleShipScreen(true, ship);
        }

        public void SelectShip()
        {           

            Faction faction = ServiceGame.GetFactionFromId(ship.owner);
            var test = FactionsManager.Instance.Factions.First(x => x.Faction.Equals(faction));
            GameManager.Instance.SetInfoPanelFlag(test.Flag);

            GameManager.Instance.SetInfoPanelTitle(ship);
        }

        /// <summary>
        /// affiche du texte d'action en cours du navire
        /// </summary>
        /// <param name="text"></param>
        public void PrintActionText(string text)
        { 
            ActionText.text = text;
        }
    }
}
