
using System.Collections;
using UnityEngine;

public class CoroutineExecutor : Singleton<CoroutineExecutor>
{
    // This class is intentionally left empty.
    // It serves as a MonoBehaviour to run coroutines from non-MonoBehaviour classes.
    public static void Execute(IEnumerator coroutine)
    {
        Instance.StartCoroutine(coroutine);
    }
}
