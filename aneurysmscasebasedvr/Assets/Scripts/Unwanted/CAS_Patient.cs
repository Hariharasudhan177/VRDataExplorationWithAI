using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAS_Patient
{
    //If you really need aditional information in order to se It correctly then the recommended solution is to implement the setter as a method
    //Setters and getter necessary to perform validation 

    public string id; 

    public string getID()
    {
        return id; 
    }
    public void setID(string value)
    {
        id = value; 
    }

    public string institution;

    public string getInstitution()
    {
        return institution;
    }
    public void setInstituition(string value)
    {
        institution = value;
    }

    public string modality;

    public string getModality()
    {
        return modality;
    }
    public void setModality(string value)
    {
        modality = value;
    }

    public int age;

    public int getAge()
    {
        return age;
    }
    public void setAge(string value)
    {
        age = int.Parse(value);
    }

    public string sex;

    public string getSex()
    {
        return sex;
    }
    public void setSex(string value)
    {
        sex = value;
    }

    public string aneurysmType;

    public string getAneurysmType()
    {
        return aneurysmType;
    }
    public void setAneurysmType(string value)
    {
        aneurysmType = value;
    }

    public string aneurysmLocation;

    public string getAneurysmLocation()
    {
        return aneurysmLocation;
    }
    public void setAneurysmLocation(string value)
    {
        aneurysmLocation = value;
    }

    public string ruptureStatus;

    public string getRuptureStatus()
    {
        return ruptureStatus;
    }
    public void setRuptureStatus(string value)
    {
        ruptureStatus = value;
    }

    public string multipleAneurysms;

    public string getMultipleAneurysms()
    {
        return multipleAneurysms;
    }
    public void setMultipleAneurysms(string value)
    {
        multipleAneurysms = value;
    }

    public string medicalHistory;

    public string getMedicalHistory()
    {
        return medicalHistory;
    }
    public void setMedicalHistory(string value)
    {
        medicalHistory = value;
    }

    public string comment;

    public string getComment()
    {
        return institution;
    }
    public void setComment(string value)
    {
        institution = value;
    }

    //starting from 2nd number to avoid column heading and leaving last one for null 
    /*for (int i = 1; i < dataRows.Length-1; i++)
    {   
        if(dataRows[i] != null)
        {
            string[] resultsComma = dataRows[i].Split(',');
            CAS_Patient newPatient = new CAS_Patient();
            newPatient.id = resultsComma[0];
            newPatient.institution = resultsComma[1];
            newPatient.modality = resultsComma[2];
            //newPatient.age = resultsComma[3];
            newPatient.sex = resultsComma[4];
            newPatient.aneurysmType = resultsComma[5];
            newPatient.aneurysmLocation = resultsComma[6];
            newPatient.ruptureStatus = resultsComma[7];
            //newPatient.multipleAneurysms = resultsComma[8];

            //newPatient.medicalHistory = resultsComma[9];
            //newPatient.comment = resultsComma[10];        

            //for(int j = 0; j <= 1000; j++)
            //{
            //    patientDetails.Add(newPatient);
            //}

        }
    }*/
}
