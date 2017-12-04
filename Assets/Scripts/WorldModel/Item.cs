using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {

    public const int NOTHING_ID = -1;
    public const int TABLE_ID = -2;

    public ItemType MyItemType;
    public int ID;

    public ItemManager ItemManagerRef;

    public ItemManager GetItemManager()
    {
        if (ItemManagerRef != null)
        {
            return ItemManagerRef;
        }

        ItemManagerRef = FindObjectOfType<ItemManager>();
        if (ItemManagerRef == null)
        {
            Debug.LogError("Missing ItemManager!");
            return null;
        }
        else
        {
            return ItemManagerRef;
        }
    }

    abstract public ItemState GetState();
    abstract public void LoadState(ItemState state);
}
