using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : Tower {

	public Vector2 aimPosition;
	public GameObject reticle;

	public override string type {
		get {
			return "Mortar Tower";
		}
	}

	void Start () {
		this.shotTimer = 0f;
		radiusCollider.radius = radius;
		damageInfo = new DamageInfo (damage, armorPen);
	}

	void Update () {
		this.shotTimer += Time.deltaTime;

		checkShoot ();

		if(GC.local.Selected == this) {
			if (UI.localUI.thingToDoNext == UI.ThingsToDoNext.PLACE_MORTAR_TARGET) {
				Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				mousePos.z = 0;
				aimPosition = new Vector2 (mousePos.x, mousePos.y);
				reticle.transform.position = aimPosition;
			}
		}
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

	protected override void attack () {
		shotTimer = 0;
		foreach (Enemy enemy in currentCollisions) {
			if (enemy != null) {
				enemy.SendMessage ("receiveDamage", damageInfo);
				Debug.Log (damageInfo.ToString ());
			}
		}
	}

	protected override void checkShoot() {
		if(this.shotTimer >= this.shootRate) {
			this.attack ();
		}
	}

	public void ChangeTarget() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		aimPosition = new Vector2 (mousePos.x, mousePos.y);
		reticle.transform.position = aimPosition;
	}

	IEnumerator WaitForKeyDown(KeyCode keyCode)
	{
		do {
			System.Threading.Thread.Sleep(1);
			yield return null;
		} while (!Input.GetKeyDown (keyCode));
	}

	IEnumerator WaitForKeyUp(KeyCode keyCode)
	{
		do {
			System.Threading.Thread.Sleep(1);
			yield return null;
		} while (!Input.GetMouseButtonUp (0));
	}

	override public void Select() {
		spriteRenderer.color = Color.red;
		reticle.SetActive (true);

	}

	override public void Deselect() {
		spriteRenderer.color = Color.white;
		reticle.SetActive (false);
	}

}
