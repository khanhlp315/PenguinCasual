using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Penguin.Network.Data
{
    [Serializable]
    public class ReponseData<T>
    {
        public List<T> data;

        public T GetSingle()
        {
            return data.First();
        }

        public List<T> GetAll()
        {
            return data;
        }
        
        public static ReponseData<T> FromJson(string jsonString)
        {
            Debug.Log(jsonString);
            return JsonUtility.FromJson<ReponseData<T>>(jsonString);
        }
    }
}