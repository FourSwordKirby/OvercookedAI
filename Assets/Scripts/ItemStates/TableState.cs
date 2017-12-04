using System;
using System.Collections.Generic;
using System.Linq;

public class TableState : ItemState {
    public int ItemIDOnTable;

    public TableState(int id) : base(id, ItemType.TABLE)
    {
        ItemIDOnTable = Item.NOTHING_ID;
    }
    public TableState(int id, int itemIDOnTable) : base(id, ItemType.TABLE)
    {
        ItemIDOnTable = itemIDOnTable;
    }


    public override object Clone()
    {
        return MemberwiseClone();
    }

    public override bool Equals(object obj)
    {
        TableState otherState = obj as TableState;
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
