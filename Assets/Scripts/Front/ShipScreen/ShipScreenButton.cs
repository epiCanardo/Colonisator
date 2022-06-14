using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Front.ShipScreen
{
    public abstract class ShipScreenButton : UnityEngine.MonoBehaviour
    {
        protected Button button;
        protected ShipScreenManager shipScreenManager;

        // Start is called before the first frame update
        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(DoSomethingOnClick);

            shipScreenManager = GameObject.Find("ShipScreen").GetComponent<ShipScreenManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        protected abstract void DoSomethingOnClick();
    }
}
