using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardControl : MonoBehaviour
{
    private CardData cardData = new CardData(0, Suite.Zero);
    private GameControl gameController;
    private SpriteRenderer spriteRenderer;
    private bool isOpened = false;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Debug.Log("its started now ");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CardData Card
    {
        get
        {
            return cardData;
        }
        set
        {
            cardData = value;
            if (isOpened)
                spriteRenderer.sprite = gameController.Data.GetCardSprite(cardData.suite.ToString() + (cardData.grade < 10 ? "0" + cardData.grade.ToString() : cardData.grade.ToString()));
        }
    }

    public bool Open
    {
        get
        {
            return isOpened;
        }
        set
        {
            if (value == true)
            {
                if (cardData.grade != 0 && cardData.suite != Suite.Zero)
                { 
                    isOpened = value;
                    spriteRenderer.sprite = gameController.Data.GetCardSprite(cardData.suite.ToString() + (cardData.grade < 10 ? "0" + cardData.grade.ToString() : cardData.grade.ToString()));
                }
            }
            else
            {
                spriteRenderer.sprite=gameController.Data.GetCardSprite("BackColor_Red");
                isOpened = value;
            }
        }
    }

    private void OnMouseDown()
    {
        gameController.CardChosen(gameObject);
    }
}

public enum Suite
{
    Zero,
    Spade,
    Club,
    Diamond,
    Heart
}

public class CardData
{
    public int grade = 0; // туз = 14, как самая старшая карта; 0 - дефолт
    public Suite suite = Suite.Zero;  // Zero - дефолт 

    public CardData(int set_grade, Suite set_suite)
    {
        grade = set_grade;
        suite = set_suite;
    }
}
