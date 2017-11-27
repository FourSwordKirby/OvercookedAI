using System;

public abstract class ItemState : ICloneable {

    public int ID;

	public ItemState(int id)
    {
        ID = id;
    }

    public abstract object Clone();
    public abstract override bool Equals(object obj);
    public abstract override int GetHashCode();
}
