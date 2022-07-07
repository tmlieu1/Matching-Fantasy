using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum gameState
{
    playing, combat, gameWin, gameLose
};
//fire
public class GameManager : MonoBehaviour
{
    public gameState theGamesState = gameState.playing;
    // player 1-4
    public GameObject[] playersList;

    // score
    public static int score;

    // player stats
    [SerializeField]
    public static float maxPlayerHp = 100;
    public static float currPlayerHp;
    public float playerDamage = 1;
    public float playerManaDamage = 3;
    [SerializeField]
    private float playerHealing = 5;
    public float maxMana = 100;
    public float manaGainRate = 25;

    [SerializeField]
    private const int maxPlayerMp = 100;
    
    [SerializeField]
    public static int currPlayerMp;

    public static bool playerDefend = false;
    public float maxEnemyHp = 5;
    
    public float enemyDamage = 1;
    
    // enemy stats
    private GameObject [] enemies;

    //private const int maxEnemyHp = 100;
    //[SerializeField]
    //public static int currEnemyHp; // temporary for no enemy
    
    private GameObject objEnemy;
    private Enemy enemy;
    public static int level = 0;
    public float levelScaleFactor = 1.25f;


    private float waitTime = 10.0f;
    private float currTime = 0.0f;

    // game over overlay
    public GameObject overlay;

    public int numberOfProjectiles;
    private const int maxNumberProjectiles = 4;

    public int attackRatio = 1;
    public int healRatio = 1;
    public int manaRatio = 1;
    public int defenceRatio = 1;

    // Start is called before the first frame update
    void Start()
    {
        overlay.SetActive(false);
        // init some values
        level = 0;
        currPlayerHp = maxPlayerHp;
        currPlayerMp = maxPlayerMp;

        enemies = Resources.LoadAll<GameObject>("Enemies"); // All Enemy prefabs must exist in the Resources/Enemies path

        overlay.SetActive(false);

        spawnEnemy();

        score = 0;

        playersList = GameObject.FindGameObjectsWithTag("PlayerManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (theGamesState.Equals(gameState.combat))
        {
            numberOfProjectiles = maxNumberProjectiles;
            theGamesState = gameState.playing;
        }

        if (currPlayerHp <= 0){

            theGamesState = gameState.gameLose;

            foreach (GameObject obj in playersList){
                PlayerManager player = obj.GetComponent<PlayerManager>();
                player.clear();
            }
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            GameObject[] enemProjectile = GameObject.FindGameObjectsWithTag("EnemyProjectile");
            for (int i = 0; i < enemProjectile.Length; i++){
                Destroy(enemProjectile[0]);
            }
            GameObject[] plyrProjectile = GameObject.FindGameObjectsWithTag("Player Projectile");
            for (int i = 0; i < plyrProjectile.Length; i++)
            {
                Destroy(plyrProjectile[0]);
            }

            overlay.SetActive(true);
            enabled = false;
        }

        if(Enemy.hp <= 0 || enemy == null)
        {
            levelUP();
            spawnEnemy();
            FindObjectOfType<AudioManager>().Play("enemySpawn");
            //theGamesState = gameState.gameWin;
        }

        currTime += Time.deltaTime;
        if (currTime >= waitTime && theGamesState.Equals(gameState.playing))
        {
            theGamesState = gameState.combat;
            currTime = 0.0f;
        }

    }

        public void takeDamage(float damage){

        currPlayerHp -= damage;
    }

    public void onCardMatch(cardTypes cardtype)
    {
        score += 10;
        switch (cardtype)
        {
            case cardTypes.attack:
                //playerAttackPool += playerDamage;
                break;
            case cardTypes.defend:
                //playerDefensePool += playerBlockPercent;
                playerDefend = true;
                FindObjectOfType<AudioManager>().Play("armorGain");
                break;
            case cardTypes.heal:
                //playerHealingPool += playerHealing;
                FindObjectOfType<AudioManager>().Play("resourceUp");
                currPlayerHp += playerHealing;
                break;
            case cardTypes.mana:
                FindObjectOfType<AudioManager>().Play("resourceUp");
                break;
        }
    }

    public void returnToMenu() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void spawnEnemy()
    {
                
        int index = Random.Range(0,enemies.Length);

        objEnemy = GameObject.Instantiate(enemies[index], GameObject.FindGameObjectWithTag("Canvas").transform);

        Vector3 scaleChange = new Vector3(-0.3f, -0.3f, -0.01f); // This scales(the image) All enemies, I didn;t want to do it in every single prefab
        objEnemy.transform.localScale += scaleChange;
        enemy = objEnemy.GetComponent<Enemy>();
        enemy.setHP(maxEnemyHp);
    }

    public void setDefense(bool defenseVal)
    {
        playerDefend = defenseVal;
    }

    public bool getDefense()
    {
        return playerDefend;
    }

    void levelUP()
    {
        level++;
        maxEnemyHp = maxEnemyHp * levelScaleFactor;
        // TODO can adjust other paramters to scale with level
        //enemyDamage = enemyDamage * level;
        //playerAttackDamage = playerAttackDamage * level;
    }
}
