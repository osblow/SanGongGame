using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Osblow.App
{
    public enum DataType
    {
        None = 0,
        Player = 1,
        Table = 2
    }

    public class DataMng : MonoBehaviour
    {
        private Dictionary<DataType, DataBase> m_dataDic = new Dictionary<DataType, DataBase>();

        public void SetData(DataType type, DataBase data)
        {
            if (!m_dataDic.ContainsKey(type))
            {
                m_dataDic.Add(type, data);
            }
            else
            {
                m_dataDic[type] = data;
            }
        }

        public T GetData<T>(DataType type)
            where T : DataBase
        {
            if (!m_dataDic.ContainsKey(type))
            {
                return null;
            }

            return (T)m_dataDic[type];
        }

        public void RemoveData(DataType type)
        {
            if (!m_dataDic.ContainsKey(type))
            {
                return;
            }

            m_dataDic.Remove(type);
        }

        private void Awake()
        {

        }
    }
}
