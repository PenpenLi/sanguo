using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ShiController : ChessPieceController {
    public override List<GameObject> getCanMove() {
        List<GameObject> result = new List<GameObject>();
        int x = chessPiece.x;
        int z = chessPiece.z;

        if (x == 4) {
            result.Add(GameObject.Find("" + (x + 1) + (z + 1)));
            result.Add(GameObject.Find("" + (x + 1) + (z - 1)));
            result.Add(GameObject.Find("" + (x - 1) + (z - 1)));
            result.Add(GameObject.Find("" + (x - 1) + (z + 1)));
        } else {
            if (chessPiece.teamId == TeamID.HEI) {
                result.Add(GameObject.Find("41"));
            } else {
                result.Add(GameObject.Find("48"));
            }
        }

        //剔除掉己方已经占领的棋子
        result = result.FindAll(obj => {
            SitController sit = obj.GetComponent<SitController>();
            if (sit.chessPieceObj != null) {
                ChessPieceController chessPieceController = sit.chessPieceObj.GetComponent<ChessPieceController>();
                if (chessPieceController.chessPiece.teamId == chessPiece.teamId) {
                    return false;
                }
            }
            return true;
        });

        return result;
    }

    // Use this for initialization
    public override void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {

    }
}
