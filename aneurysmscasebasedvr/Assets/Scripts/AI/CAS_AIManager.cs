using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Linq; 

namespace CAS
{
    public class CAS_AIManager : MonoBehaviour
    {
        public CAS_Manager manager; 

        public CAS_AIUI aiUI;
        
        List<CAS_ObjectOfInterest> objectsOfInterest;

        int currentIndexOfInterest = -1;

        double[] similarity;

        List<CAS_ContolModel> interestingLayer1;
        List<CAS_ContolModel> interestingLayer2;
        List<CAS_ContolModel> interestingLayer3;
        List<CAS_ContolModel> interestingLayer4;
        List<CAS_ContolModel> interestingLayer5;

        List<CAS_ContolModel> unInterestingModels;

        double similartiyMax = 0.5f;
        double similartiyMin = 0f;

        float uninterestedSimilartiy = 1.5f;

        bool similarityVisualisationStatus =  false;

        //Model Information 
        [HideInInspector]
        public Dictionary<string, GameObject> allModelsInformationByRecordName;
        [HideInInspector]
        public Dictionary<string, GameObject> allModelsInformationByGameObjectName;
        [HideInInspector]
        public Dictionary<string, List<string>> allModelsInformationGameobjectRecordName;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            /*if (Input.GetKeyDown(KeyCode.A))
            {
                SetObjectOfInterest(1);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                SetObjectOfInterest(0);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                ActivateSimilartiyVisualisation(); 
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                DeActivateSimilarityVisualisation();
            }*/
        }

        public List<CAS_ObjectOfInterest> GetObjectsOfInterest()
        {
            if(objectsOfInterest != null)
            {
                return objectsOfInterest;
            }

            return null; 
        }

        public void SetObjectsOfInterest(List<CAS_ObjectOfInterest> input)
        {
            objectsOfInterest = input;
            aiUI.objectOfInterestUI.UpdateExamplesDropDown(); 
        }

        public void SetObjectOfInterest(int indexOfInterest)
        {
            currentIndexOfInterest = indexOfInterest; 

            foreach (CAS_ObjectOfInterest example in objectsOfInterest)
            {
                example.gameObject.SetActive(false); 
            }

            objectsOfInterest[indexOfInterest].gameObject.SetActive(true);

            CalculateSimilarity(currentIndexOfInterest);
        }

        public void UnSetObjectOfInterest()
        {
            foreach (CAS_ObjectOfInterest example in objectsOfInterest)
            {
                example.gameObject.SetActive(false);
            }
        }

        public Dictionary<string, double> CalculateSimilarity(int indexOfInterest)
        {
            List<string> filteredLayerPatientIds = GetFilteredPatientIds();

            string[] ignoredColumns = new string[] { "manualAddedOthersFromCode", "morphoPresent", "id" };

            DataTable filteredTable = manager.dataManager.DataForSimilarityCalculation(filteredLayerPatientIds, ignoredColumns);

            DataRow GetPatientRecordWithIdAsDataRow = manager.dataManager.GetPatientRecordOfInterest(allModelsInformationGameobjectRecordName[objectsOfInterest[indexOfInterest].gameObject.name][0]);
            filteredTable.ImportRow(GetPatientRecordWithIdAsDataRow);

            double[] similarityDouble = CalculateDoubleColumnSimilarity(filteredTable, ignoredColumns);
            double[] similarityString = CalculateStringColumnSimilarity(filteredTable, ignoredColumns); 

            similarity = new double[similarityDouble.Length - 1];    
            Dictionary<string, double> similarityWithPatientId = new Dictionary<string, double>();             

            for (int i = 0; i < similarity.Length; i++)
            {
                similarity[i] = (similarityDouble[i] + similarityString[i]) / 2;

                if(!similarityWithPatientId.ContainsKey(filteredTable.Rows[i]["id"].ToString()))
                    similarityWithPatientId.Add(filteredTable.Rows[i]["id"].ToString(), similarity[i]); 
            }

            var similarityWithPatientIdOrdered = similarityWithPatientId.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            aiUI.similarityUI.PopulateData(similarityWithPatientIdOrdered);
            GetNearestNeighbours(similarityWithPatientIdOrdered, 4);
            SetSimilartiySettings(similarityWithPatientIdOrdered); 

            return similarityWithPatientIdOrdered; 
        }

