using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Diagnostics; 

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

            //ConvertCSVToDatatable(dataCSV.text);
            ConvertCSVToDatatableWithDuplicateData(dataCSV.text); 
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
        }

        void ConvertCSVToDatatable(string data)
        {
            patientDetails.Clear();

            //Split data into rows 
            string[] rowsOfInputData = data.ToString().Split(new[] { '\r', '\n' });

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
                //Checking if last row as it is null 
                else if (index < rowsOfInputData.Length - 1)
                {
                    for (int j = 0; j < 500; j++)
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

                }
                index++;
            }

            dataManager.SetPatientDetails(patientDetails); 
        }

        //index variable is wrong 
        void ConvertCSVToDatatableWithDuplicateData(string data)
        {
            patientDetails.Clear();

            //Split data into rows 
            string[] rowsOfInputData = data.ToString().Split(new[] { '\r', '\n' });

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
                //Checking if last row as it is null 
                else if (index < (rowsOfInputData.Length - 1))
                {
                    for (int j = 0; j < 8; j++)
                    {
                        DataRow newRow = patientDetails.NewRow();
                        newRow["index"] = index;
                        for (int i = 0; i < content.Length; i++)
                        {
                            if (i == 0)
                            {
                                //First time same if otherwise different id's
                                if (j == 0)
                                {
                                    newRow[patientDetails.Columns[i + 1]] = content[i];
                                }
                                else
                                {
                                    newRow[patientDetails.Columns[i + 1]] = content[i] + " (" + j + ")";
                                }
                            }
                            //check for null values 
                            //last two column data not present in csv - so issue 
                            //checking the i value for that 
                            else if (i < content.Length && content[i] != null)
                            {
                                //column plus one to account for index 
                                newRow[patientDetails.Columns[i + 1]] = content[i];
                            }
                        }
                        patientDetails.Rows.Add(newRow);
                    }

                }
                index++;
            }




            dataManager.SetPatientDetails(patientDetails);
        }

    }
}

