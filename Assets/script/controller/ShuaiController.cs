using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ShuaiController : ChessPieceController {
    public override List<GameObject> getCanMove() {
        List<GameObject> result = new List<GameObject>();
        int x = chessPiece.x;
        int z = chessPiece.z;

        if (x == 3 || x == 4) {
            result.Add(GameObject.Find("" + (x + 1) + z));
        }
        if (x == 5 || x == 4) {
            result.Add(GameObject.Find("" + (x - 1) + z));
        }

        if (z == 0 || z == 7 || z == 1 || z == 8) {
            result.Add(GameObject.Find("" + x + (z + 1)));
        }
        if (z == 2 || z == 9 || z == 1 || z == 8) {
            result.Add(GameObject.Find("" + x + (z - 1)));
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

    void Update() {
        base.Update();
    }
}
