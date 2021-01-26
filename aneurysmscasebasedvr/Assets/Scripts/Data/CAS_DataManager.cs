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

        // Update is called once per frame
        void Update()
        {

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

            manager.stepManager.SetAllModelsInformation(allModelsInformation);

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

            manager.filterAndGroupUIManager.PopulateFilterOptions();
            manager.displayPatientDetailsUIManager.PopulatePatientDisplay();
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

        //Distinct values from providing column names 
        public List<string> GetUniquePatientIds()
        {
            List<string> idList = new List<string>();

            string[] requiredColumn = { "id" };
            DataView view = new DataView(patientDetails);
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

        public TypeOfOptions GetColumnTypeOfOption(string columnHeading)
        {
            return columnTypeOfOption[columnHeading]; 
        }

        //Distinct values from providing column names 
        public Dictionary<string, List<string>> GetPatientIdsGroupBy(List<string> columnNamesString, List<List<string>> valuesString, List<string> columnNamesInteger, List<List<List<double>>> valuesInteger, string columnNamesGroupBy)
        {
            DataView filteredView = QueryBuilderStringAndIntegerGroupBy(columnNamesString, valuesString, columnNamesInteger, valuesInteger);
            if (filteredView == null)
            {
                return new Dictionary<string, List<string>>();
            }

            string[] requiredColumn = { "id", columnNamesGroupBy };

            DataTable filteredTable = filteredView.ToTable(true, requiredColumn);

            DataView filteredViewSorted = filteredTable.DefaultView;
            filteredViewSorted.Sort = columnNamesGroupBy + " desc";
            DataTable datatableSorted = filteredViewSorted.ToTable();

            Type filterOptionDataType = GetColumnType(columnNamesGroupBy);

            if (filterOptionDataType == System.Type.GetType("System.Double"))
            {
                return GetPatientIdsGroupByDouble(datatableSorted, columnNamesGroupBy);
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

            foreach (DataRow row in datatableSorted.Rows)
            {
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
                        filterIdsGroupedByIds = new List<string>();
                        filterIdsGroupedByIds.Add(row["id"].ToString());
                    }
                }
            }

            //Debug.Log(previousFilterOption + " " + currentFilterOption + " " + filterIdsGroupedByIds.Count);

            if (!filterIdsGroupBy.ContainsKey(currentFilterOption)) filterIdsGroupBy.Add(currentFilterOption, filterIdsGroupedByIds);

            return filterIdsGroupBy;
        }

        //using clustering to groupby numeric columns 
        public Dictionary<string, List<string>> GetPatientIdsGroupByDouble(DataTable datatableSorted, string columnNamesGroupBy)
        {
            Dictionary<string, List<string>> filterIdsGroupBy = new Dictionary<string, List<string>>();

            int clustersCount = 4;
            int iterations = 50;

            string[] patientIds = new string[datatableSorted.Rows.Count];
            double[] coloumnValues = new double[datatableSorted.Rows.Count]; 
            double[][] toClusterData = new double[datatableSorted.Rows.Count][]; 


            int index = 0; 
            foreach (DataRow row in datatableSorted.Rows)
            {
                toClusterData[index] = new double[] { 1, double.Parse(row[columnNamesGroupBy].ToString()) };
                patientIds[index] = row["id"].ToString();
                coloumnValues[index] = double.Parse(row[columnNamesGroupBy].ToString());
                index++; 
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

            index = 0; 
            foreach(List<double> coloumnValuesCluster in coloumnValuesClusterList)
            {
                double minValue = coloumnValuesCluster.Min<double>();
                double maxValue = coloumnValuesCluster.Max<double>();
                string key = minValue + " to " + maxValue;
                filterIdsGroupBy.Add(key, patientIdsClusterList[index]);
                index++; 
            }  

            return filterIdsGroupBy; 
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

                    integerFilter += "( " + columnName + " >= " + valuesForThisColumnName[0] + " AND " + columnName + " <= " + valuesForThisColumnName[1] + " )";

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
                filter += " AND (modelPresent = true)";
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
    }
}