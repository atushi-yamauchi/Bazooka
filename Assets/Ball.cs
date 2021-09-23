using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    /// <summary>
    /// 物理剛体
    /// </summary>
    private Rigidbody physics = null;

    /// <summary>
    /// 発射方向
    /// </summary>
    [SerializeField]
//    private LineRenderer direction = null;

    /// <summary>
    /// 運動軌跡
    /// </summary>
//    [SerializeField]
    private LineRenderer simulationLine = null;

    /// <summary>
    /// 最大付与力量
    /// </summary>
    private const float MaxMagnitude = 2f;

    /// <summary>
    /// 発射方向の力
    /// </summary>
    private Vector3 currentForce = Vector3.zero;

    /// <summary>
    /// メインカメラ
    /// </summary>
    private Camera mainCamera = null;

    /// <summary>
    /// メインカメラ座標
    /// </summary>
    private Transform mainCameraTransform = null;

    /// <summary>
    /// ドラッグ開始点
    /// </summary>
    private Vector3 dragStart = Vector3.zero;

    /// <summary>
    /// ボール位置
    /// </summary>
    private Vector3 currentPosition = Vector3.zero;

    /// <summary>
    /// 固定フレームウェイト
    /// </summary>
    private static float DeltaTime;

    /// <summary>
    /// 固定フレーム待ち時間
    /// </summary>
    private static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Awake()
    {
        this.physics = this.GetComponent<Rigidbody>();
        this.currentPosition = this.physics.position;
        this.mainCamera = Camera.main;
        this.mainCameraTransform = this.mainCamera.transform;
        DeltaTime = Time.fixedDeltaTime;
    }

    /// <summary>
    /// マウス座標をワールド座標に変換して取得
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMousePosition()
    {
        // マウスから取得できないZ座標を補完する
        var position = Input.mousePosition;
        position.z = this.mainCameraTransform.position.z;
        position = this.mainCamera.ScreenToWorldPoint(position);
        position.z = 0;

        return position;
    }

    /// <summary>
    /// ドラック開始イベントハンドラ
    /// </summary>
    public void OnMouseDown()
    {
        this.dragStart = this.GetMousePosition();
        this.currentPosition = this.physics.position;

/*        this.direction.enabled = true;
        this.direction.SetPosition(0, this.currentPosition);
        this.direction.SetPosition(1, this.currentPosition);*/
    }

    /// <summary>
    /// ドラッグ中イベントハンドラ
    /// </summary>
    public void OnMouseDrag()
    {
        var position = this.GetMousePosition();

        this.currentForce = position - this.dragStart;
        if (this.currentForce.magnitude > MaxMagnitude * MaxMagnitude)
        {
            this.currentForce *= MaxMagnitude / this.currentForce.magnitude;
        }
        this.StartCoroutine(this.Simulation());
    }

    /// <summary>
    /// 軌跡を予測して描画するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator Simulation()
    {
        // 自動的な物理運動を停止させる
        Physics.autoSimulation = false;

        var points = new List<Vector3> { this.currentPosition };
        this.Flip(this.currentForce * 6f);

        // 運動の軌跡をシミュレーションして記録する
        for (var i = 0; i < 16; i++)
        {
            Physics.Simulate(DeltaTime * 2f);
            points.Add(this.physics.position);
        }

        // もとの位置に戻す
        this.physics.velocity = Vector3.zero;
        this.transform.position = this.currentPosition;

        // 予測地点をつないで軌跡を描画
        this.simulationLine.positionCount = points.Count;
        this.simulationLine.SetPositions(points.ToArray());

        Physics.autoSimulation = true;

        yield return WaitForFixedUpdate;
    }

    /// <summary>
    /// ドラッグ終了イベントハンドラ
    /// </summary>
    public void OnMouseUp()
    {
//        this.direction.enabled = false;
        this.Flip(this.currentForce * 6f);
    }

    /// <summary>
    /// ボールをはじく
    /// </summary>
    /// <param name="force"></param>
    public void Flip(Vector3 force)
    {
        // 瞬間的に力を加えてはじく
        this.physics.AddForce(force, ForceMode.Impulse);
    }
}