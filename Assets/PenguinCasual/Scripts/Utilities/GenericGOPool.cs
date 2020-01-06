using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public abstract class IdentifiedMonoBehaviour : MonoBehaviour
    {
        [System.NonSerialized]
        public GenericGOPool pool;
        public abstract object ID { get; }
        public virtual void OnObjectHasRecycled() {}
        public virtual void OnObjectHasReused() {}
        public void ReturnToPool()
        {
            pool.Return(this);
        }
    }

    public class GenericGOPool
    {
        Dictionary<object, IdentifiedMonoBehaviour> _registedObjects = new Dictionary<object, IdentifiedMonoBehaviour>();
        protected Dictionary<object, List<IdentifiedMonoBehaviour>> _pools = new Dictionary<object, List<IdentifiedMonoBehaviour>>();

        public void RegisterPooledGO(IdentifiedMonoBehaviour obj)
        {
            if (_registedObjects.ContainsKey(obj.ID))
            {
                Debug.LogError($"Object with ID {obj.ID} is already existed");
                return;
            }
            
            _registedObjects[obj.ID] = obj;
        }

        public IdentifiedMonoBehaviour Instantiate(object id, Vector3 position, float angle)
        {
            SafeCheckIfPoolIsCreated(id);

            IdentifiedMonoBehaviour obj = null;
            List<IdentifiedMonoBehaviour> pool = _pools[id];
            if (pool.Count > 0)
            {

                obj = pool[pool.Count - 1];
                pool.RemoveAt(pool.Count - 1);
            }
            else
            {
                obj = CreateObject(id, false);
            }

            obj.transform.position = position;
            obj.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            obj.gameObject.SetActive(true);
            obj.OnObjectHasReused();

            return obj;
        }

        public void Return(IdentifiedMonoBehaviour obj)
        {
            SafeCheckIfPoolIsCreated(obj.ID);
            _pools[obj.ID].Add(obj);
            obj.gameObject.SetActive(false);
            obj.OnObjectHasRecycled();
        }

        IdentifiedMonoBehaviour CreateObject(object id, bool addToPool)
        {
            if (!_registedObjects.ContainsKey(id))
            {
                Debug.LogError($"Object with ID {id} is not registered");
                return null;
            }

            IdentifiedMonoBehaviour prefab = _registedObjects[id];
            IdentifiedMonoBehaviour instance = GameObject.Instantiate(prefab);
            instance.pool = this;

            if (addToPool)
            {
                instance.gameObject.SetActive(false);
                _pools[id].Add(instance);
            }

            return instance;
        }

        protected void SafeCheckIfPoolIsCreated(object id)
        {
            if (!_pools.ContainsKey(id))
                _pools[id] = new List<IdentifiedMonoBehaviour>();
        }
    }

}
