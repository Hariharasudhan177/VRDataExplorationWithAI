using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

namespace CAS
{
    public class CAS_SimilarityMetric : MonoBehaviour
    {
        public CAS_DataManager dataManager;

        DataTable patientDetails; 

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            /*if (Input.GetKeyDown(KeyCode.A))
            {
                patientDetails = dataManager.GetPatientDetails();
                CalculateSimilarity(); 
            }*/
        }

        public void CalculateSimilarity()
        {
            //Casebase cleaning 
            //For categorical vairables NA will be considered as new category. But this will mean two NA will be equal cases 
            //For Numerical values -1 will not be considerer or removed for case base .. So will be in the last layer.
            //Later can be considered 

            string filter = "";

            List<string> requiredDoubleColumns = new List<string>();
            List<string> requiredStringColumns = new List<string>();

            int index = 0;

            foreach (DataColumn column in patientDetails.Columns)
            {
                //Like this remove column when necessary 
                if (column.ColumnName == "manualAddedOthersFromCode" || column.ColumnName == "morphoPresent" || column.ColumnName == "id")
                {
                    continue;
                }

                if (index > 0)
                {
                    filter += " AND ";
                }

                if (dataManager.GetColumnType(column.ColumnName) == System.Type.GetType("System.Double"))
                {
                    filter += "( " + column.ColumnName + " <> -1 )";
                    requiredDoubleColumns.Add(column.ColumnName);
                }
                else if (dataManager.GetColumnType(column.ColumnName) == System.Type.GetType("System.String")) 
                {
                    filter += "( " + column.ColumnName + " <> 'N/A' )";
                    requiredStringColumns.Add(column.ColumnName);
                } 
                else if (dataManager.GetColumnType(column.ColumnName) == System.Type.GetType("System.Boolean")) 
                {
                    filter += "( " + column.ColumnName + " = true )";
                }
                else if (dataManager.GetColumnType(column.ColumnName) == System.Type.GetType("System.Int32")) 
                {
                    filter += "( " + column.ColumnName + " <> -1 )";
                }

                index++;
            }

            Debug.Log(filter);

            //Creating a view for filter
            DataView filteredView = new DataView(patientDetails);
            filteredView.RowFilter = filter;
            DataTable filteredTable = filteredView.ToTable();

            Debug.Log(filteredTable.Rows.Count);

            //Take double columns seperately. 
            DataTable filteredTableDoubleColoumns = filteredView.ToTable(false, requiredDoubleColumns.ToArray());
            //Take string columns seperately. 
            DataTable filteredTableStringColoumns = filteredView.ToTable(false, requiredStringColumns.ToArray());

            //double 
            Accord.Statistics.Filters.Normalization normalization = new Accord.Statistics.Filters.Normalization(filteredTableDoubleColoumns);
            // Now we can process another table at once:
            DataTable filteredTableDoubleColoumnsNormalized = normalization.Apply(filteredTableDoubleColoumns);

            Debug.Log(filteredTableDoubleColoumnsNormalized.ToString());

            double[] similarityDouble = new double[filteredTableDoubleColoumnsNormalized.Rows.Count - 1]; 

            DataRow questionRow = filteredTableDoubleColoumnsNormalized.Rows[0];

            int rowIndex = 0; 
            foreach (DataRow dataRow in filteredTableDoubleColoumnsNormalized.Rows)
            {
                if (rowIndex == 0)
                {
                    rowIndex++;
                    continue; 
                }

                int itemIndex = 0;

                double[] questionArray = new double[dataRow.ItemArray.Length];
                double[] currentArray = new double[dataRow.ItemArray.Length];

                foreach (var item in dataRow.ItemArray)
                {
                    questionArray[itemIndex] = double.Parse(questionRow[itemIndex].ToString());
                    currentArray[itemIndex] = double.Parse(item.ToString());
                    itemIndex++;
                }
                //weight if needed can be given here somehow 
                double distance = Accord.Math.Distance.Euclidean(questionArray, currentArray);
                similarityDouble[rowIndex-1] = distance/ filteredTableDoubleColoumns.Columns.Count; 
                rowIndex++; 
            }

            Debug.Log(string.Join("\n", similarityDouble));

            //string
            Accord.Statistics.Filters.Codification codification = new Accord.Statistics.Filters.Codification(filteredTableStringColoumns);
            DataTable filteredTableStringColoumnsEncoded = codification.Apply(filteredTableStringColoumns);

            double[][] similarityStringOfAllColumns = new double[filteredTableStringColoumnsEncoded.Columns.Count][];

            int columnIndex = 0; 
            foreach(DataColumn column in filteredTableStringColoumnsEncoded.Columns)
            {
                double[] similarityStringForEachColumn = new double[filteredTableStringColoumnsEncoded.Rows.Count-1]; 
                DataView dataView = new DataView(filteredTableStringColoumnsEncoded);
                DataTable filteredTableStringColoumnsEncodedFeatureSelected = dataView.ToTable(false, column.ColumnName);

                int rowIndexFiltered = 0;
                int[] indices = new int[filteredTableStringColoumnsEncoded.Rows.Count]; 
                foreach (DataRow row in filteredTableStringColoumnsEncodedFeatureSelected.Rows)
                {
                    indices[rowIndexFiltered] = (int) row.ItemArray[0]; 
                    rowIndexFiltered++; 
                }

                double[][] result = Accord.Math.Jagged.OneHot(indices);

                double[] questionRowString = result[0];

                int rowIndexFilteredOtherRows = 0; 
                foreach(double[] otherRows in result)
                {
                    if (rowIndexFilteredOtherRows == 0)
                    {
                        rowIndexFilteredOtherRows++; 
                        continue; 
                    }

                    double distance = Accord.Math.Distance.Dice(questionRowString, otherRows);
                    similarityStringForEachColumn[rowIndexFilteredOtherRows-1] = distance;
                    rowIndexFilteredOtherRows++; 
                }

                similarityStringOfAllColumns[columnIndex] = similarityStringForEachColumn;
                columnIndex++; 
            }

            double[] similarityString = new double[filteredTableStringColoumnsEncoded.Rows.Count - 1];

            int eachRowIndex = 0; 
            foreach(double eachRow in similarityString)
            {
                foreach (double[] similarityStringOfAllColumn in similarityStringOfAllColumns)
                {
                    similarityString[eachRowIndex] += similarityStringOfAllColumn[eachRowIndex]; 
                }
                similarityString[eachRowIndex] = similarityString[eachRowIndex] / filteredTableStringColoumnsEncoded.Columns.Count;
                eachRowIndex++; 
            }

            foreach (DataRow row in filteredTableStringColoumns.Rows)
            {
                //Debug.Log(string.Join(",", row.ItemArray)); 
            }

           
            Debug.Log(string.Join("\n", similarityString));

            double[] similarityTotal = new double[filteredTableStringColoumnsEncoded.Rows.Count - 1];

            for(int i=0; i<similarityTotal.Length; i++)
            {
                similarityTotal[i] = (similarityDouble[i] + similarityString[i]) / 2; 
            }

            Debug.Log(string.Join("\n", similarityTotal));
        }
    }
}

