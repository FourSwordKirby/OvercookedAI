using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Action
{
    AIState ApplyAction(AIState currentState);
    bool isValid(AIState currentState);

    string ToString(AIState currentState);
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
            MealState meal = cloneState.ItemStateList[pot.mealID] as MealState;
            if(meal.IsSpawned)
                meal.CookIngredients();
        }

        return cloneState;
    }

    public bool isValid(AIState currentState)
    {
        return true;
    }

    public string ToString(AIState currentState)
    {
        return "Idle";
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

            if (ingredient.ingredientType == spawnType && !ingredient.IsSpawned)
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

    public string ToString(AIState currentState)
    {
        return "Spawn: " + spawnType;
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
        cloneState.CurrentPlayerState.PickUp(id);

        bool hasCleared = false;
        foreach (int tableID in cloneState.TableStateIndexList)
        {
            TableSpace tState = cloneState.ItemStateList[tableID] as TableSpace;
            if (tState.ItemIDOnTable == id)
            {
                tState.ItemIDOnTable = Item.NOTHING_ID;
                hasCleared = true;
                break;
            }
        }

        if(!hasCleared)
        {
            foreach (int boardID in cloneState.TableStateIndexList)
            {
                BoardState bState = cloneState.ItemStateList[boardID] as BoardState;
                if (bState.HoldingItemID == id)
                {
                    bState.HoldingItemID = Item.NOTHING_ID;
                    break;
                }
            }

        }


        return cloneState;
    }

    public bool isValid(AIState currentState)
    {
        if (!currentState.CurrentPlayerState.HandsFree())
            return false;
        else
        {
            if (id == Item.NOTHING_ID)
            {
                return false;
            }

            if (currentState.ItemStateList[id].MyItemType == ItemType.INGREDIENT)
                return (currentState.ItemStateList[id] as IngredientState).IsSpawned;

            if (currentState.ItemStateList[id].MyItemType == ItemType.MEAL
                || currentState.ItemStateList[id].MyItemType == ItemType.TABLE
                || currentState.ItemStateList[id].MyItemType == ItemType.BOARD)
                return false;

            return true;
        }
    }

    public string ToString(AIState currentState)
    {
        return "Picked Up Item: " + currentState.ItemStateList[id];
    }
}

//Dropping off an item at the id having nothing left in hand
public class DropOffAction : Action
{
    int id;

    public DropOffAction(int id)
    {
        this.id = id;
    }

    public AIState ApplyAction(AIState currentState)
    {
        AIState cloneState = currentState.Clone() as AIState;

        int droppedItemID = cloneState.CurrentPlayerState.Drop();

        if(cloneState.ItemStateList[id].MyItemType == ItemType.TABLE)
        {
            TableSpace table = cloneState.ItemStateList[id] as TableSpace;
            table.ItemIDOnTable = droppedItemID;
        }
        if(cloneState.ItemStateList[id].MyItemType == ItemType.BOARD)
        {
            BoardState board = cloneState.ItemStateList[id] as BoardState;
            board.HoldingItemID = droppedItemID;
        }
        if (cloneState.ItemStateList[id].MyItemType == ItemType.POT)
        {
            PotState pot = cloneState.ItemStateList[id] as PotState;
            MealState meal = cloneState.ItemStateList[pot.mealID] as MealState;

            if (!meal.IsSpawned)
            {
                meal.IsSpawned = true;
            }

            pot.currentMealSize++;
            meal.ContainedIngredientIDs.Add(droppedItemID);
        }
        if (cloneState.ItemStateList[id].MyItemType == ItemType.PLATE)
        {
            PlateState plate = cloneState.ItemStateList[id] as PlateState;
            MealState meal = cloneState.ItemStateList[plate.MealID] as MealState;

            if (!meal.IsSpawned)
            {
                meal.IsSpawned = true;
            }

            meal.ContainedIngredientIDs.Add(droppedItemID);
        }
        return cloneState;
    }

