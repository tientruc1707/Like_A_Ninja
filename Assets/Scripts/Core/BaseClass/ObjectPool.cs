
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Stack<GameObject> myPool = new();
    private GameObject baseObject;
    private GameObject tmp;
    private ReturnToPool returnToPool;

    public ObjectPool(GameObject baseObj)
    {
        this.baseObject = baseObj;
    }

    public void CreatePool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            tmp = Object.Instantiate(baseObject);
            returnToPool = tmp.AddComponent<ReturnToPool>();
            returnToPool.pool = this;
            tmp.SetActive(false);
            AddToPool(tmp);
        }

    }

    public GameObject Get()
    {
        if (myPool.Count == 0)
        {
            Debug.LogWarning("Object pool is empty, consider increasing the pool size.");
            return null;
        }

        tmp = myPool.Pop();
        tmp.SetActive(true);
        return tmp;

    }

    public void AddToPool(GameObject gameObject)
    {
        if (gameObject == null)
        {
            Debug.LogWarning("Attempted to add a null GameObject to the pool.");
            return;
        }

        myPool.Push(gameObject);

    }

    public void SetActiveAll()
    {
        foreach (GameObject obj in myPool)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
            }

        }

    }

}
