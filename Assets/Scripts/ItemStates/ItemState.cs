using System;

public abstract class ItemState : ICloneable {

    public int ID;

	public ItemState(int id)
    {
        ID = id;
    }

    public abstract object Clone();
}
