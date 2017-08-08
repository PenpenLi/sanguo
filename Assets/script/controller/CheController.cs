using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CheController : ChessPieceController {
    public override List<GameObject> getCanMove() {
        List<GameObject> result = new List<GameObject>();
        int x = chessPiece.x;
        int z = chessPiece.z;
        //向右寻找
        for (int p = x + 1; p <= 8; p++) {
            GameObject sitObject = GameObject.Find("" + p + z);
            SitController sit = sitObject.GetComponent<SitController>();
            if (sit.chessPieceObj == null) {
                result.Add(sitObject);
            } else {
                ChessPieceController chessPieceController = sit.chessPieceObj.GetComponent<ChessPieceController>();
                if (chessPieceController.chessPiece.teamId != chessPiece.teamId) {
                    result.Add(sitObject);
                }
                break;
            }
        }
        //向左寻找
        for (int p = x - 1; p >=0; p--) {
            GameObject sitObject = GameObject.Find("" + p + z);
            SitController sit = sitObject.GetComponent<SitController>();
            if (sit.chessPieceObj == null) {
                result.Add(sitObject);
            } else {
                ChessPieceController chessPieceController = sit.chessPieceObj.GetComponent<ChessPieceController>();
                if (chessPieceController.chessPiece.teamId != chessPiece.teamId) {
                    result.Add(sitObject);
                }
                break;
            }
        }
        //向上寻找
        for (int p = z - 1; p >= 0; p--) {
            GameObject sitObject = GameObject.Find("" + x + p);
            SitController sit = sitObject.GetComponent<SitController>();
            if (sit.chessPieceObj == null) {
                result.Add(sitObject);
            } else {
                ChessPieceController chessPieceController = sit.chessPieceObj.GetComponent<ChessPieceController>();
                if (chessPieceController.chessPiece.teamId != chessPiece.teamId) {
                    result.Add(sitObject);
                }
                break;
            }
        }
        //向下寻找
        for (int p = z + 1; p <=9 ; p++) {
            GameObject sitObject = GameObject.Find("" + x + p);
            SitController sit = sitObject.GetComponent<SitController>();
            if (sit.chessPieceObj == null) {
                result.Add(sitObject);
            } else {
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

    public void Update() {
        base.Update();
    }
}
