using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Item", menuName = "Survival Game/Iventory/New item")]
public class ItemSO : ScriptableObject
{
    public enum ItemType { Generic, Consumable, Weapon}

    [Header("General")]
    public ItemType itemType;
    public Sprite icon;
    public string itemName = "New Item";
    public string description = "New Item Decription";
    [Space]
    public bool isStackable;
    public int maxStack = 1;

    [Header("Consumable")]
    public float healthChange = 50f;
    public float hungerChange = 50f;
    public float thirstChange = 50f;
}
