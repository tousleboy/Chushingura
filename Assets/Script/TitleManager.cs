using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    GameObject mainImage;
    GameObject button;
    Image I;
    // Start is called before the first frame update
    void Start()
    {
        mainImage = transform.Find("MainImage").gameObject;
        button = transform.Find("Button").gameObject;
        I = mainImage.GetComponent<Image>();

        I.color = Color.black;
        button.SetActive(false);
        StartCoroutine("TurnOn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TurnOn()
    {
        float t = 0.0f;
        float speed = 0.5f;
        while(t <= 1.0f)
        {
            I.color = Color.Lerp(Color.black, Color.white, t);
            t += speed * Time.deltaTime;
            yield return null;
        }
        button.SetActive(true);
    }

    IEnumerator TurnOff()
    {
        float t = 0.0f;
        float speed = 0.5f;
        Button bt = button.GetComponent<Button>();
        bt.interactable = false;
        while(t <= 1.0f)
        {
            I.color = Color.Lerp(Color.white, Color.black, t);
            t += speed * Time.deltaTime;
            yield return null;
        }
    }

    public void StartTurnOff()
    {
        StartCoroutine("TurnOff");
    }
}
