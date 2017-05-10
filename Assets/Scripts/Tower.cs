using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour, ISelectable, IUpgradable {

	public float shootRate;
	public float radius;
	public float damage;

	public GameObject cannonProjectile;
	public CircleCollider2D radiusCollider;
	private List<Enemy> currentCollisions = new List<Enemy>();

	private float shotTimer;
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

	void Start () {}
	void Update () {} 

	public virtual void upgradeDamage() {}
	public virtual void upgradeShotSpeed() {}
	public virtual void upgradeRange() {}

	void OnTriggerEnter2D() {}
	void OnTriggerExit2D() {}

	virtual void checkShoot() {}
	virtual void attack () {}


}
