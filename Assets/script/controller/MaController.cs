using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MaController : ChessPieceController {
    public override List<GameObject> getCanMove() {
        List<GameObject> result = new List<GameObject>();
        int x = chessPiece.x;
        int z = chessPiece.z;

        // 向前走
        if (z <= 7) {
            GameObject sitObject = GameObject.Find("" +x+ (z + 1));
            SitController sitController = sitObject.GetComponent<SitController>();
            //查看是否有阻挡
            if (sitController.chessPieceObj == null) {
                sitObject = GameObject.Find("" + (x + 1) + (z + 2) );
                if (sitObject != null) {
                    result.Add(sitObject);
                }
                sitObject = GameObject.Find("" + (x - 1) + (z + 2) );
                if (sitObject != null) {
                    result.Add(sitObject);
                }
            }
        }
        //向后走
        if (z >= 2) {
            GameObject sitObject = GameObject.Find("" + x + (z - 1) );
            SitController sitController = sitObject.GetComponent<SitController>();
            //查看是否有阻挡
            if (sitController.chessPieceObj == null) {
                sitObject = GameObject.Find("" + (x + 1) + (z - 2) );
                if (sitObject != null) {
                    result.Add(sitObject);
                }
                sitObject = GameObject.Find("" + (x - 1) + (z - 2) );
                if (sitObject != null) {
                    result.Add(sitObject);
                }
            }
        }
        //向左走
        if (x >= 2) {
            GameObject sitObject = GameObject.Find("" + (x - 1) + z );
            SitController sitController = sitObject.GetComponent<SitController>();
            //查看是否有阻挡
            if (sitController.chessPieceObj == null) {
                sitObject = GameObject.Find(""+(x - 2) + (z - 1));
                if (sitObject != null) {
                    result.Add(sitObject);
                }
                sitObject = GameObject.Find("" + (x - 2) + (z + 1) );
                if (sitObject != null) {
                    result.Add(sitObject);
                }
            }
        }
        //向右走
        if (x <= 6) {
            GameObject sitObject = GameObject.Find("" + (x + 1) + z );
            SitController sitController = sitObject.GetComponent<SitController>();
            //查看是否有阻挡
            if (sitController.chessPieceObj == null) {
                sitObject = GameObject.Find("" + (x + 2) + (z - 1) );
                if (sitObject != null) {
                    result.Add(sitObject);
                }
                sitObject = GameObject.Find("" + (x + 2) + (z + 1) );
                if (sitObject != null) {
                    result.Add(sitObject);
                }
            }
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

    void Update() {
        base.Update();
    }
}
