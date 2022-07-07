using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum cardTypes
{
    attack,
    heal,
    defend,
    mana
}

// Responsible for the card states
public enum cardStates
{
    faceDown,
    faceUp,
    selected,
    deselected,
    matched
};
public class CardScript : MonoBehaviour
{
    public cardStates currentState = cardStates.faceDown;
    private cardStates prevState = cardStates.faceUp;
    public cardTypes cardType = cardTypes.attack; //default
    public int matchID;
    private bool selected = false;
    
    // Sprites for each state
    public Sprite faceDownSprite;
    private Sprite faceUpSprite;


    private SpriteRenderer sprite;
    private Color defualtColor;

    private PlayerManager playerBoard;

    public Sprite faceDownColorSprite;

    private float currTime = 0;
    private float revertTime = 10;
    private bool showColors = false;


    // Start is called before the first frame update
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = faceDownSprite;
        defualtColor = sprite.color;
        }

    private void Start()
    {
        playerBoard.allCards.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("showColors = " + showColors);
        // if we are currently showing the color
        if (showColors)
        {
            currTime += Time.deltaTime;
        }
        if(currTime > revertTime)
        {
            currTime = 0;
            showColors = false;
            sprite.sprite = faceDownSprite;
        }
        if (currentState != prevState)
        {
            // This switch determines what the card should do based on its current state
            switch (currentState)
            {
                case cardStates.faceUp:
                    if (playerBoard.faceUpCards.Count < 2)
                    {
                        sprite.sprite = faceUpSprite;
                        playerBoard.faceUpCards.Add(this);
                        selected = true;
                        prevState = currentState;

                    }
                    else  // Don't flip
                    {
                        // TODO play animation like shake
                        
                        currentState = cardStates.faceDown;
                        prevState = currentState;
                    }
                    break;

                case cardStates.faceDown:
                    if (showColors)
                    {
                        sprite.sprite = faceDownColorSprite;
                    }
                    else
                    {
                        sprite.sprite = faceDownSprite;
                    }
                    
                    playerBoard.faceUpCards.Remove(this);
                    prevState = currentState; //cardStates.faceDown;

                    break;

                case cardStates.matched:
                    // TODO send info about card stats i.e. how much damage delt
                    // TODO animate matched card

                    Destroy(gameObject);
                    break;

                // TODO 
                case cardStates.selected:
                    // TODO highlight card
                    // TODO Play animation
                    prevState = currentState;
                    break;

                // TODO
                case cardStates.deselected:
                    // TODO un-highlight card
                    // TODO stop animation
                    currentState = cardStates.faceDown;
                    prevState = currentState;
                    break;
            }
        }
    }


    public void changeFaceDownToColor()
    {
        if (currentState.Equals(cardStates.faceDown) && !showColors)
        {
            Debug.Log("waddup");
            showColors = true;
            sprite.sprite = faceDownColorSprite;
        }
    }


    void OnTOuchDown()
    {
        // TODO if prevState == faceDown then
        //currentState = cardStates.selected;
    }

    void OnTouchUp()
    {
        if (playerBoard.faceUpCards.Count < 2)
            flip(); 

    }

    // This is active when an object is being held down (maybe we don't want to flip card until finger release)
    void OnTouchStay()
    {
        // this may not be needed, if we use ontouchDown()
        //currentState = cardStates.selected;
    }

    // This occurs when your finger is on object then slide finger off to cancel action
    void OnTouchExit()
    {
        // TODO, this should check the previous state. if prevState == faceDown, then cardStates.deselcted
        //currentState = cardStates.deselected;
    }

    // Sets which board the card belongs to
    public void setBoard(PlayerManager playerBoard)
    {
        this.playerBoard = playerBoard;
    }
 
     // This function sets the card type
    public void setCardType(cardTypes type, Sprite sprite, Sprite faceDownColorSprite, int ID)
    {
        cardType = type;
        faceUpSprite = sprite;
        matchID = ID;
        this.faceDownColorSprite = faceDownColorSprite;
    }

     /* This function flips the card over if it is currently faced up it will flip face down,
     * if the card is face down, it will flip face up.
     */
    public void flip() // if we transition to cardStates.selected or deslected then this function needs to change
    {

        if (currentState == cardStates.faceUp)
        {
            prevState = currentState;
            currentState = cardStates.faceDown;
            FindObjectOfType<AudioManager>().Play("flipCardDown");
        }
        else if (currentState == cardStates.faceDown)
        {
            prevState = currentState;
            currentState = cardStates.faceUp;
            FindObjectOfType<AudioManager>().Play("flipCardUp");
        }
        else
            Debug.Log("Unable to flip card from " + currentState + " state");
    }

    /* public void matched()
     * This function switched the card state to matched
     */
    public void matched()
    {
        currentState = cardStates.matched;
        FindObjectOfType<AudioManager>().Play("matched");
    }
}
 
 