using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Item {
    public IngredientType MyIngredientType;
    public bool IsSpawned;
    public bool IsPrepared;
    public bool IsCooking;

    private GameObject OnionModel;
    private GameObject UnknownModel;
    private GameObject MushroomModel;

    private void Awake()
    {
        OnionModel = transform.Find("Onion Model").gameObject;
        UnknownModel = transform.Find("Unknown Model").gameObject;
        MushroomModel = transform.Find("Mushroom Model").gameObject;
    }

    private void Start()
    {
        ItemManager im = FindObjectOfType<ItemManager>();
        if (im == null)
        {
            Debug.LogError("Missing Item Manager!");
        }
        else
        {
            im.RegisterIngredientItem(this);
        }

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (!IsSpawned)
        {
            OnionModel.SetActive(false);
            UnknownModel.SetActive(false);
            MushroomModel.SetActive(false);
            transform.position = new Vector3(transform.position.x, -2f, transform.position.z);
            return;
        }

        transform.position = new Vector3(transform.position.x, .8f, transform.position.z);
        switch (MyIngredientType)
        {
            case IngredientType.ONION:
                OnionModel.SetActive(true);
                UnknownModel.SetActive(false);
                MushroomModel.SetActive(false);
                break;
            case IngredientType.MUSHROOM:
                OnionModel.SetActive(false);
                UnknownModel.SetActive(false);
                MushroomModel.SetActive(true);
                break;
        }
    }

    override public ItemState GetState()
    {
        return new IngredientState(ID, MyIngredientType, IsSpawned, IsPrepared, IsCooking);
    }

    public override void LoadState(ItemState state)
    {
        IngredientState iState = state as IngredientState;
        MyIngredientType = iState.ingredientType;
        IsSpawned = iState.IsSpawned;
        IsPrepared = iState.IsPrepared;
        IsCooking = iState.IsCooking;
        UpdateVisual();
    }
}
