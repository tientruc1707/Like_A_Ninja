using UnityEngine;

public class AssetLibrary
{
    public static Item[] Items { get; private set; }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] private static void Initialize() => Items = Resources.LoadAll<Item>("Items/");

}
