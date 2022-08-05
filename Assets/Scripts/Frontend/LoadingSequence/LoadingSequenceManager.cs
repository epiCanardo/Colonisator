using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Front.QuickActions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Front.LoadingSequence
{

    public class LoadingSequenceManager : UnityEngine.MonoBehaviour
    {
        public TextMeshProUGUI mainText;
        public Image fadeimage;
        public List<RectTransform> panelsToShow;
        private PopulateWorld populateWorld;

        // Start is called before the first frame update
        void Start()
        {
            populateWorld = FindObjectOfType<PopulateWorld>(true);
            
            mainText.text = "Création du monde...";
            StartCoroutine("PopulateWorldTask");
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator PopulateWorldTask()
        {
            populateWorld.GenerateMap();
            yield return new WaitForSeconds(10);
            populateWorld.PopulateWorldTask();
            mainText.text = "Recutement des matelots...";
            yield return new WaitForSeconds(5);
            mainText.text = string.Empty;
            fadeimage.DOFade(0, 3);
            yield return new WaitForSeconds(3);
            gameObject.SetActive(false);

            foreach (var panel in panelsToShow)
            {
                panel.gameObject.SetActive(true);
            }
        }
    }
}
