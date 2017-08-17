using UnityEngine;
using System.Collections;
using System.IO;
using System;

/// <summary>
/// Protobuf 工具类，提供解码、编码功能
/// 该类使用protobuf-net工具进行protobuf消息处理
/// </summary>
public class ProtobufTool {
    /// <summary>
    /// 将消息序列化为二进制的方法
    /// </summary>
    /// <param name="model">要序列化的对象</param>
    /// <returns></returns>
    static public byte[] Serialize(object model) {
        try {
            //涉及格式转换，需要用到流，将二进制序列化到流中
            using (MemoryStream ms = new MemoryStream()) {
                //使用ProtoBuf工具的序列化方法
                ProtoBuf.Serializer.Serialize(ms, model);
                //定义二级制数组，保存序列化后的结果
                byte[] result = new byte[ms.Length];
                //将流的位置设为0，起始点
                ms.Position = 0;
                //将流中的内容读取到二进制数组中
                ms.Read(result, 0, result.Length);
                return result;
            }
        } catch (Exception ex) {
            Debug.Log("序列化失败: " + ex.ToString());
            return null;
        }
    }

    /// <summary>
    /// 将收到的消息反序列化成对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="msg"></param>
    /// <returns></returns>
    static public T DeSerialize<T>(byte[] msg) {
        T result;
        try {
            using (MemoryStream ms = new MemoryStream()) {
                //将消息写入流中
                ms.Write(msg, 0, msg.Length);
                //将流的位置归0
                ms.Position = 0;
                //使用工具反序列化对象
                result = ProtoBuf.Serializer.Deserialize<T>(ms);
                ms.Dispose();
                return result;
            }
        } catch (Exception ex) {
            Debug.Log("反序列化失败: " + ex.ToString());
            return default(T);
        }
    }

}
