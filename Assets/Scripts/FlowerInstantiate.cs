using UnityEngine;
using System.Collections;

public class FlowerInstantiate : MonoBehaviour {

	public Transform petalOnePrefab;
	public Transform petalTwoPrefab;
	
	public Color[] petalColors;

	// Use this for initialization
	void Start () {
		// choose number of petals
		int numPetals = Random.Range(13,18);

		float radius = 0.5f;

		// instatiate petals radially
		Vector3 center = gameObject.transform.position;
		Vector3 pos;

		for(int i = 0; i < numPetals; i++){
			float angle = ((i * 1.0f) / numPetals) * Mathf.PI * 2;

			float x = Mathf.Sin(angle) * radius;
			float y = Mathf.Cos(angle) * radius;

			pos = new Vector3(x,y) + center;

			// randomly choose petal prefab
			// instantiate
			Transform petalToInstantiate;

			if(Random.value > 0.5f){
				petalToInstantiate = petalOnePrefab;
			}
			else{
				petalToInstantiate = petalTwoPrefab;
			}

			Transform newPetal = (Transform) Instantiate(petalToInstantiate, pos, Quaternion.Euler(0, 0, -1 * angle * Mathf.Rad2Deg));
			
			// change color
			int order = Random.Range(0,numPetals);
			int colorIdx;
			if(order <= (numPetals / 3)){
				colorIdx = 0;
			}
			else if(order <= (2 * numPetals / 3)){
				colorIdx = 1;
			}
			else{
				colorIdx = 2;
			}
			newPetal.GetComponent<SpriteRenderer>().material.color = petalColors[colorIdx];

			// change order
			newPetal.GetComponent<SpriteRenderer>().sortingOrder = 49 - order;

			// add as child of center
			newPetal.transform.parent = gameObject.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
