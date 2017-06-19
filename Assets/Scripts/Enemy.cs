using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, ISelectable{

	public float health, speed, size, nodeDistThreshhold, speedMin, speedMax, jiggleVariance, maxVariance, armor;
	public float maxHealth;
	public int pathNodeIndex; 
	private Vector2 nextPointDirection, offset;
	public float t;
	public SpriteRenderer spriteRenderer;

	public GameObject healthBar;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		speed = Random.Range (speedMin, speedMax);
		pathNodeIndex = 0;
		transform.position = new Vector3(GC.nodes [pathNodeIndex].x, GC.nodes [pathNodeIndex].y, -1);
	}
	
	// Update is called once per frame
	void Update () {
		Movement ();
	}

	void Movement() {
//		ComputeT ();
		Vector2 AB = GC.nodes [pathNodeIndex + 1] - GC.nodes [pathNodeIndex];
		t += (this.speed * Time.deltaTime * 1/AB.magnitude);	
		if(t > 1) {
			if(pathNodeIndex + 2 < GC.nodes.Length) {
				pathNodeIndex++;
				t -= (int)t;
			} else {
				Destroy (this.gameObject); //TODO: add lives
			}
		}
		jiggleOffset ();
		transform.position = ComputeX () + (Vector3)offset;

	}

	void jiggleOffset() {
		offset += new Vector2 (Random.Range (-jiggleVariance, jiggleVariance), Random.Range (-jiggleVariance, jiggleVariance));
		if(offset.magnitude > maxVariance) {
			offset = offset / offset.magnitude * maxVariance;
		} 
	}

	void ComputeT() {
		Vector2 AB = GC.nodes [pathNodeIndex + 1] - GC.nodes [pathNodeIndex];
		float l = AB.magnitude;
		Vector2 AX = (Vector2)transform.position - GC.nodes [pathNodeIndex];
		t = Vector2.Dot(AX/l, AB/l);
	}

	Vector3 ComputeX() {
		Vector2 AB = GC.nodes [pathNodeIndex + 1] - GC.nodes [pathNodeIndex];
		return (Vector3)(GC.nodes [pathNodeIndex] + t * AB);
	}

	void receiveDamage(DamageInfo damageInfo) {
		float armorRemaining = armor - damageInfo.armorPen;
		if(armorRemaining < 0) {
			armorRemaining = 0;
		}
		damageInfo.damage -= armorRemaining;
		if(damageInfo.damage < 1) {
			damageInfo.damage = 1;
		}
		health -= damageInfo.damage;
		if(health <= 0) {
			if (GC.local.Selected != null && this.GetHashCode () == GC.local.Selected.GetHashCode ()) {
				GC.local.Selected = null;
			}
			Destroy (this.gameObject);
			GC.local.gold += 5;
		}
		float calcHealth = health / maxHealth;
		setHealthBar(calcHealth);
	}

	void setHealthBar(float health) {
		healthBar.transform.localScale = new Vector3(health, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}

	public void Select() {
		spriteRenderer.color = Color.red;
	}

	public void Deselect() {
		spriteRenderer.color = Color.white;
	}

	public string UIInfo {
		get {
			return "Type: " + this.gameObject.name + "\n"
			+ "Health: " + health;
		}
	} 

	public string type {
		get {
			return "Enemy";
		}
	}
 }
