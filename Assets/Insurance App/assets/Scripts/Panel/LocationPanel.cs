using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LocationPanel : MonoBehaviour, IPanel
{
    public RawImage mapImg;
    public InputField mapNote;
    public Text caseNumberText;

    public string apiKey;

    public float xCord, yCord;
    public int zoom;
    public int imgSize;
    public string url = "https://maps.googleapis.com/maps/api/staticmap?";
    public string addressUrl = "https://maps.googleapis.com/maps/api/geocode/json?";

    public void OnEnable()
    {
        caseNumberText.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
    }

    public IEnumerator Start()
    {
        //fetch GEO data
        //if user has enable the location service, then continue
        if(Input.location.isEnabledByUser)
        {
            //start service
            Input.location.Start();

            //wait for a maximum time to proceed
            int maxWait = 20;
            while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1f);
                maxWait--;
            }

            //service didn't initialize in 20 secs
            if(maxWait < 1)
            {
                Debug.LogError("Service overtime!");
                yield break;
            }

            if(Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.LogError("Unable to determine device location!");
            }
            else
            {
                //get the coordinate from LocationService
                xCord = Input.location.lastData.latitude;
                yCord = Input.location.lastData.longitude;
            }

            //stop service
            Input.location.Stop();
        }
        else
        {
            Debug.LogError("You need to enable location service.");
        }    


        StartCoroutine(GetLocationCoroutine());
    }

    IEnumerator GetLocationCoroutine()
    {
        //construct url
        url = url + "center=" + xCord + "," + yCord + "&zoom=" + zoom + "&size=" + imgSize + "x" + imgSize + "&key=" + apiKey;

        //download the map
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return webRequest.SendWebRequest(); //wait until return the map data
            if (webRequest.error != null)
            {
                //if return error, show error message
                Debug.LogError("Map Error");
            }

            //make image texture as map texture
            mapImg.texture = DownloadHandlerTexture.GetContent(webRequest);
        }
    }

    IEnumerator GetLocationAddress()
    {
        addressUrl = addressUrl + "latlng=" + xCord + "," + yCord + "&key=" + apiKey;
        
        using (UnityWebRequest webLocation = UnityWebRequest.Get(addressUrl))
        {
            yield return webLocation.SendWebRequest();
            if(webLocation.error != null)
            {
                Debug.LogError("Getting address error");
            }
        }

        //JsonUtility.FromJson(addressUrl);
    }

    public void ProcessInfo()
    {
        if (!string.IsNullOrEmpty(mapNote.text))
        {
            UIManager.Instance.activeCase.locationNote = mapNote.text;
        }
    }
}
