using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    // Unity Editor fields
    public Text scoreText;

    private int _score = 0;
    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            scoreText.text = value.ToString();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
