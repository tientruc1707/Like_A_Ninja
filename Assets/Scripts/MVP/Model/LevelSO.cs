using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSO : ScriptableObject
{
    public List<ItemSO> itemList;
    public int width;
    public int height;

}
