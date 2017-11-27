using System;
using System.Collections.Generic;

public class TableState : ItemState {
    /// <summary>
    /// A list of item IDs which are on the infinite sized table.
    /// </summary>
    public List<int> ItemIDsOnTable;

    public TableState(int id) : base(id)
    {
    }

    public override object Clone()
    {
        throw new NotImplementedException();
    }
}
