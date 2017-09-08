using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.net;
using Assets.script.constant;

public class CreateRoleController : MonoBehaviour {
    public InputField roleNameField;
    public Button enterBut;

    private void Start() {
        //登录按钮增加事件
        enterBut.onClick.AddListener(OnCreateRole);
    }

    public void OnCreateRole() {
        string roleName = roleNameField.text.Trim();
        NetManager.SendCreateRole(roleName);
    }

}