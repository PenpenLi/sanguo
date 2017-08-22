using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.net;

public class CreateRoleController : MonoBehaviour {
    public InputField roleNameField;
    public Button enterBut;
    LoginHandler loginHandler;

    private void Start() {
        //登录按钮增加事件
        enterBut.onClick.AddListener(OnCreateRole);
        //注册登录消息处理
        loginHandler = new LoginHandler();
        MessageDispatcher.RegisterHandler(MessageTypeEnum.LOGIN, loginHandler);
    }

    private void Update() {
    }

    private void OnDestroy() {
        MessageDispatcher.RemoveHandler(MessageTypeEnum.LOGIN);
    }

    public void OnCreateRole() {
        string roleName = roleNameField.text.Trim();
        NetManager.SendCreateRole(roleName);
    }

}