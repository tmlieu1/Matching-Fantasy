using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
   
    private float damage;
    public float speed= 1f;
    public float lifetime = 200;
    public GameObject targetObject;
    
    private Vector3 targetPos;
    private GameManager GM;

    private string tagOfTarget;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (gameObject.tag == "Player Projectile")
        {
            damage = GM.playerDamage;
            targetObject = GameObject.FindGameObjectWithTag("Enemy");
            tagOfTarget = "Enemy";
        }
        else if (gameObject.tag == "Player Mana Projectile")
        {
            damage = GM.playerManaDamage;
            targetObject = GameObject.FindGameObjectWithTag("Enemy");
            tagOfTarget = "Enemy";
        }
        else if (gameObject.tag == "EnemyProjectile")
        {
            damage = GM.enemyDamage * GM.levelScaleFactor / 4 ; // divide by 4 because 4 projectiles are sent
            tagOfTarget = "Player";
        }
        targetPos = targetObject.transform.position;
    }
    void Update()
    {    
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPos) < 0.2f)
        {
            GetComponent<Animator>().Play("Destroy");
        }
        
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == tagOfTarget)
        {
            GetComponent<Animator>().Play("Destroy"); //This calls a destroy script
        }
    }

    public float getDamage()
    {
        return damage;
    }
}
