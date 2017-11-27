using System;

public class PlateState : ItemState
{
    public int HoldingItemID;

    public PlateState(int id) : base(id)
    {
    }

    public override object Clone()
    {
        throw new NotImplementedException();
    }
}
