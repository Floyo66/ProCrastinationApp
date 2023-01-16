using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]

public class Item : ScriptableObject{

    [Header("Only gameplay")]
    public TileBase tile; // not important
    public ItemType type;
    public ActionType actionType;  // not important
    public Vector2Int range = new Vector2Int (5, 4);  // not important

    [Header ("'Only UI")]
    public bool stackable = true;

    [Header ("Both")]
    public Sprite image;

  }

public enum ItemType 
{
    BuildingBlock,
    Tool
}
public enum ActionType
{
    Dig,
    mine 
}

