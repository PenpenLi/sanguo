using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class XiangController : ChessPieceController {
    public override List<GameObject> getCanMove() {
        List<GameObject> result = new List<GameObject>();
        int x = chessPiece.x;
        int z = chessPiece.z;


        if (z != 5) {
            result.Add(GameObject.Find("" + (x + 2) + (z - 2)));
            result.Add(GameObject.Find("" + (x - 2) + (z - 2)));
        }
        if (z != 4) {
            result.Add(GameObject.Find("" + (x + 2) + (z + 2)));
            result.Add(GameObject.Find("" + (x - 2) + (z + 2)));
        }
        //剔除掉己方已经占领的棋子
        result = result.FindAll(obj => {
            if (obj == null) {
                return false;
            }
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

    public void Update() {
        base.Update();
    }
}
