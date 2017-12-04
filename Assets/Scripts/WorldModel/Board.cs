using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : Item {

    public Item HoldingItem;
    public Vector3 HoldingPosition;

    public override ItemState GetState()
    {
        return new BoardState(ID, HoldingItem == null ? Item.NOTHING_ID : HoldingItem.ID);
    }

    public override void LoadState(ItemState state)
    {
        BoardState bState = state as BoardState;
        if (bState.HoldingItemID == Item.NOTHING_ID)
        {
            HoldingItem = null;
        }
        else
        {
            HoldingItem = GetItemManager().ItemList[bState.HoldingItemID];
        }

        if (HoldingItem != null)
        {
            HoldingItem.transform.position = HoldingPosition;
        }
    }

    private void Awake()
    {
        HoldingPosition = transform.Find("Holding Position").position;
    }

    private void Start()
    {
        GetItemManager().RegisterBoard(this);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
