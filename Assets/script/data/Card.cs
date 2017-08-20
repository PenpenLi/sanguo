using UnityEngine;
using System.Collections;

/// <summary>
/// 卡片数据结构
/// </summary>
public class Card {
    public string name;
    //卡片类型
    public ChessPiece chessPiece;
    //背景材质
    public string bgMaterials;
    //精灵材质
    public string spriteMaterials;

    public Card(ChessPiece chessPiece, string bgMaterials, string spriteMaterials) {
        this.chessPiece = chessPiece;
        this.bgMaterials = bgMaterials;
        this.spriteMaterials = spriteMaterials;
    }
}