        public double[] CalculateDoubleColumnSimilarity(DataTable filteredTable, string[] ignoredColumns)
        {
            List<string> requiredDoubleColumns = manager.dataManager.GetDoubleColumns(ignoredColumns);

            //Creating a view for filter
            DataView filteredView = new DataView(filteredTable);

            //Take double columns seperately. 
            DataTable filteredTableDoubleColoumns = filteredView.ToTable(false, requiredDoubleColumns.ToArray());

            //Normalization 
            Accord.Statistics.Filters.Normalization normalization = new Accord.Statistics.Filters.Normalization(filteredTableDoubleColoumns);

            // Now we can process another table at once:
            DataTable filteredTableDoubleColoumnsNormalized = normalization.Apply(filteredTableDoubleColoumns);

            double[] similarityDouble = new double[filteredTableDoubleColoumnsNormalized.Rows.Count - 1];

            DataRow questionRow = filteredTableDoubleColoumnsNormalized.Rows[filteredTableDoubleColoumnsNormalized.Rows.Count - 1];

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
                similarityDouble[rowIndex - 1] = distance / filteredTableDoubleColoumns.Columns.Count;
                rowIndex++;
            }

            return similarityDouble; 
        }

        public double[] CalculateStringColumnSimilarity(DataTable filteredTable, string[] ignoredColumns)
        {
            //string
            List<string> requiredStringColumns = manager.dataManager.GetStringColumns(ignoredColumns);

            //Creating a view for filter
            DataView filteredView = new DataView(filteredTable);

            //Take double columns seperately. 
            DataTable filteredTableStringColoumns = filteredView.ToTable(false, requiredStringColumns.ToArray());

            Accord.Statistics.Filters.Codification codification = new Accord.Statistics.Filters.Codification(filteredTableStringColoumns);
            DataTable filteredTableStringColoumnsEncoded = codification.Apply(filteredTableStringColoumns);

            double[][] similarityStringOfAllColumns = new double[filteredTableStringColoumnsEncoded.Columns.Count][];

           
            int columnIndex = 0;
            foreach (DataColumn column in filteredTableStringColoumnsEncoded.Columns)
            {
                double[] similarityStringForEachColumn = new double[filteredTableStringColoumnsEncoded.Rows.Count - 1];
                DataView dataView = new DataView(filteredTableStringColoumnsEncoded);
                DataTable filteredTableStringColoumnsEncodedFeatureSelected = dataView.ToTable(false, column.ColumnName);

                int rowIndexFiltered = 0;
                int[] indices = new int[filteredTableStringColoumnsEncoded.Rows.Count];
                foreach (DataRow row in filteredTableStringColoumnsEncodedFeatureSelected.Rows)
                {
                    indices[rowIndexFiltered] = (int)row.ItemArray[0];
                    rowIndexFiltered++;
                }

                double[][] result = Accord.Math.Jagged.OneHot(indices);

                double[] questionRowString = result[result.Length-1];

                int rowIndexFilteredOtherRows = 0;
                foreach (double[] otherRows in result)
                {
                    if (rowIndexFilteredOtherRows == 0)
                    {
                        rowIndexFilteredOtherRows++;
                        continue;
                    }

                    double distance = Accord.Math.Distance.Dice(questionRowString, otherRows);
                    similarityStringForEachColumn[rowIndexFilteredOtherRows - 1] = distance;
                    rowIndexFilteredOtherRows++;
                }

                similarityStringOfAllColumns[columnIndex] = similarityStringForEachColumn;
                columnIndex++;
            }

            double[] similarityString = new double[filteredTableStringColoumnsEncoded.Rows.Count - 1];

            int eachRowIndex = 0;
            foreach (double eachRow in similarityString)
            {
                foreach (double[] similarityStringOfAllColumn in similarityStringOfAllColumns)
                {
                    similarityString[eachRowIndex] += similarityStringOfAllColumn[eachRowIndex];
                }
                similarityString[eachRowIndex] = similarityString[eachRowIndex] / filteredTableStringColoumnsEncoded.Columns.Count;
                eachRowIndex++;
            }

            return similarityString;
        }

        public void GetNearestNeighbours(Dictionary<string, double> similarityWithPatientId, int count)
        {
            Dictionary<string, double> neighbours = new Dictionary<string, double>();
            List<string> patientIds = similarityWithPatientId.Keys.ToList(); 

            for(int i=0; i< count; i++)
            {
                neighbours.Add(patientIds[i], similarityWithPatientId[patientIds[i]]); 
            }

            Classification(neighbours); 
        }

        public void Classification(Dictionary<string, double> similarityWithPatientId)
        {
            Dictionary<string, List<string>> targets = new Dictionary<string, List<string>>();
            Dictionary<string, string> neighboursWithTarget = new Dictionary<string, string>();

            foreach (string patientId in similarityWithPatientId.Keys)
            {
                string target = manager.dataManager.GetTargetValue(patientId);
                //neighboursWithTarget.Add(patientId, target + " - " + similarityWithPatientId[patientId].ToString("0.0000"));
                neighboursWithTarget.Add(patientId, target);

                if (!targets.Keys.Contains(target))
                {
                    List<string> patientIds = new List<string>();
                    patientIds.Add(patientId); 
                    targets.Add(target, patientIds);
                }
                else{
                    targets[target].Add(patientId); 
                }
            }

            Dictionary<string, double> weights = new Dictionary<string, double>(); 

            foreach (string target in targets.Keys)
            {
                double weight = 0f; 
                foreach (string patientId in targets[target])
                {
                    if(similarityWithPatientId[patientId] != 0)
                    {
                        weight += 1 / similarityWithPatientId[patientId];
                    }
                }
                weights.Add(target, weight); 
            }

            var orderedBySimilarity = weights.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            aiUI.classificationUI.PopulateData(neighboursWithTarget, orderedBySimilarity.Keys.ToList()[orderedBySimilarity.Keys.ToList().Count-1]); 
        }

        public List<string> GetFilteredPatientIds()
        {
            List<string> frontLayerPatientIds = new List<string>();

            foreach (string patientId in manager.stepManager.stepParents[0].GetModelsInThisStep().Keys.ToList())
            {
                frontLayerPatientIds.Add(manager.stepManager.allModelsInformationGameobjectRecordName[patientId][0]);
            }

            return frontLayerPatientIds; 
        }

        public void SetSimilartiySettings(Dictionary<string, double> similarityWithPatientIdOrdered)
        {
            interestingLayer1 = new List<CAS_ContolModel>();
            interestingLayer2 = new List<CAS_ContolModel>();
            interestingLayer3 = new List<CAS_ContolModel>();
            interestingLayer4 = new List<CAS_ContolModel>();
            interestingLayer5 = new List<CAS_ContolModel>();

            unInterestingModels = new List<CAS_ContolModel>();

            foreach (CAS_EachStepManager eachStepParent in manager.stepManager.stepParents)
            {
                foreach (string patientId in eachStepParent.GetModelsInThisStep().Keys.ToList())
                {
                    CAS_ContolModel controlModel = manager.stepManager.stepParents[0].GetModelsInThisStep()[patientId].GetComponent<CAS_ContolModel>();
                    controlModel.SetSimilarity(uninterestedSimilartiy, 3);
                    unInterestingModels.Add(controlModel);
                }
            }

            List<string> patientIdKeys = similarityWithPatientIdOrdered.Keys.ToList(); 

            foreach (string patientId in patientIdKeys)
            {
                if (manager.stepManager.stepParents[0].GetModelsInThisStep().ContainsKey(patientId))
                {
                    CAS_ContolModel controlModel = manager.stepManager.stepParents[0].GetModelsInThisStep()[patientId].GetComponent<CAS_ContolModel>();

                    if (interestingLayer1.Count < 5)
                    {
                        controlModel.SetSimilarity(similarityWithPatientIdOrdered[patientId], 1);
                        interestingLayer1.Add(controlModel);
                    }
                    else if (interestingLayer2.Count < 10)
                    {
                        controlModel.SetSimilarity(similarityWithPatientIdOrdered[patientId], 1);
                        interestingLayer2.Add(controlModel);
                    }
                    else if (interestingLayer3.Count < 15)
                    {
                        controlModel.SetSimilarity(similarityWithPatientIdOrdered[patientId], 1);
                        interestingLayer3.Add(controlModel);
                    }
                    else if (interestingLayer4.Count < 20)
                    {
                        controlModel.SetSimilarity(similarityWithPatientIdOrdered[patientId], 1);
                        interestingLayer4.Add(controlModel);
                    }
                    else
                    {
                        controlModel.SetSimilarity(similarityWithPatientIdOrdered[patientId], 2);
                        interestingLayer5.Add(controlModel);
                    }

                    if (unInterestingModels.Contains(controlModel)) unInterestingModels.Remove(controlModel); 
                }
            }

            similartiyMin = similarityWithPatientIdOrdered[patientIdKeys[0]];
            similartiyMax = similarityWithPatientIdOrdered[patientIdKeys[patientIdKeys.Count - 1]]; 
        }

        public void ActivateSimilartiyVisualisation()
        {
            int index = 0; 

            foreach(CAS_EachStepManager eachStepParent in manager.stepManager.stepParents)
            {
    
                if (index == 0)
                {
                    PlaceObjectsWithLayer();
                }
                else
                {
                    foreach (string patientId in manager.stepManager.stepParents[0].GetModelsInThisStep().Keys.ToList())
                    {
                        manager.stepManager.stepParents[0].GetModelsInThisStep()[patientId].GetComponent<CAS_ContolModel>().ActivateSimilartiySettings(new Vector3(0f,0f,0f), Color.white);
                    }
                }

                index++; 
            }
        }

        public void DeActivateSimilarityVisualisation()
        {
            foreach (CAS_EachStepManager eachStepParent in manager.stepManager.stepParents)
            {
                foreach (string patientId in eachStepParent.GetModelsInThisStep().Keys.ToList())
                {
                    CAS_ContolModel controlModel = manager.stepManager.stepParents[0].GetModelsInThisStep()[patientId].GetComponent<CAS_ContolModel>();
                    controlModel.DeActivateSimilartiySettings();
                }
            }
        }

        public void PlaceObjectsWithLayer()
        {
            PlaceObjects(interestingLayer1);
            PlaceObjects(interestingLayer2);
            PlaceObjects(interestingLayer3);
            PlaceObjects(interestingLayer4);
            PlaceObjects(interestingLayer5);

            PlaceObjects(unInterestingModels);
        }

        public void PlaceObjects(List<CAS_ContolModel> models)
        {
            int totalNumberOfModels = models.Count;

            float angleRequired = 90f / (totalNumberOfModels/2f);
            float radius = 1f;

            int index = 0;
            int leftIndex = 0;
            int rightIndex = 0;
            bool toIncreaseLeft = false;

            foreach (CAS_ContolModel model in models)
            {
                float currentAngle = 90f;

                if (toIncreaseLeft)
                {
                    currentAngle -= (leftIndex * angleRequired);
                }
                else
                {
                    currentAngle += (rightIndex * angleRequired);
                } 

                float adjustLengthOfRadius = radius * (float)model.GetSimilarity() * 10f;

                model.ActivateSimilartiySettings(new Vector3(
                    adjustLengthOfRadius * Mathf.Cos(currentAngle * (Mathf.PI / 180f)),
                    1.5f,
                    adjustLengthOfRadius * Mathf.Sin(currentAngle * (Mathf.PI / 180f))),
                    InterpolateScaleColour((float)model.GetSimilarity())); 

                index++;

                if (toIncreaseLeft)
                {
                    toIncreaseLeft = false;
                    rightIndex++;
                }
                else
                {
                    toIncreaseLeft = true;
                    leftIndex++;
                }

            }
        }

        public Color InterpolateScaleColour(float similartiy)
        {

            float a = 1f;

            float rMax = 1f;
            float rMin = 0.65f;

            float gMax = 0.95f;
            float gMin = 0.25f;

            float bMax = 0.75f;
            float bMin = 0f;

            float g = gMax - ((((float)similartiyMax - similartiy) / ((float)similartiyMax - (float)similartiyMin)) * (gMax - gMin));
            float b = bMax - ((((float)similartiyMax - similartiy) / ((float)similartiyMax - (float)similartiyMin)) * (bMax - bMin));
            float r = rMax - ((((float)similartiyMax - similartiy) / ((float)similartiyMax - (float)similartiyMin)) * (rMax - rMin));

            return new Color(r, g, b, a);

        }

        public void SetAllModelsInformation(Dictionary<string, GameObject> allModelsInformation)
        {
            allModelsInformationByRecordName = new Dictionary<string, GameObject>();
            allModelsInformationByGameObjectName = new Dictionary<string, GameObject>();
            allModelsInformationGameobjectRecordName = new Dictionary<string, List<string>>();

            allModelsInformationByRecordName = allModelsInformation;

            foreach (string id in allModelsInformationByRecordName.Keys)
            {

                if (!allModelsInformationByGameObjectName.ContainsKey(allModelsInformation[id].name))
                {
                    allModelsInformationByGameObjectName.Add(allModelsInformation[id].name, allModelsInformation[id]);
                }

                if (allModelsInformationGameobjectRecordName.ContainsKey(allModelsInformation[id].name))
                {
                    allModelsInformationGameobjectRecordName[allModelsInformation[id].name].Add(id);
                }
                else
                {
                    List<string> idList = new List<string>();
                    idList.Add(id);
                    allModelsInformationGameobjectRecordName.Add(allModelsInformation[id].name, idList);
                }

            }
        }
    }
}