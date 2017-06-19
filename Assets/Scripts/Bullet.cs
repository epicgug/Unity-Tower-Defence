using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private Vector3 currentPosition;
	private Enemy endPointEnemy;

	public float moveSpeed;

	private DamageInfo damageInfo;

	private float increment;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(increment <=1)
			increment += moveSpeed / 100; 
		if (endPointEnemy != null) {
			transform.position = Vector3.Lerp (transform.position, endPointEnemy.transform.position, increment);
		} else {
			Destroy (this.gameObject);
		}
	}

	void ReceiveEndPoint(Enemy enemy) {
		this.endPointEnemy = enemy;
	}

	void setDamageInfo(DamageInfo damageInfo) {
		this.damageInfo = damageInfo;
	}

	void OnTriggerEnter2D (Collider2D col) {
		if(col.gameObject.tag == "Enemy") {
			Destroy (this.gameObject);
			col.SendMessage ("receiveDamage", damageInfo);
		}
	}
}
