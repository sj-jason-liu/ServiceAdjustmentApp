using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UIManager is NULL");
            }

            return _instance;
        }
    }

    public Case activeCase;
    public ClientInfoPanel clientInfoPanel;
    public LocationPanel locationPanel;
    public GameObject borderPanel;

    private void Awake()
    {
        _instance = this;
    }

    public void CreateNewCase()
    {
        activeCase = new Case();
        int newCaseID = Random.Range(0, 999);
        activeCase.caseID = newCaseID.ToString();

        clientInfoPanel.gameObject.SetActive(true);
        borderPanel.SetActive(true);
    }

    public void SubmitButton()
    {
        Case awsCase = new Case();
        awsCase.caseID = activeCase.caseID;
        awsCase.name = activeCase.name;
        awsCase.date = activeCase.date;
        awsCase.locationNote = activeCase.locationNote;
        awsCase.photoTaken = activeCase.photoTaken;
        awsCase.photoNote = activeCase.photoNote;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/case#" + awsCase.caseID + ".dat");
        bf.Serialize(file, awsCase);
        file.Close();
    }
}
