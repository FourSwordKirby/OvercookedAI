using System;

public abstract class ItemState : ICloneable {

    public int ID;
    public ItemType MyItemType;

	public ItemState(int id, ItemType itemType)
    {
        ID = id;
        MyItemType = itemType;
    }

    public abstract object Clone();
    public abstract override bool Equals(object obj);
    public abstract override int GetHashCode();
}
