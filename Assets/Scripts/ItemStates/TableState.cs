using System;
using System.Collections.Generic;
using System.Linq;

public class TableState : ItemState {
    /// <summary>
    /// A list of item IDs which are on the infinite sized table.
    /// </summary>
    public List<int> ItemIDsOnTable { get; private set; }

    public TableState(int id) : base(id)
    {
        ItemIDsOnTable = new List<int>();
    }

    private TableState(int id, List<int> itemIDsOnTable) : base(id)
    {
        ItemIDsOnTable = itemIDsOnTable;
    }

    public override object Clone()
    {
        return new TableState(ID, new List<int>(ItemIDsOnTable));
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
        return ItemIDsOnTable.SequenceEqual(otherState.ItemIDsOnTable);
    }

    /// <summary>
    /// Adds a new item to the Table.
    /// </summary>
    /// <param name="id">id of the new item</param>
    /// <returns>a new TableState with the item added, or the same TableState if the item already exists</returns>
    public TableState AddItem(int id)
    {
        int index = ItemIDsOnTable.BinarySearch(id);
        if (index >= 0)
        {
            return this;
        }
        else
        {
            TableState newState = (TableState)Clone();
            newState.ItemIDsOnTable.Insert(~index, id);
            return newState;
        }
    }

    /// <summary>
    /// Removes an item from the table.
    /// </summary>
    /// <param name="id">id of the item to remove</param>
    /// <returns>a new TableState with the item removed, or the same TableState if the item didn't exist.</returns>
    public TableState RemoveItem(int id)
    {
        int index = ItemIDsOnTable.BinarySearch(id);
        if (index < 0)
        {
            return this;
        }
        else
        {
            TableState newState = (TableState)Clone();
            newState.ItemIDsOnTable.RemoveAt(index);
            return newState;
        }
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int ret = 17;
            foreach(int id in ItemIDsOnTable)
            {
                ret = 33 * ret + id;
            }
            return ret;
        }
    }
}
