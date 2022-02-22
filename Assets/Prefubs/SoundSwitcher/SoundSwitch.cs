using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundSwitch : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    private Image image;
    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        PlayerPrefs.SetInt("Sound", PlayerPrefs.GetInt("Sound", 1) == 0 ? 1 : 0);
        if (PlayerPrefs.GetInt("Sound")==1)
        {
            image.sprite = onSprite;
            GetComponent<AudioSource>().Play();
        }
        else
        {
            image.sprite = offSprite;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            image.sprite = onSprite;
        }
        else
        {
            image.sprite = offSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
