using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

namespace CAS {
    public class CAS_EnvironmentSwitch : MonoBehaviour
    {
        //Skybox 
        //ButtonImage 
        //Environment 
        //MainPanel background 
        //InsidePanel background
        //Model material 

        private List<CAS_ButtonToSwitch> buttons;
        private List<CAS_MainPanelToSwitch> mainPanels;
        private List<CAS_InsidePanelToSwitch> insidePanels;
        private List<CAS_InsidePanelNormalToSwitch> insideNormalPanels;
        private List<CAS_TextToSwitch> texts;
        private List<CAS_ScrollSwitch> scrolls; 
        private List<CAS_ContolModel> models;
        private List<CAS_ObjectOfInterest> interestedModels;
        private List<CAS_ModelIdentifier> modelSwitches;
        private List<CAS_SliderBackgroundToSwitch> sliderBackgroundToSwitches;
        private List<CAS_SliderHandleToSwitch> sliderHandleToSwitches;
        private List<CAS_TabGroupSwitch> tabGroupSwitches;
        private List<CAS_TabButtonSwitch> tabButtonSwitches;
        private List<CAS_InstantiatedTextToSwitch> instantiatedTextToSwitches;
        private List<Camera> cameras; 
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
        private Color colorSliderScrollN = Color.white;

        public Camera oculusCamera;
        public Camera viveCamera; 

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
            oculusCamera.clearFlags = CameraClearFlags.Skybox;
            viveCamera.clearFlags = CameraClearFlags.Skybox; 

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

            foreach (CAS_InsidePanelNormalToSwitch insidePanelNormalToSwitch in insideNormalPanels)
            {
                insidePanelNormalToSwitch.ChangeImage(buttonS);
            }

            foreach (CAS_TextToSwitch textToSwitch in texts)
            {
                textToSwitch.ChangeText(textColorS);
            }

            foreach (CAS_InstantiatedTextToSwitch instantiatedTextToSwitch in instantiatedTextToSwitches)
            {
                instantiatedTextToSwitch.ChangeText(textColorS);
            }

            foreach (CAS_ContolModel controlModel in models)
            {
                controlModel.ChangeMaterialForSwitch(modelMaterialS, "_Edgecolor");
            }

            foreach (CAS_ObjectOfInterest interestedModel in interestedModels)
            {
                interestedModel.ChangeMaterialForSwitch(modelMaterialS, "_Edgecolor");
            }

            foreach (CAS_ModelIdentifier modelSwitch in modelSwitches)
            {
                modelSwitch.SetToSciFi();
            }

            foreach (CAS_ScrollSwitch scroll in scrolls)
            {
                scroll.ChangeColor(colorS); 
            }

            foreach (CAS_SliderBackgroundToSwitch sliderBackgroundToSwitche in sliderBackgroundToSwitches)
            {
                sliderBackgroundToSwitche.ChangeColor(colorS, buttonS);
            }

