using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchPanel : MonoBehaviour, IPanel
{
    public InputField caseNumberInput;
    public SelectPanel selectPanel;

    public void ProcessInfo()
    {
        //download the list from S3 bucket
        AWSManager.Instance.GetList(caseNumberInput.text, () =>
        {
            selectPanel.gameObject.SetActive(true);
        });

        //compare those to caseNumberInput

        //if find a match
        //download that object 
    }
}
