using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using Assets.Scripts.Service;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Front.MainManagers
{
    public class ShipManager : UnityEngine.MonoBehaviour
    {
        public Ship ship;
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

        private bool isSwinging = true;
        private Vector3 cardConsequencesPosition;

        public void AssignCrew()
        {
            NpcsManager.Instance.InstanciateNPC(NpcType.Captain, CaptainSpot, gameObject);
        }

        public void AssignColors()
        {
            Faction faction = ServiceGame.GetFactionFromId(ship.owner);
            FactionManager factionManager = FactionsManager.Instance.Factions.First(x => x.Faction.Equals(faction));

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
                sailMesh.material.color = factionManager.Colors[0]; // la teinte des voiles est donnée par la couleur du joueur
            }

            // couleur du sprite pour la minimap            
            MiniMapRenderer.sprite = Resources.Load<Sprite>($"Textures/Icons/Ships/{factionManager.MainColor}");

            // couleur de l'anneau de sélection
            MeshRenderer ringMesh = selectionCircle.GetComponent<MeshRenderer>();
            ringMesh.material = new Material(ringMesh.material);
            ringMesh.material.color = factionManager.Colors[0]; // la teinte des l'anneau est donné par la couleur du joueur

            //MinimapSprite.GetComponent<SpriteRenderer>().color = new Color32(colors[0].r, colors[0].g, colors[0].b, 255);

            //var fullPixels = MiniMapRenderer.sprite.texture.GetPixels();

            //// couleur du map renderer
            //for (int i = 0; i < fullPixels.Length; i++)
            //{
            //    if (fullPixels[i].a > 0)
            //        fullPixels[i] = Color.gray; //colors[1];
            //}
            //Texture2D text = new Texture2D((int)MiniMapRenderer.sprite.rect.width, (int)MiniMapRenderer.sprite.rect.height);
            ////text.SetPixels32(fullPixels);
            //text.SetPixels(fullPixels);
            //text.Apply();
            //text.name = "test";
            //var bytes = text.EncodeToPNG();
            //File.WriteAllBytes("Assets/Textures/Icones/text.png", bytes);

            //Sprite newSprite = Sprite.Create(text, MiniMapRenderer.sprite.rect, MiniMapRenderer.sprite.pivot);
            //newSprite.name = "test";
            //MiniMapRenderer.sprite = newSprite;            
        }

        public void Move(Vector3 direction)
        {
            transform.Translate(direction);
        }

        private void OnMouseDown()
        {
            SelectShip();
        }

        private void Start()
        {
            if (selectionCircle != null)
                selectionCircle.transform.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetRelative(true).SetEase(Ease.Linear);
        }

        private void Update()
        {
            //MiniMapRenderer.transform.eulerAngles = new Vector3(90, 0, 0);
        }

        public void SelectShip()
        {           
            Faction faction = ServiceGame.GetFactionFromId(ship.owner);
            FactionManager factionManager = FactionsManager.Instance.Factions.First(x => x.Faction.Equals(faction));
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
