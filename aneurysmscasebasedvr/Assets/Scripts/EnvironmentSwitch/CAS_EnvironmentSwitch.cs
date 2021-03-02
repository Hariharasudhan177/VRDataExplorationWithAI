using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

namespace CAS {
    public class CAS_EnvironmentSwitch : MonoBehaviour
    {
        //Skybox 
        //ButtonImage 
        //Environment 
        //MainPanel background 
        //InsidePanel background
        //Model material 

        private CAS_ButtonToSwitch[] buttons;
        private CAS_MainPanelToSwitch[] mainPanels;
        private CAS_InsidePanelToSwitch[] insidePanels;
        private CAS_TextToSwitch[] texts;
        private CAS_ScrollSwitch[] scrolls; 
        private CAS_ContolModel[] models;
        private CAS_SliderBackgroundToSwitch[] sliderBackgroundToSwitches;
        private CAS_SliderHandleToSwitch[] sliderHandleToSwitches; 

        //Normal 
        public Sprite buttonN;
        public Sprite mainPanelN;
        public Sprite insidePanelN;
        public Color textColorN;
        public Material modelMaterialN;
        public GameObject environmentN;
        public Color colorN;
        public Color sliderBackgroundN;
        public Color sliderHandleN;

        //Scifi
        public Sprite buttonS;
        public Sprite mainPanelS;
        public Sprite insidePanelS;
        public Color textColorS;
        public Material modelMaterialS;
        public GameObject environmentS;
        public Color colorS;
        public Color sliderBackgroundS;
        public Color sliderHandleS;

        // Start is called before the first frame update
        void Start()
        {
            buttons = FindObjectsOfType<CAS_ButtonToSwitch>();
            mainPanels = FindObjectsOfType<CAS_MainPanelToSwitch>();
            insidePanels = FindObjectsOfType<CAS_InsidePanelToSwitch>();
            texts = FindObjectsOfType<CAS_TextToSwitch>();
            models = FindObjectsOfType<CAS_ContolModel>();
            scrolls = FindObjectsOfType<CAS_ScrollSwitch>();
            sliderBackgroundToSwitches = FindObjectsOfType<CAS_SliderBackgroundToSwitch>();
            sliderHandleToSwitches = FindObjectsOfType<CAS_SliderHandleToSwitch>(); 
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SwitchToSciFiEnvironment(); 
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                SwitchToNormalEnvironment(); 
            }

        }

        public void SwitchToSciFiEnvironment()
        {
            Camera.main.clearFlags = CameraClearFlags.Skybox; 

            foreach (CAS_ButtonToSwitch buttonToSwitch in buttons)
            {
                buttonToSwitch.ChangeImage(buttonS); 
            }

            foreach (CAS_MainPanelToSwitch mainPanelToSwitch in mainPanels)
            {
                mainPanelToSwitch.ChangeImage(mainPanelS);
            }

            foreach (CAS_InsidePanelToSwitch insidePanelToSwitch in insidePanels)
            {
                insidePanelToSwitch.ChangeImage(insidePanelS);
            }

            foreach (CAS_TextToSwitch textToSwitch in texts)
            {
                textToSwitch.ChangeText(textColorS);
            }

            foreach (CAS_ContolModel contolModel in models)
            {
                //controlModel.ChangeImage(buttonN);
            }

            foreach(CAS_ScrollSwitch scroll in scrolls)
            {
                scroll.ChangeColor(colorS); 
            }

            foreach (CAS_SliderBackgroundToSwitch sliderBackgroundToSwitche in sliderBackgroundToSwitches)
            {
                sliderBackgroundToSwitche.ChangeColor(sliderBackgroundS, buttonS);
            }

            foreach (CAS_SliderHandleToSwitch sliderHandleToSwitch in sliderHandleToSwitches)
            {
                sliderHandleToSwitch.ChangeColor(sliderHandleS);
            }

            environmentS.SetActive(true);
            environmentN.SetActive(false); 
        }

        public void SwitchToNormalEnvironment()
        {
            Camera.main.clearFlags = CameraClearFlags.SolidColor;

            foreach (CAS_ButtonToSwitch buttonToSwitch in buttons)
            {
                buttonToSwitch.ChangeImage(buttonN);
            }

            foreach (CAS_MainPanelToSwitch mainPanelToSwitch in mainPanels)
            {
                mainPanelToSwitch.ChangeImage(mainPanelN);
            }

            foreach (CAS_InsidePanelToSwitch insidePanelToSwitch in insidePanels)
            {
                insidePanelToSwitch.ChangeImage(insidePanelN);
            }

            foreach (CAS_TextToSwitch textToSwitch in texts)
            {
                textToSwitch.ChangeText(textColorN);
            }

            foreach (CAS_ContolModel contolModel in models)
            {
                //controlModel.ChangeImage(buttonN);
            }

            foreach (CAS_ScrollSwitch scroll in scrolls)
            {
                scroll.ChangeColor(colorN);
            }

            foreach (CAS_SliderBackgroundToSwitch sliderBackgroundToSwitche in sliderBackgroundToSwitches)
            {
                sliderBackgroundToSwitche.ChangeColor(sliderBackgroundN, buttonN);
            }

            foreach (CAS_SliderHandleToSwitch sliderHandleToSwitch in sliderHandleToSwitches)
            {
                sliderHandleToSwitch.ChangeColor(sliderHandleN);
            }

            environmentS.SetActive(false);
            environmentN.SetActive(true);
        }
    }
}

