using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuControl : MonoBehaviour
{
    [SerializeField] private Text coinsText;
    // Start is called before the first frame update
    void Start()
    {
        Coins = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGameScene()
    {
        SceneManager.LoadScene(1);
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

    public void ExitGame()
    {
        Application.Quit();
    }
}
