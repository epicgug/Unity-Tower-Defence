using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
	public float radius;
	public Transform trans;
	public Rigidbody2D rb;

	public float Left { get { return this.transform.position.x - this.radius; } }
	public float Right { get { return this.transform.position.x + this.radius; } }
	public float Top { get { return this.transform.position.y + this.radius; } }
	public float Bottom { get { return this.transform.position.y - this.radius; } }

	public Vector2 MinPoint { get { return new Vector2 (this.Left, this.Bottom); } }
	public Vector2 MaxPoint { get { return new Vector2 (this.Right, this.Top); } }

	public float Radius {
		get {
			return this.radius;
		}

		set {
			this.radius = value;
			transform.localScale = radius * new Vector3(.3f, .3f, .3f);
		}
	}
	// Use this for initialization
	void Start () {
		float theta = Random.Range(0, 2*Mathf.PI);
		Radius = Random.Range (.8f, 1.6f);
		rb.velocity = new Vector2 (Mathf.Cos (theta), Mathf.Sin (theta)) * Random.Range (10, 11);
	}
	
	// Update is called once per frame
	void Update () {
		CollisionBoundaryx ();
		CollisionBoundaryy ();
		trans.eulerAngles += new Vector3 (0, 0, -trans.eulerAngles.z + (Mathf.Rad2Deg * Mathf.Atan2 (rb.velocity.y, rb.velocity.x)));
	}

	void CollisionBoundaryy () {
		if(this.Top > 5) {
			trans.position = new Vector3(trans.position.x, 5 - radius, 0);
		} else if(this.Bottom < -5) {
			trans.position = new Vector3 (trans.position.x, -5 + radius, 0);
		} else {
			return;
		}
		rb.velocity -= Vector2.up * 2 * rb.velocity.y;
	}

	void CollisionBoundaryx () {
		if(this.Left < -9.6f) {
			trans.position = new Vector3 (-9.6f + radius, trans.position.y, 0);
		} else if(this.Right > 9.6f) {
			trans.position = new Vector3 (9.6f - radius, trans.position.y, 0);
		} else {
			return;
		}
		rb.velocity -= Vector2.right * 2 * rb.velocity.x;
	}

	void OnCollisionEnter2D() {
		Radius += Random.Range(-.1f, .1f);
	}
}