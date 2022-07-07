using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{

    public static float hp = 100;
    private float maxHp = 100;

    public GameObject attackProjectile;

    private GameManager GM;

    private GameObject[] numOfPlayers;

    public Slider slider;

    public float speed = 10;
    private float waitTime;
    public float startWaitTime = 10;
    public GameObject[] moveSpots;
    private int randomSpot;
    float rot = 0;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        maxHp = GM.maxEnemyHp;
        hp = maxHp;
        numOfPlayers = GameObject.FindGameObjectsWithTag("Player");

        moveSpots = GameObject.FindGameObjectsWithTag("MoveSpots");
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);

    }

    private void move()
    {
        Vector3 targetPos = moveSpots[randomSpot].transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        //if (transform.position.x < targetPos.x)
        //{
        //    transform.rotation = Quaternion.identity;
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Euler(0, 180, 0);
        //}

        //if (transform.position.y < targetPos.y)
        //{
        //    transform.rotation = Quaternion.identity;
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Euler(-180, 0, 0);
        //}


        if (Vector2.Distance(transform.position, moveSpots[randomSpot].transform.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
                rot += 90;
                transform.Rotate(0, 0, rot, Space.Self);
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GM.theGamesState == gameState.combat)
        {
            for(int i = 0; i < 4; i++)
            {
                Vector3 pos = transform.position;
                Quaternion rot = transform.rotation;
                GameObject gO = Instantiate(attackProjectile, pos, rot);
                ProjectileScript proj = gO.GetComponent<ProjectileScript>();
                proj.targetObject = numOfPlayers[i];
            }
            FindObjectOfType<AudioManager>().Play("enemyAttack");
        }

        if(hp <= 0 || GM.theGamesState.Equals(gameState.gameLose))
        {
            Destroy(gameObject);
        }

        move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player Projectile" || collision.gameObject.tag == "Player Mana Projectile")
        {
            hp -= collision.GetComponent<ProjectileScript>().getDamage();
            slider.value = hp/maxHp;
            GetComponent<Animator>().Play("Hit");
            FindObjectOfType<AudioManager>().Play("enemyHit");       
        }
    }

    public void setHP(float hp)
    {
        maxHp = hp;
    }
}
