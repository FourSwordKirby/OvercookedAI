using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {

    public const int NOTHING_ID = -1;
    public const int TABLE_ID = -2;

    public ItemType MyItemType;
    public int ID;

    abstract public ItemState GetState();
    abstract public void LoadState(ItemState state);
}
