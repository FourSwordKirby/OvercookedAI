public class PlayerState {
    public int HoldingItemID;

    public PlayerState()
    {
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