    public bool isValid(AIState currentState)
    {
        if (currentState.CurrentPlayerState.HandsFree())
            return false;
        else
        {
            int droppedItemID = currentState.CurrentPlayerState.HoldingItemID;

            if (currentState.ItemStateList[droppedItemID].MyItemType == ItemType.MEAL
                || currentState.ItemStateList[droppedItemID].MyItemType == ItemType.TABLE
                || currentState.ItemStateList[droppedItemID].MyItemType == ItemType.BOARD)
                return false;

            
            if (currentState.ItemStateList[id].MyItemType == ItemType.TABLE)
            {
                TableSpace table = currentState.ItemStateList[id] as TableSpace;
                return table.IsFree();
            }
            else if (currentState.ItemStateList[id].MyItemType == ItemType.BOARD)
            {
                return (currentState.ItemStateList[droppedItemID].MyItemType != ItemType.INGREDIENT);
            }
            else if (currentState.ItemStateList[id].MyItemType == ItemType.POT)
            {
                if (currentState.ItemStateList[droppedItemID].MyItemType != ItemType.INGREDIENT)
                    return false;
                else
                {
                    if (!(currentState.ItemStateList[droppedItemID] as IngredientState).IsPrepared &&
                        (currentState.ItemStateList[droppedItemID] as IngredientState).IsSpawned)
                    {
                        PotState pot = currentState.ItemStateList[id] as PotState;
                        return pot.currentMealSize < PotState.MAX_ITEMS_PER_POT;
                    }
                    else
                        return false;
                }
            }
            else if (currentState.ItemStateList[id].MyItemType == ItemType.PLATE)
            {
                if (currentState.ItemStateList[droppedItemID].MyItemType != ItemType.INGREDIENT)
                    return false;
                else
                {
                    return ((currentState.ItemStateList[droppedItemID] as IngredientState).IsPrepared &&
                            (currentState.ItemStateList[droppedItemID] as IngredientState).IsSpawned);
                }
            }
            else
                return false;
        }
    }

    public string ToString(AIState currentState)
    {
        return "Dropped off item at: " + currentState.ItemStateList[id];
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
        AIState cloneState = currentState.Clone() as AIState;

        int heldItemID = cloneState.CurrentPlayerState.HoldingItemID;

        MealState meal1;
        if (cloneState.ItemStateList[heldItemID].MyItemType == ItemType.POT)
        {
            PotState pot = (cloneState.ItemStateList[heldItemID] as PotState);
            meal1 = (cloneState.ItemStateList[pot.mealID] as MealState);

            //Removing the meal from the pot
            pot.mealID = cloneState.MealStateIndexList.FindLast(x => cloneState.ItemStateList[x].MyItemType == ItemType.MEAL &&
                                                                        (cloneState.ItemStateList[x] as MealState).IsSpawned == false);
        }
        else
        {
            PlateState plate = (cloneState.ItemStateList[heldItemID] as PlateState);
            meal1 = (cloneState.ItemStateList[plate.MealID] as MealState);

            //Removing the meal from the plate
            plate.MealID = cloneState.MealStateIndexList.FindLast(x => cloneState.ItemStateList[x].MyItemType == ItemType.MEAL &&
                                                                    (cloneState.ItemStateList[x] as MealState).IsSpawned == false);
        }

        //Transferring/combining it with the other meal
        if (cloneState.ItemStateList[id].MyItemType == ItemType.POT)
        {
            PotState pot = cloneState.ItemStateList[id] as PotState;
            MealState meal2 = cloneState.ItemStateList[pot.mealID] as MealState;

            meal2.ContainedIngredientIDs.AddRange(meal1.ContainedIngredientIDs);
        }
        else
        {
            PlateState plate = cloneState.ItemStateList[id] as PlateState;
            MealState meal2 = cloneState.ItemStateList[plate.MealID] as MealState;

            meal2.ContainedIngredientIDs.AddRange(meal1.ContainedIngredientIDs);
        }

        return cloneState;
    }

