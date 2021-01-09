using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Linq;
using System;
using System.Globalization;

namespace CAS
{
    public enum TypeOfOptions{
        specific, 
        demographic, 
        morphological, 
        others, 
        nofilter
    }

    //using LINQ and normal query - Use one in future 
    public class CAS_DataManager : MonoBehaviour
    {
        //Manager 
        public CAS_Manager manager;

        //Patient details main table
        private DataTable patientDetails;

        //Model details 
        Dictionary<string, GameObject> allModelsInformation;

        //Legend details 
        Dictionary<string, TypeOfOptions> columnTypeOfOption;

        //From where data should be loaded
        public string inputPath = "/Data/GlobalRecords.csv";

        public void Start()
        {
            string path = Application.streamingAssetsPath + inputPath;

            //Read data from path 
            TextAsset dataCSV = new TextAsset(System.IO.File.ReadAllText(path));

            //Initialization 
            patientDetails = new DataTable("patientDetails");
            allModelsInformation = new Dictionary<string, GameObject>();
            columnTypeOfOption = new Dictionary<string, TypeOfOptions>();

            ConvertCSVToDatatable(dataCSV.text);
        }

        void ConvertCSVToDatatable(string data)
        {

            patientDetails.Clear();

            //Split data into rows 
            string[] rowsOfInputData = data.ToString().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            string dataWithoutModel= "";
            int numberOfDataWithoutModel = 0;

            int index = 0;
            foreach (string row in rowsOfInputData)
            {
                string[] content = row.Split(new[] { ',' });

                //DataTypeColumn which is second row 
                string[] dataTypes = rowsOfInputData[1].Split(new[] { ',' });

                //optionTypeColumn which is second row 
                string[] optionTypes = rowsOfInputData[2].Split(new[] { ',' });

                //First row of the dataset is header. So reading and adding columns 
                if (index == 0)
                {
                    {
                        //Id Column 
                        DataColumn column = new DataColumn("index");
                        column.DataType = System.Type.GetType("System.Int32");
                        column.AutoIncrement = true;
                        column.AutoIncrementSeed = 1;
                        column.AutoIncrementStep = 1;
                        patientDetails.Columns.Add(column);
                    }

                    //Other Columns from the data 
                    for (int i = 0; i < content.Length; i++)
                    {
                        DataColumn column = new DataColumn(content[i]);
                        if (dataTypes[i] == "string")
                        {
                            column.DataType = System.Type.GetType("System.String"); 
                        }else if(dataTypes[i] == "integer")
                        {
                            column.DataType = System.Type.GetType("System.Double");
                        }
                        patientDetails.Columns.Add(column);
                    }

                    //Adding some important columns 
                    //To know if corresponding model is present
                    {
                        DataColumn column = new DataColumn("morphoPresent");
                        column.DataType = System.Type.GetType("System.Boolean");
                        patientDetails.Columns.Add(column); 
                    }

                    //To know if corresponding morpho is present
                    {
                        DataColumn column = new DataColumn("modelPresent");
                        column.DataType = System.Type.GetType("System.Boolean");
                        patientDetails.Columns.Add(column);
                    }

                    //Add option types 
                    FindAndSetTypesOfOptions(content, optionTypes); 
                }
                //Second row data type and third row is type of parameter
                else if(index != 1 && index != 2)
                {                
                    DataRow newRow = patientDetails.NewRow();
                    
                    for (int i = 0; i < content.Length; i++)
                    {
                        //check for null values  
                        //checking the i value for that 
                        if (i < content.Length && content[i] != null)
                        {
                            //column plus one to account for index 
                            if (dataTypes[i] == "string")
                            {
                                if (content[i] != "")
                                {
                                    newRow[patientDetails.Columns[i + 1]] = content[i];
                                }
                                else
                                {
                                    newRow[patientDetails.Columns[i + 1]] = "N/A";
                                }
                            }
                            else if(dataTypes[i] == "integer")
                            {
                                if (content[i] != "" && content[i] != "N/A")
                                {
                                    newRow[patientDetails.Columns[i + 1]] = double.Parse(content[i]);
                                }
                                else
                                {
                                    newRow[patientDetails.Columns[i + 1]] = -1.0d;
                                }
                            }
                        }
                    }

                    if(double.Parse(newRow["Dmax"].ToString()) == -1.0d)
                    {
                        newRow["morphoPresent"] = true; 
                    }
                    else
                    {
                        newRow["morphoPresent"] = false; 
                    }

                    bool modelPresent = CheckIfCorrespondingModelIsPresent(content[0]);
                    newRow["modelPresent"] = modelPresent;
                    if (!modelPresent)
                    {
                        dataWithoutModel += content[0] + " ";
                        numberOfDataWithoutModel++; 
                    }

                    patientDetails.Rows.Add(newRow);

                }

                index++;
            }

            manager.stepManager.allModelsInformation = allModelsInformation;

            Debug.Log("There are " + numberOfDataWithoutModel + " for which model is misssing and they are " + dataWithoutModel);

            //PrintDataMissingInformation 
            int numberOfmodelsForWhichDataIsMissing = 0;
            string modelsForWhichDataIsMissing = "";
            foreach (Transform child in manager.stepManager.stepParents[0].transform)
            {
                if (child.GetComponent<CAS_PrepareModels>().dataAvailable < 0)
                {
                    modelsForWhichDataIsMissing += child.name + " ";
                    numberOfmodelsForWhichDataIsMissing++;
                }
            }

            Debug.Log("There are " + numberOfmodelsForWhichDataIsMissing + " for which data is misssing and they are " + modelsForWhichDataIsMissing);

            manager.filterAndGroupManager.PopulateFilterOptions();   
        }

