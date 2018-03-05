using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyTextScript : MonoBehaviour {

    public int advance = 0;
    CameraScript camScript;

	void Start () {
        camScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
    }
	
	// Update is called once per frame
	void Update () {
        advance++;
        transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y + 0.1f), Quaternion.Euler(0f,0f,0f));

        if (advance > 50)
        {
            Destroy(transform.gameObject);
        }
	}
}
