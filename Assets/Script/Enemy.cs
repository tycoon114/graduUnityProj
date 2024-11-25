using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int destroyScore = 100;
    public float moveSpeed = 0.5f;
    public GameObject explosion;
	// Use this for initialization
	void Start () {
		
	}


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Rocket")
        {

           Instantiate(explosion,transform.position, Quaternion.identity);

            GameManager.instance.AddScore(destroyScore);
            //rocket
            // Destroy(col.gameObject);
           col.gameObject.SetActive(false);
            //enemy
            Destroy(gameObject);
        }

    }
    void MoveControl() {
        float yMove = moveSpeed * Time.deltaTime;
        transform.Translate(0, -yMove, 0);
    }
	void Update () {
        MoveControl();
	}
}
