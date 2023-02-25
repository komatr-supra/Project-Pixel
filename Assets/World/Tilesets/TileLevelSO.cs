using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTileLevelSO", menuName = "Level Data/TileLevelSO", order = 0)]
public class TileLevelSO : ScriptableObject
{
    public TileBase tileRule;
    public Sprite[] propsSprites;
    public bool isWalkable;
    public int minLenght;
    public int maxHeight;
    
}