            foreach (CAS_SliderHandleToSwitch sliderHandleToSwitch in sliderHandleToSwitches)
            {
                sliderHandleToSwitch.ChangeColor(colorS);
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
            oculusCamera.clearFlags = CameraClearFlags.SolidColor;
            viveCamera.clearFlags = CameraClearFlags.SolidColor;

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

            foreach (CAS_InsidePanelNormalToSwitch insidePanelNormalToSwitch in insideNormalPanels)
            {
                insidePanelNormalToSwitch.ChangeImage(insidePanelN);
            }

            foreach (CAS_TextToSwitch textToSwitch in texts)
            {
                textToSwitch.ChangeText(textColorN);
            }

            foreach (CAS_InstantiatedTextToSwitch instantiatedTextToSwitch in instantiatedTextToSwitches)
            {
                instantiatedTextToSwitch.ChangeText(textColorN);
            }

            foreach (CAS_ContolModel controlModel in models)
            {
                controlModel.ChangeMaterialForSwitch(modelMaterialN, "_Color");
            }

            foreach (CAS_ObjectOfInterest interestedModel in interestedModels)
            {
                interestedModel.ChangeMaterialForSwitch(modelMaterialN, "_Color");
            }

            foreach (CAS_ModelIdentifier modelSwitch in modelSwitches)
            {
                modelSwitch.SetToNormal();
            }

            foreach (CAS_ScrollSwitch scroll in scrolls)
            {
                scroll.ChangeColor(colorSliderScrollN);
            }

            foreach (CAS_SliderBackgroundToSwitch sliderBackgroundToSwitche in sliderBackgroundToSwitches)
            {
                sliderBackgroundToSwitche.ChangeColor(colorSliderScrollN, buttonN);
            }

            foreach (CAS_SliderHandleToSwitch sliderHandleToSwitch in sliderHandleToSwitches)
            {
                sliderHandleToSwitch.ChangeColor(colorSliderScrollN);
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
            buttons = FindObjectsOfTypeAll<CAS_ButtonToSwitch>();
            mainPanels = FindObjectsOfTypeAll<CAS_MainPanelToSwitch>();
            insidePanels = FindObjectsOfTypeAll<CAS_InsidePanelToSwitch>();
            insideNormalPanels = FindObjectsOfTypeAll<CAS_InsidePanelNormalToSwitch>();
            texts = FindObjectsOfTypeAll<CAS_TextToSwitch>();
            models = FindObjectsOfTypeAll<CAS_ContolModel>();
            interestedModels = FindObjectsOfTypeAll<CAS_ObjectOfInterest>();
            modelSwitches = FindObjectsOfTypeAll<CAS_ModelIdentifier>();
            scrolls = FindObjectsOfTypeAll<CAS_ScrollSwitch>();
            sliderBackgroundToSwitches = FindObjectsOfTypeAll<CAS_SliderBackgroundToSwitch>();
            sliderHandleToSwitches = FindObjectsOfTypeAll<CAS_SliderHandleToSwitch>();
            tabGroupSwitches = FindObjectsOfTypeAll<CAS_TabGroupSwitch>();
            tabButtonSwitches = FindObjectsOfTypeAll<CAS_TabButtonSwitch>();
            if (instantiatedTextToSwitches == null)
            {
                instantiatedTextToSwitches = new List<CAS_InstantiatedTextToSwitch>();
            }

            initialized = true; 
        }

        public static List<T> FindObjectsOfTypeAll<T>()
        {
            List<T> results = new List<T>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var s = SceneManager.GetSceneAt(i);
                if (s.isLoaded)
                {
                    var allGameObjects = s.GetRootGameObjects();
                    for (int j = 0; j < allGameObjects.Length; j++)
                    {
                        var go = allGameObjects[j];
                        results.AddRange(go.GetComponentsInChildren<T>(true));
                    }
                }
            }
            return results;
        }

        public void AddToInstantiatedTextToSwitch(CAS_InstantiatedTextToSwitch instantiatedTextToSwitch)
        {
            if(instantiatedTextToSwitches == null)
            {
                instantiatedTextToSwitches = new List<CAS_InstantiatedTextToSwitch>(); 
            }

            if (sciFi)
            {
                instantiatedTextToSwitch.ChangeText(textColorS); 
            }
            else
            {
                instantiatedTextToSwitch.ChangeText(textColorN);
            }

            instantiatedTextToSwitches.Add(instantiatedTextToSwitch); 
        }

        public void RemoveFromInstantiatedTextToSwitch(CAS_InstantiatedTextToSwitch instantiatedTextToSwitch)
        {
            if (instantiatedTextToSwitches == null)
            {
                instantiatedTextToSwitches = new List<CAS_InstantiatedTextToSwitch>();
            }

            instantiatedTextToSwitches.Remove(instantiatedTextToSwitch);
        }
    }
}

