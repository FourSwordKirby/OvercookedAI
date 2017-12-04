using System;

public class BoardState : ItemState
{
    public int HoldingItemID;

    public BoardState(int id, int holdingItemID = Item.NOTHING_ID)
        : base(id, ItemType.BOARD)
    {
        HoldingItemID = holdingItemID;
    }

    public bool IsFree()
    {
        return HoldingItemID == Item.NOTHING_ID;
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }

    public override bool Equals(object obj)
    {
        BoardState otherState = obj as BoardState;
        if (otherState == null)
        {
            return false;
        }

        if (otherState == this)
        {
            return true;
        }

        return this.ID == otherState.ID;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int ret = 17;
            ret = 33 * ret + ID;
            return ret;
        }
    }
}
