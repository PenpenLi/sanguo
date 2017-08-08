using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PaoController : ChessPieceController {
    public override List<GameObject> getCanMove() {
        List<GameObject> result = new List<GameObject>();
        int x = chessPiece.x;
        int z = chessPiece.z;

        //炮架标志
        bool hasPaojia = false;
        //向右寻找
        for (int p = x + 1; p <= 8; p++) {
            GameObject sitObject = GameObject.Find("" + p + z);
            SitController sit = sitObject.GetComponent<SitController>();
            if (sit.chessPieceObj == null ) {
                if (!hasPaojia) {
                    result.Add(sitObject);
                }
            } else {
                if (!hasPaojia) {
                    hasPaojia = true;
                    continue;
                }
                ChessPieceController chessPieceController = sit.chessPieceObj.GetComponent<ChessPieceController>();
                if (chessPieceController.chessPiece.teamId != chessPiece.teamId) {
                    result.Add(sitObject);
                }
                break;
            }
        }
        hasPaojia = false;
        //向左寻找
        for (int p = x - 1; p >= 0; p--) {
            GameObject sitObject = GameObject.Find("" + p + z);
            SitController sit = sitObject.GetComponent<SitController>();
            if (sit.chessPieceObj == null) {
                if (!hasPaojia) {
                    result.Add(sitObject);
                }
            } else {
                if (!hasPaojia) {
                    hasPaojia = true;
                    continue;
                }
                ChessPieceController chessPieceController = sit.chessPieceObj.GetComponent<ChessPieceController>();
                if (chessPieceController.chessPiece.teamId != chessPiece.teamId) {
                    result.Add(sitObject);
                }
                break;
            }
        }
        hasPaojia = false;
        //向上寻找
        for (int p = z - 1; p >= 0; p--) {
            GameObject sitObject = GameObject.Find("" + x + p);
            SitController sit = sitObject.GetComponent<SitController>();
            if (sit.chessPieceObj == null) {
                if (!hasPaojia) {
                    result.Add(sitObject);
                }
            } else {
                if (!hasPaojia) {
                    hasPaojia = true;
                    continue;
                }
                ChessPieceController chessPieceController = sit.chessPieceObj.GetComponent<ChessPieceController>();
                if (chessPieceController.chessPiece.teamId != chessPiece.teamId) {
                    result.Add(sitObject);
                }
                break;
            }
        }
        hasPaojia = false;
        //向下寻找
        for (int p = z + 1; p <= 9; p++) {
            GameObject sitObject = GameObject.Find("" + x + p);
            SitController sit = sitObject.GetComponent<SitController>();
            if (sit.chessPieceObj == null) {
                if (!hasPaojia) {
                    result.Add(sitObject);
                }
            } else {
                if (!hasPaojia) {
                    hasPaojia = true;
                    continue;
                }
                ChessPieceController chessPieceController = sit.chessPieceObj.GetComponent<ChessPieceController>();
                if (chessPieceController.chessPiece.teamId != chessPiece.teamId) {
                    result.Add(sitObject);
                }
                break;
            }
        }
        return result;
    }

    // Use this for initialization
    public override void Start() {
        base.Start();
    }

    // Update is called once per frame
     void Update() {
        base.Update();
    }
}
