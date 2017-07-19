using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    //当前选中棋子的控制器
    public ChessPieceController pickCpc;

    public List<GameObject> sits;

    public TeamID currentTeamId = TeamID.HONG;


    // Use this for initialization
    void Start() {
        //Debug.Log("为座位增加脚本...");
        ////为每个座位对象添加脚本组件
        //GameObject[] objs=GameObject.FindGameObjectsWithTag("Sit");
        //foreach (GameObject obj in objs) {
        //    obj.AddComponent<SitController>();
        //}        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  //camare2D.ScreenPointToRay (Input.mousePosition);  
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                GameObject tmp = hit.collider.gameObject;
                //判断选中的位置
                if ("Sit".Equals(tmp.tag)) {
                    Debug.Log("选中位置,sit=" + tmp.name);
                    SitController sitController = tmp.GetComponent<SitController>();
                    //选中了一个空座位
                    if (pickCpc == null || sitController.chessPieceObj != null) {
                        return;
                    } else {
                        if (sitController.chessPieceObj == null) {//选中的位置没有棋子
                            if (sits.Contains(tmp)) {//如果该位置可以走
                                moveTo(tmp);
                            }
                        }
                    }
                } else if ("ChessPiece".Equals(tmp.tag)) {
                    ChessPieceController cpc = tmp.GetComponent<ChessPieceController>();
                    if (pickCpc == null) {
                        if (cpc.chessPiece.teamId == currentTeamId) {
                            pick(cpc);
                        }
                    } else {
                        if (cpc == pickCpc) {
                            clearState();
                        } else if (cpc.chessPiece.teamId == pickCpc.chessPiece.teamId) {
                            clearState();
                            pick(cpc);
                        } else {
                            GameObject sitobj = cpc.sitController.gameObject;
                            if (sits.Contains(sitobj)) {
                                moveTo(sitobj);
                            }
                        }
                    }
                }
            }
        }
    }

    private void pick(ChessPieceController cpc) {
        //选中棋子
        pickCpc = cpc;
        cpc.pick();
        sits = cpc.getCanMove();
        sits.ForEach(obj => {
            obj.GetComponent<MeshRenderer>().enabled = true;
        });
    }

    private void moveTo(GameObject sit) {
        if (currentTeamId == TeamID.HONG) {
            currentTeamId = TeamID.HEI;
        } else {
            currentTeamId = TeamID.HONG;
        }
        SitController sitc = sit.GetComponent<SitController>();
        if (sitc.chessPieceObj != null) {
            //把该位置原来的棋子注销
            Destroy(sitc.chessPieceObj);
        }
        pickCpc.moveTo(sit);
        sitc.chessPieceObj = pickCpc.gameObject;
        clearState();
    }

    private void clearState() {
        sits.ForEach(obj => {
            obj.GetComponent<MeshRenderer>().enabled = false;
        });
        pickCpc.unpick();
        pickCpc = null;
        sits = null;
    }
}
