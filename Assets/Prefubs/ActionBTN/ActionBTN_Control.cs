using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionBTN_Control : MonoBehaviour
{
    [SerializeField] private Text btnText;
    private Button btn;
    private Image img;
    private void Awake()
    {
        btn = GetComponent<Button>();
        img = GetComponent<Image>();
        btn.onClick.AddListener(ClickSound);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveButton(bool isActive)
    {
        btn.enabled = isActive;
        if (isActive)
        {
            btnText.color = Color.white;
            img.color = Color.white;
        }
        else
        {
            btnText.color = Color.grey;
            img.color = Color.grey;
        }
    }

    void ClickSound()
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
