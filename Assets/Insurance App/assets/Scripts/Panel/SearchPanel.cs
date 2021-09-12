using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchPanel : MonoBehaviour, IPanel
{
    public InputField caseNumberInput;

    public void ProcessInfo()
    {
        //download the list from S3 bucket
        AWSManager.Instance.GetList(caseNumberInput.text);

        //compare those to caseNumberInput

        //if find a match
        //download that object 
    }
}
