using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Goal
{
    bool IsGoal(AIState currentState);
}

/// <summary>
/// This defines the conditions for completing a single recipe
/// </summary>
public class RecipeGoal : Goal
{
    public bool IsGoal(AIState currentState)
    {
        Debug.Log("NEEDS TO BE IMPLEMENTED");
        return true;
    }
}
