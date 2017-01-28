using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public float minX;
	public float minY;
	public float maxX;
	public float maxY;
	public int width;
	public int height;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	float getFirstHorizontal() {
		float cellWidth = (maxX - minX) / width;
		return  (cellWidth / 2) + minX;
	}

	float getFirstVertical() {
		float cellHeight = (maxY - minY) / height;
		return  (cellHeight / 2) + minY;
	}

	float getLastHorizontal() {
		float cellWidth = (maxX - minX) / width;
		return  maxX - (cellWidth / 2);
	}

	float getLastVertical() {
		float cellHeight = (maxY - minY) / height;
		return  maxY - (cellHeight / 2);
	}

	public Vector3 remapPoint(Vector3 input) {
		Vector2 index = new Vector2 ();
		index.x = (input.x - getFirstHorizontal ()) / (getLastHorizontal () - getFirstHorizontal ());
		index.y = (input.y - getFirstVertical ()) / (getLastVertical () - getFirstVertical ()); 
		index.x *= width - 1;
		index.y *= height - 1;
		index.x = (int) (index.x + .5f);
		index.y = (int) (index.y + .5f);
		index.x /= width - 1;
		index.y /= height - 1;
		Vector3 output = new Vector3 ();
		output.x = Mathf.Lerp (getFirstHorizontal (), getLastHorizontal (), index.x);
		output.y = Mathf.Lerp (getFirstVertical (), getLastVertical (), index.y);
		output.z = -3;
		return output;
	}
	
}
