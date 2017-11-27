using System;


public class IngredientState : ItemState {

    public IngredientType Ingredient;
    public bool IsSpawned;
    public bool IsPrepared;

    public IngredientState(int id, IngredientType ingredientTypeName, bool isSpawned, bool isPrepared)
        : base(id)
    {
        Ingredient = ingredientTypeName;
        IsSpawned = isSpawned;
        IsPrepared = isPrepared;
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
            && this.Ingredient == otherState.Ingredient
            && this.IsSpawned == otherState.IsSpawned
            && this.IsPrepared == otherState.IsPrepared;
    }

    public override int GetHashCode()
    {
        int ret = 0;
        ret |= (ID << 7);
        ret |= ((int)Ingredient << 2); // Assumes no more than 32 ingredients
        ret |= ((IsPrepared ? 1 : 0) << 1);
        ret |= IsSpawned ? 1 : 0;
        return ret;
    }

    public override object Clone()
    {
        throw new NotImplementedException();
    }
}
