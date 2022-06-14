using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Front.MainManagers
{
    public class TechnicCountersManager : UnityEngine.MonoBehaviour
    {
        public TextMeshProUGUI fpsCounter;
    
        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating("GetFPS", 1, 1);
        }

        public void GetFPS()
        {
            var fps = Convert.ToInt32(1f / Time.unscaledDeltaTime);
            fpsCounter.text = $"FPS : {fps}";
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
