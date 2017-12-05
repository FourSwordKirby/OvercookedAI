using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Item
{
    public Meal HeldMeal;
    public Vector3 HoldingPosition;

    private void Awake()
    {
        HoldingPosition = transform.Find("Holding Position").position;
    }

    private void Start()
    {
        GetItemManager().RegisterPot(this);
        HeldMeal.transform.position = HoldingPosition;
    }

    public override ItemState GetState()
    {
        return new PotState(ID, HeldMeal.ID);
    }

    public override void LoadState(ItemState state)
    {
        PotState pState = state as PotState;
        HeldMeal = GetItemManager().ItemList[pState.mealID] as Meal;
        HeldMeal.transform.position = HoldingPosition;
    }
}
