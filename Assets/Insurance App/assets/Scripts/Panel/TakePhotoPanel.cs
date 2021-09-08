using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakePhotoPanel : MonoBehaviour, IPanel
{
    public RawImage photo;
    public InputField photoNote;
    public Text caseNumText;

    public void OnEnable()
    {
        caseNumText.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
    }

    public void TakePhotoButton()
    {
		TakePicture(512);
    }

    public void ProcessInfo()
    {
        if(!string.IsNullOrEmpty(photoNote.text))
        {
			UIManager.Instance.activeCase.photoTaken = photo;
			UIManager.Instance.activeCase.photoNote = photoNote.text;
        }    
    }

	private void TakePicture(int maxSize)
	{
		NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
		{
			Debug.Log("Image path: " + path);
			if (path != null)
			{
				// Create a Texture2D from the captured image
				Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize);
				if (texture == null)
				{
					Debug.Log("Couldn't load texture from " + path);
					return;
				}

				photo.texture = texture;
				photo.gameObject.SetActive(true);
			}
		}, maxSize);

		Debug.Log("Permission result: " + permission);
	}
}
