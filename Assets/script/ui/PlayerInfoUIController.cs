using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.manager;

public class PlayerInfoUIController : MonoBehaviour {
    public Text nameText;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        nameText.text = PlayerManager.self.player.role.name;
    }
}
