using System;

public class PlayerState {
    public int HoldingItemID;

    public PlayerState()
    {
        HoldingItemID = Item.NOTHING_ID;
    }

    private PlayerState(int itemID)
    {
        HoldingItemID = itemID;
    }

    public void PickUp(int itemID)
    {
        if (!HandsFree())
            throw new System.Exception("Picked up item while hands were occupied");

        HoldingItemID = itemID;
    }

    public int Drop()
    {
        if (!HandsFree())
            throw new System.Exception("Dropped item while there was nothing in hand");

        int droppedItemID = HoldingItemID;
        HoldingItemID = Item.NOTHING_ID;

        return droppedItemID;
    }

    public bool HandsFree()
    {
        return HoldingItemID == Item.NOTHING_ID;
    }

    internal PlayerState Clone()
    {
        return new PlayerState(this.HoldingItemID);
    }

    public override bool Equals(object obj)
    {
        PlayerState otherState = obj as PlayerState;
        if(otherState == null)
        {
            return false;
        }

        if (this == otherState)
        {
            return true;
        }

        return HoldingItemID == otherState.HoldingItemID;
    }

    public override int GetHashCode()
    {
        return HoldingItemID;
    }
}
