using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo {
	public float damage;
	public float armorPen;

	public DamageInfo (float damage, float armorPen) {
		this.damage = damage;
		this.armorPen = armorPen;
	}
}
