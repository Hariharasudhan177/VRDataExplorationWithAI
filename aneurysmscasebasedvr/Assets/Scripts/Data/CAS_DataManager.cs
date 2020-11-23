using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Linq;

namespace CAS
{
    //using LINQ and normal query - Use one in future 
    public class CAS_DataManager : MonoBehaviour
    {
        private DataTable patientDetails;

        public DataTable GetPatientDetails()
        {
            return patientDetails; 
        }

        public void SetPatientDetails(DataTable _patientDetails)
        {
            patientDetails = _patientDetails; 
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

        // Start is called before the first frame update
        void Start()
        {

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

            if(filter == "")
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

