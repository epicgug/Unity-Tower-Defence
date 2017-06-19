using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE : Tower {

	public override string type {
		get {
			return "AoE Tower";
		}
	}

//	public string UIInfo {
//		get {
//			return "Fire Rate: " +  string.Format("{0:0.00}", shootRate) + "\n"
//				+ "Range: " + string.Format("{0:0.00}", radius) + "\n"
//				+ "Damage: "+ damage;
//		}
//	} 

	void Start () {
		this.shotTimer = 0f;
		radiusCollider.radius = radius;
		damageInfo = new DamageInfo (damage, armorPen);
	}
	
	void Update () {
		this.shotTimer += Time.deltaTime;

		checkShoot ();
	}

//	public override void Select() {
//		spriteRenderer.color = Color.red;
//		//		UI.localUI.testInfoHash.text = this.GetHashCode () + "";
//	}
//
//	public override void Deselect() {
//		spriteRenderer.color = Color.white;
//	}

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
		
	protected override void checkShoot() {
		if(this.shotTimer >= this.shootRate) {
			this.attack ();
		}
	}

	protected override void attack () {
		shotTimer = 0;
		foreach (Enemy enemy in currentCollisions) {
			if (enemy != null) {
				enemy.SendMessageUpwards ("receiveDamage", damageInfo);

			}
		}
	}
}
