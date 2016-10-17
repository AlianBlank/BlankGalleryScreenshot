// /**
//  *　　　　　　　　┏┓　　　┏┓+ +
//  *　　　　　　　┏┛┻━━━┛┻┓ + +
//  *　　　　　　　┃　　　　　　　┃ 　
//  *　　　　　　　┃　　　━　　　┃ ++ + + +
//  *　　　　　　 ████━████ ┃+
//  *　　　　　　　┃　　　　　　　┃ +
//  *　　　　　　　┃　　　┻　　　┃
//  *　　　　　　　┃　　　　　　　┃ + +
//  *　　　　　　　┗━┓　　　┏━┛
//  *　　　　　　　　　┃　　　┃　　　　　　　　　　　
//  *　　　　　　　　　┃　　　┃ + + + +
//  *　　　　　　　　　┃　　　┃　　　　Code is far away from bug with the animal protecting　　　　　　　
//  *　　　　　　　　　┃　　　┃ + 　　　　神兽保佑,代码无bug　　
//  *　　　　　　　　　┃　　　┃
//  *　　　　　　　　　┃　　　┃　　+　　　　　　　　　
//  *　　　　　　　　　┃　 　　┗━━━┓ + +
//  *　　　　　　　　　┃ 　　　　　　　┣┓
//  *　　　　　　　　　┃ 　　　　　　　┏┛
//  *　　　　　　　　　┗┓┓┏━┳┓┏┛ + + + +
//  *　　　　　　　　　　┃┫┫　┃┫┫
//  *　　　　　　　　　　┗┻┛　┗┻┛+ + + +
//  *
//  *
//  * ━━━━━━感觉萌萌哒━━━━━━
//  * 
//  * 说明：保存文件到相册的插件
//  *     Android 平台 
//  *       文件列表： 
//  *                 BlankGalleryScreenshot.jar 文件一个
//  *     IOS 平台 
//  *                 AHGalleryScreenshot.h 文件一个
//  *                 AHGalleryScreenshot.mm 文件一个
//  * 需要在清单文件中 添加一个Activity
//  *       <!-- 保存图片到相册插件 -->
//  *       <activity android:name="com.alianhome.galleryscreenshot.MainActivity" />
//  *
//  * 文件名：BlankGalleryScreenshot.cs
//  * 创建时间：2016年08月03日 
//  * 创建人：Blank Alian
//  * 联系方式：wangfj11@foxmail.com
//  */
#if UNITY_IPHONE || UNITY_IOS
using System.Runtime.InteropServices;
#endif
using UnityEngine;
using UnityEngine.Events;


public class BlankGalleryScreenshot : MonoBehaviour
{
#if UNITY_IPHONE || UNITY_IOS
    [DllImport("__Internal")]
    private static extern int addImageToGallery(string path);
#endif

    private static BlankGalleryScreenshot _instance;
    public static BlankGalleryScreenshot Instance
    {
        get
        {
            if (_instance == null)
            {
                string GalleryScreenshotBridgeLink = "GalleryScreenshotBridgeLink";
                GameObject go = GameObject.Find(GalleryScreenshotBridgeLink);
                if (go != null)
                {
                    Destroy(go);
                }
                go = new GameObject(GalleryScreenshotBridgeLink);
                _instance = go.AddComponent<BlankGalleryScreenshot>();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 把Application.persistentDataPath 目录下的图像文件移动到相册目录 并且注册到相册
    /// </summary>
    /// <param name="filePath"></param>
    public void SaveGalleryScreenshot(string filePath)
    {
#if UNITY_ANDROID

        AndroidJavaClass ajc = new AndroidJavaClass("com.alianhome.galleryscreenshot.MainActivity");

        ajc.CallStatic("addImageToGallery", filePath);
#endif
#if UNITY_IPHONE || UNITY_IOS
        addImageToGallery(filePath);
#endif
    }

    /// <summary>
    /// 状态发生改变的时候触发的事件 。目前只有两种情况  Start  OR Finish
    /// </summary>
    public event UnityAction<string> GalleryScreenshotStateChangeEvent;

    void GalleryScreenshotStateChange(string state)
    {
        OnGalleryScreenshotStateChangeEvent(state);

        if (state.Equals("Finish"))
        {
            Debug.Log("保存完成");
        }
        else if (state.Equals("Start"))
        {
            Debug.Log("开始保存");
        }
    }

    protected virtual void OnGalleryScreenshotStateChangeEvent(string state)
    {
        var handler = GalleryScreenshotStateChangeEvent;
        if (handler != null)
        {
            handler(state);
        }
    }
}
