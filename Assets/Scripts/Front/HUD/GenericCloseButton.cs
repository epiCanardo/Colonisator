using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Front.ScriptableObjects.Ancestors;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.HUD
{
    public class GenericCloseButton : UnityEngine.MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public TextMeshProUGUI text;
        public GameObject objectToClose;
        private Color color;

        // Start is called before the first frame update
        void Start()
        {
            //var button = GetComponent<Button>();
            //text = button.GetComponent<TextMeshProUGUI>();
            color = text.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            text.color = Color.red;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            text.color = color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
           //text.color = color;
            //objectToClose.SetActive(false);
            //Destroy(objectToClose);
           // GameManager.Instance.CurrentOpenedBoard -= 1;
            MenusManager.Instance.TryDestroyMenu(objectToClose.GetComponent<UIBoard>().key);
        }
    }
}
