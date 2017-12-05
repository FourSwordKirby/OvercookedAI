﻿using System;

public class PlateState : ItemState
{
    public int mealID;

    public PlateState(int id) : base(id, ItemType.PLATE)
    {
        mealID = Item.NOTHING_ID;
    }

    public bool IsEmpty()
    {
        return mealID == Item.NOTHING_ID;
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (this == obj)
        {
            return true;
        }

        PlateState otherState = obj as PlateState;
        if (obj == null)
        {
            return false;
        }

        return this.ID == otherState.ID
            && this.mealID == otherState.mealID;
    }

    public override int GetHashCode()
    {
        return (ID << 16) | mealID;
    }
}
