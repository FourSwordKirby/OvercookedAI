using System;

public class PotState : ItemState
{
    public PotState(int id) : base(id)
    {
    }

    public override object Clone()
    {
        throw new NotImplementedException();
    }
}