        public bool CheckIfCorrespondingModelIsPresent(string patientId)
        {
            bool modelPresent = false; 

            GameObject matchingModel = GameObject.Find(patientId);

            if (matchingModel != null)
            {
                if (!allModelsInformation.ContainsKey(patientId))
                {
                    allModelsInformation.Add(patientId, matchingModel);
                }
                matchingModel.GetComponent<CAS_PrepareModels>().dataAvailable += 1;
                modelPresent = true; 
            }


            if (matchingModel == null)
            {
                foreach (Transform child in manager.stepManager.stepParents[0].transform)
                {
                    if (child.name.Contains(patientId))
                    {
                        matchingModel = child.gameObject;
                        if (!allModelsInformation.ContainsKey(patientId))
                        {
                            allModelsInformation.Add(patientId, matchingModel);
                        }
                        matchingModel.GetComponent<CAS_PrepareModels>().dataAvailable += 1;
                        modelPresent = true; 
                        break;
                    }
                }
            }

            return modelPresent; 
        }

        public DataTable GetPatientDetails()
        {
            return patientDetails; 
        }
            
        public void SetPatientDetails(DataTable _patientDetails)
        {
            patientDetails = _patientDetails; 
        }

        public void FindAndSetTypesOfOptions(String[] columnHeadings, String[] optionTypes)
        {
            TypeOfOptions typeOfThisColumn;

            List<string> specific = new List<string>();
            List<string> demographic = new List<string>();
            List<string> morphological = new List<string>();
            List<string> others = new List<string>();

            for (int i = 0; i < columnHeadings.Length; i++)
            {
                if (optionTypes[i].ToLower() == "specific")
                {
                    typeOfThisColumn = TypeOfOptions.specific;
                    specific.Add(columnHeadings[i]); 
                }
                else if (optionTypes[i].ToLower() == "demographic")
                {
                    typeOfThisColumn = TypeOfOptions.demographic;
                    demographic.Add(columnHeadings[i]);
                }
                else if (optionTypes[i].ToLower() == "morphological")
                {
                    typeOfThisColumn = TypeOfOptions.morphological;
                    morphological.Add(columnHeadings[i]);
                }
                else if (optionTypes[i].ToLower() == "others")
                {
                    typeOfThisColumn = TypeOfOptions.others;
                    others.Add(columnHeadings[i]);
                }
                else 
                {
                    typeOfThisColumn = TypeOfOptions.nofilter;
                }

                columnTypeOfOption.Add(columnHeadings[i], typeOfThisColumn);
            }

            Dictionary<string, List<string>> optionsUnderSpecificTypes = new Dictionary<string, List<string>>();
            optionsUnderSpecificTypes.Add("specific", specific);
            optionsUnderSpecificTypes.Add("demographic", demographic);
            optionsUnderSpecificTypes.Add("morphological", morphological);
            optionsUnderSpecificTypes.Add("others", others);

            manager.filterAndGroupManager.SetOptionsUnderSpecificTypes(optionsUnderSpecificTypes); 
        }

        //Column Names
        public string[] GetFilterOptions()
        {
            return patientDetails.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();
        }

        //Distinct values from providing column names 
        public List<object> GetFilterSubOptions(string columnName)
        {
            return patientDetails.DefaultView
                .ToTable(true, columnName)
                .Rows
                .Cast<DataRow>()
                .Select(row => row[columnName])
                .ToList();
        }

        //Column type
        public Type GetColumnType(string columnName)
        {
            return patientDetails.Columns[columnName].DataType; 
        }

        //Distinct values from providing column names 
        public List<string> GetFilteredPatientIds(List<string> columnNames, List<List<string>> values)
        {
            DataView filteredView = QueryBuilder(columnNames, values);
            if(filteredView == null)
            {
                return null; 
            }

            string[] requiredColumn = { "id" }; 
            DataTable filteredTable = filteredView.ToTable(true, requiredColumn);

            List<string> idList = new List<string>(); 
            foreach (DataRow row in filteredTable.Rows)
            {
                idList.Add(row["id"].ToString());
            }

            return idList; 
        }

