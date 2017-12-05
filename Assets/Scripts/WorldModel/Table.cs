using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Item {

    public Item HeldItem;
    public Transform HoldingPosition;

    private void Awake()
    {
        HoldingPosition = transform.Find("Holding Position");
    }

    public void Start()
    {
        GetItemManager().RegisterTableItem(this);
        if (HeldItem != null)
        {
            Debug.Log("Moved item " + HeldItem.name + " to table holding position.");
            HeldItem.transform.position = HoldingPosition.position;
        }
    }
    

    public override ItemState GetState()
    {
        return new TableSpace(ID, HeldItem == null ? Item.NOTHING_ID : HeldItem.ID);
    }

    public override void LoadState(ItemState state)
    {
        TableSpace tState = state as TableSpace;

        if (!tState.IsFree())
        {
            HeldItem = GetItemManager().ItemList[tState.ItemIDOnTable];
            if (HeldItem != null)
            {
                HeldItem.transform.position = HoldingPosition.position;
            }
        }
    }
}
