using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletType
{
    public string BulletName;
    public GameObject BulletPrefab;
    public int BulletCount;
    public float bulletSpeed;
}
public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] List<BulletType> bulletTypes = new List<BulletType>();
    // Object pool
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();

    // Singleton
    public static ObjectPoolManager instance;

    private void Awake()
    {
        instance = this;

        foreach (var bulletType in bulletTypes)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            for (int i = 0; i < bulletType.BulletCount; i++)
            {
                GameObject obj = Instantiate(bulletType.BulletPrefab);
                obj.transform.SetParent(transform);
                obj.name = bulletType.BulletName;
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }
            objectPool.Add(bulletType.BulletName, objectQueue);
        }
    }

    private void Start()
    {
        
    }

    public GameObject SpawnBullet(string key, Vector3 position, Quaternion rotation)
    {
        if (!objectPool.ContainsKey(key))
        {
            Debug.LogError("Object pool does not contain key: " + key);
            return null;
        }
        GameObject obj = objectPool[key].Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    public void DespawnBullet(GameObject obj)
    {
        if (!objectPool.ContainsKey(obj.name))
        {
            Debug.LogError("Object pool does not contain key: " + obj.name);
            return;
        }
        obj.SetActive(false);
        objectPool[obj.name].Enqueue(obj);
    }
}
