using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public class PedestalPool
    {
        Dictionary<PedestalType, Pedestal> _cachePedestalPrefabs;
        Dictionary<PedestalType, List<Pedestal>> _pools = new Dictionary<PedestalType, List<Pedestal>>();
        Transform _root;

        public PedestalPool(Transform root, List<Pedestal> prefabs)
        {
            _root = root;
            _cachePedestalPrefabs = new Dictionary<PedestalType, Pedestal>();
            foreach (var pedestal in prefabs)
            {
                _cachePedestalPrefabs[pedestal.type] = pedestal;
            }
        }


        public void PreCreate(PedestalType type, int totalInstance)
        {
            SafeCheckIfPoolIsCreated(type);
            if (_pools[type].Count < totalInstance)
            {
                int totalCreate = totalInstance - _pools[type].Count;
                for (int i = 0; i < totalCreate; i++)
                {
                    CreatePedestal(type, true);
                }
            }
        }
        
        public Pedestal Instantiate(PedestalType type, Vector3 position, float angle)
        {
            SafeCheckIfPoolIsCreated(type);

            Pedestal pedestal = null;
            List<Pedestal> pool = _pools[type];
            if (pool.Count > 0)
            {
                
                pedestal = pool[pool.Count - 1];
                pool.RemoveAt(pool.Count - 1);
            }
            else
            {
                pedestal = CreatePedestal(type, false);
            }

            pedestal.transform.position = position;
            pedestal.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            pedestal.gameObject.SetActive(true);

            return pedestal;
        }

        public void Return(Pedestal pedestal)
        {
            SafeCheckIfPoolIsCreated(pedestal.type);
            _pools[pedestal.type].Add(pedestal);
            pedestal.gameObject.SetActive(false);
        }

        public void CleanUp()
        {
            foreach (var kv in _pools)
            {
                foreach (var pedestal in _pools[kv.Key])
                {
                    GameObject.Destroy(pedestal);
                }
                _pools[kv.Key].Clear();
            }
        }

        Pedestal CreatePedestal(PedestalType type, bool addToPool)
        {
            Pedestal prefab = _cachePedestalPrefabs[type];
            Pedestal instance = GameObject.Instantiate(prefab, _root);

            if (addToPool)
            {
                instance.gameObject.SetActive(false);
                _pools[type].Add(instance);
            }

            return instance;
        }

        void SafeCheckIfPoolIsCreated(PedestalType type)
        {
            if (!_pools.ContainsKey(type))
                _pools[type] = new List<Pedestal>();
        }
    }
}
