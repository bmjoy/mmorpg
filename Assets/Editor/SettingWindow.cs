using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SettingWindow:EditorWindow  {

    private List<MacorItem> _macorItemList=new List<MacorItem>();
    private Dictionary<string,bool> _dictionary=new Dictionary<string, bool>();
    private string m_Macor = null;

   
    void OnEnable()
    {
        m_Macor = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        _macorItemList.Clear();
        _macorItemList.Add(new MacorItem() { Name = "DEBUG_MODEL", DisplayName = "调试模式", IsDebug = true, IsRelease = false });
        _macorItemList.Add(new MacorItem() { Name = "DEBUG_LOG", DisplayName = "打印日志", IsDebug = true, IsRelease = false });
        _macorItemList.Add(new MacorItem() { Name = "STAT_TD", DisplayName = "开启统计", IsDebug = false, IsRelease = true });
        for (int i = 0; i < _macorItemList.Count; i++)
        {
            if (string.IsNullOrEmpty(m_Macor) && m_Macor.IndexOf(_macorItemList[i].Name) != -1)
            {
                _dictionary[_macorItemList[i].Name] = true;
            }
            else
            {
                _dictionary[_macorItemList[i].Name] = false;
            }
        }
    }

    void OnGUI()
    {
        for (int i = 0; i < _macorItemList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            _dictionary[_macorItemList[i].Name] = GUILayout.Toggle(_dictionary[_macorItemList[i].Name], _macorItemList[i].DisplayName);
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("保存", GUILayout.Width(100)))
        {
            SaveMacor();
        }
    }

    private void SaveMacor()
    {
        m_Macor = string.Empty;
        foreach (var item in _dictionary)
        {
            if (item.Value)
            {
                m_Macor += string.Format("{0};", item.Key);
            }
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, m_Macor);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, m_Macor);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, m_Macor);
    }

    public class MacorItem
    {
        public string Name;

        public string DisplayName;

        public bool IsDebug;

        public bool IsRelease;
    }
}
