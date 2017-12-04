using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Action
{
    AIState ApplyAction(AIState currentState);
    bool isValid(AIState currentState);
}

public class IdleAction : Action
{
    public IdleAction()  {    }

    public AIState ApplyAction(AIState currentState)
    {
        AIState cloneState = currentState.Clone() as AIState;

        foreach(int id in cloneState.PotStateIndexList)
        {
            PotState pot = cloneState.ItemStateList[id] as PotState;
            MealState meal = cloneState.ItemStateList[pot.MealID] as MealState;

            meal.CookIngredients();
        }

        return cloneState;
    }

    public bool isValid(AIState currentState)
    {
        return true;
    }
}

//Can only spawn an ingredient if your hands are empty
public class SpawnAction : Action
{
    IngredientType spawnType;
    public SpawnAction(IngredientType type)
    {
        spawnType = type;
    }

    public AIState ApplyAction(AIState currentState)
    {
        AIState cloneState = currentState.Clone() as AIState;

        foreach (int id in cloneState.IngredientStateIndexList)
        {
            IngredientState ingredient = cloneState.ItemStateList[id] as IngredientState;

            if (ingredient.ingredientType == spawnType)
            {
                ingredient.IsSpawned = true;
                if (spawnType == IngredientType.MUSHROOM)
                    cloneState.mushroomSpawnCount++;
                if (spawnType == IngredientType.ONION)
                    cloneState.onionSpawnCount++;

                cloneState.CurrentPlayerState.PickUp(ingredient.ID);
                break;
            }
        }

        return cloneState;
    }

    public bool isValid(AIState currentState)
    {
        if (!currentState.CurrentPlayerState.HandsFree())
            return false;

        if (spawnType == IngredientType.MUSHROOM)
            return currentState.mushroomSpawnCount < AIState.MAX_INGREDIENT_TYPE_SPAWN;
        else if (spawnType == IngredientType.ONION)
            return currentState.onionSpawnCount < AIState.MAX_INGREDIENT_TYPE_SPAWN;
        else
            throw new NotImplementedException();
    }
}

//Picking up an item
public class PickUpAction : Action
{
    int id;

    public PickUpAction(int id)
    {
        this.id = id;
    }

    public AIState ApplyAction(AIState currentState)
    {
        AIState cloneState = currentState.Clone() as AIState;
        cloneState.CurrentPlayerState.PickUp(ingredient.ID);

        return cloneState;
    }

    public bool isValid(AIState currentState)
    {
        if (!currentState.CurrentPlayerState.HandsFree())
            return false;
        else
        {
            if (currentState.ItemStateList[id].MyItemType == ItemType.INGREDIENT)
                return (currentState.ItemStateList[id] as IngredientState).IsSpawned;

            if (currentState.ItemStateList[id].MyItemType == ItemType.MEAL
                || currentState.ItemStateList[id].MyItemType == ItemType.TABLE)
                return false;

            return true;
        }
    }
}

//Dropping off an item and having nothing left in hand
public class DropOffAction : Action
{
    int id;

    public DropOffAction(int id)
    {
        this.id = id;
    }

    public AIState ApplyAction(AIState currentState)
    {
        throw new NotImplementedException();
    }

    public bool isValid(AIState currentState)
    {
        throw new NotImplementedException();
    }
}

//Used for things like moving a soup from a plate to a pot etc.
public class TransferAction : Action
{
    int id;

    public TransferAction(int id)
    {
        this.id = id;
    }

    public AIState ApplyAction(AIState currentState)
    {
        throw new NotImplementedException();
    }

    public bool isValid(AIState currentState)
    {
        throw new NotImplementedException();
    }
}

//Used to prepare an ingredient
public class PrepareAction : Action
{
    int id;

    public PrepareAction(int id)
    {
        this.id = id;
    }

    public AIState ApplyAction(AIState currentState)
    {
        throw new NotImplementedException();
    }

    public bool isValid(AIState currentState)
    {
        throw new NotImplementedException();
    }
}

//Used to drop off a final meal
public class SubmitOrderAction : Action
{
    public SubmitOrderAction()
    {
    }

    public AIState ApplyAction(AIState currentState)
    {
        throw new NotImplementedException();
    }

    public bool isValid(AIState currentState)
    {
        throw new NotImplementedException();
    }
}