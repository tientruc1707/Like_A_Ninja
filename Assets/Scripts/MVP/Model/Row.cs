using UnityEngine;

public class Row : MonoBehaviour
{
    public Tile[] tiles;
    private void OnEnable()
    {
        tiles = GetComponentsInChildren<Tile>();
    }
}
