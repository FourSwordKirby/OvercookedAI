using System;


public class IngredientState : ItemState {

    public IngredientType ingredientType;
    public bool IsSpawned;
    public bool IsPrepared;
    public bool IsInMeal;

    public IngredientState(int id, IngredientType ingredientTypeName, bool isSpawned, bool isPrepared, bool isCooking)
        : base(id, ItemType.INGREDIENT)
    {
        ingredientType = ingredientTypeName;
        IsSpawned = isSpawned;
        IsPrepared = isPrepared;
        IsInMeal = isCooking;
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

        IngredientState otherState = obj as IngredientState;
        if(otherState == null)
        {
            return false;
        }

        return this.ID == otherState.ID
            && this.ingredientType == otherState.ingredientType
            && this.IsSpawned == otherState.IsSpawned
            && this.IsPrepared == otherState.IsPrepared
            && this.IsInMeal == otherState.IsInMeal;
    }

    public override int GetHashCode()
    {
        int ret = 0;
        ret |= (ID << 8);
        ret |= ((int)ingredientType << 3); // Assumes no more than 32 ingredients
        ret |= IsSpawned ? 1 : 0;
        ret |= ((IsPrepared ? 1 : 0) << 1);
        ret |= ((IsInMeal ? 1 : 0) << 2);
        return ret;
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}
