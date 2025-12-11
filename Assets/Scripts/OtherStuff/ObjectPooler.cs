using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;

    [Serializable]
    public struct ObjectInfo
    {
        public int id;
        public GameObject prefab;
        public int StartCount;
    }
    [SerializeField]
    private List<ObjectInfo> objectsInfos;

    private Dictionary<int, Pool> pools;

    private void Awake()
    {
        instance = this;
        InitPool();
    }

    public void InitPool()
    {
        pools = new Dictionary<int, Pool>();

        var emptyGO = new GameObject();
        foreach(var obj in objectsInfos)
        {
            var container = Instantiate(emptyGO, transform, false);
            container.name = obj.id.ToString();
            pools[obj.id] = new Pool(container.transform);

            for (int i = 0; i < obj.StartCount; i++)
            {
                var go = InstantiateObject(obj.id, container.transform,Vector3.zero, Quaternion.identity);
                pools[obj.id].Objects.Enqueue(go);
            }
        }
    }

    private GameObject InstantiateObject(int id, Transform parent, Vector3 position, Quaternion rotation)
    {
        var go = Instantiate(objectsInfos.Find(x => x.id == id).prefab, position, rotation, parent);
        go.SetActive(false);
        return go;
    }
    public GameObject GetObject(int id, Vector3 position, Quaternion rotation)
    {
        var obj = pools[id].Objects.Count > 0 ?
            pools[id].Objects.Dequeue(): InstantiateObject(id, pools[id].Container, position, rotation);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    public void DestroyObject(GameObject obj)
    {
        var projectile = obj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.destroy = false;
        }
        pools[obj.GetComponent<IPooledObject>().ID].Objects.Enqueue(obj);
        projectile.SetLifeTimeZero();
        obj.SetActive(false);
    }
}
