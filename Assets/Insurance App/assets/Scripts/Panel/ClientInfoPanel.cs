using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientInfoPanel : MonoBehaviour, IPanel
{
    public Text caseNumberText;
    public InputField firstNameInput, lastNameInput;

    private void OnEnable()
    {
        caseNumberText.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
    }

    public void ProcessInfo()
    {
        string inputFirstName = firstNameInput.text;
        string inputLastName = lastNameInput.text;
        if(!string.IsNullOrEmpty(inputFirstName) || !string.IsNullOrEmpty(inputLastName))
        {
            UIManager.Instance.NewCaseName(inputFirstName, inputLastName);
        }
        else
        {
            //error message
            Debug.Log("Please input name!");
        }
    }
}
