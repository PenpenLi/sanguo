using UnityEngine;
using System.Collections;

public class CameraLookAt : MonoBehaviour {
    public Transform target;
    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if ((Input.mouseScrollDelta.y < 0 && Camera.main.fieldOfView >= 3) || Input.mouseScrollDelta.y > 0 && Camera.main.fieldOfView <= 80) {
            Camera.main.fieldOfView += Input.mouseScrollDelta.y * 20 * Time.deltaTime;
        }
    }
}
