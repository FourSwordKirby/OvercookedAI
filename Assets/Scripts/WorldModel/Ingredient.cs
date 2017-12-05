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
    private GameObject MushroomModel;
    private GameObject PrepOnionModel;
    private GameObject PrepMushroomModel;

    private GameObject UnknownModel;


    private void Awake()
    {
        OnionModel = transform.Find("Onion Model").gameObject;
        MushroomModel = transform.Find("Mushroom Model").gameObject;
        PrepOnionModel = transform.Find("Prep Onion Model").gameObject;
        PrepMushroomModel = transform.Find("Prep Mushroom Model").gameObject;
        UnknownModel = transform.Find("Unknown Model").gameObject;

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
            MushroomModel.SetActive(false);
            PrepOnionModel.SetActive(false);
            PrepMushroomModel.SetActive(false);
            UnknownModel.SetActive(false);
            transform.position = new Vector3(transform.position.x, -2f, transform.position.z);
            return;
        }

        transform.position = new Vector3(transform.position.x, .8f, transform.position.z);
        switch (MyIngredientType)
        {
            case IngredientType.ONION:
                OnionModel.SetActive(!IsPrepared);
                MushroomModel.SetActive(false);
                PrepOnionModel.SetActive(IsPrepared);
                PrepMushroomModel.SetActive(false);
                UnknownModel.SetActive(false);
                break;
            case IngredientType.MUSHROOM:
                OnionModel.SetActive(false);
                MushroomModel.SetActive(!IsPrepared);
                PrepOnionModel.SetActive(false);
                PrepMushroomModel.SetActive(IsPrepared);
                UnknownModel.SetActive(false);
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
