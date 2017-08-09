using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameController : MonoBehaviour {

    // public static int[][] qipan = { }

    //当前选中棋子的控制器
    public ChessPieceController pickCpc;

    public List<GameObject> sits;
    //红方卡片
    public List<Card> teamHong = new List<Card>();
    //黑方卡片
    public List<Card> teamHei = new List<Card>();

    public TeamID currentTeamId = TeamID.HONG;

    void Init() {
        ChessPiece chessPiece = new ChessPiece(TeamID.HONG, ChessPieceType.BING, 0, 6);
        Card temCard = new Card(chessPiece, "dunpaiBg", "180008");
        teamHong.Add(temCard);
        foreach (Card card in teamHong) {
            ChessPieceType c = card.chessPiece.chessPieceType;
            string name = c.ToString();
            GameObject sitObj = GameObject.Find(card.chessPiece.SitName());
            GameObject gameObj = Resources.Load("prefabs/cardPfb/" + name) as GameObject;
            gameObj = Instantiate(gameObj);
            gameObj.transform.position = sitObj.transform.position;
            Material material = Resources.Load("wujiang/Materials/" + card.bgMaterials) as Material;
            Texture2D texture = Resources.Load("wujiang/" + card.spriteMaterials) as Texture2D;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            MeshRenderer meshRenderer = gameObj.GetComponent<MeshRenderer>();
            meshRenderer.materials[0] = material;
            foreach (SpriteRenderer spriteRenderer in gameObj.GetComponentsInChildren<SpriteRenderer>()) {
                spriteRenderer.sprite = sprite;
                break;
            }
        }
    }

    // Use this for initialization
    void Start() {
        //Init();
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
        cpc.Pick();
        sits = cpc.GetCanMove();
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
        pickCpc.MoveTo(sit);
        sitc.chessPieceObj = pickCpc.gameObject;
        clearState();
    }

    private void clearState() {
        sits.ForEach(obj => {
            obj.GetComponent<MeshRenderer>().enabled = false;
        });
        pickCpc.Unpick();
        pickCpc = null;
        sits = null;
    }
}
