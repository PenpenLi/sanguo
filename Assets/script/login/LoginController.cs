using Assets.script.constant;
using Assets.Scripts.login;
using Assets.Scripts.manager;
using Assets.Scripts.net;
using Assets.Scripts.tool;
using org.alan;
using org.alan.chess.proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour {


    public AppConfig appConfig;

    public UserMeta userMeta;

    public bool isLoad = false;
    public InputField userNameInput;
    public InputField passwordInput;
    public Button loginBut;
    public Button registerBut;
    public Button findPwdBut;
    public Button guestBut;
    public Toggle rememberMeTog;
    string _userName;
    string _password;

    private void Start() {
        appConfig = ApplicationManager.appConfig;
        userMeta = ApplicationManager.userMeta;
        //登录按钮增加事件
        loginBut.onClick.AddListener(LoginHandle);
        //loginBut.OnPointerClick()
        //增加注册按钮事件
        registerBut.onClick.AddListener(RegisterHandler);
        //试玩按钮事件,游客登录
        guestBut.onClick.AddListener(GuestLogin);
        //找回密码按钮事件
        findPwdBut.onClick.AddListener(FindPwd);
        userNameInput.text = userMeta.userName;
        passwordInput.text = userMeta.password;

    }

    /// <summary>
    /// 登录处理
    /// </summary>
    private void LoginHandle() {
        _userName = userNameInput.text.Trim();
        _password = passwordInput.text.Trim();
        Login(_userName, _password);
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    private void Login(string userName, string password) {
        if (userName == "") {
            PopupManager.ShowTimerPopUp("username is null");
            return;
        }
        Debug.Log(userName + "," + password);

        byte[] result = Encoding.Default.GetBytes(password);    //tbPass为输入密码的文本框
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] output = md5.ComputeHash(result);
        string pswMD5 = BitConverter.ToString(output).Replace("-", "");  //tbMd5pass为输出加密文本的文本框

        Dictionary<string, string> dic = new Dictionary<string, string> {
            { "appID", appConfig.appID },
            { "channelID", appConfig.channelID },
            { "username", userName },
            { "psw", pswMD5 }
        };
        // dic.Add("email", "test@tt.com");
        StartCoroutine(ClientHttp.getInstance().POST(appConfig.asdkUrl + "/account/login", dic, LoginResult)); // wai 注册地址：http://host:port/account/register 请求方式：POST

        PopupManager.AddWindow(PopupWindowName.WAITING_NET);
    }
    /// <summary>
    /// 登录消息返回回调
    /// </summary>
    /// <param name="result"></param>
    private void LoginResult(string result) {
        Debug.Log(result);
        PopupManager.RemoveWindow(PopupWindowName.WAITING_NET);
        if (result == null || result.StartsWith("error")) {
            PopupManager.ShowClosePopUp(result);
            SceneManager.LoadScene("scene/main");
            return;
        }
        LoginData loginData = JsonUtility.FromJson<LoginData>(result);
        if (loginData.state == 1) {
            LoginDataS loginS = JsonUtility.FromJson<LoginDataS>(result);
            // LoginDataSuccess loginSucc = JsonUtility.FromJson<LoginDataSuccess>(loginS.data);
            LoginCenter(loginS.data.userID.ToString(), loginS.data.username, loginS.data.token);
            PopupManager.AddWindow(PopupWindowName.WAITING_NET);
            //  SceneManager.LoadScene("Main");
        } else {
            PopupManager.ShowClosePopUp(loginData.des);
        }
    }
    /// <summary>
    /// 注册事件响应函数
    /// </summary>
    private void RegisterHandler() {
        transform.Find("LoginPlane").gameObject.SetActive(false);
        transform.Find("RegisterPlane").gameObject.SetActive(true);
        transform.Find("RegisterPlane/ButtonRegister").gameObject.GetComponent<Button>().onClick.AddListener(delegate () {
            string userName = transform.Find("RegisterPlane/UserName").gameObject.GetComponent<InputField>().text;
            string password = transform.Find("RegisterPlane/Password").gameObject.GetComponent<InputField>().text;
            string passwordAgain = transform.Find("RegisterPlane/PasswordAgain").gameObject.GetComponent<InputField>().text;
            Register(userName, password, passwordAgain);
        });
        transform.Find("RegisterPlane/ButtonBack").gameObject.GetComponent<Button>().onClick.AddListener(delegate () {
            transform.Find("LoginPlane").gameObject.SetActive(true);
            transform.Find("RegisterPlane").gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="passwordAgain"></param>
    private void Register(string userName, string password, string passwordAgain) {
        if (userName == "") {
            PopupManager.ShowTimerPopUp("username is null");
            return;
        } else if (password != passwordAgain) {
            PopupManager.ShowTimerPopUp("password passwordAgain not same");
            return;
        }
        Dictionary<string, string> dic = new Dictionary<string, string> {
            { "appID", appConfig.appID },
            { "channelID", appConfig.channelID },
            { "username", userName },
            { "psw", password }
        };
        StartCoroutine(ClientHttp.getInstance().POST(appConfig.asdkUrl + "/account/register", dic, RegisterResult));

    }

    private void RegisterResult(string result) {
        Debug.Log(result);
        PopupManager.RemoveWindow(PopupWindowName.WAITING_NET);
        if (result == null || result.StartsWith("error")) {
            PopupManager.ShowClosePopUp(result);
            return;
        }
        LoginData loginData = JsonUtility.FromJson<LoginData>(result);
        if (loginData.state == 1) {
            string userName = userNameInput.text;
            string password = passwordInput.text;

            transform.Find("RegisterPlane").gameObject.SetActive(false);
            transform.Find("LoginPlane").gameObject.SetActive(true);
            userNameInput.text = userName;
            passwordInput.text = password;

        } else {
            Debug.Log(loginData.des);
            PopupManager.ShowTimerPopUp(loginData.des);
        }

        // return JsonUtility.FromJson<GameStatus>(result);{"data":{"userID":18535,"username":"test123","token":"1393ba316c11bec1b8012d11e7563cd0"},"state":1},{"des":"The username or password is incorrect","state":5}
    }

    /// <summary>
    /// 查找密码
    /// </summary>
    private void FindPwd() {
        Dictionary<string, string> dic = new Dictionary<string, string> {
            { "appID", appConfig.appID },
            { "channelID", appConfig.channelID }
        };
        StartCoroutine(ClientHttp.getInstance().POST(appConfig.asdkUrl + "/account/resetPwd", dic, GuestLoginResult));
        PopupManager.AddWindow(PopupWindowName.WAITING_NET);
    }
    /// <summary>
    /// 游客登录
    /// </summary>
    private void GuestLogin() {
        Dictionary<string, string> dic = new Dictionary<string, string> {
            { "appID", appConfig.appID },
            { "channelID", appConfig.channelID }
        };
        StartCoroutine(ClientHttp.getInstance().POST(appConfig.asdkUrl + "/account/guest", dic, GuestLoginResult));
        PopupManager.AddWindow(PopupWindowName.WAITING_NET);
    }
    /// <summary>
    /// 游客登录，回调
    /// </summary>
    /// <param name="result"></param>
    private void GuestLoginResult(string result) {
        // Debug.Log(result);
        PopupManager.RemoveWindow(PopupWindowName.WAITING_NET);
        QuickRegisters quickRegisters = JsonUtility.FromJson<QuickRegisters>(result);
        if (quickRegisters.state == 1) {
            userNameInput.text = quickRegisters.data.guestName;
            passwordInput.text = quickRegisters.data.psw;
        } else {
            Debug.Log("quick fail");
        }
    }

    private void LoginCenter(string userId, string userName, string token) {
        if (userName == "") {
            PopupManager.ShowTimerPopUp("username is empty");
            return;
        }
        // Debug.Log(userName + "." + userID);
        Dictionary<string, string> dic = new Dictionary<string, string> {
            { "userId", userId },
            { "token", token },
            { "platform", appConfig.appID },
            { "channel", appConfig.channelID }
        };
        // dic.Add("email", "test@tt.com");
        StartCoroutine(ClientHttp.getInstance().POST(appConfig.centerUrl + "/user/certify", dic, LoginCenterResult)); // waiwang       注册地址：http://host:port/account/register 请求方式：POST

    }

    /// <summary>
    /// 登录认证服务器返回信息
    /// </summary>
    /// <param name="result"></param>
    private void LoginCenterResult(string result) {
        Debug.Log(result);
        PopupManager.RemoveWindow(PopupWindowName.WAITING_NET);
        if (result == null || result.StartsWith("error")) {
            PopupManager.ShowClosePopUp(result);
            return;
        }
        PlayerManager.self.loginDataCenter = JsonUtility.FromJson<LoginDataCenter>(result);
        if (PlayerManager.self.loginDataCenter.code != 0) {
            PopupManager.ShowClosePopUp(PlayerManager.self.loginDataCenter.dec);
            return;
        } else {
            LoginGame();
        }
    }

    private void LoginGame() {
        NetManager.LoginGameServer();
        PlayerPrefs.SetString("userName", _userName);
        PlayerPrefs.SetString("password", _password);
        //StartCoroutine(Tool.LoadScene("scene/main"));
    }

    private void SaveUser() {
        UserMeta userMeta = new UserMeta {
            userName = _userName,
            password = _password
        };
        ApplicationManager.SaveUserInfo(userMeta);
    }

    private void OnApplicationQuit() {
        Debug.Log("login OnApplicationQuit");
        if (NetManager.clientSocket != null) {
            NetManager.clientSocket.Close();
        }
    }
}
