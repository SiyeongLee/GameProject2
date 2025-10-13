
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

public class itemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int maxStack = 99;




}
