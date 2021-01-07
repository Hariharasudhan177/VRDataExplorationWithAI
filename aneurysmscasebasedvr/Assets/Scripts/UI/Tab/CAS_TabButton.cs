using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CAS
{
    [RequireComponent(typeof(Image))]
    public class CAS_TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public CAS_TabGroup tabGroup;

        public Image background;

        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            background = GetComponent<Image>();
            tabGroup.Subcribe(this);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
