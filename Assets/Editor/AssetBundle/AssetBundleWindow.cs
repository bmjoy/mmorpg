using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class AssetBundleWindow : EditorWindow
{
    private AssetBundleDAL dal;
    private List<AssetBundleEntity> m_List;
    private Dictionary<string, bool> m_dic;

    private string[] arrTag = {"All","Scene","Role","Effect","Audio","None"};
    private int tagIndex = 0;
    private string[] arrBuildTarget = {"Windows","Android","iOS" };



#if UNITY_STANDALONE_WIN
    private BuildTarget target = BuildTarget.StandaloneWindows;
    private int buildTargetIndex = 0;
#elif UNITY_ANDROID
    private BuildTarget target = Android;
    private int buildTargetIndex = 1;
#elif UNITY_IPHONE
    private BuildTarget target = BuildTarget.iOS;
    private int buildTargetIndex = 2;
#endif

    private Vector2 pos;
    void OnEnable()
    {
        string path = Application.dataPath + "/Editor/AssetBundle/AssetBundleConfig.xml";
        dal = new AssetBundleDAL(path);

        m_List = dal.GetList();

        m_dic=new Dictionary<string, bool>();

        for (int i = 0; i < m_List.Count; i++)
        {
            m_dic[m_List[i].Key] = true;
        }
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal("box");
        tagIndex = EditorGUILayout.Popup(tagIndex, arrTag, GUILayout.Width(100));
        if (GUILayout.Button("选定Tag"))
        {
            EditorApplication.delayCall = OnSelectTagCB;
        }
        buildTargetIndex = EditorGUILayout.Popup(buildTargetIndex, arrBuildTarget, GUILayout.Width(100));
        if (GUILayout.Button("选定Target"))
        {
            EditorApplication.delayCall = OnSelectTargetCB;
        }

        if (GUILayout.Button("打Assetbundle包",GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnAssetBundleCB;
        }

        if (GUILayout.Button("清空Assetbundle包", GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnClearAssetBundleCB;
        }

        EditorGUILayout.Space();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box");
        GUILayout.Label("包名");
        GUILayout.Label("标记", GUILayout.Width(100));
        GUILayout.Label("保存路径", GUILayout.Width(200));
        GUILayout.Label("版本", GUILayout.Width(100));
        GUILayout.Label("大小", GUILayout.Width(100));
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        pos=EditorGUILayout.BeginScrollView(pos);
        for (int i = 0; i < m_List.Count; i++)
        {
            GUILayout.BeginHorizontal("box");
            m_dic[m_List[i].Key] = GUILayout.Toggle(m_dic[m_List[i].Key], "", GUILayout.Width(20));
            GUILayout.Label(m_List[i].Name);
            GUILayout.Label(m_List[i].Tag, GUILayout.Width(100));
            GUILayout.Label(m_List[i].ToPath, GUILayout.Width(200));
            GUILayout.Label(m_List[i].Version.ToString(), GUILayout.Width(100));
            GUILayout.Label(m_List[i].Size.ToString(), GUILayout.Width(100));
            GUILayout.EndHorizontal();

            foreach (string path in m_List[i].PathList)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Space(40);
                GUILayout.Label(path);
                GUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();


    }

    private void BuildAssetBundle(AssetBundleEntity entity)
    {
        AssetBundleBuild[] arrBuilds=new AssetBundleBuild[1];
        AssetBundleBuild build=new AssetBundleBuild();
        build.assetBundleName = String.Format("{0}.{1}",entity.Name,entity.Tag.Equals("Scene",StringComparison.CurrentCultureIgnoreCase) ? "unity3d" : "assetbundle");
        build.assetNames = entity.PathList.ToArray();

        arrBuilds[0] = build;
        string toPath = Application.dataPath + "/../AssetBundles/" + arrBuildTarget[buildTargetIndex] + entity.ToPath;

        if (!Directory.Exists(toPath))
        {
            Directory.CreateDirectory(toPath);
        }

        BuildPipeline.BuildAssetBundles(toPath, arrBuilds, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    private void OnClearAssetBundleCB()
    {
        throw new System.NotImplementedException();
    }

    private void OnAssetBundleCB()
    {
        List < AssetBundleEntity > list=new List<AssetBundleEntity>();
        foreach (var entity in m_List)
        {
            if (m_dic[entity.Key])
            {
                list.Add(entity);
                
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            BuildAssetBundle(list[i]);
        }
    }

    private void OnSelectTargetCB()
    {
        switch (buildTargetIndex)
        {
            case 0:
                target= BuildTarget.StandaloneWindows;
                break;
            case 1:
                target = BuildTarget.Android;
                break;
            case 2:
                target = BuildTarget.iOS;
                break;
        }
    }

    private void OnSelectTagCB()
    {
        switch (tagIndex)
        {
            case 0:
                foreach (var entity in m_List)
                {
                    m_dic[entity.Key] = true;
                }
                break;
            case 1:
                foreach (var entity in m_List)
                {
                    m_dic[entity.Key] = entity.Tag.Equals("Scene");
                }
                break;

            case 2:
                foreach (var entity in m_List)
                {
                    m_dic[entity.Key] = entity.Tag.Equals("Role");
                }
                break;
            case 3:
                foreach (var entity in m_List)
                {
                    m_dic[entity.Key] = entity.Tag.Equals("Effect");
                }
                break;
            case 4:
                foreach (var entity in m_List)
                {
                    m_dic[entity.Key] = entity.Tag.Equals("Audio");
                }
                break;
            case 5:
                foreach (var entity in m_List)
                {
                    m_dic[entity.Key] = false;
                }
                break;
        }
    }
}
