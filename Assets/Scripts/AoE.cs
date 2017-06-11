using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE : Tower {

	public string type {
		get {
			return "AoE Tower";
		}
	}

	public string UIInfo {
		get {
			return "Fire Rate: " +  string.Format("{0:0.00}", shootRate) + "\n"
				+ "Range: " + string.Format("{0:0.00}", radius) + "\n"
				+ "Damage: "+ damage;
		}
	} 

	void Start () {
		this.shotTimer = 0f;
		radiusCollider.radius = radius;
	}
	
	void Update () {
		this.shotTimer += Time.deltaTime;

		checkShoot ();
	}

	public override void Select() {
		spriteRenderer.color = Color.red;
		//		UI.localUI.testInfoHash.text = this.GetHashCode () + "";
	}

	public override void Deselect() {
		spriteRenderer.color = Color.white;
	}

	public override void upgradeDamage() {
		damage += 1;
		GC.local.gold -= damageUpgradeCost;
	}

	public override void upgradeShotSpeed() {
		if(shootRate < shootRateUpgradeChange) {
			Debug.Log ("fix");
		} else {
			shootRate -= shootRateUpgradeChange;
			GC.local.gold -= shootRateUpgradeCost;
		}
	}

	public override void upgradeRange() {
		radius += 0.5f;
		GC.local.gold -= rangeUpgradeCost;
		radiusCollider.radius = radius;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.GetComponent<Enemy> () != null) {
			Debug.Log ("entered" + col.ToString ());
			currentCollisions.Add (col.gameObject.GetComponent<Enemy> ());
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		currentCollisions.Remove (col.gameObject.GetComponent<Enemy>());
		if(currentCollisions.Count == 0) {
			currentCollisions.Clear ();
		}
		currentCollisions.TrimExcess ();
	}

	protected override void checkShoot() {
		if(this.shotTimer >= this.shootRate) {
			this.attack ();
		}
	}

	protected override void attack () {
		shotTimer = 0;
		foreach (Enemy enemy in currentCollisions) {
			if (enemy != null)
				enemy.SendMessageUpwards ("receiveDamage", damage);
		}
	}
}
