using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : MonoBehaviour
{
    public GameObject wall1;
    public GameObject wall2;
    public GameObject boss;
    GameObject player;
    GameObject mainCamera;
    float range = 8.5f;
    bool eventStart = false;
    bool eventEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        wall1.SetActive(false);
        wall2.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        float x = player.transform.position.x;
        if(x >= transform.position.x && x <= transform.position.x + range && !eventStart && !eventEnd)
        {
            eventStart = true;
            mainCamera.GetComponent<CameraManager>().locked = true;
            wall1.SetActive(true);
            wall2.SetActive(true);
        }
        if(boss == null)
        {
            wall1.SetActive(false);
            wall2.SetActive(false);
        }
        if(!eventEnd && x >= transform.position.x + range)
        {
            mainCamera.GetComponent<CameraManager>().resetPos = true;
            eventEnd = true;
        }
    }
}
