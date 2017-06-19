using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour, ISelectable, IUpgradable {

	public float shootRate;
	public float radius;
	public float damage;
	public float armorPen;
	public DamageInfo damageInfo;


	public CircleCollider2D radiusCollider;
	public List<Enemy> currentCollisions = new List<Enemy>();
	public SpriteRenderer spriteRenderer;

	public float shotTimer;
	public Enemy targetEnemy;

	public float shootRateUpgradeChange;
	public float damageUpgradeChange;
	public float rangeUpgradeChange;
	public float shootRateUpgradeCost;
	public float damageUpgradeCost;
	public float rangeUpgradeCost;

	public virtual string UIInfo {
		get {
			return "Fire Rate: " +  string.Format("{0:0.00}", shootRate) + "\n"
				+ "Range: " + string.Format("{0:0.00}", radius) + "\n"
				+ "Damage: "+ damage;
		}
	} 

	public virtual string type {
		get {
			return "Tower";
		}
	}

	public virtual void Select() {
		spriteRenderer.color = Color.red;
	}

	public virtual void Deselect() {
		spriteRenderer.color = Color.white;
	}

	public virtual void upgradeDamage() {}
	public virtual void upgradeShotSpeed() {}
	public virtual void upgradeRange() {}

	protected virtual void checkShoot() {}
	protected virtual void attack () {}

	public void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.GetComponent<Enemy> () != null) {
			currentCollisions.Add (col.gameObject.GetComponent<Enemy> ());
		}
	}

	public void OnTriggerExit2D (Collider2D col) {
		currentCollisions.Remove (col.gameObject.GetComponent<Enemy>());
		if(currentCollisions.Count == 0) {
			currentCollisions.Clear ();
		}
		currentCollisions.TrimExcess ();
	}


		
}
