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
    public AIState ApplyAction(AIState currentState)
    {
        throw new NotImplementedException();
    }

    public bool isValid(AIState currentState)
    {
        return true;
    }
}

//Maybe subclasses of this for moving from place to place
public class PickUpAction : Action
{
    public AIState ApplyAction(AIState currentState)
    {
        throw new NotImplementedException();
    }

    public bool isValid(AIState currentState)
    {
        return true;
    }
}

public class DropOffAction : Action
{
    public AIState ApplyAction(AIState currentState)
    {
        throw new NotImplementedException();
    }

    public bool isValid(AIState currentState)
    {
        return true;
    }
}

//Used for things like moving a soup from a plate to a pot etc.
public class TransferAction : Action
{
    public AIState ApplyAction(AIState currentState)
    {
        throw new NotImplementedException();
    }

    public bool isValid(AIState currentState)
    {
        return true;
    }
}

public class PrepareAction : Action
{
    public AIState ApplyAction(AIState currentState)
    {
        throw new NotImplementedException();
    }

    public bool isValid(AIState currentState)
    {
        return true;
    }
}