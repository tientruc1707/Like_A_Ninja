using UnityEngine;

public class AssetLibrary
{
    public static ItemSO[] Items { get; private set; }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] private static void Initialize() => Items = Resources.LoadAll<ItemSO>("Items/");

}
