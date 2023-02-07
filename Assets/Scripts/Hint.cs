using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    public static Hint Instance { get; private set; }
    public Text hintWord;
    // Start is called before the first frame update
    private float counter;
    public void showWrongStep()
    {
        hintWord.text = "×ßÆå´íÎó";
        gameObject.SetActive(true);
        counter = 2f;
    }

    public void showWrongSelection()
    {
        hintWord.text = "Ñ¡Æå´íÎó";
        gameObject.SetActive(true);
        counter = 2f;
    }
    void Start()
    {
        Instance = this;
        counter = -0.001f;
        hintWord = transform.Find("Text").GetComponent<Text>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (counter > 0)
        {
            counter -= Time.deltaTime;
            
        } else
        {
            gameObject.SetActive(false);
        }
    }
}
