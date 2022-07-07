using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //checks if the game is over or not
    private bool gameActive = true;

    // specify boards width and height
    [SerializeField]
    private int width = 7;
    [SerializeField]
    private int height = 4;

    public int attackVal = 0;
    public float defenseVal = 0f;
    
   
    public float mana = 0;
    private float manaGainRate = 25;
    public float maxMana = 100;

    public float cardTimeout = 1.0f;
    private bool checkMatchInUse = false;

    //public Sprite[] sprites;  // 0 = Face down, 1 = attack, 2 = defense, 3 heal
    private GameObject canvas;
    private Vector2 boardPosition;
    public RectTransform boardObj;
    public GameObject prefabCard;
    public GameObject playerObj;
    private PlayerScript playerScript;

    //private GameObject objGameManager;
    private GameManager GM;

    //public Sprite[] sprites; // Change this to list of lists// maybe make this a dictionary to call attack cards etc
    private Sprite[] attackCards;// put this list inside sprites list
    private Sprite[] manaCards;
    private Sprite[] healingCards;
    private Sprite[] defenseCards;
    private Sprite[] faceDownColorCards;

    // These are updated and set by the game manager
    private int attackRatio;
    private int healRatio;
    private int manaRatio;
    private int defenceRatio;

    private RectTransform canvasRectTransform;
    private RectTransform cardDimensions;
    public List<CardScript> faceUpCards = new List<CardScript>();// mabye set to private and use a getter and setter so this doesn't show up in editor

    public List<CardScript> allCards = new List<CardScript>();

    public List<GameObject> board = new List<GameObject>();

    private Vector2 cardPositionMax;

    // Start is called before the first frame update
    void Start()
    {

        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        maxMana = GM.maxMana;
        manaGainRate = GM.manaGainRate;


        boardPosition = boardObj.GetComponent<RectTransform>().position;
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        cardPositionMax = new Vector2(canvasRectTransform.rect.width - (boardObj.rect.xMin + boardObj.rect.xMax), 
                                      canvasRectTransform.rect.height - (boardObj.rect.yMin + boardObj.rect.yMax));

        cardDimensions = prefabCard.GetComponent<RectTransform>();

        attackCards = Resources.LoadAll<Sprite>("Cards/Attack");
        manaCards = Resources.LoadAll<Sprite>("Cards/Mana"); 
        healingCards = Resources.LoadAll<Sprite>("Cards/Heal");
        defenseCards = Resources.LoadAll<Sprite>("Cards/Defense");
        faceDownColorCards = Resources.LoadAll<Sprite>("Cards/FaceDownColors");
        playerScript = playerObj.GetComponent<PlayerScript>();

        initBoard();
        shuffleBoard();
        setCardLocations(boardPosition);

    }

    void Update()
    {
        if (faceUpCards.Count == 2)
        {
            StartCoroutine(checkMatch(cardTimeout)); // delayed card flips and matches.
        }
        if (board.Count <= 0 && gameActive)
        {
            //play audio
            FindObjectOfType<AudioManager>().Play("shuffle");

            //reset board
            initBoard();
            shuffleBoard();
            setCardLocations(boardPosition);
        }
        if(mana == maxMana)  // temp for testing purposes of the frenzy mechanic.
        {
            //manaFrenzy();
            mana = 0;
            manaReveal();
            //manaAttack();
        }
    }

    private void manaAttack()
    {
        playerScript.fireManaProjectile();
    }

    private void manaReveal()
    {
        Debug.Log("we in");
        foreach (GameObject card in board)
        {
            card.GetComponent<CardScript>().changeFaceDownToColor();
        }
    }

    private void manaFrenzy()
    {
        int remainingCards = board.Count;
        int frenzyMatches = Mathf.FloorToInt((remainingCards / 2)/2);

        Debug.Log("matches = " + frenzyMatches);

        CardScript card1;
        CardScript card2;

        for(int i = 0; i < frenzyMatches; i++)
        {
            if (allCards[i].currentState.Equals(cardStates.faceUp) || allCards[i] == null)
            {
                continue;
            }
            card1 = allCards[i];
            Debug.Log("i = " + i);
            card1.flip();
            for(int j = 0; j < board.Count; j++)
            {
                
                if (allCards[j].currentState.Equals(cardStates.faceUp) || allCards[j] == null)
                {
                    continue;
                }
                Debug.Log("j = " + j);
                card2 = allCards[j];
                card2.flip();
                
                if(card1.matchID == card2.matchID)
                {
                    card2.flip();
                    card1.flip();
                    break;
                }
                
                /*
                if(card1.matchID == card2.matchID)
                {
                    matchedCards(card1, card2);
                    break;
                }
                else
                {
                    card1.flip();
                    card2.flip();
                }*/
            }
        }
        mana = 0;
        Debug.Log("yusssss");
    }

    /* This function acts as a delay before fliping cards*/
    private IEnumerator checkMatch(float time)
    {
        if (checkMatchInUse)
            yield break;
        checkMatchInUse = true;

        CardScript card1 = faceUpCards[0];
        CardScript card2 = faceUpCards[1];
        // Matched!
        if (card1.matchID == card2.matchID)
        {

            GM.onCardMatch(card1.cardType);
            if(card1.cardType == cardTypes.attack)
            {
                //playerObj.GetComponent<PlayerScript>().fire();
                playerScript.fire();
            }
            else if(card1.cardType == cardTypes.mana)
            {
                gainMana(manaGainRate);
            }

            // Clean up rhe cards
            yield return new WaitForSecondsRealtime(time); // if we place this below clearing the cards we can continue to make matches
            faceUpCards.Clear();
            board.Remove(card1.gameObject);
            board.Remove(card2.gameObject);
            
            card1.matched();
            card2.matched();

        }
        else  // No match
        {
            yield return new WaitForSecondsRealtime(time);
            //flip cards back over
            card1.flip();
            card2.flip();
            faceUpCards.Clear();
        }
        checkMatchInUse = false;
    }

    private void matchedCards(CardScript card1, CardScript card2)
    {
        GM.onCardMatch(card1.cardType);
        if (card1.cardType == cardTypes.attack)
        {
            //playerObj.GetComponent<PlayerScript>().fire();
            playerScript.fire();
        }
        else if (card1.cardType == cardTypes.mana)
        {
            gainMana(manaGainRate);
        }

        // Clean up rhe cards
        //yield return new WaitForSecondsRealtime(time); // if we place this below clearing the cards we can continue to make matches
        faceUpCards.Clear();
        board.Remove(card1.gameObject);
        board.Remove(card2.gameObject);

        card1.matched();
        card2.matched();
    }
  
    /* This function initializes the cards in the boardList to later
     */
    void initBoard()
    {
        attackRatio = GM.attackRatio;
        healRatio = GM.healRatio;
        manaRatio = GM.manaRatio;
        defenceRatio = GM.defenceRatio;

        GameObject card1, card2;
        int totalCards = width * height;
        int numOfPairs = totalCards/2;
        int totalRatio = attackRatio + defenceRatio + healRatio + manaRatio;

        int numOfAttack = Mathf.RoundToInt((float)numOfPairs * attackRatio / (float)totalRatio);
        int numOfDefense = Mathf.RoundToInt((float)numOfPairs * defenceRatio / (float)totalRatio);
        int numOfHeal = Mathf.RoundToInt((float)numOfPairs * healRatio / (float)totalRatio);
        int numOfMana = Mathf.RoundToInt((float)numOfPairs * manaRatio / (float)totalRatio);
                
        int total = numOfAttack + numOfMana + numOfHeal + numOfDefense;
                       
        if (totalCards % 2 != 0) // checks if there is an even number of cards
        {
            Debug.LogError("width * height is not even");
        }

        
        // This checks if there are not enough cards, then adds them
        while (total < numOfPairs)
        {
            int randomIndex = Random.Range(0, 4); // Where 4 is the number of card types

            switch (randomIndex)
            {
                case 0:
                    numOfAttack++;
                    break;
                case 1:
                    numOfDefense++;
                    break;
                case 2:
                    numOfHeal++;
                    break;
                case 3:
                    numOfMana++;
                    break;
            }
            total++;
        }
        // This checks if there are too many cards, then subracts them
        while (total > numOfPairs)
        {
            int randomIndex = Random.Range(0, 4); // Where 4 is the number of card types

            switch (randomIndex)
            {
                case 0:
                    if (numOfAttack > 0)
                    {
                        numOfAttack--;
                        total--;
                    }
                    break;

                case 1:
                    if (numOfDefense > 0)
                    {
                        numOfDefense--;
                        total--;
                    }
                    break;

                case 2:
                    if (numOfHeal > 0)
                    {
                        numOfHeal--;
                        total--;
                    }
                    break;

                case 3:
                    if (numOfMana > 0)
                    {
                        numOfMana--;
                        total--;
                    }
                    break;
            }
            
        }

        if (total != numOfPairs)
        {
            Debug.Log(numOfAttack);
            Debug.Log(numOfMana);
            Debug.Log(numOfHeal);
            Debug.Log(numOfDefense);
            Debug.Log(total);
            Debug.LogError("Error calculating cards, fix your math!");
        }
        

        for (int i=0; i<numOfAttack; i++)
        {
            (card1, card2) = initCards(cardTypes.attack, attackCards[Random.Range(0, attackCards.Length)], faceDownColorCards[2]);
            board.Add(card1);
            board.Add(card2);
        }
        for (int i = 0; i < numOfMana; i++)
        {
            (card1, card2) = initCards(cardTypes.mana, manaCards[Random.Range(0, manaCards.Length)], faceDownColorCards[0]);
            board.Add(card1);
            board.Add(card2);
        }
        for (int i = 0; i < numOfHeal; i++)
        {
            (card1, card2) = initCards(cardTypes.heal, healingCards[Random.Range(0, healingCards.Length)], faceDownColorCards[1]);
            board.Add(card1);
            board.Add(card2);
        }
        for (int i = 0; i< numOfDefense; i++)
        {
            (card1, card2) = initCards(cardTypes.defend, defenseCards[Random.Range(0, defenseCards.Length)], faceDownColorCards[3]);
            board.Add(card1);
            board.Add(card2);
        }
    }

    public void gainMana(float gain)
    {
        mana += gain;
        mana = Mathf.Clamp(mana, 0, maxMana);
    }

    // This function creates a pair of cards
    (GameObject, GameObject) initCards(cardTypes cardType, Sprite sprite, Sprite faceDownColor)
    {
        int matchID = sprite.GetHashCode(); // No mater what the sprite is, it will always match with similar sprites

        GameObject card1 = Instantiate(prefabCard, boardObj);
        CardScript cardScript1 = card1.GetComponent<CardScript>();
        cardScript1.setBoard(this);
        cardScript1.setCardType(cardType, sprite, faceDownColor, matchID);

        GameObject card2 = Instantiate(prefabCard, boardObj);
        CardScript cardScript2 = card2.GetComponent<CardScript>();
        cardScript2.setBoard(this);
        cardScript2.setCardType(cardType, sprite, faceDownColor, matchID);

        return (card1, card2);
    }

    private void shuffleBoard()
    {
        List<GameObject> shuffledBoard = new List<GameObject>();
        while (board.Count > 0)
        {
            int randIndex = Random.Range(0, board.Count - 1);
            shuffledBoard.Add(board[randIndex]);
            board.RemoveAt(randIndex);  // not eficient but it works
        }
        board = shuffledBoard;
    }

    void setCardLocations(Vector2 boardPostion)
    {
        int i = 0;
        for (int y=0; y<height; y++)
        {
            for (int x=0; x<width; x++)//(y != 0 ? -y:-1)(x != 0 ? x : 1)
            {
                Vector2 location = new Vector2(((cardDimensions.rect.width) + (cardDimensions.rect.width/2)) * x + 30, -y * (( cardDimensions.rect.height) + (cardDimensions.rect.height/8)) - 24);

                //Debug.Log(location);

                board[i].GetComponent<RectTransform>().anchoredPosition = location;
                i++;
            }
        }
    }

   
   public void clear()
   {
       gameActive = false;
       foreach(GameObject card in board){
           Destroy(card);
       }
   }
}