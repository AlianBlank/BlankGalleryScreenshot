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
            if (!string.IsNullOrEmpty(imageFilePath))
            {
                BlankGalleryScreenshot.Instance.AddImageToGallery(imageFilePath);
            }
        }
        if (GUILayout.Button("Copy Video", GUILayout.Width(200), GUILayout.Height(200)))
        {
            StartCoroutine(MoveVideo());
        }
        if (GUILayout.Button("Save Video", GUILayout.Width(200), GUILayout.Height(200)))
        {
            if (!string.IsNullOrEmpty(videoFilePath))
            {
                BlankGalleryScreenshot.Instance.AddVideoToGallery(videoFilePath);
            }
        }
    }

    string videoFilePath;

    private IEnumerator MoveVideo()
    {
        WWW www = new WWW(Application.streamingAssetsPath + "/videodemo.mp4");
        yield return www;

        Debug.Log(www.error);
        Debug.Log("read finsh");
        videoFilePath = Application.persistentDataPath + "/videodemo.mp4";
        Debug.Log(videoFilePath);
        File.WriteAllBytes(videoFilePath, www.bytes);
        www.Dispose();
    }


    string imageFilePath;

    private IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        texture2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture2D.Apply();
        imageFilePath = Application.persistentDataPath + "/" + DateTime.Now.ToFileTime() + ".jpg";
        Debug.Log(imageFilePath);

        File.WriteAllBytes(imageFilePath, texture2D.EncodeToPNG());
        Destroy(texture2D);
        texture2D = null;
        Resources.UnloadUnusedAssets();
        GC.Collect();
        BlankGalleryScreenshot.Instance.AddImageToGallery(imageFilePath);
    }
}
