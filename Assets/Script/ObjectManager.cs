using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {
    
    public static ObjectManager instance;

    public GameObject rocketPrefab;

    List<GameObject> bullets = new List<GameObject>();

    private void Awake()
    {
        if (ObjectManager.instance == null)
        {
            ObjectManager.instance = this;
        }
    }

	void Start () {
        CreateBullets(5); // bullet 5 create
	}

    void CreateBullets(int bulletCount)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            //instantiate() 로 생성한 게임 오브잭트를  변수에 담고자 하면, as + 데이터 타임ㅇ믈 명령어 뒤에 붙혀 저야됨

            GameObject bullet = Instantiate(rocketPrefab) as GameObject;
            bullet.transform.parent = transform;
            bullet.SetActive(false);

            bullets.Add(bullet);

        }

    }
    public void ClearBullets()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i].SetActive(false);
        }

    }
    public GameObject GetBullet(Vector3 pos)
    {
        GameObject reqBullet = null;
        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i].activeSelf == false)
            {
                reqBullet = bullets[i];//

                break;

            }

        }
        if (reqBullet == null)//추가 총알 생성
        {
            GameObject newBullet = Instantiate(rocketPrefab) as GameObject;
            newBullet.transform.parent = transform;

            bullets.Add(newBullet);
            reqBullet = newBullet;
        }

        reqBullet.SetActive(true);
        reqBullet.transform.position = pos;

        return reqBullet;


    }

	void Update () {
		
	}


}
