using UnityEngine;
using System.Collections;

public class CameraLookAt : MonoBehaviour {
    public float near = 10.0f;
    public float far = 100.0f;

    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    public float sensitivetyZ = 2f;
    public float sensitivetyMove = 2f;
    public float sensitivetyMouseWheel = 5f;

    public GameObject target;
    float damping = 5.0f;

    void Update() {
        Camera camera = gameObject.GetComponent<Camera>();
        // 滚轮实现镜头缩进和拉远
        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            camera.fieldOfView = camera.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * sensitivetyMouseWheel;
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, near, far);
        }
        //鼠标右键实现视角转动，类似第一人称视角
        if (Input.GetMouseButton(0)) {
            float rotationX = Input.GetAxis("Mouse X") * sensitivityX;
            float rotationY = Input.GetAxis("Mouse Y") * sensitivityY;
            transform.Translate(Vector3.left * rotationX);
            transform.Translate(Vector3.down * rotationY);
            //transform.Rotate(rotationX, -rotationY, 0);
        }

        //鼠标右键实现视角转动，类似第一人称视角
        if (Input.GetMouseButton(1)) {
            //Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
            //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
            float rotationX = Input.GetAxis("Mouse X") * sensitivityX;
            float rotationY = Input.GetAxis("Mouse Y") * sensitivityY;
            transform.Rotate(rotationX, -rotationY, 0);
        }

        //键盘按钮←和→实现视角水平旋转
        if (Input.GetAxis("Horizontal") != 0) {
            float rotationZ = Input.GetAxis("Horizontal") * sensitivetyZ;
            //transform.up
            transform.Translate(Vector3.left * rotationZ);
            //float view = camera.fieldOfView - rotationZ * sensitivetyMouseWheel;
            //camera.fieldOfView = Mathf.Clamp(view, near, far);
        }
        //键盘按钮←和→实现视角水平旋转
        if (Input.GetAxis("Vertical") != 0) {
            float rotationZ = Input.GetAxis("Vertical") * sensitivetyZ;
            transform.Translate(Vector3.down * rotationZ);
            //float view = camera.fieldOfView - rotationZ * sensitivetyMouseWheel;
            //camera.fieldOfView = Mathf.Clamp(view, near, far);
            //transform.Rotate(rotationZ,0, 0);
        }
    }

}
