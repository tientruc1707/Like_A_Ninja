using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    public ObjectPool pool;

    private void OnDisable()
    {
        pool.AddToPool(gameObject);
    }
}
