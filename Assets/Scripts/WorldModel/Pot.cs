using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Item
{
    public Meal HeldMeal;
    public Transform HoldingPosition;

    private void Awake()
    {
        HoldingPosition = transform.Find("Holding Position");
    }

    private void Start()
    {
        GetItemManager().RegisterPot(this);
        HeldMeal.transform.position = HoldingPosition.position;
    }

    private void Update()
    {
        HeldMeal.transform.position = HoldingPosition.position;
    }

    public override ItemState GetState()
    {
        return new PotState(ID, HeldMeal.ID);
    }

    public override void LoadState(ItemState state)
    {
        PotState pState = state as PotState;
        HeldMeal = GetItemManager().ItemList[pState.mealID] as Meal;
    }
}
