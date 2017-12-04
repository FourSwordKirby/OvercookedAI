using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Item {

    public int ItemIDOnTable;
    public Vector3 HoldingPosition;

    private void Awake()
    {
        ItemIDOnTable = Item.NOTHING_ID;
        HoldingPosition = transform.Find("Holding Position").position;
    }

    public void Start()
    {
        ItemManager im = FindObjectOfType<ItemManager>();
        if(im == null)
        {
            Debug.LogError("Missing ItemManager!");
        }
        else
        {
            im.RegisterTableItem(this);
        }
    }
    

    public override ItemState GetState()
    {
        return new TableSpace(ID, ItemIDOnTable);
    }

    public override void LoadState(ItemState state)
    {
        ItemIDOnTable = (state as TableSpace).ItemIDOnTable;
    }
}
