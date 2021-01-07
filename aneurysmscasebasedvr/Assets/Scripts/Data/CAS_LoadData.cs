using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Diagnostics;
using System; 

namespace CAS
{
    public class CAS_LoadData : MonoBehaviour
    {
        public string inputPath = "/Data/GlobalRecords.csv";
        public DataTable patientDetails;

        public CAS_DataManager dataManager;

        // Start is called before the first frame update
        void Start()
        {
            string path = Application.streamingAssetsPath + inputPath;

            //Read data from path 
            TextAsset dataCSV = new TextAsset(System.IO.File.ReadAllText(path));

            //New patients datatable 
            patientDetails = new DataTable();

            ConvertCSVToDatatable(dataCSV.text);
            //ConvertCSVToDatatableWithDuplicateData(dataCSV.text); 
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                UnityEngine.Debug.Log("Number of rows is " + patientDetails.Rows.Count);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DataRow[] rows = patientDetails.Select("institution = 'UniversityHospitalMagdeburg' AND aneurysmLocation = 'MCA-Bif'");
                UnityEngine.Debug.Log(rows.Length);
                UnityEngine.Debug.Log(sw.ElapsedMilliseconds);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                UnityEngine.Debug.Log("Number of rows is " + patientDetails.Rows.Count);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DataRow[] rows = patientDetails.Select("aneurysmType = 'TER'");
                UnityEngine.Debug.Log(rows.Length);
                UnityEngine.Debug.Log(sw.ElapsedMilliseconds);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                AssignModelToDataTable(); 
            }
        }

        void ConvertCSVToDatatable(string data)
        {
            patientDetails.Clear();

            //Split data into rows 
            string[] rowsOfInputData = data.ToString().Split(new[] { '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

            int index = 0;
            foreach (string row in rowsOfInputData)
            {
                
                string[] content = row.Split(new[] { ',' });

                //First row of the dataset is header. So reading columns 
                if (index == 0)
                {
                    patientDetails.Columns.Add("index");
                    for (int i = 0; i < content.Length; i++)
                    {
                        patientDetails.Columns.Add(content[i]);
                    }
                }
                //Checking if last row as it is null - (Removed the -1 but should check whether this condition is necessary) 
                else if (index < rowsOfInputData.Length)
                {
                    
                        DataRow newRow = patientDetails.NewRow();
                        newRow["index"] = index;
                        for (int i = 0; i < content.Length; i++)
                        {
                            //check for null values 
                            //last two column data not present in csv - so issue 
                            //checking the i value for that 
                            if (i < content.Length && content[i] != null)
                            {
                                //column plus one to account for index 
                                newRow[patientDetails.Columns[i + 1]] = content[i];
                            }
                        }
                        patientDetails.Rows.Add(newRow);
                

                }
                index++;
            }

            dataManager.SetPatientDetails(patientDetails); 
        }

        //Print the important details in a concatenated string .. dont waster anzthing as later might be useful 
        public void AssignModelToDataTable()
        {
            Dictionary<string, GameObject> allModelsInformation = new Dictionary<string, GameObject>(); 

            GameObject step1Parent = GameObject.Find("Step1"); 
            DataRow[] rows = patientDetails.Select();

            int numberOfRowsWithValidModels = 0; 
            foreach (DataRow row in rows)
            {
                string rowStrig = "";
                GameObject matchingModel = GameObject.Find(row.ItemArray[1].ToString());
                if(matchingModel != null)
                {
                    if (!allModelsInformation.ContainsKey(row.ItemArray[1].ToString()))
                    {
                        allModelsInformation.Add(row.ItemArray[1].ToString(), matchingModel);
                    }                    

                    matchingModel.GetComponent<CAS_PrepareModels>().dataAvailable += 1; 
                    numberOfRowsWithValidModels++;
                }
                
                if(matchingModel == null)
                {
                    foreach(Transform child in step1Parent.transform)
                    {
                        if (child.name.Contains(row.ItemArray[1].ToString())){
                            matchingModel = child.gameObject;
                            if (!allModelsInformation.ContainsKey(row.ItemArray[1].ToString()))
                            {
                                allModelsInformation.Add(row.ItemArray[1].ToString(), matchingModel);
                            }
                            matchingModel.GetComponent<CAS_PrepareModels>().dataAvailable += 1;
                            numberOfRowsWithValidModels++; 
                            break; 
                        }
                    }     
                }

                if(matchingModel == null)
                {
                    UnityEngine.Debug.Log(row.ItemArray[1].ToString());
                }

                foreach (object item in row.ItemArray)
                {
                    rowStrig = rowStrig + item.ToString() + " ";              
                }               
            }



            UnityEngine.Debug.Log(numberOfRowsWithValidModels);

            foreach (Transform child in step1Parent.transform)
            {
                if (child.GetComponent<CAS_PrepareModels>().dataAvailable < 0)
                {
                    UnityEngine.Debug.Log(child.name);
                }
            }

            //dataManager.stepManager.allModelsInformation = allModelsInformation; 
        }
    }
}

