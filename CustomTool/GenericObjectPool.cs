using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GenericObjectPool<T> where T : MonoBehaviour, IPoolable
{
    private readonly T prefab;
    private readonly Queue<T> pool = new();

    public GenericObjectPool(T prefab, int initialCount,Transform parent)
    {
        this.prefab = prefab;
        for (int i = 0; i < initialCount; i++)
        {
            T instance = Object.Instantiate(prefab);
            instance.gameObject.name = typeof(T).Name + "_Pooled";
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(parent);
            pool.Enqueue(instance);
        }
    }

    public T Spawn()
    {
        T obj = pool.Count > 0 ? pool.Dequeue() : Object.Instantiate(prefab);
        if (pool.Count == 0) obj.gameObject.name = typeof(T).Name + "_Pooled";
        obj.gameObject.SetActive(true);
        obj.OnSpawned();
        return obj;
    }

    public void Despawn(T obj)
    {
        obj.OnDespawned();
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    public void ClearPool()
    {
        foreach (var item in pool)
        {
            Object.Destroy(item.gameObject);
        }
        pool.Clear();
    }
}
