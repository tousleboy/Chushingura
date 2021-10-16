using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;
    public GameObject text;
    //public GameObject headSet;
    //public GameObject MoneyText;
    public GameObject Pannel1;
    public GameObject Pannel2;
    public GameObject Fade;
    public Sprite gameOverSpr;
    public Sprite gameClearSpr;
    /*public GameObject Life;
    public Sprite lifeZero;
    public Sprite lifeOne;
    public Sprite lifeTwo;
    public Sprite lifeThree;*/

    //Image lifeImage;
    Text message;
    //Text money;
    public GameObject Boss;
    //AudioSource soundPlayer;
    //Animator hsAnimator;
    //public AudioClip piron;


    public bool bossIsGoal = true;
    //public bool hsAlwaysActive = true;
    public bool startWithFade = false;
    public static bool fadeDone = false;
    bool goal = false;

    public static int score = 0;

    string messages;
    string nowMessages;
    string oldMessages;

    public static int battleScore = 100;
    public static int totalMoney = 0;
    public static int totalDefeats = 0;
    // Start is called before the first frame update
    void Start()
    {
        mainImage.SetActive(false);
        Pannel1.SetActive(false);
        Pannel2.SetActive(false);
        //lifeImage = Life.GetComponent<Image>();
        message = text.GetComponent<Text>();
        PlayerController.gameState = "playing";
        //money = MoneyText.GetComponent<Text>();
        /*soundPlayer = headSet.GetComponent<AudioSource>();
        hsAnimator = headSet.GetComponent<Animator>();

        if(hsAlwaysActive == false)
        {
            headSet.SetActive(false);
        }*/

        nowMessages = PlayerController.messages;
        oldMessages = PlayerController.messages;

        //UpdateScore();

        /*if(CheckPointManager.progress != 0)
        {
            GameObject[] cps = GameObject.FindGameObjectsWithTag("Respawn");
            int i = 0;
            bool done = false;
            while(i < cps.Length && !done)
            {
                int progress = cps[i].GetComponent<CheckPointManager>().individualNum;
                if(progress == CheckPointManager.progress) done = true;
                if(!done) i++;
            }
            Vector3 pos = cps[i].transform.position;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = pos;
        }*/

        if(startWithFade && !fadeDone)
        {
            StartCoroutine("FadeIn");
        }

        if(CheckPointManager.progress != 0)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject[] cps = GameObject.FindGameObjectsWithTag("Respawn");
            int i = 0;
            bool done = false;
            while(i < cps.Length && !done)
            {
                int progress = cps[i].GetComponent<CheckPointManager>().individualNum;
                if(progress == CheckPointManager.progress) done = true;
                if(!done) i++;
            }
            Vector3 pos = cps[i].transform.position;
            player.transform.position = pos;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        /*if(PlayerController.life >= 3)
        {
            lifeImage.sprite = lifeThree;
        }
        else if(PlayerController.life == 2)
        {
            lifeImage.sprite = lifeTwo;
        }
        else if(PlayerController.life == 1)
        {
            lifeImage.sprite = lifeOne;
        }
        else
        {
            lifeImage.sprite = lifeZero;
        }*/

        /*if(PlayerController.score != 0)
        {
            score += PlayerController.score;
            PlayerController.score = 0;
            UpdateScore();
        }*/

        nowMessages = PlayerController.messages;
        if(nowMessages != oldMessages)
        {
            StartCoroutine("StoryTeller");
            oldMessages = nowMessages;
        }

        if(PlayerController.gameState == "gameover")
        {
            Pannel1.SetActive(true);
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            mainImage.SetActive(true);
        }
        if(bossIsGoal && !goal)
        {
            if(Boss == null)
            {
                goal = true;
                StartCoroutine("BossDefeated");
            }
        }
        if(PlayerController.gameState == "gameclear" && !bossIsGoal && !goal)
        {
            UpdateScore();
            Pannel2.SetActive(true);
            mainImage.GetComponent<Image>().sprite = gameClearSpr;
            mainImage.SetActive(true);
            //CheckPointManager.progress = 0;
            goal = true;
            fadeDone = false;
            CheckPointManager.progress = 0;
        }
    }

    public void UpdateScore()
    {
        totalMoney += PlayerController.money;
        totalDefeats += PlayerController.defeats;
        Debug.Log(totalMoney);
        Debug.Log(totalDefeats);
        Debug.Log(battleScore);
    }

    IEnumerator StoryTeller()
    {
        string recieved = nowMessages;
        string[] messages = recieved.Split(' ');
        int length = messages.Length;
        int i;
        for(i = 0; i < length; i++)
        {
            /*if(headSet.activeSelf)
            {
                hsAnimator.SetTrigger("Call");
                soundPlayer.PlayOneShot(piron);
            }*/
            message.text = messages[i];
            yield return new WaitForSeconds(messages[i].Length / 5.0f);
            if(recieved != nowMessages)
            {
                yield break;
            }
        }
        message.text = "";
    }

    IEnumerator FadeIn()
    {
        Image I = Fade.GetComponent<Image>();
        float t = 0.0f;
        float speed = 0.3f;
        //yield return null;
        fadeDone = true;
        PlayerController.gameState = "wait";
        Fade.SetActive(true);
        I.color = Color.black;
        while(t <= 1.0f)
        {
            I.color = Color.Lerp(Color.black, Color.clear, t);
            t += speed * Time.deltaTime;
            yield return null;
        }
        PlayerController.gameState = "playing";
        Fade.SetActive(false);
    }

    IEnumerator BossDefeated()
    {
        UpdateScore();
        Image I = Fade.GetComponent<Image>();
        GameObject NextButton = Pannel2.transform.Find("NextButton").gameObject;
        GameObject musicPlayer = GameObject.FindGameObjectWithTag("Music");
        float t = 0.0f;
        float speed = 0.5f;
        PlayerController.gameState = "gameclear";
        Fade.SetActive(true);
        fadeDone = false;
        CheckPointManager.progress = 0;
        yield return new WaitForSeconds(0.5f);
        while(t <= 1.0f)
        {
            I.color = Color.Lerp(Color.clear, Color.black, t);
            t += speed * Time.deltaTime;
            yield return null;
        }
        if(musicPlayer != null)
        {
            float i;
            float downspeed = 0.01f;
            AudioSource auds = musicPlayer.GetComponent<AudioSource>();
            for(i = 1.0f; i >= 0.0f; i -= downspeed)
            {
                auds.volume = i;
                yield return null;
            }
        }
        yield return new WaitForSeconds(1.0f);
        NextButton.GetComponent<ChangeScene>().Load();
    }
}
