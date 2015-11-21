using UnityEngine;
using System.Collections;

public class ImageScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
        if (Input.anyKeyDown == true)
            Application.LoadLevel("MenuScene");
        }
}
