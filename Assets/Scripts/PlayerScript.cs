using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject manaProjectilePrefab;
    private GameObject gameManagerObj;
    private GameManager theGameManager;
    // Start is called before the first frame update
    void Start()
    {
        //transform.localRotation = Quaternion.LookRotation(Vector3.back ,Vector3.up); // Flip backwards
        gameManagerObj = GameObject.FindGameObjectWithTag("GameManager");
        theGameManager = gameManagerObj.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (theGameManager.theGamesState.Equals(gameState.combat))
        {
            theGameManager.numberOfProjectiles = 4;
        }
        */

    }

    public void fire()
    {
        FindObjectOfType<AudioManager>().Play("throw1");
        GameObject projectile = Instantiate(projectilePrefab, transform);
        //projectile.GetComponent<ProjectileScript>().targetObject = GameObject.FindGameObjectWithTag("Enemy");
    }

    public void fireManaProjectile()
    {
        FindObjectOfType<AudioManager>().Play("throw1");
        GameObject projectile = Instantiate(manaProjectilePrefab, transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // note need to figure out when to reset the defense pool
        if(collision.gameObject.tag == "EnemyProjectile")
        {
            float realDamage;
            if (GameManager.playerDefend)
            {
                theGameManager.numberOfProjectiles -= 1;
                realDamage = 0;
            }
            else
            {
                realDamage = collision.GetComponent<ProjectileScript>().getDamage();
            }
            theGameManager.takeDamage(realDamage);
            if(theGameManager.numberOfProjectiles == 0 && theGameManager)
            {
                theGameManager.setDefense(false);
                FindObjectOfType<AudioManager>().Play("armorBreak");
            }

            GetComponent<Animator>().Play("Hit");
        }
    }
}
