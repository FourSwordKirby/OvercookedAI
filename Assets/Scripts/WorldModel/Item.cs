using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public const int NOTHING_ID = -1;
    public const int TABLE_ID = -2;

    public ItemType MyItemType;
    public int ID;

    public static int ItemCount = 0;

    protected void SetID()
    {
        ID = ItemCount;
        ++ItemCount;
    }

    private void Awake()
    {
        SetID();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
