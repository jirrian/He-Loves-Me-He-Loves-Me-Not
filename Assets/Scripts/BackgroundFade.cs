using UnityEngine;
using System.Collections;

public class BackgroundFade : MonoBehaviour {

	Vector3 pos;
	// Use this for initialization
	void Start () {
		
	
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y < 25f){
			pos = new Vector3(0, transform.position.y + 0.008f, 0);
			transform.position = pos;
		}
	}
}
