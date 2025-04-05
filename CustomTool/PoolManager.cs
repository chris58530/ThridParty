using System;
using System.Collections.Generic;
using UnityEngine;
public class PoolManager : MonoBehaviour
{
    private Dictionary<Type, object> pools = new();

    public void RegisterPool<T>(T prefab, int initialCount,Transform parent) where T : MonoBehaviour, IPoolable
    {
        if (pools.ContainsKey(typeof(T))) return;

        var pool = new GenericObjectPool<T>(prefab, initialCount,parent);
        pools.Add(typeof(T), pool);
    }

    public T Spawn<T>() where T : MonoBehaviour, IPoolable
    {
        if (pools.TryGetValue(typeof(T), out var pool))
        {
            return ((GenericObjectPool<T>)pool).Spawn();
        }
        Debug.LogError($"No pool registered for type {typeof(T)}");
        return null;
    }

    public void Despawn<T>(T instance) where T : MonoBehaviour, IPoolable
    {
        if (pools.TryGetValue(typeof(T), out var pool))
        {
            ((GenericObjectPool<T>)pool).Despawn(instance);
        }
        else
        {
            Debug.LogWarning($"No pool found for type {typeof(T)}");
        }
    }

    public void ClearAllPools()
    {
        foreach (var kv in pools)
        {
            var method = kv.Value.GetType().GetMethod("ClearPool");
            method?.Invoke(kv.Value, null);
        }
        pools.Clear();
    }
}
