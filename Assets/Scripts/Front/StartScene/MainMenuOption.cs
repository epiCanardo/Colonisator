using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.StartScene
{
    public class MainMenuOption : UnityEngine.MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public TextMeshProUGUI text;
        public ItemType type;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (type != ItemType.None)
                text.DOFaceColor(Color.red, 1);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (type != ItemType.None)
                text.DOFaceColor(Color.white, 1);
        }

        private void OnMouseOver()
        {
            if (type != ItemType.None)
                text.DOScale(2, 2).SetEase(Ease.InBounce);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (type)
            {
                case ItemType.NewGame:
                    //SceneManager.UnloadSceneAsync(0);
                    //SceneManager.LoadScene(1);
                    StartSceneManager.Instance.CurrentState = MenuState.NewGameMenu;
                    StartSceneManager.Instance.PrevisousState = MenuState.MainMenu;
                    StartSceneManager.Instance.ShowMenu();
                    FlagsCreationManager.Instance.ShowBaseFlag();
                    break;
                case ItemType.Load:
                    break;
                case ItemType.Options:
                    break;
                case ItemType.Quit:
                    Application.Quit();
                    break;
                case ItemType.Back:
                    var previsousState = StartSceneManager.Instance.PrevisousState;
                    StartSceneManager.Instance.PrevisousState = StartSceneManager.Instance.CurrentState;
                    StartSceneManager.Instance.CurrentState = previsousState;
                    StartSceneManager.Instance.ShowMenu();
                    break;
                default:
                    break;
            }
        }

        public enum ItemType
        {
            NewGame,
            Load,
            Options,
            Quit,
            Back,
            ValidateFlag,
            PrevisousBase,
            NextBase,
            None
        }
    }
}