﻿using System;
using System.Collections.Generic;
using System.Linq;

public class TableSpace : ItemState {
    public int ItemIDOnTable;

    public TableSpace(int id) : base(id, ItemType.TABLE)
    {
        ItemIDOnTable = Item.NOTHING_ID;
    }
    public TableSpace(int id, int itemIDOnTable) : base(id, ItemType.TABLE)
    {
        ItemIDOnTable = itemIDOnTable;
    }


    public override object Clone()
    {
        return MemberwiseClone();
    }

    public override bool Equals(object obj)
    {
        TableSpace otherState = obj as TableSpace;
        if (otherState == null)
        {
            return false;
        }

        if (this == otherState)
        {
            return true;
        }

        // Items are always assumed to be sorted.
        return ItemIDOnTable == otherState.ItemIDOnTable;
    }

    public override int GetHashCode()
    {
        return (ID << 16) | ItemIDOnTable;
    }

    public bool IsFree()
    {
        return ItemIDOnTable == Item.NOTHING_ID;
    }
}
