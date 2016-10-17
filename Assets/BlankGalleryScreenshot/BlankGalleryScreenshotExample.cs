using System;
using UnityEngine;
using System.Collections;
using System.IO;

public class BlankGalleryScreenshotExample : MonoBehaviour
{
    void OnGUI()
    {
        if (GUILayout.Button("Save", GUILayout.Width(200), GUILayout.Height(200)))
        {
			StartCoroutine(CaptureScreenshot());
        }
		if (GUILayout.Button("Save Image", GUILayout.Width(200), GUILayout.Height(200)))
		{
			if (!string.IsNullOrEmpty (filePath)) {
				BlankGalleryScreenshot.Instance.SaveGalleryScreenshot(filePath);
			}
		}
    }
	string filePath;

	private IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();

		Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        texture2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture2D.Apply();
		texture2D.Compress (false);
        filePath = Application.persistentDataPath + "/" + DateTime.Now.ToFileTime() + ".jpg";
		Debug.Log (filePath);

	    File.WriteAllBytes(filePath, texture2D.EncodeToPNG ());
		Destroy (texture2D);
		texture2D = null;
		Resources.UnloadUnusedAssets ();
		GC.Collect ();
        BlankGalleryScreenshot.Instance.SaveGalleryScreenshot(filePath);
    }
}
