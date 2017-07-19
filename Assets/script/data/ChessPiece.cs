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
}

