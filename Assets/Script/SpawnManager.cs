using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour {
    public static SpawnManager instance;
    List<GameObject> enemies = new List<GameObject>();
    Vector3[] positions = new Vector3[5];

    public GameObject enemy;
    public bool isSpawn = false;

    public float spawnDelay = 1.5f;
    float spawnTimer = 0f;

    private void Awake()
    {
        if (SpawnManager.instance == null)
        {
            SpawnManager.instance = this;
        }

    }
	// Use this for initialization
	void Start () {
        CreatePositions();
	}
    void CreatePositions() {
        float viewPosY = 1.2f;
        float gapX = 1f / 6f;
        float viewPosX = 0f;
        for (int i = 1; i < positions.Length; i++) {
            viewPosX = gapX + gapX * i;

            Vector3 viewpos = new Vector3(viewPosX,viewPosY, 0);

            Vector3 WorldPos = Camera.main.ViewportToWorldPoint(viewpos);

            WorldPos.z = 0f;
            positions[i] = WorldPos;

        }
    }

    void SpawnEnemy()//spawn이 트루 일때 랜덤 적 생성
    {
        if (isSpawn == true)
        {
            if (spawnTimer > spawnDelay)
            {
                int rand = Random.Range(0, positions.Length);

                GameObject enemyObj = Instantiate(enemy, positions[rand], Quaternion.identity) as GameObject;
                enemies.Add(enemyObj);

                //Instantiate(enemy, positions[rand], Quaternion.identity);
                spawnTimer = 0f;
            }
            spawnTimer += Time.deltaTime;
        }

    }
    public void ClearEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                Destroy(enemies[i]);
            }

        }

        enemies.Clear();
    }

	// Update is called once per frame
	void Update () {
        SpawnEnemy();
	}
}
