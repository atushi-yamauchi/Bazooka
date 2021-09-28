using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    private GameObject dir;
    private GameObject push;
    private GameObject stage;
    private GameObject friend;
    private GameObject enemy;
    private GameObject panel;
    public GameObject panelGameJudge;
    string sCount;
    string scenes;
    int shotCount;
    int fastCount;
    int endCount;
    int enemyCount;
    public GameObject root;
    private Rigidbody[] rbAll;
    private string sceneNum;

    // Start is called before the first frame update
    void Start()
    {
        //必要なゲームオブジェクトを取得
        this.dir = GameObject.Find("Director");
        this.friend = GameObject.Find("Friend");
        this.enemy = GameObject.Find("Enemy");
        this.panel = GameObject.Find("GameJudge");
        this.push = GameObject.Find("Push");
        this.stage = GameObject.Find("Stage");
        fastCount = this.friend.transform.childCount;

        //ゲーム開始時の残り弾数を表示
        shotCount = 3;
        sCount = shotCount.ToString();
        this.dir.GetComponent<Text>().text = "残弾数" + sCount + "発";
        scenes = SceneManager.GetActiveScene().name;
        sceneNum = scenes.Replace("GameScenes", "");
        panelGameJudge.SetActive(false);
        push.SetActive(false);

        //現在のステージを表示
        this.stage.GetComponent<Text>().text = "STAGE" + sceneNum;
    }

    private bool isSleepingAll()
    {
        rbAll = root.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbAll)
        {
            if (rb.IsSleeping() == false)
            {
                return false;
            }
        }
        return true;
    }

    //弾数を管理するメソッド
    public bool shotRquest()
    {
        bool ret = false;
        if (0 < shotCount)
        {
            ret = true;
            shotCount--;
            sCount = shotCount.ToString();
            this.dir.GetComponent<Text>().text = "残弾数" + sCount + "発";
        }
        return ret;
    }

    void Update()
    {
        endCount = this.friend.transform.childCount;
        enemyCount = this.enemy.transform.childCount;

        if (fastCount != endCount)
        {
            panelGameJudge.SetActive(true);
            this.panel.GetComponent<Text>().text = "GAME OVER";
            this.panel.GetComponent<Text>().color = Color.blue;
            push.SetActive(true);
            this.push.GetComponent<Text>().text = "Push Retry";
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(scenes);
            }
        }

        if (isSleepingAll())
        {
            if (enemyCount == 0)
            {
                if (fastCount == endCount)
                {
                    panelGameJudge.SetActive(true);
                    this.panel.GetComponent<Text>().text = "GAME CLEAR";
                    this.panel.GetComponent<Text>().color = Color.yellow;
                    push.SetActive(true);
                    this.push.GetComponent<Text>().text = "Push NextStage";

                    int num = int.Parse(sceneNum) + 1;
                    if (Input.GetMouseButtonDown(0))
                    {
                        SceneManager.LoadScene("GameScenes" + num);
                    }
                }
            }

            if(sCount == "0")
            {
                panelGameJudge.SetActive(true);
                this.panel.GetComponent<Text>().text = "GAME OVER";
                this.panel.GetComponent<Text>().color = Color.blue;
                push.SetActive(true);
                this.push.GetComponent<Text>().text = "Push Retry";
                if (Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadScene(scenes);
                }
            }
        }
    }
}
    
