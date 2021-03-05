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
        private CAS_TabGroupSwitch[] tabGroupSwitches;
        private CAS_TabButtonSwitch[] tabButtonSwitches; 

        //Normal 
        public Sprite buttonN;
        public Sprite buttonIdleN;
        public Sprite buttonActiveN;
        public Sprite mainPanelN;
        public Sprite insidePanelN;
        private Color textColorN = Color.black;
        public Material modelMaterialN;
        public GameObject environmentN;
        private Color colorN = Color.black;
        public Color sliderBackgroundN;
        public Color sliderHandleN;

        //Scifi
        public Sprite buttonS;
        public Sprite buttonIdleS;
        public Sprite buttonActiveS;
        public Sprite mainPanelS;
        public Sprite insidePanelS;
        private Color textColorS = new Color(0.79f, 0.82f, 1f, 1f);
        public Material modelMaterialS;
        public GameObject environmentS;
        private Color colorS = new Color(0.79f, 0.82f, 1f, 1f);
        public Color sliderBackgroundS;
        public Color sliderHandleS;

        bool sciFi = false; 

        bool initialized = false; 
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                sciFi = true;
                SwitchToSciFiEnvironment(); 
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                sciFi = false;
                SwitchToNormalEnvironment(); 
            }

        }

        public void SwitchEnvironment()
        {
            sciFi = !sciFi;

            if (sciFi)
            {
                SwitchToSciFiEnvironment();
            }
            else
            {
                SwitchToNormalEnvironment();
            }
        }

        public void SwitchToSciFiEnvironment()
        {
            if (!initialized) Initialize(); 

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

            foreach (CAS_ContolModel controlModel in models)
            {
                controlModel.ChangeMaterialForSwitch(modelMaterialS, "_Color");
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

            foreach (CAS_TabButtonSwitch tabButtonSwitch in tabButtonSwitches)
            {
                tabButtonSwitch.ChangeImage(buttonIdleS);
            }

            foreach (CAS_TabGroupSwitch tabGroupSwitch in tabGroupSwitches)
            {
                tabGroupSwitch.ChangeImages(buttonIdleS, buttonActiveS);
            }

            environmentS.SetActive(true);
            environmentN.SetActive(false); 
        }

        public void SwitchToNormalEnvironment()
        {
            if (!initialized) Initialize();

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

            foreach (CAS_ContolModel controlModel in models)
            {
                controlModel.ChangeMaterialForSwitch(modelMaterialN, "_Color");
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

            foreach (CAS_TabButtonSwitch tabButtonSwitch in tabButtonSwitches)
            {
                tabButtonSwitch.ChangeImage(buttonIdleN);
            }

            foreach (CAS_TabGroupSwitch tabGroupSwitch in tabGroupSwitches)
            {
                tabGroupSwitch.ChangeImages(buttonIdleN, buttonActiveN);
            }

            environmentS.SetActive(false);
            environmentN.SetActive(true);
        }

        public void Initialize()
        {
            buttons = FindObjectsOfType<CAS_ButtonToSwitch>();
            mainPanels = FindObjectsOfType<CAS_MainPanelToSwitch>();
            insidePanels = FindObjectsOfType<CAS_InsidePanelToSwitch>();
            texts = FindObjectsOfType<CAS_TextToSwitch>();
            models = FindObjectsOfType<CAS_ContolModel>();
            scrolls = FindObjectsOfType<CAS_ScrollSwitch>();
            sliderBackgroundToSwitches = FindObjectsOfType<CAS_SliderBackgroundToSwitch>();
            sliderHandleToSwitches = FindObjectsOfType<CAS_SliderHandleToSwitch>();
            tabGroupSwitches = FindObjectsOfType<CAS_TabGroupSwitch>();
            tabButtonSwitches = FindObjectsOfType<CAS_TabButtonSwitch>();

            initialized = true; 
        }
    }
}

