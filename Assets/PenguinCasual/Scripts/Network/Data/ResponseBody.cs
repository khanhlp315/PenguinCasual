using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Penguin.Network.Data
{
    [Serializable]
    public class ResponseBodyWithSingleEntity<T>
    {
        [SerializeField]
        private T data;

        public T Get()
        {
            return data;
        }
    }
    
    [Serializable]
    public class ResponseBodyWithMultipleEntities<T>
    {
        [SerializeField]
        private List<T> data;

        public List<T> GetAll()
        {
            return data;
        }
    }
}