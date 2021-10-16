using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static int progress = 0;
    public int individualNum = 1; //not 0
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && progress != individualNum)
        {
            progress = individualNum;
            GameManager.totalDefeats += PlayerController.defeats;
            GameManager.totalMoney += PlayerController.money;

            PlayerController.defeats = 0;
            PlayerController.money = 0;
        }
    }
}
