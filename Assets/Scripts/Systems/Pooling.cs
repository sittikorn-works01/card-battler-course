using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardBattlerCourse.Systems
{

    public class Pooling : Singleton<Pooling>
    {
        private class Pool
        {
            public string name;
            public byte preloadAmount = 0;

            public GameObject parentGroup;
            public Stack<GameObject> objectPool;
        }

        private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

        private void CreatePoolParent(string poolName, int amount = 0)
        {
            if (!pools.ContainsKey(poolName))
            {
                GameObject poolParent = new GameObject(poolName);
                poolParent.transform.SetParent(gameObject.transform);

                var pool = new Pool()
                {
                    name = poolName,
                    preloadAmount = (byte)amount,
                    parentGroup = poolParent,
                    objectPool = new Stack<GameObject>(),
                };

                pools.Add(poolName, pool);
            }
        }
        public bool CheckPoolIsLoad(string poolName)
        {
            if (pools.ContainsKey(poolName))
            {
                return true;
            }
            return false;
        }

        public void PreLoad(string poolName, GameObject prefab, int amount)
        {
            CreatePoolParent(poolName, amount);

            for (int i = 0; i < amount; i++)
                ReturnObject(poolName, Instantiate(prefab));
        }

        public T GetObject<T>(string poolName, T prefab) where T : Component
        {
            return GetObject(poolName, prefab.gameObject).GetComponent<T>();
        }

        public T GetObject<T>(string poolName, T prefab, Transform parent) where T : Component
        {
            return GetObject(poolName, prefab.gameObject, parent).GetComponent<T>();
        }

        public GameObject GetObject(string poolName, GameObject prefab)
        {
            if (!pools.ContainsKey(poolName))
                CreatePoolParent(poolName);

            if (pools[poolName].objectPool.Count > 0)
            {
                GameObject obj = pools[poolName].objectPool.Pop();

                if (obj != null)
                {
                    obj.transform.position = -Vector3.one * 1000;
                    obj.transform.SetParent(null);
                    obj.SetActive(true);

                    return obj;
                }
            }

            return Instantiate(prefab);
        }


        public GameObject GetObject(string poolName, GameObject prefab, Transform parent)
        {
            if (!pools.ContainsKey(poolName))
                CreatePoolParent(poolName);

            if (pools[poolName].objectPool.Count > 0)
            {
                GameObject obj = pools[poolName].objectPool.Pop();

                if (obj != null)
                {
                    obj.transform.SetParent(parent);
                    obj.transform.localPosition = prefab.transform.localPosition;
                    obj.transform.localRotation = prefab.transform.localRotation;
                    obj.transform.localScale = prefab.transform.localScale;
                    obj.SetActive(true);
                    return obj;
                }
            }

            return Instantiate(prefab, parent);
        }

        public T GetObjectUi<T>(string poolName, T prefab, Transform parent) where T : Component
        {
            return GetObjectUi(poolName, prefab.gameObject, parent).GetComponent<T>();
        }

        public GameObject GetObjectUi(string poolName, GameObject prefab, Transform parent)
        {
            if (!pools.ContainsKey(poolName))
            {
                CreatePoolParent(poolName);
            }

            if (pools[poolName].objectPool.Count > 0)
            {
                GameObject obj = pools[poolName].objectPool.Pop();

                if (obj != null)
                {
                    obj.transform.SetParent(parent);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localRotation = Quaternion.identity;
                    obj.transform.localScale = Vector3.one;
                    obj.SetActive(true);
                    return obj;
                }
            }

            return Instantiate(prefab, parent);
        }

        public void ReturnObject(string poolName, GameObject obj)
        {
            if (obj == null)
            {
                //Debug.LogWarning("Object got destroyed out of pooling at poolName : \"" + poolName + "\"");
                return;
            }

            if (!pools.ContainsKey(poolName))
                CreatePoolParent(poolName);

            obj.SetActive(false);
            obj.transform.SetParent(pools[poolName].parentGroup.transform);
            pools[poolName].objectPool.Push(obj);
        }

        public void ReturnObject(string poolName, GameObject obj, float delay, Action<GameObject> onBeforeReturn = null, Action onReturnToPool = null)
        {
            StartCoroutine(Wait(poolName, obj, delay, onBeforeReturn, onReturnToPool));
        }

        IEnumerator Wait(string poolName, GameObject obj, float delay, Action<GameObject> onBeforeReturn = null, Action onReturnToPool = null)
        {
            yield return new WaitForSeconds(delay);

            if (onBeforeReturn != null)
                onBeforeReturn(obj);

            ReturnObject(poolName, obj);

            onReturnToPool?.Invoke();
        }

        public void CleanPool(string poolName)
        {
            if (!pools.TryGetValue(poolName, out Pool pool))
                return;

            Destroy(pool.parentGroup);
            pools.Remove(poolName);
        }

        public void CleanAllPool()
        {
            foreach (var pool in pools.Values)
            {
                Destroy(pool.parentGroup);
            }
            pools.Clear();
        }
    }

}