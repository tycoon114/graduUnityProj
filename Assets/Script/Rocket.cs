﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
  public float moveSpeed = 0.45f;
	// Use this for initialization
	void Start () {

      
	}
    private void OnBecameInvisible()
    {
      // Destroy(gameObject);
        gameObject.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
        float moveX = moveSpeed * Time.deltaTime;
        transform.Translate(moveX, 0, 0);
	}
}
