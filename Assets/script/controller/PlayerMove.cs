using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

    //定义移动速度
    public float MoveSpeed = 10f;
    //定义旋转速度
    public float RotateSpeed = 0.05f;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //如果按下W或上方向键
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            MoveForward();
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            MoveBack();
        }

        //如果按下A或左方向键
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            //以RotateSpeed为速度向左旋转
            MoveLeft();
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            MoveRight();
        }
    }

    void MoveForward() {
        //以MoveSpeed的速度向正前方移动
        this.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
    }
    void MoveBack() {
        //以MoveSpeed的速度向正后方移动
        this.transform.Translate(Vector3.back * MoveSpeed * Time.deltaTime);
    }
    void MoveLeft() {
        //以MoveSpeed的速度向正左方移动
        this.transform.Translate(Vector3.left * MoveSpeed * Time.deltaTime);
    }
    void MoveRight() {
        //以MoveSpeed的速度向正右方移动
        this.transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
    }
}
