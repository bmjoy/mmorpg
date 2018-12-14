using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Menu  {

    [MenuItem("FXCTools/Settings")]
    public static void Settings()
    {
        SettingWindow window = (SettingWindow)EditorWindow.GetWindow(typeof(SettingWindow));
        window.titleContent = new GUIContent("全局设置");
        window.Show();
    }


    [MenuItem("FXCTools/CreateAssetBundles")]
    public static void CreateAssetBundles()
    {
        AssetBundleWindow win = EditorWindow.GetWindow<AssetBundleWindow>();
        win.titleContent=new GUIContent("资源打包");
        win.Show();

        //string path = Application.dataPath + "/../AssetBundles";

        //if (!Directory.Exists(path))
        //{
        //    Directory.CreateDirectory(path);
        //}

        //string path = Application.dataPath + "/Editor/AssetBundle/AssetBundleConfig.xml";
        //AssetBundleDAL assetBundleDal=new AssetBundleDAL(path);

        //var list = assetBundleDal.GetList();

        //for (int i = 0; i < list.Count; i++)
        //{
        //    Debug.Log(list[i].Name);
        //}
    }
}
