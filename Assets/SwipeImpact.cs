using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeImpact : MonoBehaviour
{
    public GameObject prefabBullt;
    public float power;
    public Vector3 startPos;
    public GameObject playerObj;
    public GameDirector director;
    public LineRenderer simulationLine = null;
    private static float DeltaTime;
    private GameObject simObject;
    private Rigidbody simBullet;
    private Vector3 currentPosition = Vector3.zero;
    private Camera mainCamera = null;
    private Transform mainCameraTransform = null;
    private Vector3 currentForce = Vector3.zero;
    private Vector3 dragStart = Vector3.zero;
    private static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private const float MaxMagnitude = 2f;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        GameObject objDir = GameObject.Find("GameDirector");
        this.director = objDir.GetComponent<GameDirector>();
        player = GameObject.Find("Player");
        simObject = GameObject.Find("Ball");
        this.simBullet = this.simObject.GetComponent<Rigidbody>();
        this.currentPosition = this.simBullet.position;
        this.mainCamera = Camera.main;
        this.mainCameraTransform = this.mainCamera.transform;
        DeltaTime = Time.fixedDeltaTime;
        simObject.transform.position = player.transform.position + new Vector3(0, 1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = GetMousePosition();
            preMousePosition = startPos;
            this.currentPosition = this.simBullet.position;
        }
        if(Input.GetMouseButton(0))
        {
            Vector2 delta = GetMousePosition() - new Vector3(preMousePosition.x, preMousePosition.y);
            if(0.1f < delta.magnitude)
            {
                preMousePosition = GetMousePosition();
                float xLength = preMousePosition.x - startPos.x;
                float yLength = preMousePosition.y - startPos.y;
                Vector2 dir = new Vector2(-xLength, -yLength);
                StartCoroutine(Simulation(dir));
            }
            SimilationInterval += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (director.shotRquest())
            {
                GameObject objBullt = Instantiate(prefabBullt) as GameObject;
                Vector3 endPos = GetMousePosition();
                float xLength = endPos.x - startPos.x;
                float yLength = endPos.y - startPos.y;
                Vector3 swipeLength = new Vector3(-xLength, -yLength, 0);
                swipeLength = swipeLength.normalized;
                Rigidbody sphere = objBullt.GetComponent<Rigidbody>();
                sphere.useGravity = true;
//                sphere.AddForce(swipeLength * power, ForceMode.Impulse);
                Flip(sphere, swipeLength);
                sphere.transform.position = playerObj.transform.position + (Vector3.up * 1f);
                this.simulationLine.positionCount = 0;
            }
        }
    }

/*    public void OnMouseDown()
    {
        this.dragStart = this.GetMousePosition();
        this.currentPosition = this.simBullet.position;  //1,simBulletをcurrentPositionに代入
    }*/

    public void OnMouseDrag()
    {
        var position = this.GetMousePosition();
        float xLength = position.x - startPos.x;
        float yLength = position.y - startPos.y;
        currentForce = new Vector3(-xLength, -yLength, 0);

        currentForce = currentForce.normalized;
                if (this.currentForce.magnitude > MaxMagnitude * MaxMagnitude)
                {
                    this.currentForce *= MaxMagnitude / this.currentForce.magnitude;
                }
        //        this.StartCoroutine(this.Simulation());
//        Simulation();
    }

    private IEnumerator Simulation(Vector2 dir)
    {
        Vector3 preVelcity = simBullet.velocity;
        // 自動的な物理運動を停止させる
        Physics.autoSimulation = false;
        simBullet.isKinematic = false;

        var points = new List<Vector3> { this.currentPosition };
        this.Flip(simBullet, dir.normalized);

        // 運動の軌跡をシミュレーションして記録する
        for (var i = 0; i < 160; i++)
        {
            Physics.Simulate(DeltaTime * 2f);
            points.Add(this.simBullet.position);
        }

        // もとの位置に戻す
        this.simBullet.velocity = preVelcity;
        this.simBullet.transform.position = this.currentPosition;

        // 予測地点をつないで軌跡を描画
        this.simulationLine.positionCount = points.Count;
        this.simulationLine.SetPositions(points.ToArray());
        simBullet.isKinematic = true;

        Physics.autoSimulation = true;

        yield return WaitForFixedUpdate;
    }

    private void FixedUpdate()
    {
        if (SimilationInterval > 0.2)
        {
            SimilationInterval = 0;
            OnMouseDrag();
        }
    }

    private void Flip(Rigidbody rb, Vector3 dir)
    {
        rb.AddForce(dir * power, ForceMode.Impulse);
    }


    private Vector3 GetMousePosition()
    {
        // マウスから取得できないZ座標を補完する
        var position = Input.mousePosition;
        position.z = this.mainCameraTransform.position.z;
        position = this.mainCamera.ScreenToWorldPoint(position);
        position.z = 0;

        return position;
    }

    private float SimilationInterval;
    private Vector2 preMousePosition;
}
