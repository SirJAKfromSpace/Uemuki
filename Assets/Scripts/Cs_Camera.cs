using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_Camera : MonoBehaviour {

    public Vector3 offset;
    Transform playerTransform;

    // Use this for initialization
    private void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate () {
        transform.position = playerTransform.position + offset;
	}
}
