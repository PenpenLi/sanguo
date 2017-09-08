using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using org.alan.chess.proto;
using Assets.Scripts.manager;
using Assets.script.manager;
using Assets.script.constant;

public class GameController : MonoBehaviour, IResponseHandler {
    //当前选中棋子的控制器
    public ChessPieceController pickCpc;
    public List<GameObject> sits;
    Queue<MarsMessage> messageQueue = new Queue<MarsMessage>();
    public RespGameInit respGameInit;
    public RespCurrentGoInfo goInfo;
    public int selfTeamId;
    public long selfPlayerId;
    private void Awake() {
        BattleStatus battleStatus = PlayerManager.self.statusManager.currentStatus as BattleStatus;
        respGameInit = battleStatus.respGameInit;
        selfPlayerId = PlayerManager.self.player.role.roleUid;
        foreach (TeamInfo teamInfo in respGameInit.teamInfos) {
            foreach (PlayerFighter fighter in teamInfo.fighters) {
                if (fighter.playerId == selfPlayerId) {
                    selfTeamId = teamInfo.teamId;
                }
            }
        }
        List<CardSprite> sprites = respGameInit.allSprite;
        Debug.LogFormat("sprite size is {0}", sprites.Count);
        foreach (CardSprite cardSprite in sprites) {
            //开始初始化棋牌数据
            int type = cardSprite.type;
            ChessPieceType c = (ChessPieceType)(type - 1);
            string sitName = cardSprite.x + "" + cardSprite.z;
            string name = c.ToString();
            //Debug.LogFormat(sitName);
            GameObject sitObj = GameObject.Find(sitName);
            GameObject gameObj = Resources.Load("prefabs/cardPfb/" + name) as GameObject;
            gameObj = Instantiate(gameObj);
            ChessPieceController cpc = gameObj.GetComponent<ChessPieceController>();
            cpc.chessPiece = new ChessPiece((TeamID)(cardSprite.team), c, cardSprite.x, cardSprite.z);
            gameObj.transform.position = sitObj.transform.position;
            if (selfTeamId == (int)TeamID.HEI) {
                Quaternion quaternion = gameObj.transform.rotation;
                gameObj.transform.rotation = Quaternion.Euler(0, quaternion.x, quaternion.z);
            }
            Material material = Resources.Load("wujiang/Materials/" + cardSprite.bg) as Material;
            Texture2D texture = Resources.Load("wujiang/" + cardSprite.texture) as Texture2D;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            MeshRenderer meshRenderer = gameObj.GetComponent<MeshRenderer>();
            meshRenderer.materials[0] = material;
            foreach (SpriteRenderer spriteRenderer in gameObj.GetComponentsInChildren<SpriteRenderer>()) {
                spriteRenderer.sprite = sprite;
                break;
            }
        }

        GameObject heiGameObject = GameObject.Find("heiqi");
        if (heiGameObject != null) {
            heiGameObject.SetActive(false);
            Destroy(heiGameObject);
        }
        GameObject hongqiGameObject = GameObject.Find("hongqi");
        if (hongqiGameObject != null) {
            hongqiGameObject.SetActive(false);
            Destroy(hongqiGameObject);
        }

        if (selfTeamId != (int)TeamID.HONG) {
            GameObject team1Camera = GameObject.Find("Team1Camera");
            if (team1Camera != null) {
                team1Camera.SetActive(false);
                Destroy(team1Camera);
            }
        }
        if (selfTeamId != (int)TeamID.HEI) {
            GameObject team2Camera = GameObject.Find("Team2Camera");
            if (team2Camera != null) {
                team2Camera.SetActive(false);
                Destroy(team2Camera);
            }
        }

        MessageDispatcher.RegisterHandler(MessageConst.Battle.TYPE, this);
        //初始化完成，向服务器发送消息
        NetManager.SendGameInitDone();
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
        OnReceive();
        if (goInfo == null) {
            return;
        }
        if (goInfo.currentTeamId != selfTeamId) {
            return;
        }
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
                                MoveTo(tmp);
                            }
                        }
                    }
                } else if ("ChessPiece".Equals(tmp.tag)) {
                    Debug.Log("选中棋子,name=" + tmp.name);
                    ChessPieceController cpc = tmp.GetComponent<ChessPieceController>();
                    if (pickCpc == null) {
                        if (cpc.chessPiece.teamId == (TeamID)goInfo.currentTeamId) {
                            Pick(cpc);
                        }
                    } else {
                        if (cpc == pickCpc) {
                            ClearState();
                        } else if (cpc.chessPiece.teamId == pickCpc.chessPiece.teamId) {
                            ClearState();
                            Pick(cpc);
                        } else {
                            GameObject sitobj = cpc.sitController.gameObject;
                            if (sits.Contains(sitobj)) {
                                MoveTo(sitobj);
                            }
                        }
                    }
                }
            }
        }
    }

    private void Pick(ChessPieceController cpc) {
        //选中棋子
        pickCpc = cpc;
        cpc.Pick();
        sits = cpc.GetCanMove();
        sits.ForEach(obj => {
            obj.GetComponent<MeshRenderer>().enabled = true;
        });
    }


    private void MoveTo(GameObject sit) {
        BattlePoint fromPoint = new BattlePoint() {
            x = pickCpc.chessPiece.x,
            z = pickCpc.chessPiece.z
        };
        string name = sit.name;
        int _x = Convert.ToInt32(name.Substring(0, 1));
        int _z = Convert.ToInt32(name.Substring(1, 1));
        Debug.LogFormat("棋子移动目的地[{0},{1}]", _x, _z);
        BattlePoint toPoint = new BattlePoint() {
            x = _x,
            z = _z
        };
        MoveChess moveChess = new MoveChess() {
            fromPoint = fromPoint,
            toPoint = toPoint
        };
        NetManager.SendMoveChess(moveChess);
    }

    public void ReceiveChessMove(RespMoveChess respMoveChess) {
        long roleUid = respMoveChess.roleUid;
        MoveChess moveChess = respMoveChess.moveChess;
        string toSitName = moveChess.toPoint.x + "" + moveChess.toPoint.z;
        GameObject toSit = GameObject.Find(toSitName);
        string fromSitName = moveChess.fromPoint.x + "" + moveChess.fromPoint.z;
        Debug.LogFormat("收到服务器移动信息,from={0},to={1}", fromSitName, toSitName);
        GameObject fromSit = GameObject.Find(fromSitName);
        SitController sitController = fromSit.GetComponent<SitController>();
        GameObject pickObject = sitController.chessPieceObj;
        ChessPieceController pickCpc = pickObject.GetComponent<ChessPieceController>();
        SitController sitc = toSit.GetComponent<SitController>();
        if (sitc.chessPieceObj != null) {
            //把该位置原来的棋子注销
            Destroy(sitc.chessPieceObj);
        }
        pickCpc.MoveTo(toSit);
        sitc.chessPieceObj = pickCpc.gameObject;
        ClearState();
    }

    public void ReceiveGoInfo(RespCurrentGoInfo rcgi) {
        goInfo = rcgi;
    }

    private void ClearState() {
        if (sits != null) {
            sits.ForEach(obj => {
                obj.GetComponent<MeshRenderer>().enabled = false;
            });
        }

        if (pickCpc != null) {
            pickCpc.Unpick();
        }
        pickCpc = null;
        sits = null;
    }

    private void OnDestroy() {
        MessageDispatcher.RemoveHandler(MessageConst.Battle.TYPE);
    }

    private void OnApplicationQuit() {
        Debug.Log("login OnApplicationQuit");
        NetManager.clientSocket.Close();
    }

    public void OnReceive() {
        if (messageQueue.Count > 0) {
            MarsMessage message = messageQueue.Dequeue();
            if (message == null) {
                Debug.LogFormat("消息={）}，消息队列情况长度={1}", message, messageQueue.Count);
                return;
            }
            switch (message.cmd) {
                case MessageConst.Battle.RESP_MOVE_CHESS:
                    Debug.Log("移动棋子");
                    RespMoveChess respMoveChess = ProtobufTool.DeSerialize<RespMoveChess>(message.data);
                    ReceiveChessMove(respMoveChess);
                    break;
                case MessageConst.Battle.RESP_CURRENT_GO_INFO:
                    Debug.Log("当前行动信息");
                    RespCurrentGoInfo respCurrentGoInfo = ProtobufTool.DeSerialize<RespCurrentGoInfo>(message.data);
                    ReceiveGoInfo(respCurrentGoInfo);
                    break;
                default:
                    break;
            }
        }
    }

    public void Handle(int cmd, byte[] data) {
        MarsMessage message = new MarsMessage {
            cmd = cmd,
            data = data
        };
        messageQueue.Enqueue(message);
    }
}
