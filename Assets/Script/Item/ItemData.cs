using System;
using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}

public enum ConsumableType
{
    Hunger,
    Health,
    SpeedBuff,
    JumpBuff
}

public enum EquipType
{
    Weapon,
    Magica
}
public enum EquipBuff
{
    None,
    DoubleJump
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
    public float duration;
}
[System.Serializable]
public class ItemEquipBuff
{
    public EquipBuff type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;
    public EquipType equipType;
    [HideInInspector] public int equipTypeIndex;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("EquipBuff")]
    public ItemEquipBuff[] equipBuffs;

    private void OnEnable()
    {
        var all = (EquipType[])Enum.GetValues(typeof(EquipType));
        equipTypeIndex = Array.IndexOf(all, equipType);
    }
}