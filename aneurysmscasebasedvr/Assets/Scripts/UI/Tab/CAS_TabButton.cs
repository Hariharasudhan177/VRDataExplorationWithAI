using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; 

namespace CAS
{
    [RequireComponent(typeof(Image))]
    public class CAS_TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public CAS_TabGroup tabGroup;
        public Button tabButton; 

        public Image background;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(tabButton.interactable) tabGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tabButton.interactable)  tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tabButton.interactable) tabGroup.OnTabExit(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            tabButton = GetComponent<Button>();
            background = GetComponent<Image>();
            tabGroup.Subcribe(this);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
