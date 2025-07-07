using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    public ItemSO item;
    public Image icon;
    public Button button;
    public void OnEnable()
    {
        icon.sprite = item.sprite;
    }
}
