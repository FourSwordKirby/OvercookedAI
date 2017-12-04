using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Vector3 HoldPosition;
    public Item HoldingItem;

    private Animator anim;

    public ItemManager ItemManagerRef;

    public ItemManager GetItemManager()
    {
        if (ItemManagerRef != null)
        {
            return ItemManagerRef;
        }

        ItemManagerRef = FindObjectOfType<ItemManager>();
        if (ItemManagerRef == null)
        {
            Debug.LogError("Missing ItemManager!");
            return null;
        }
        else
        {
            return ItemManagerRef;
        }
    }

    public bool IsHolding
    {
        get { return HoldingItem != null; }
    }

    private void Awake()
    {
        HoldPosition = transform.Find("Hold Position").position;
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("IsHolding", IsHolding);
	}

    public void LoadState(PlayerState s)
    {
        if (s.HandsFree())
        {
            HoldingItem = null;
        }
        else
        {
            HoldingItem = GetItemManager().ItemList[s.HoldingItemID];
            HoldingItem.transform.position = HoldPosition;
        }
    }

    public PlayerState GetPlayerState()
    {
        return new PlayerState();
    }
}
