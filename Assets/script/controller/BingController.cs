using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BingController : ChessPieceController {
    public override List<GameObject> getCanMove() {
        List<GameObject> result = new List<GameObject>();
        int x = chessPiece.x;
        int z = chessPiece.z;
        if (chessPiece.teamId == TeamID.HONG && z != 0) {
            //红兵向前行
            result.Add(GameObject.Find("" + x + (z - 1)));
        } else if (chessPiece.teamId == TeamID.HEI && z != 9) {//黑兵
            result.Add(GameObject.Find("" + x + (z + 1)));
        }
        if (isCross()) {
            //如果不在最右侧
            if (x != 8) {
                //红兵向右行
                result.Add(GameObject.Find("" + (x + 1) + z));
            }
            //如果不在最左边
            if (x != 0) {
                result.Add(GameObject.Find("" + (x - 1) + z));
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

    //是否已经过河
    public bool isCross() {
        if (chessPiece.teamId == TeamID.HONG && chessPiece.z < 5) {
            return true;
        }
        if (chessPiece.teamId == TeamID.HEI && chessPiece.z > 4) {
            return true;
        }
        return false;
    }

    // Use this for initialization
    public override void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {

    }


}
