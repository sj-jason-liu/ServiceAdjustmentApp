using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour, IPanel
{
    public Text infoText;

    public void OnEnable()
    {
        infoText.text = UIManager.Instance.activeCase.name;
    }

    public void ProcessInfo()
    {
        
    }
}