        //Distinct values from providing column names 
        public List<string> GetFilteredPatientIdsInteger(List<string> columnNames, List<List<double>> values)
        {
            DataView filteredView = QueryBuilderIntegerColumns(columnNames, values);
            if (filteredView == null)
            {
                return null;
            }

            string[] requiredColumn = { "id" };
            DataTable filteredTable = filteredView.ToTable(true, requiredColumn);

            List<string> idList = new List<string>();
            foreach (DataRow row in filteredTable.Rows)
            {
                idList.Add(row["id"].ToString());
            }

            return idList;
        }

        //Distinct values from providing column names with selectable columns 
        public List<object> GetFilteredPatientRequiredColumns(List<string> columnNames, List<List<string>> values, string[] requiredColumns)
        {

            DataView filteredView = QueryBuilder(columnNames, values);
            if (filteredView == null)
            {
                return null;
            }

            DataTable filteredTable = filteredView.ToTable(true, requiredColumns);

            //returning rows itself 
            List<object> tableToStringList = new List<object>();
            foreach (DataRow row in filteredTable.Rows)
            {
                tableToStringList.Add(row);
            }

            return tableToStringList;
        }

        //Entire row from patient id 
        public Dictionary<string, string> GetPatientRecordWithId(string id)
        {
            List<string> columnNames = new List<string>();
            columnNames.Add("id");

            List<string> value = new List<string>();
            value.Add(id);

            List<List<string>> values = new List<List<string>>();
            values.Add(value);

            DataView filteredView = QueryBuilder(columnNames, values);

            if (filteredView == null)
            {
                return null;
            }

            DataTable filteredTable = filteredView.ToTable(false);

            //returning row as key value pair 
            Dictionary<string, string> rowToKeyValuePair = new Dictionary<string, string>();
            foreach(var column in filteredTable.Columns)
            {
                rowToKeyValuePair.Add(column.ToString(), filteredTable.Rows[0][column.ToString()].ToString()); 
            }

            return rowToKeyValuePair;

        }

        // Update is called once per frame
        void Update()
        {
            //Test GetFilteredPatientIds
            if (Input.GetKeyDown(KeyCode.N))
            {
                List<string> columns = new List<string>();
                columns.Add("aneurysmLocation");
                columns.Add("institution"); 
                List<string> value1 = new List<string>();
                value1.Add("MCA-Bif");
                List<string> value2 = new List<string>();
                value2.Add("UniversityHospitalMagdeburg");
                List<List<string>> values = new List<List<string>>();
                values.Add(value1);
                values.Add(value2);
                List<string> filterRows = GetFilteredPatientIds(columns, values);
                foreach(object row in filterRows)
                {
                    Debug.Log(row); 
                }
            }
        }

        public DataView QueryBuilderIntegerColumns(List<string> columnNames, List<List<double>> values)
        {
            int index = 0;
            string filter = "";
            foreach (string columnName in columnNames)
            {
                List<double> valuesForThisColumnName = values[index];

                if (index != 0)
                {
                    filter += " AND "; 
                }
                
                filter = "( " + columnName + " >= " + valuesForThisColumnName[0] + " AND " + columnName + " <= " + valuesForThisColumnName[1] + " )"; 

                index++;
            }
            Debug.Log(filter); 

            if(filter == "")
            {
                return null; 
            }

            //Creating a view for filter
            DataView filteredView = new DataView(patientDetails);
            filteredView.RowFilter = filter;

            return filteredView; 
        }

        public DataView QueryBuilder(List<string> columnNames, List<List<string>> values)
        {
            int index = 0;
            string filter = "";
            foreach (string columnName in columnNames)
            {
                if (index == 0)
                {
                    filter += columnName + " in (";
                }
                else
                {
                    filter += " AND " + columnName + " in (";
                }

                List<string> valuesForThisColumnName = values[index];
                int subIndex = 0;
                foreach (string value in valuesForThisColumnName)
                {
                    if (subIndex == 0)
                    {
                        filter += "'" + value + "'";
                    }
                    else
                    {
                        filter += "," + "'" + value + "'";
                    }
                    subIndex++;
                }
                filter += ")";
                index++;
            }
            Debug.Log(filter);

            if (filter == "")
            {
                return null;
            }

            //Creating a view for filter
            DataView filteredView = new DataView(patientDetails);
            filteredView.RowFilter = filter;

            return filteredView;
        }

        public TypeOfOptions GetColumnTypeOfOption(string columnHeading)
        {
            return columnTypeOfOption[columnHeading]; 
        }
    }
}