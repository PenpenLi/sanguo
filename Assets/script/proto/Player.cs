//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: Player.proto
// Note: requires additional types generated from: Role.proto
namespace org.alan.chess.proto
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Player")]
  public partial class Player : global::ProtoBuf.IExtensible
  {
    public Player() {}
    

    private org.alan.chess.proto.Role _role = null;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"role", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public org.alan.chess.proto.Role role
    {
      get { return _role; }
      set { _role = value; }
    }

    private int _sceneId = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"sceneId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int sceneId
    {
      get { return _sceneId; }
      set { _sceneId = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}