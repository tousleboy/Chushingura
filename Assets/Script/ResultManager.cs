using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public GameObject defeats;
    public GameObject money;
    public GameObject battleRank;
    public Sprite mosho;
    public Sprite busho;
    public Sprite jizamurai;
    public Sprite ashigaru;
    Text dt;
    Text mt;

    // Start is called before the first frame update
    void Start()
    {
        dt = defeats.GetComponent<Text>();
        mt = money.GetComponent<Text>();

        int d = GameManager.totalDefeats;
        int m = GameManager.totalMoney;

        dt.text = d.ToString();
        mt.text = m.ToString();

        Image brI = battleRank.GetComponent<Image>();
        int bs = GameManager.battleScore;
        if(d < 8) bs -= 40;
        if(bs >= 95) brI.sprite = mosho;
        else if(bs >= 80) brI.sprite = busho;
        else if(bs >= 60) brI.sprite = jizamurai;
        else brI.sprite = ashigaru;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
