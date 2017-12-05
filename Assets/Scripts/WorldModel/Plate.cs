using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Item {

    public Meal HoldingMeal;
    public bool IsSubmitted;


    // Use this for initialization
    void Start()
    {
        GetItemManager().RegisterPlate(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSubmitted)
        {
            int plateNumber = GetItemManager().PlateIndexList.IndexOf(ID);
            transform.position = GetItemManager().SubmittedTableRef.Locations[plateNumber].position;
        }
    }

    public override ItemState GetState()
    {
        return new PlateState(ID, HoldingMeal.ID, IsSubmitted);
    }

    public override void LoadState(ItemState state)
    {
        PlateState pState = state as PlateState;
        HoldingMeal = GetItemManager().ItemList[pState.mealID] as Meal;
        IsSubmitted = pState.IsSubmitted;
    }
}
