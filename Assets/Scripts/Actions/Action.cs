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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public bool isValid(AIState currentState)
    {
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
        throw new NotImplementedException();
    }

    public bool isValid(AIState currentState)
    {
        throw new NotImplementedException();
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