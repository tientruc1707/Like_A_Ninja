using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : Singleton<PoolSystem>
{
    private Dictionary<GameObject, ObjectPool> pools = new();

    public void CreatePool(GameObject prefab, int initialSize)
    {
        if (pools.ContainsKey(prefab))
        {
            Debug.LogWarning($"Pool for {prefab.name} already exists.");
            return;
        }

        ObjectPool newPool = new(prefab);
        newPool.CreatePool(initialSize);
        pools[prefab] = newPool;
    }

    public GameObject GetFromPool(GameObject prefab)
    {
        if (pools.TryGetValue(prefab, out ObjectPool pool))
        {
            return pool.Get();
        }
        else
        {
            Debug.Log($"No pool found for {prefab.name}. Consider creating it first.");
            return null;
        }
    }

    public void ReturnToPool(GameObject prefab, GameObject gameObject)
    {
        if (pools.TryGetValue(prefab, out ObjectPool pool))
        {
            pool.AddToPool(gameObject);
        }
        else
        {
            Debug.Log($"No pool found for {prefab.name}. Cannot return object.");
        }
    }

    public void ClearActiveObject(GameObject prefab)
    {
        if (pools.TryGetValue(prefab, out ObjectPool pool))
        {
            pool.SetActiveAll();
        }
        else
        {
            Debug.Log($"No pool found for {prefab.name}. Cannot set active all.");
        }
    }
}
