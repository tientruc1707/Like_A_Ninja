using DG.Tweening;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.DOScale(new Vector3(2, 2, 2), 1).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
