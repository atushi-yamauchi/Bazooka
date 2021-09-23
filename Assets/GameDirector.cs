using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    private GameObject dir;
    private GameObject friend;
    private GameObject enemy;
    private GameObject panel;
    public GameObject panelGameJudge;
    string sCount;
    int shotCount;
    int fastCount;
    int endCount;
    int enemyCount;
    public GameObject root;
    private Rigidbody[] rbAll;

    // Start is called before the first frame update
    void Start()
    {
        //必要なゲームオブジェクトを取得
        this.dir = GameObject.Find("Director");
        friend = GameObject.Find("Friend");
        enemy = GameObject.Find("Enemy");
        this.panel = GameObject.Find("GameJudge");
        fastCount = this.friend.transform.childCount;

        //ゲーム開始時の残り弾数を表示
        shotCount = 3;
        sCount = shotCount.ToString();
        this.dir.GetComponent<Text>().text = "残弾数" + sCount + "発";

        panelGameJudge.SetActive(false);
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
        }

        if (isSleepingAll())
        {
            if (enemyCount == 0)
            {
                if (fastCount == endCount)
                {
                    panelGameJudge.SetActive(true);
                    this.panel.GetComponent<Text>().text = "GAME CLEAR";
                }
            }

            if(sCount == "0")
            {
                panelGameJudge.SetActive(true);
                this.panel.GetComponent<Text>().text = "GAME OVER";
            }
        }
    }
}
    
