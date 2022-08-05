using UnityEngine.UI;

namespace Assets.Scripts.Front.MainManagers
{
    public class MinimapManager : UnityEngine.MonoBehaviour
    {
        public RawImage miniMap;

        private static MinimapManager _instance;
        public static MinimapManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MinimapManager();
                }

                return _instance;
            }
        }

        private void Start()
        {
            //miniMap.texture = ModManager.Instance.GenerateMinimapColors();
        }

        //public void UpdateMiniMap(Texture2D texture)
        //{
        //    miniMap.GetComponent<RawImage>().texture = texture;
        //}
    }
}
