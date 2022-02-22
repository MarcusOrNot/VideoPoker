using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    [SerializeField] private CardControl[] cards;
    [SerializeField] private GameObject[] holdObjects;
    [SerializeField] private ActionBTN_Control[] actionButtons;
    [SerializeField] private TableControl tableComb;
    public SoundController soundControl;
    [SerializeField] private Text combi_text;
    [SerializeField] private Text coinsText;
    private GameState gameState = GameState.None;
    private DataLoad _data;
    private List<CardData> card_pack = new List<CardData>();
    private int _level = 0;
    private int bid = 1; 
    // Start is called before the first frame update
    void Start()
    {
        
        _data = GameObject.FindObjectOfType<DataLoad>();
        Coins = Coins;
        SetBidLevel(0);
        SetState(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameState State
    {
        get
        {
            return gameState;
        }
        set
        {
            gameState = value;
            for (int i = 0; i < actionButtons.Length; i++) actionButtons[i].SetActiveButton(false);
            switch(gameState)
            {
                case GameState.Start:
                    CardData[] gen_cards = GenCards();
                    for (int i=0; i<cards.Length; i++)
                    {
                        cards[i].Card = gen_cards[i];
                        cards[i].Open = true;
                        holdObjects[i].SetActive(false);
                    }
                    actionButtons[1].SetActiveButton(true);
                    tableComb.HighlightCell(-1, 1);
                    combi_text.text = "ВЫБЕРИТЕ КАРТЫ, КОТОРЫЕ НЕ НУЖНО МЕНЯТЬ";
                    if (Coins <= 0) Coins = 100;

                    break;
                case GameState.ResultAfterChange:
                    Coins = Coins - (_level + 1);
                    for (int i = 0; i < cards.Length; i++)
                        if (!holdObjects[i].activeSelf)
                            cards[i].Card = PullOutRandCard();
                    //combi_text.text = GetCombination(CardsData).ToString();
                    actionButtons[0].SetActiveButton(true);
                    actionButtons[2].SetActiveButton(true);
                    
                    PokerCombination resCombi = GetCombination(CardsData);
                    if (resCombi != PokerCombination.None)
                    {
                        int winSum = bid* int.Parse(tableComb.GetCellValue(_level, (int)resCombi - 1));
                        tableComb.HighlightCell(_level, (int)resCombi - 1);
                        combi_text.text = "ВЫИГРЫШ = "+winSum.ToString()+ ". ДАВАЙТЕ ЕЩЕ РАЗ?";
                        Coins += winSum;
                        soundControl.PlaySound(SoundType.WinSound);
                    }
                    else
                    {
                        combi_text.text = "НЕ ПОВЕЗЛО. ПОПРОБУЙТЕ ЕЩЕ!";
                    }
                    
                    break;
            }
        }
    }

    public void SetState(int stateVal)
    {
        switch(stateVal)
        {
            case 1: State = GameState.Start; break;
            case 2: State = GameState.ResultAfterChange; break;
        }
    }

    public DataLoad Data
    {
        get
        {
            return _data;
        }
    }

    public CardData[] CardsData
    {
        get
        {
            CardData[] cardsData = new CardData[cards.Length];
            for (int i = 0; i < cards.Length; i++)
                cardsData[i] = cards[i].Card;
            return cardsData;
        }
    }

    public int Coins
    {
        get
        {
            return PlayerPrefs.GetInt("Coins", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Coins", value);
            coinsText.text = value.ToString();
        }
    }

    public void SetBidLevel(int level)
    {
        _level = level;
        tableComb.HighlightColumn(level);
    }

    public void IncreaseLevel()
    {
        if (_level < 4) SetBidLevel(_level + 1);
        else SetBidLevel(0);
    }

    public void CardChosen(GameObject chosenCard)
    {
        if (State == GameState.Start)
        {
            int cardPos = GetCardPosByObject(chosenCard);
            holdObjects[cardPos].SetActive(!holdObjects[cardPos].activeSelf);
            soundControl.PlaySound(SoundType.CardClick);
        }
        else
        {
            soundControl.PlaySound(SoundType.IncorrectClick);
        }
    }

    private int GetCardPosByObject(GameObject cardFind)
    {
        for (int i = 0; i < cards.Length; i++)
            if (cards[i].gameObject == cardFind)
                return i;

        return 0;
    }

    private CardData[] GenCards()
    {
        CardData[] resCards = new CardData[5];

        //Создаем колоду
        Suite[] suits = new Suite[4] { Suite.Club, Suite.Diamond, Suite.Heart, Suite.Spade };
        card_pack = new List<CardData>();
        for (int i = 2; i <= 14; i++)
            for (int j = 0; j < suits.Length; j++)
                card_pack.Add(new CardData(i, suits[j]));

        //Генерация
        for (int i=0; i<5; i++)
        {
            resCards[i] = PullOutRandCard();
        } 
        //Тестовые данные
        /*resCards[0] = TestPullOutCard(11, Suite.Club);
        resCards[1] = TestPullOutCard(11, Suite.Diamond);
        resCards[2] = TestPullOutCard(5, Suite.Spade);
        resCards[3] = TestPullOutCard(9, Suite.Heart);
        resCards[4] = TestPullOutCard(4, Suite.Club);*/
        return resCards;
    } 

    private CardData PullOutRandCard()  //Вытаскиваем случайную карту
    {
        CardData rand_card = card_pack[Random.Range(0, card_pack.Count)];
        card_pack.Remove(rand_card);
        return rand_card;
    }

    /*private CardData TestPullOutCard(int grade, Suite suite)  //Вытаскиваем нужную карту
    {
        for (int i=0; i<card_pack.Count; i++) if (card_pack[i].suite==suite && card_pack[i].grade==grade)
            {
                card_pack.Remove(card_pack[i]);
                return card_pack[i];
            }
        return null;
    } */

    //Получаем комбинацию
    private PokerCombination GetCombination(CardData[] base_cards)
    {
        CardData[] sortedCards = new CardData[base_cards.Length]; for (int i = 0; i < base_cards.Length; i++) sortedCards[i] = base_cards[i];
        // сортировка
        CardData temp;
        for (int i = 0; i < sortedCards.Length - 1; i++)
            for (int j = i + 1; j < sortedCards.Length; j++)
                if (sortedCards[i].grade < sortedCards[j].grade)
                {
                    temp = sortedCards[i];
                    sortedCards[i] = sortedCards[j];
                    sortedCards[j] = temp;
                }

        Suite flash= Suite.Zero;
        int straight = 0;

        int[] diff = new int[4];
        for (int i = 0; i < sortedCards.Length-1; i++) {
            diff[i] = sortedCards[i].grade - sortedCards[i+1].grade;
            //Debug.Log("pos " + i.ToString() + " : " + sortedCards[i].grade.ToString());
        }

        //Проверка на flash
        if (sortedCards[0].suite == sortedCards[1].suite && sortedCards[0].suite == sortedCards[2].suite && sortedCards[0].suite == sortedCards[3].suite &&
            sortedCards[0].suite == sortedCards[4].suite) flash = sortedCards[0].suite;

        if ( (diff[0]==1 || (sortedCards[4].grade == 2 && sortedCards[0].grade == 14) ) && diff[1] == 1 && diff[2] == 1 && diff[3] == 1)
            straight = sortedCards[0].grade;

        if (flash != Suite.Zero && straight != 0)  //Проверям на ройал и стрит флеш
        {
            if (sortedCards[0].grade == 14 && sortedCards[4].grade==10 ) return PokerCombination.RoyalFlush;
            else return PokerCombination.StraightFlush;
        }

        /*теперь нам всё равно, какие ненулевые значения принимают diff.
        поскольку интересно только равенство. }
         получим двоичное число; k - й разряд = 0 тогда, когда diff[k] <> 0 */
        int code = 0;
        for (int i=0; i<4; i++)
        {
            code *= 2;
            if (diff[i] != 0) code += 1;
        }
        /*это число -синдром следующих комбинаций:
        каре: XXXXy | yXXXX, т.е. 0001 | 1000 */
        if (code == 1 || code == 16) return PokerCombination.Kare;
        //{ 3 + 2: XXXYY = 0010, YYXXX = 0100 }
        if (code == 2 || code == 8) return PokerCombination.FullHouse;
        if (flash != 0) return PokerCombination.Flush;  //Промежутком флеш
        if (straight != 0) return PokerCombination.Straight;  //Промежутком стрит
        //{ 3: XXXyz = 0011, yXXXz = 1001, yzXXX = 1100 }
        if (code == 3 || code == 9 || code == 12) return PokerCombination.Set;
        //{ 2 + 2: XXYYz = 0101, XXzYY = 0110, zXXYY = 1010 }
        if (code == 5 || code == 6 || code == 10) return PokerCombination.TwoPairs;
        //{ 2: XXyzt = 0111, yXXzt = 1011, yzXXt = 1101, yztXX = 1110 }
        if ( (code == 7 && sortedCards[0].grade>10) || (code == 11 && sortedCards[1].grade > 10) 
            || (code == 13 && sortedCards[2].grade > 10) || (code == 14 && sortedCards[3].grade > 10)) return PokerCombination.JackOrBetter;

        return PokerCombination.None;
    }

    public void GoHome()
    {
        SceneManager.LoadScene(0);
    }
}

public enum GameState
{
    None,
    Start,  //Генерация новых карт и выбор карт для удержания
    ResultAfterChange  //Отображение получившейся комбинации и результат
}

public enum PokerCombination
{
    None,
    RoyalFlush,
    StraightFlush,
    Kare,
    FullHouse,
    Flush,
    Straight,
    Set,
    TwoPairs,
    JackOrBetter
}
