using UnityEngine;

public class AssetLibrary
{
    public static Item[] Items { get; private set; }

    public static Sprite locked;
    public static Sprite unlocked;
    public static Sprite star1;
    public static Sprite star2;
    public static Sprite star3;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] private static void Initialize() => Items = Resources.LoadAll<Item>("Items/");

}
