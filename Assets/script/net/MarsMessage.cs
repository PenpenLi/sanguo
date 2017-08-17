using UnityEngine;
using System.Collections;
using ProtoBuf;

[ProtoContract]
public class MarsMessage{
    /* 消息类型 */
    [ProtoMember(1, IsRequired = true)]
    public int messageType;
    /* 子命令字*/
    [ProtoMember(2, IsRequired = true)]
    public int cmd;
    /* 数据*/
    [ProtoMember(3, IsRequired = true)]
    public byte[] data;
}
