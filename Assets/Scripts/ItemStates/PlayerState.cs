public class PlayerState {
    public int HoldingItemID;

    public PlayerState()
    {
        HoldingItemID = Item.NOTHING_ID;
    }

    public void PickUp(int itemID)
    {
        if (!HandsFree())
            throw new System.Exception("Picked up item while hands were occupied");

        HoldingItemID = itemID;
    }

    public void Drop()
    {
        if (!HandsFree())
            throw new System.Exception("Dropped item while there was nothing in hand");

        HoldingItemID = Item.NOTHING_ID;
    }

    public bool HandsFree()
    {
        return HoldingItemID == Item.NOTHING_ID;
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
