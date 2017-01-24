using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour {
	public float shootRate;
	public float radius;
	public float damage;
	public float turnRate;
	public float shotSpeed;

	public Transform cannonMuzzle;
	public GameObject cannonProjectile;
	public CircleCollider2D radiusCollider;
	public LineRenderer line;

	private List<Enemy> currentCollisions = new List<Enemy>();
	private float timer;
	private Quaternion lookRotation;
	public Enemy enemy;
	// Use this for initialization
	void Start () {
		radiusCollider.radius = radius;
		// Debug.Log(AssetDatabase.GetAssetPath);
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if(enemy != null) {
			line.SetPosition (0, transform.position);
			line.SetPosition (1, enemy.transform.position);
		}

		if(currentCollisions.Count != 0) {
			if(lookRotation != null)
				if(Time.deltaTime > 0)
					cannonMuzzle.rotation = Quaternion.Lerp (cannonMuzzle.rotation, lookRotation, Time.deltaTime * turnRate);
		}

		if (enemy != null) {
			AimPosition (enemy.transform.position);	
		}

		calculateFarthestEnemy ();
		checkShoot ();
	}

	void OnCollisionEnter2D(Collision2D col) {
//		Debug.Log ("enter");
		currentCollisions.Add (col.gameObject.GetComponent<Enemy>());
	}

	void OnTriggerEnter2D(Collider2D col) {
		//Debug.Log ("entered");
		currentCollisions.Add (col.gameObject.GetComponent<Enemy>());
	}

	void OnTriggerExit2D (Collider2D col) {
		currentCollisions.Remove (col.gameObject.GetComponent<Enemy>());
		if(currentCollisions.Count == 0) {
			currentCollisions.Clear ();
		}
		currentCollisions.TrimExcess ();

	}

	void checkShoot() {
		if(timer >= shootRate && enemy != null) {
			shootTarget ();
		}
	}

	void calculateFarthestEnemy() {
		Enemy farthestTarget = null;
		int highestIndex = 0;
		float farthestT = 0;
		Enemy farthestEnemy = null;
		for(int i = 0; i < currentCollisions.Count; i++) {
			if (currentCollisions [i] != null) {
				if (currentCollisions [i].pathNodeIndex >= highestIndex) {
					highestIndex = currentCollisions [i].pathNodeIndex;
					farthestTarget = (currentCollisions [i]);
				}
			}
		}
		for(int i = 0; i < currentCollisions.Count; i++) {
			Enemy collider = currentCollisions [i];
			if (collider == null)
				continue;
			if (collider.pathNodeIndex != highestIndex)
				continue;
			if (collider.t > farthestT) {
				farthestT = collider.t;
				farthestEnemy = collider;
			}
		}
		enemy = farthestEnemy;

	}

	void shootTarget() {
		timer = 0f;
		GameObject bullet = (GameObject)Instantiate (cannonProjectile, cannonMuzzle.position, cannonMuzzle.rotation);
		if (bullet != null) {
			bullet.SendMessage ("ReceiveEndPoint", enemy);
			bullet.SendMessage ("setDamage", damage);
		}
	}

	void AimPosition(Vector3 target) {
//		Vector3 aimPoint = new Vector3 (target.x, target.y, 0) - transform.position;
//		lookRotation = Quaternion.LookRotation (aimPoint);
		Vector3 dir = enemy.transform.position - transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}


}
