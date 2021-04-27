using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Linq;
using System;

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

        //Model details 
        Dictionary<string, GameObject> objectOfInterestModelsInformation;

        //Legend details 
        Dictionary<string, TypeOfOptions> columnTypeOfOption;

        //From where data should be loaded
        public string inputPath = "/Data/GlobalRecords.csv";

        //Red, Green, Blue, Orange, Yellow, Teal, Pink, Lavender, Apricot, Brown, Maroon, Olive, Beige, Cyan, Mint, Purple, Lime, 
        //DarkOliveGreen, Palevioletred, Goldenrod, Darkslategray
        public Color[] colorsForGrouping = new Color[] { new Color(0.901f, 0.098f, 0.294f, 1f), new Color(0.235f, 0.705f, 0.294f, 1f),
            new Color(0.262f, 0.388f, 0.847f, 1f), new Color(0.960f, 0.509f, 0.192f, 1f), new Color(1f, 0.882f, 0.098f, 1f),
            new Color(0.274f, 0.6f, 0.564f, 1f), new Color(0.862f, 0.745f, 1f, 1f), new Color(0.980f, 0.745f, 0.831f, 1f),
            new Color(1f, 0.847f, 0.694f, 1f), new Color(0.603f, 0.388f, 0.141f, 1f), new Color(0.603f, 0.388f, 0.141f, 1f),
            new Color(0.501f, 0f, 0f, 1f), new Color(0.501f, 0.501f, 0f, 1f), new Color(1f, 0.980f, 0.784f, 1f),
            new Color(0.258f, 0.831f, 0.956f, 1f), new Color(0.666f, 1f, 0.764f, 1f), new Color(0.568f, 0.117f, 0.705f, 1f),
            new Color(0.749f, 0.937f, 0.270f, 1f), new Color(0.333f, 0.419f, 0.184f, 1f), new Color(0.858f, 0.439f, 0.576f, 1f),
            new Color(0.854f, 0.647f, 0.125f, 1f), new Color(0.184f, 0.309f, 0.309f, 1f)};

        string modelWithMultipleRecords = "";
        int modelWithMultipleRecordsCount = 0;

        private void Awake()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
        }

        public void Start()
        {
            string path = Application.streamingAssetsPath + inputPath;

            //Read data from path 
            TextAsset dataCSV = new TextAsset(System.IO.File.ReadAllText(path));

            //Initialization 
            patientDetails = new DataTable("patientDetails");
            allModelsInformation = new Dictionary<string, GameObject>();
            objectOfInterestModelsInformation = new Dictionary<string, GameObject>(); 
            columnTypeOfOption = new Dictionary<string, TypeOfOptions>();

            ConvertCSVToDatatable(dataCSV.text);
        }

        // Update is called once per frame
        void Update()
        {

        }


        //model present was converted from bool to numeric but not in database 
        void ConvertCSVToDatatable(string data)
        {

            patientDetails.Clear();
            List<CAS_ObjectOfInterest> objectsOfInterest = new List<CAS_ObjectOfInterest>(); 

            //Split data into rows 
            string[] rowsOfInputData = data.ToString().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            string dataWithoutModel= "";
            int numberOfDataWithoutModel = 0;

            int index = 0;
            int exampleIndex = 0; 
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

                    //For Example
                    {
                        DataColumn column = new DataColumn("notExample");
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

                    newRow["notExample"] = true;

                    int modelPresent = CheckIfCorrespondingModelIsPresent(content[0]);
                    newRow[patientDetails.Columns[0 + 1]] = content[0] + "_" + modelPresent; 

                    if (modelPresent == -1)
                    {
                        newRow["modelPresent"] = false;

                        dataWithoutModel += content[0] + " ";
                        numberOfDataWithoutModel++;
                    }

                    if (modelPresent != -1)
                    {
                        newRow["modelPresent"] = true;

                        if (exampleIndex < 2)
                        {
                            bool qualifiedForExample = true;
                            
                            foreach(var item in newRow.ItemArray)
                            {
                                if(item.ToString() == "N/A" || item.ToString() == "-1")
                                {
                                    qualifiedForExample = false; 
                                } 
                            }

                            if (qualifiedForExample)
                            {
                                newRow["notExample"] = false;
                                GameObject matchingModel = GameObject.Find(content[0] + "_" + modelPresent);
                                allModelsInformation.Remove(content[0] + "_" + modelPresent);
                                objectOfInterestModelsInformation.Add(content[0] + "_" + modelPresent, matchingModel); 
                                matchingModel.AddComponent<CAS_ObjectOfInterest>();
                                objectsOfInterest.Add(matchingModel.GetComponent<CAS_ObjectOfInterest>());
                                matchingModel.transform.parent = manager.aiManager.transform;
                                matchingModel.GetComponent<CAS_ObjectOfInterest>().Initialize();
                                matchingModel.SetActive(false); 
                                exampleIndex++;
                            }
                        }
                    }

                    patientDetails.Rows.Add(newRow);

                }

                index++;
            }

            manager.stepManager.InitializeAfterDataRead(); 
            manager.aiManager.SetObjectsOfInterest(objectsOfInterest); 
            manager.stepManager.SetAllModelsInformation(allModelsInformation);
            manager.aiManager.SetAllModelsInformation(objectOfInterestModelsInformation); 

            Debug.Log("There are " + numberOfDataWithoutModel + " for which model is misssing and they are " + dataWithoutModel);

            //PrintDataMissingInformation 
            int numberOfmodelsForWhichDataIsMissing = 0;
            string modelsForWhichDataIsMissing = "";
            foreach (Transform child in manager.stepManager.stepParents[0].transform)
            {
                if (child.GetComponent<CAS_ModelIdentifier>().dataAvailable < 0)
                {
                    modelsForWhichDataIsMissing += child.name + " ";
                    numberOfmodelsForWhichDataIsMissing++;
                }
            }

            Debug.Log("There are " + numberOfmodelsForWhichDataIsMissing + " for which data is misssing and they are " + modelsForWhichDataIsMissing);
            Debug.Log("There are " + patientDetails.Rows.Count + " rows of data ");
            Debug.Log("There are " + manager.stepManager.stepParents[0].transform.childCount + " model present ");
            Debug.Log("There are " + modelWithMultipleRecordsCount + " models for which multiple records present and they are " +  modelWithMultipleRecords);

            manager.filterAndGroupUIManager.PopulateFilterOptions();
            manager.displayPatientDetailsUIManager.PopulatePatientDisplay();
        }

        public int CheckIfCorrespondingModelIsPresent(string patientId)
        {
            int modelPresent = -1; 

            GameObject matchingModel = GameObject.Find(patientId);

            if(matchingModel == null)
            {
                matchingModel = GameObject.Find(patientId + "_" + 0);
            }

            if (matchingModel != null)
            {
                modelPresent = 0;

                if (!allModelsInformation.ContainsKey(patientId + "_" + modelPresent))
                {
                    matchingModel.name = patientId + "_" + modelPresent;
                    allModelsInformation.Add(patientId + "_" + modelPresent, matchingModel);
                }
                else
                {
                    modelPresent = CheckIfModelAlreadyMapped(patientId, 1);
                    GameObject instantiatedModel = Instantiate(matchingModel, matchingModel.transform.parent);
                    instantiatedModel.name = patientId + "_" + modelPresent;

                    modelWithMultipleRecords += patientId + " ";
                    modelWithMultipleRecordsCount++;
                    allModelsInformation.Add(patientId + "_" + modelPresent, instantiatedModel);
                }
                matchingModel.GetComponent<CAS_ModelIdentifier>().dataAvailable += 1;
            }


            if (matchingModel == null)
            {
                foreach (Transform child in manager.stepManager.transform)
                {
                    if (child.name.Contains(patientId))
                    {
                        modelPresent = 0;
                        matchingModel = child.gameObject;

                        if (!allModelsInformation.ContainsKey(patientId + "_" + modelPresent))
                        {
                            matchingModel.name = patientId + "_" + modelPresent;
                            allModelsInformation.Add(patientId + "_" + modelPresent, matchingModel);
                        }
                        else
                        {
                            modelPresent = CheckIfModelAlreadyMapped(patientId, 1);
                            GameObject instantiatedModel = Instantiate(matchingModel, matchingModel.transform.parent);
                            instantiatedModel.name = patientId + "_" + modelPresent;

                            modelWithMultipleRecords += patientId + " ";
                            modelWithMultipleRecordsCount++;
                            allModelsInformation.Add(patientId + "_" + modelPresent, instantiatedModel);
                        }
                        matchingModel.GetComponent<CAS_ModelIdentifier>().dataAvailable += 1;
                        break;
                    }
                }
            }

            return modelPresent; 
        }

        public int CheckIfModelAlreadyMapped(string patientId, int numberOfTimesMapped)
        {
            if (allModelsInformation.ContainsKey(patientId + "_" + numberOfTimesMapped))
            {
                return CheckIfModelAlreadyMapped(patientId, numberOfTimesMapped + 1);
            }
            else
            {
                return numberOfTimesMapped; 
            }
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

            manager.filterAndGroupUIManager.SetOptionsUnderSpecificTypes(optionsUnderSpecificTypes); 
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

        //Column type
        public List<string> GetAllColumnNames()
        {
            List<string> coloumnNames = new List<string>(); 
            
            foreach(DataColumn coloumn in patientDetails.Columns)
            {
                coloumnNames.Add(coloumn.ColumnName.ToString()); 
            }

            return coloumnNames; 
        }

        //Column type
        public List<string> GetAllColumnNamesForCompare()
        {
            DataView filteredView = new DataView(patientDetails);
            DataTable filteredTable = filteredView.ToTable();

            if (filteredTable.Columns.Contains("manualAddedOthersFromCode")) filteredTable.Columns.Remove("manualAddedOthersFromCode");
            filteredTable.Columns.Remove("index");
            filteredTable.Columns.Remove("modelPresent");
            filteredTable.Columns.Remove("morphoPresent");
            filteredTable.Columns.Remove("notExample");

            List<string> coloumnNames = new List<string>();

            foreach (DataColumn coloumn in filteredTable.Columns)
            {
                coloumnNames.Add(coloumn.ColumnName.ToString());
            }

            return coloumnNames;
        }

        //Distinct values from providing column names 
        public List<string> GetUniquePatientIds()
        {
            List<string> idList = new List<string>();

            string[] requiredColumn = { "id" };

            string filter = "modelPresent = true";

            DataView view = new DataView(patientDetails);
            view.RowFilter = filter; 
            DataTable uniqueIdTable = view.ToTable(true, requiredColumn);

            foreach (DataRow row in uniqueIdTable.Rows)
            {
                idList.Add(row["id"].ToString());
            }

            return idList; 
        }

        //Distinct values from providing column names 
        public List<string> GetFilteredPatientIdsStringAndInteger(List<string> columnNamesString, List<List<string>> valuesString, List<string> columnNamesInteger, List<List<List<double>>> valuesInteger)
        {
            List<string> idList = new List<string>();
            DataView filteredView = QueryBuilderStringAndInteger(columnNamesString, valuesString, columnNamesInteger, valuesInteger);
            if (filteredView == null)
            {
                return idList;
            }

            string[] requiredColumn = { "id" };
            DataTable filteredTable = filteredView.ToTable(true, requiredColumn);

            foreach (DataRow row in filteredTable.Rows)
            {
                idList.Add(row["id"].ToString());
            }

            return idList;
        }


        //Entire row from patient id 
        public DataTable GetPatientRecordWithId(string id)
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

            if (filteredTable.Columns.Contains("manualAddedOthersFromCode")) filteredTable.Columns.Remove("manualAddedOthersFromCode");
            filteredTable.Columns.Remove("index");
            filteredTable.Columns.Remove("modelPresent");
            filteredTable.Columns.Remove("morphoPresent");
            filteredTable.Columns.Remove("notExample");

            return filteredTable; 
        }

        //Entire row from patient id 
        public List<string> GetPatientRecordWithIdWithoutKeys(string id)
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

            if(filteredTable.Columns.Contains("manualAddedOthersFromCode")) filteredTable.Columns.Remove("manualAddedOthersFromCode");
            filteredTable.Columns.Remove("index"); 
            filteredTable.Columns.Remove("modelPresent");
            filteredTable.Columns.Remove("morphoPresent");
            filteredTable.Columns.Remove("notExample");


            //returning row as key value pair 
            List<string> listOfRowValues = new List<string>();
            foreach (var column in filteredTable.Columns)
            {
                listOfRowValues.Add(filteredTable.Rows[0][column.ToString()].ToString());
            }

            return listOfRowValues;

        }


        //Entire row from patient id 
        public DataRow GetPatientRecordOfInterest(string id)
        {
            Debug.Log(id); 
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

            return filteredTable.Rows[0];

        }

        //Entire row from patient id 
        public string GetTargetValue(string id)
        {
            Debug.Log(id);
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

            return filteredTable.Rows[0]["ruptureStatus"].ToString();

        }

        public DataView QueryBuilderStringAndInteger(List<string> columnNamesString, List<List<string>> valuesString, List<string> columnNamesInteger, List<List<List<double>>> valuesInteger)
        {
            string filter = "";
            string integerFilter = "";
            string stringFilter = "";

            int indexInteger = 0;
            foreach (string columnName in columnNamesInteger)
            {
                if (indexInteger != 0)
                {
                    integerFilter += " AND ";
                }

                List<List<double>> valuesForThisColumnName = valuesInteger[indexInteger];

                int numberOfConditionsForThisColumn = 0;
                integerFilter += "( ";
                foreach (List<double> valueForThisColumnName in valuesForThisColumnName)
                {
                    if (numberOfConditionsForThisColumn != 0)
                    {
                        integerFilter += " OR ";
                    }

                    integerFilter += "( " + columnName + " >= " + valueForThisColumnName[0] + " AND " + columnName + " <= " + valueForThisColumnName[1] + " )";

                    numberOfConditionsForThisColumn++; 
                }
                integerFilter += " )";

                indexInteger++;
            }

            int indexString = 0;
            foreach (string columnName in columnNamesString)
            {
                if (indexString == 0)
                {
                    stringFilter += columnName + " in (";
                }
                else
                {
                    stringFilter += " AND " + columnName + " in (";
                }

                List<string> valuesForThisColumnName = valuesString[indexString];
                int subIndex = 0;
                foreach (string value in valuesForThisColumnName)
                {
                    if (subIndex == 0)
                    {
                        stringFilter += "'" + value + "'";
                    }
                    else
                    {
                        stringFilter += "," + "'" + value + "'";
                    }
                    subIndex++;
                }
                stringFilter += ")";
                indexString++;
            }

            if(stringFilter != "" && integerFilter != "")
            {
                filter = integerFilter + " AND " + stringFilter; 
            }else if(stringFilter != "")
            {
                filter = stringFilter; 
            }else if(integerFilter != "")
            {
                filter = integerFilter; 
            }

            if (filter != "")
            {
                filter += " AND (modelPresent = true) AND (notExample = true)";
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

        //Distinct values from providing column names 
        public Dictionary<string, List<string>> GetPatientIdsGroupBy(List<string> columnNamesString, List<List<string>> valuesString, List<string> columnNamesInteger, List<List<List<double>>> valuesInteger, string columnNamesGroupBy, int clustersCount)
        {
            DataView filteredView = QueryBuilderStringAndIntegerGroupBy(columnNamesString, valuesString, columnNamesInteger, valuesInteger);

            if (filteredView == null)
            {
                return new Dictionary<string, List<string>>();
            }

            string[] requiredColumn = { "id", columnNamesGroupBy };

            DataTable filteredTable = filteredView.ToTable(false, requiredColumn);

            DataView filteredViewSorted = filteredTable.DefaultView;
            filteredViewSorted.Sort = columnNamesGroupBy + " asc";
            DataTable datatableSorted = filteredViewSorted.ToTable();

            Type filterOptionDataType = GetColumnType(columnNamesGroupBy);

            if (filterOptionDataType == System.Type.GetType("System.Double"))
            {
                return GetPatientIdsGroupByDouble(datatableSorted, columnNamesGroupBy, clustersCount);
            }
            
            if (filterOptionDataType == System.Type.GetType("System.String"))
            {
                return GetPatientIdsGroupByString(datatableSorted, columnNamesGroupBy); 
            }

            return new Dictionary<string, List<string>>(); 
        }

        public Dictionary<string, List<string>> GetPatientIdsGroupByString(DataTable datatableSorted, string columnNamesGroupBy)
        {
            Dictionary<string, List<string>> filterIdsGroupBy = new Dictionary<string, List<string>>();

            string previousFilterOption = "";
            string currentFilterOption = "";

            List<string> filterIdsGroupedByIds = new List<string>();

            int index = 0;
            foreach (DataRow row in datatableSorted.Rows)
            {
                index++;
                previousFilterOption = currentFilterOption;
                currentFilterOption = row[columnNamesGroupBy].ToString();

                //Debug.Log(previousFilterOption + " " + currentFilterOption);

                if (previousFilterOption == currentFilterOption)
                {
                    filterIdsGroupedByIds.Add(row["id"].ToString());
                }
                else
                {
                    //Debug.Log(previousFilterOption + " " + currentFilterOption + " " + filterIdsGroupedByIds.Count);
                    if (previousFilterOption != "")
                    {
                        filterIdsGroupBy.Add(previousFilterOption, filterIdsGroupedByIds);
                    }

                    filterIdsGroupedByIds = new List<string>();
                    filterIdsGroupedByIds.Add(row["id"].ToString());
                }
            }

            //Debug.Log(previousFilterOption + " " + currentFilterOption + " " + filterIdsGroupedByIds.Count);

            if (!filterIdsGroupBy.ContainsKey(currentFilterOption)) filterIdsGroupBy.Add(currentFilterOption, filterIdsGroupedByIds);

            return filterIdsGroupBy;
        }

        //using clustering to groupby numeric columns 
        public Dictionary<string, List<string>> GetPatientIdsGroupByDouble(DataTable datatableSorted, string columnNamesGroupBy, int clustersCount)
        {
            Dictionary<string, List<string>> filterIdsGroupBy = new Dictionary<string, List<string>>();

            List<string> dataNotAvailable = new List<string>();
            DataView dataNotAvailableView = new DataView(datatableSorted);
            string filter = "( " + columnNamesGroupBy + " <> -1 )";
            dataNotAvailableView.RowFilter = filter;
            DataTable datatableSortedWithoutNAdata = dataNotAvailableView.ToTable();

            //int clustersCount = 4;
            int iterations = 50;

            string[] patientIds = new string[datatableSortedWithoutNAdata.Rows.Count];
            double[] coloumnValues = new double[datatableSortedWithoutNAdata.Rows.Count];
            double[][] toClusterData = new double[datatableSortedWithoutNAdata.Rows.Count][];

            int index = 0; 
            foreach (DataRow row in datatableSorted.Rows)
            {
                if(double.Parse(row[columnNamesGroupBy].ToString()) != -1)
                {
                    toClusterData[index] = new double[] { 1, double.Parse(row[columnNamesGroupBy].ToString()) };
                    patientIds[index] = row["id"].ToString();
                    coloumnValues[index] = double.Parse(row[columnNamesGroupBy].ToString());
                    index++;
                }
                else
                {
                    dataNotAvailable.Add(row["id"].ToString()); 
                }
            }

            KMeansResults result = KMeans.Cluster(toClusterData, clustersCount, iterations, 0);

            List<List<string>> patientIdsClusterList = new List<List<string>>();
            List<List<double>> coloumnValuesClusterList = new List<List<double>>();

            for (int i = 0; i < result.clusters.Length; i++)
            {
                patientIdsClusterList.Add(new List<string>());
                coloumnValuesClusterList.Add(new List<double>()); 

                for (int j = 0; j < result.clusters[i].Length; j++)
                {
                    patientIdsClusterList[i].Add(patientIds[result.clusters[i][j]]);
                    coloumnValuesClusterList[i].Add(coloumnValues[result.clusters[i][j]]); 
                }
            }

            filterIdsGroupBy.Add("N/A", dataNotAvailable);

            index = 0; 
            foreach(List<double> coloumnValuesCluster in coloumnValuesClusterList)
            {
                if (coloumnValuesCluster.Count < 1) continue; 
                double minValue = coloumnValuesCluster.Min<double>();
                double maxValue = coloumnValuesCluster.Max<double>();
                string key = minValue + " to " + maxValue;
                filterIdsGroupBy.Add(key, patientIdsClusterList[index]);
                index++; 
            }  

            return filterIdsGroupBy; 
        }

        //For AI 
        public DataTable DataForSimilarityCalculation(List<string> filteredPatientIds, string[] ignoredColumns)
        {
            string filter = "";
            int index = 0;

            foreach (DataColumn column in patientDetails.Columns)
            {
                //Like this remove column when necessary 
                bool columnIgnored = false;
                foreach (string ignoredColumn in ignoredColumns)
                {
                    if (column.ColumnName == ignoredColumn)
                    {
                        columnIgnored = true;
                    }
                }
                //Like this remove column when necessary 
                if (columnIgnored)
                {
                    continue;
                }
                //

                if (index > 0)
                {
                    filter += " AND ";
                }

                if (GetColumnType(column.ColumnName) == System.Type.GetType("System.Double"))
                {
                    filter += "( " + column.ColumnName + " <> -1 )";
                }
                else if (GetColumnType(column.ColumnName) == System.Type.GetType("System.String"))
                {
                    filter += "( " + column.ColumnName + " <> 'N/A' )";
                }
                else if (GetColumnType(column.ColumnName) == System.Type.GetType("System.Boolean"))
                {
                    filter += "( " + column.ColumnName + " = true )";
                }
                else if (GetColumnType(column.ColumnName) == System.Type.GetType("System.Int32"))
                {
                    filter += "( " + column.ColumnName + " <> -1 )";
                }

                index++;
            }

            //For PatientIds in filter 
            if(filter != "")
            {
                filter += " AND id in (";
            }
            else
            {
                filter += "id in (";
            }

            int subIndex = 0; 
            foreach(string filteredPatientId in filteredPatientIds)
            {
                if (subIndex == 0)
                {
                    filter += "'" + filteredPatientId + "'";
                }
                else
                {
                    filter += "," + "'" + filteredPatientId + "'";
                }
                subIndex++;
            }

            filter += ")";

            Debug.Log(filter); 

            //Creating a view for filter
            DataView filteredView = new DataView(patientDetails);
            filteredView.RowFilter = filter;
            DataTable filteredTable = filteredView.ToTable();

            return filteredTable; 
        }

        public List<string> GetDoubleColumns(string[] ignoredColumns)
        {
            List<string> requiredColumns = new List<string>();

            foreach (DataColumn column in patientDetails.Columns)
            {
                bool columnIgnored = false; 
                foreach(string ignoredColumn in ignoredColumns)
                {
                    if(column.ColumnName == ignoredColumn)
                    {
                        columnIgnored = true; 
                    }
                }

                //Like this remove column when necessary 
                if (columnIgnored)
                {
                    continue;
                }

                if (GetColumnType(column.ColumnName) == System.Type.GetType("System.Double"))
                {
                    requiredColumns.Add(column.ColumnName);
                }
            }

            return requiredColumns; 
        }

        public List<string> GetStringColumns(string[] ignoredColumns)
        {
            List<string> requiredColumns = new List<string>();

            foreach (DataColumn column in patientDetails.Columns)
            {
                bool columnIgnored = false;
                foreach (string ignoredColumn in ignoredColumns)
                {
                    if (column.ColumnName == ignoredColumn)
                    {
                        columnIgnored = true;
                    }
                }

                //Like this remove column when necessary 
                if (columnIgnored)
                {
                    continue;
                }

                if (GetColumnType(column.ColumnName) == System.Type.GetType("System.String"))
                {
                    requiredColumns.Add(column.ColumnName);
                }
            }

            return requiredColumns;
        }

        //Unnecessary - check later 
        public DataView QueryBuilderStringAndIntegerGroupBy(List<string> columnNamesString, List<List<string>> valuesString, List<string> columnNamesInteger, List<List<List<double>>> valuesInteger)
        {
            string filter = "";
            string integerFilter = "";
            string stringFilter = "";

            int indexInteger = 0;
            foreach (string columnName in columnNamesInteger)
            {
                if (indexInteger != 0)
                {
                    integerFilter += " AND ";
                }

                List<List<double>> valuesForThisColumnName = valuesInteger[indexInteger];

                int numberOfConditionsForThisColumn = 0;
                integerFilter += "( ";
                foreach (List<double> valueForThisColumnName in valuesForThisColumnName)
                {
                    if (numberOfConditionsForThisColumn != 0)
                    {
                        integerFilter += " OR ";
                    }

                    integerFilter += "( " + columnName + " >= " + valueForThisColumnName[0] + " AND " + columnName + " <= " + valueForThisColumnName[1] + " )";

                    numberOfConditionsForThisColumn++;
                }
                integerFilter += " )";

                indexInteger++;
            }

            int indexString = 0;
            foreach (string columnName in columnNamesString)
            {
                if (indexString == 0)
                {
                    stringFilter += columnName + " in (";
                }
                else
                {
                    stringFilter += " AND " + columnName + " in (";
                }

                List<string> valuesForThisColumnName = valuesString[indexString];
                int subIndex = 0;
                foreach (string value in valuesForThisColumnName)
                {
                    if (subIndex == 0)
                    {
                        stringFilter += "'" + value + "'";
                    }
                    else
                    {
                        stringFilter += "," + "'" + value + "'";
                    }
                    subIndex++;
                }
                stringFilter += ")";
                indexString++;
            }

            if (stringFilter != "" && integerFilter != "")
            {
                filter = integerFilter + " AND " + stringFilter;
            }
            else if (stringFilter != "")
            {
                filter = stringFilter;
            }
            else if (integerFilter != "")
            {
                filter = integerFilter;
            }

            if (filter != "")
            {
                filter += " AND (modelPresent = true) AND (notExample = true)";
            }
            else
            {
                filter += "(modelPresent = true) AND (notExample = true)";
            }

            Debug.Log(filter);

            //Creating a view for filter
            DataView filteredView = new DataView(patientDetails);
            filteredView.RowFilter = filter;
            return filteredView;
        }

        //Distinct values from providing column names 
        public List<string> GetFilteredPatientIdsInteger(List<string> columnNames, List<List<double>> values)
        {
            List<string> idList = new List<string>();
            DataView filteredView = QueryBuilderInteger(columnNames, values);
            if (filteredView == null)
            {
                return idList;
            }

            string[] requiredColumn = { "id" };
            DataTable filteredTable = filteredView.ToTable(true, requiredColumn);

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

        public DataView QueryBuilderInteger(List<string> columnNames, List<List<double>> values)
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

            if (filter != "")
            {
                filter += " AND (modelPresent = true) AND (notExample = true)";
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

            if (filter != "")
            {
                filter += " AND (modelPresent = true)";
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

        public CAS_FilterAndGroupUIStep.SortByStructure GetSortedBy(string columnNameSortBy)
        {
            CAS_FilterAndGroupUIStep.SortByStructure sorted = new CAS_FilterAndGroupUIStep.SortByStructure();

            string filter = "(modelPresent = true) AND (notExample = true)"; 

            DataView filterView = new DataView(patientDetails);
            filterView.RowFilter = filter; 
            string[] requiredColumn = { "id", columnNameSortBy };
            DataTable filteredTable = filterView.ToTable(false, requiredColumn);

            DataView filteredViewSorted = filteredTable.DefaultView;
            filteredViewSorted.Sort = columnNameSortBy + " asc";
            DataTable datatableSorted = filteredViewSorted.ToTable();

            Type filterOptionDataType = GetColumnType(columnNameSortBy);

            if (filterOptionDataType == System.Type.GetType("System.Double"))
            {
                sorted.isString = false;
            }
            else
            {
                sorted.isString = true;
            }

            List<string> patientIds = new List<string>();
            List<string> stringValues = new List<string>();
            List<double> doubleValues = new List<double>(); 

            foreach (DataRow row in datatableSorted.Rows)
            {
                patientIds.Add(row["id"].ToString());

                if (filterOptionDataType == System.Type.GetType("System.Double"))
                {
                    doubleValues.Add(double.Parse(row[columnNameSortBy].ToString()));
                }
                else
                {
                    stringValues.Add(row[columnNameSortBy].ToString());
                }
            }

            sorted.patientIds = patientIds;
            sorted.stringValues = stringValues;
            sorted.doubleValues = doubleValues;

            if (!sorted.isString)
            {
                sorted.min = doubleValues.Min();
                sorted.max = doubleValues.Max(); 
            }

            return sorted; 
        }
    }
}