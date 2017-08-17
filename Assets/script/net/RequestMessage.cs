using UnityEngine;
using System.Collections;
using System;

[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
public class RequestMessage : Attribute {
    public int messageType;
    public int cmd;
}
