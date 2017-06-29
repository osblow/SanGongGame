using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPReader : MonoBehaviour 
{
    string[] m_tips;


	// Use this for initialization
	void Start () 
	{
        loadXml();
	}

    void loadXml()
    {
        string filePath2 = @"D:\SanGong\client\Assets\Resources\Texture\ddzcards_xml.xml";
        string filePath = Application.dataPath + @"/Resources/Texture/ddzcards_xml.xml";

        bool b = File.Exists(filePath2);

        if (File.Exists(filePath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNodeList node = xmlDoc.SelectSingleNode("plist").ChildNodes;
            foreach (XmlElement nodeList in node)
            {
                foreach (XmlElement xe in nodeList)
                {
                    if (xe.Name == "array")
                    {
                        int i = 0;
                        m_tips = new string[xe.ChildNodes.Count];
                        foreach (XmlElement xe1 in xe.ChildNodes)
                        {
                            //                          Debug.Log(xe1.InnerText);  
                            m_tips[i] = xe1.InnerText;
                            i++;
                        }
                        break;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update () 
	{
		
	}
}
