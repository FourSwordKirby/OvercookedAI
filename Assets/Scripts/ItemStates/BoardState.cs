using System;

public class BoardState : ItemState
{
    public int HoldingItemID;

    public BoardState(int id)
        : base(id)
    {
        HoldingItemID = Item.NOTHING_ID;
    }

    public bool IsFree()
    {
        return HoldingItemID == Item.NOTHING_ID;
    }

    public override object Clone()
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object obj)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
