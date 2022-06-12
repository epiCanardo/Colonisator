using ColanderSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Colfront.GamePlay
{
    public abstract class UIBoard : MonoBehaviour, IPointerDownHandler
    {        
        // sur le clic, passe en premier plan
        public void OnPointerDown(PointerEventData eventData)
        {
            transform.SetAsLastSibling();
        }
    }
}