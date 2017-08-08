using UnityEngine;
using System.Collections;

public class Rot : MonoBehaviour {
    public float RotSpeed = -1;
    public float RotSpeedX = 0;
    public float RotSpeedZ = 0;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(RotSpeedX, RotSpeed, RotSpeedZ);
    }
}