    public bool isValid(AIState currentState)
    {
        if (currentState.CurrentPlayerState.HandsFree())
            return false;
        else
        {
            int droppedItemID = currentState.CurrentPlayerState.HoldingItemID;

            if (currentState.ItemStateList[droppedItemID].MyItemType == ItemType.MEAL
                || currentState.ItemStateList[droppedItemID].MyItemType == ItemType.TABLE
                || currentState.ItemStateList[droppedItemID].MyItemType == ItemType.BOARD
                || currentState.ItemStateList[droppedItemID].MyItemType == ItemType.INGREDIENT)
                return false;

            if (currentState.ItemStateList[id].MyItemType == ItemType.MEAL
                || currentState.ItemStateList[id].MyItemType == ItemType.TABLE
                || currentState.ItemStateList[id].MyItemType == ItemType.BOARD
                || currentState.ItemStateList[id].MyItemType == ItemType.INGREDIENT)
                return false;

            MealState transferredMeal;
            if(currentState.ItemStateList[droppedItemID].MyItemType == ItemType.POT)
            {
                PotState pot = (currentState.ItemStateList[droppedItemID] as PotState);
                transferredMeal = (currentState.ItemStateList[pot.mealID] as MealState);
            }
            else
            {
                PlateState plate = (currentState.ItemStateList[droppedItemID] as PlateState);
                transferredMeal = (currentState.ItemStateList[plate.MealID] as MealState);
            }

            if (!transferredMeal.IsSpawned)
                return false;
            else
            {
                if (currentState.ItemStateList[id].MyItemType == ItemType.POT)
                {
                    PotState pot1 = currentState.ItemStateList[id] as PotState;
                    return pot1.HasCapacity(transferredMeal.MealSize());
                }
                else
                    return true;
            }
        }
    }

    public string ToString(AIState currentState)
    {
        return "Transfered contents of " + currentState.ItemStateList[currentState.CurrentPlayerState.HoldingItemID] 
            + " to " + currentState.ItemStateList[id];
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
        AIState cloneState = currentState.Clone() as AIState;

        BoardState board = cloneState.ItemStateList[id] as BoardState;
        IngredientState ingredient = cloneState.ItemStateList[board.HoldingItemID] as IngredientState;

        Debug.Log("To get this up and running quickly, preparing an ingredient is a simple boolean flip");
        ingredient.IsPrepared = true;

        return cloneState;
    }

    public bool isValid(AIState currentState)
    {
        if (!currentState.CurrentPlayerState.HandsFree())
            return false;

        if (currentState.ItemStateList[id].MyItemType != ItemType.BOARD)
            return false;
        else
        {
            BoardState board = currentState.ItemStateList[id] as BoardState;
            if (currentState.ItemStateList[board.HoldingItemID].MyItemType != ItemType.INGREDIENT)
                return false;
            else
            {
                IngredientState ingredient = currentState.ItemStateList[board.HoldingItemID] as IngredientState;
                return !ingredient.IsPrepared;
            }
        }
    }

    public string ToString(AIState currentState)
    {
        return "Preparing item on " + currentState.ItemStateList[id];
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
        AIState cloneState = currentState.Clone() as AIState;

        int heldItemID = cloneState.CurrentPlayerState.HoldingItemID;

        PlateState plate = (cloneState.ItemStateList[heldItemID] as PlateState);
        MealState meal = cloneState.ItemStateList[plate.MealID] as MealState;

        //Removing the meal from the plate
        plate.MealID = cloneState.MealStateIndexList.FindLast(x => cloneState.ItemStateList[x].MyItemType == ItemType.MEAL &&
                                                                (cloneState.ItemStateList[x] as MealState).IsSpawned == false);

        Debug.Log("Potentially score the meal submission here?");

        return cloneState;
    }

    public bool isValid(AIState currentState)
    {
        if (currentState.CurrentPlayerState.HandsFree())
            return false;
        else
        {
            int heldItemID = currentState.CurrentPlayerState.HoldingItemID;

            if (currentState.ItemStateList[heldItemID].MyItemType != ItemType.PLATE)
                return false;
            else
            {
                PlateState plate = (currentState.ItemStateList[heldItemID] as PlateState);
                return (currentState.ItemStateList[plate.MealID] as MealState).IsSpawned;
            }
        }
    }

    public string ToString(AIState currentState)
    {
        int heldItemID = currentState.CurrentPlayerState.HoldingItemID;

        PlateState plate = (currentState.ItemStateList[heldItemID] as PlateState);
        MealState meal = currentState.ItemStateList[plate.MealID] as MealState;

        return "Submitting meal " + meal;
    }
}