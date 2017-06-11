using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour, ISelectable, IUpgradable {

	public float shootRate;
	public float radius;
	public float damage;


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
		get { return ""; }
	} 

	public virtual string type {
		get {
			return "Tower";
		}
	}

	public virtual void Select() {}
	public virtual void Deselect() {}

	public virtual void upgradeDamage() {}
	public virtual void upgradeShotSpeed() {}
	public virtual void upgradeRange() {}

	protected virtual void checkShoot() {}
	protected virtual void attack () {}


}
