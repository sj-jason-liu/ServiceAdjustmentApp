using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OverviewInfoPanel : MonoBehaviour, IPanel
{
    public Text caseNumText;
    public Text nameText;
    public Text dateText;
    public Text locationNote;
    public RawImage photoTaken;
    public Text photoNote;

    public void OnEnable()
    {
        caseNumText.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
        nameText.text = "NAME " + UIManager.Instance.activeCase.name;
        dateText.text = "DATE " + DateTime.Today.ToString();
        locationNote.text = UIManager.Instance.activeCase.locationNote;
        photoTaken = UIManager.Instance.activeCase.photoTaken;
        photoNote.text = UIManager.Instance.activeCase.photoNote;
    }

    public void ProcessInfo()
    {
        throw new System.NotImplementedException();
    }
}
