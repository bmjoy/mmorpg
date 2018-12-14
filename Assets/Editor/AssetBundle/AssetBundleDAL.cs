using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class AssetBundleDAL
{

    private string m_Path;

    private List<AssetBundleEntity> m_List;

    public AssetBundleDAL(string path)
    {
        m_Path = path;
        m_List=new List<AssetBundleEntity>();
    }

    public List<AssetBundleEntity> GetList()
    {
        m_List.Clear();

        XDocument doc = XDocument.Load(m_Path);
        XElement root = doc.Root;
        XElement abNode = root.Element("AssetBundle");

        int index = 0;
        foreach (XElement item in abNode.Elements("Item"))
        {
            AssetBundleEntity entity=new AssetBundleEntity();
            entity.Key = "key" + ++index;
            entity.Name = item.Attribute("Name").Value;
            entity.Tag = item.Attribute("Tag").Value;
            entity.Version = item.Attribute("Version").Value.ToInt();
            entity.Size = item.Attribute("Size").Value.ToLong();
            entity.ToPath = item.Attribute("ToPath").Value;

            foreach (XElement path in item.Elements("Path"))
            {
                entity.PathList.Add(String.Format("Assets/{0}",path.Attribute("Value").Value));
            }
            m_List.Add(entity);
        }

        return m_List;
    }
}
