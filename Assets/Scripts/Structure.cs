using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour {

	public CircleCollider2D circleCollider;
	public BoxCollider2D boxCollider;
	public float radius;
	public float attackRate;
	public float lastAttackTime;
	public int level;

	public static float cost;
	public Tower[] levelGuides;
	// Use this for initialization
	void Start () {
		circleCollider.radius = radius;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
