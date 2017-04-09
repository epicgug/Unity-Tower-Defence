using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public static Grid local;

	public float minX;
	public float minY;
	public float maxX;
	public float maxY;
	public int width;
	public int height;

	private bool[] towerSpaces = new bool[144];

	// Use this for initialization
	void Start () {
		Grid.local = this;
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

	Vector3 computeIndex(Vector3 input) {
		Vector3 index = new Vector2 ();
		index.x = (input.x - getFirstHorizontal ()) / (getLastHorizontal () - getFirstHorizontal ());
		index.y = (input.y - getFirstVertical ()) / (getLastVertical () - getFirstVertical ()); 
		index.x *= width - 1;
		index.y *= height - 1;
		index.x = (int) (index.x + .5f);
		index.y = (int) (index.y + .5f);
		return index;
	}

	public Vector3 remapPoint(Vector3 input) {
		Vector3 index = computeIndex (input);
		index.x /= width - 1;
		index.y /= height - 1;
		Vector3 output = new Vector3 ();
		output.x = Mathf.Lerp (getFirstHorizontal (), getLastHorizontal (), index.x);
		output.y = Mathf.Lerp (getFirstVertical (), getLastVertical (), index.y);
		output.z = -3;
		return output;
	}

	int getIndex(int x, int y) {
		return x + (width * y);
	}

	public bool placeTower(Vector3 position) {
		Vector3 posIndex = computeIndex (position);
		int index = getIndex ((int)posIndex.x, (int)posIndex.y);
		bool isTowerThere = towerSpaces[index];
		if(isTowerThere) {
			return false;
		}
		towerSpaces [index] = true;
		return true;
	}
}
