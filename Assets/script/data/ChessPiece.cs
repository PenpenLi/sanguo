using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable]
public enum TeamID { HONG, HEI }
[System.Serializable]
public enum ChessPieceType {
    SHUAI, SHI, XIANG, MA, CHE, PAO, BING
}

[System.Serializable]
public class ChessPiece {
    public TeamID teamId;
    public ChessPieceType chessPieceType;
    public int x;
    public int z;

    public ChessPiece(TeamID teamId, ChessPieceType chessPieceType, int x, int z) {
        this.teamId = teamId;
        this.chessPieceType = chessPieceType;
        this.x = x;
        this.z = z;
    }

    public string SitName() {
        return x + "" + z;
    }
}

