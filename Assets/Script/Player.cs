using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool canShoot = false;
    public GameObject explosion;
   // public GameObject rocket;
    
    float shootDelay = 0.5f;
    float shootTimer = 0;
    public float moveSpeed = 0.5f;
   public  string commands ="";
    Vector3 playerPos;
	// Use this for initialization
	private void Start() {
        playerPos = transform.position;
	}

    void MoveControl() {
        float moveX = moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        transform.Translate(moveX, 0, 0);

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        viewPos.x = Mathf.Clamp01(viewPos.x);

        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        transform.position = worldPos;


    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
             Instantiate(explosion, transform.position, Quaternion.identity);
       

            GameManager.instance.KillPlayer();

            Destroy(col.gameObject);
           // Destroy(gameObject);
            InactivePlayer();

        }

    }
    void InactivePlayer()
    {
        gameObject.SetActive(false);
        canShoot = false;

        transform.position = playerPos;

    }
    void ShootControl()
    {
        if (canShoot == true)
        {
            if (shootTimer > shootDelay) {
               // Instantiate(rocket, transform.position, rocket.transform.rotation);
                ObjectManager.instance.GetBullet(transform.position);
                shootTimer = 0;
            }

            shootTimer += Time.deltaTime;
        }
    }

	void Update () {
        MoveControl();
        ShootControl();
	}


}
