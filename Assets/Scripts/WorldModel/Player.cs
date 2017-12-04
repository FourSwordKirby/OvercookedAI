using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Vector3 HoldPosition;
    public bool IsHolding;

    private Animator anim;

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
        IsHolding = !s.HandsFree();
    }
}
