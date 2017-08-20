using UnityEngine;
using System.Collections;

public interface IResponseHandler {

    void Handle(int cmd, byte[] data);
}
