using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoad : MonoBehaviour
{
    private static List<Sprite> cardSprites = null;

    private void Awake()
    {
        cardSprites = new List<Sprite>();
        Object[] allSprites = Resources.LoadAll("PlayingCards", typeof(Sprite));
        for (int i = 0; i < allSprites.Length; i++)
            cardSprites.Add(allSprites[i] as Sprite);
        //Debug.Log("Cards count is "+cardSprites.Count);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite GetCardSprite(string card_name)
    {
        for (int i = 0; i < cardSprites.Count; i++)
            if (cardSprites[i].name.Equals(card_name))
                return cardSprites[i];

        return null;
    }
}
