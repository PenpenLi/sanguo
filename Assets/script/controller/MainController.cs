using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour {

    public Button tiantiBut;
    public Button pipeiBut;
    public Button buzhenBut;

    // Use this for initialization
    void Start() {
        tiantiBut.onClick.AddListener(OnTianTi);
        pipeiBut.onClick.AddListener(OnPiPei);
        pipeiBut.onClick.AddListener(OnBuZhen);
    }

    void OnTianTi() {
        SceneManager.LoadScene("scene/game");
    }

    void OnPiPei() {
    }
    void OnBuZhen() {
    }
    // Update is called once per frame
    void Update() {

    }

}
