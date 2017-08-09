using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class ChessPieceController : MonoBehaviour {

    //是否被选中
    //public bool isPick;

    [SerializeField]
    public ChessPiece chessPiece;

    //该棋子所属的位置
    public SitController sitController;
    //该棋子的角色
    public Transform sprite;
    //技能显示面板
    public SkillCanvasController skillCanvasController;
    private int frameNum;

    public virtual void Start() {
        //获取到自己的坐标位置
        int x = chessPiece.x;
        int z = chessPiece.z;

        //Debug.LogFormat("初始化象棋，name={0},x={1},z={2}", this.gameObject.name, x, z);
        string name = x + "" + z;
        //根据座位获取到自己的位置
        GameObject go = GameObject.Find(name);
        transform.position = go.transform.position;
        Debug.Log(name + "," + go.name);
        sitController = go.GetComponent<SitController>();
        sitController.chessPieceObj = this.gameObject;
        foreach (Transform tf in GetComponentsInChildren<Transform>()) {
            if (tf.name.Equals("sprite")) {
                sprite = tf;
                break;
            }
        }

        skillCanvasController = GameObject.Find("skillCanvas").GetComponent<SkillCanvasController>();
        System.Random r = new System.Random();
        frameNum = x+z;
        //Debug.Log("shui ji shu:" +frameNum);
    }

    public void Update() {
        if (sprite != null) {
            frameNum++;
            int a = frameNum / 50;
            int b = frameNum % 50;
            if (b == 0) {
                if (a % 2 == 0) {
                    sprite.Translate(new Vector3(0, 0, 0.1f));
                } else {
                    sprite.Translate(new Vector3(0, 0, -0.1f));
                }
            }
        }
    }
    //private void OnMouseDown() {
    //    if (!isPick) {
    //        pick();
    //    } else {
    //        unpick();
    //    }
    //}

    public void Pick() {
        //if (!isPick) {
        this.gameObject.transform.position += new Vector3(0, 3, 0);
        SpriteRenderer sr = sprite.GetComponent<SpriteRenderer>();
        skillCanvasController.Reset(sr.sprite).Show();

        //isPick = true;
        //}
    }
    public void Unpick() {
        //if (isPick) {
        this.gameObject.transform.position -= new Vector3(0, 3, 0);
        // isPick = false;
        // }
    }

    public abstract List<GameObject> GetCanMove();

    public bool MoveTo(GameObject gameObject) {
        //List<GameObject> canMoves = getCanMove();
        //如果可以行走
        //if (canMoves.Contains(gameObject)) {
        //Debug.Log(gameObject.transform.position.y);
        Vector3 v3 = this.gameObject.transform.position;
        v3.x = gameObject.transform.position.x;
        v3.z = gameObject.transform.position.z;
        this.gameObject.transform.position = v3;
        SitController sit = gameObject.GetComponent<SitController>();
        sit.chessPieceObj = this.gameObject;

        string name = gameObject.name;
        chessPiece.x = Convert.ToInt16(name.Substring(0, 1));
        chessPiece.z = Convert.ToInt16(name.Substring(1, 1));


        sitController.chessPieceObj = null;
        sitController = sit;
        return true;
        // }
        // return false;
    }
}
