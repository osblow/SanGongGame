//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: hall.proto
namespace com.sansanbbox.protobuf.lobby
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"InvitationRuleResponse")]
  public partial class InvitationRuleResponse : global::ProtoBuf.IExtensible
  {
    public InvitationRuleResponse() {}
    
    private uint _code;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"code", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint code
    {
      get { return _code; }
      set { _code = value; }
    }
    private uint _invitation_code = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"invitation_code", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint invitation_code
    {
      get { return _invitation_code; }
      set { _invitation_code = value; }
    }
    private string _invitation_rule_message;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"invitation_rule_message", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string invitation_rule_message
    {
      get { return _invitation_rule_message; }
      set { _invitation_rule_message = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"InvitationResponse")]
  public partial class InvitationResponse : global::ProtoBuf.IExtensible
  {
    public InvitationResponse() {}
    
    private uint _code;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"code", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint code
    {
      get { return _code; }
      set { _code = value; }
    }
    private string _message;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"message", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string message
    {
      get { return _message; }
      set { _message = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GameRecord")]
  public partial class GameRecord : global::ProtoBuf.IExtensible
  {
    public GameRecord() {}
    
    private uint _room_number;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"room_number", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint room_number
    {
      get { return _room_number; }
      set { _room_number = value; }
    }
    private uint _game_count;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"game_count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint game_count
    {
      get { return _game_count; }
      set { _game_count = value; }
    }
    private string _room_rule_name;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"room_rule_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string room_rule_name
    {
      get { return _room_rule_name; }
      set { _room_rule_name = value; }
    }
    private string _game_time;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"game_time", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string game_time
    {
      get { return _game_time; }
      set { _game_time = value; }
    }
    private uint _room_id;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"room_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint room_id
    {
      get { return _room_id; }
      set { _room_id = value; }
    }
    private readonly global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.GameRecord.Player> _player = new global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.GameRecord.Player>();
    [global::ProtoBuf.ProtoMember(6, Name=@"player", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.GameRecord.Player> player
    {
      get { return _player; }
    }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Player")]
  public partial class Player : global::ProtoBuf.IExtensible
  {
    public Player() {}
    
    private bool _is_owner;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"is_owner", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool is_owner
    {
      get { return _is_owner; }
      set { _is_owner = value; }
    }
    private uint _account_id = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"account_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint account_id
    {
      get { return _account_id; }
      set { _account_id = value; }
    }
    private string _player_head_img = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"player_head_img", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string player_head_img
    {
      get { return _player_head_img; }
      set { _player_head_img = value; }
    }
    private string _player_nike_name = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"player_nike_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string player_nike_name
    {
      get { return _player_nike_name; }
      set { _player_nike_name = value; }
    }
    private int _point = default(int);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"point", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int point
    {
      get { return _point; }
      set { _point = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Card")]
  public partial class Card : global::ProtoBuf.IExtensible
  {
    public Card() {}
    
    private uint _card;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"card", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint card
    {
      get { return _card; }
      set { _card = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserGameRecode")]
  public partial class UserGameRecode : global::ProtoBuf.IExtensible
  {
    public UserGameRecode() {}
    
    private uint _game_index_name;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"game_index_name", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint game_index_name
    {
      get { return _game_index_name; }
      set { _game_index_name = value; }
    }
    private readonly global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.UserGameRecode.User> _user = new global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.UserGameRecode.User>();
    [global::ProtoBuf.ProtoMember(2, Name=@"user", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.UserGameRecode.User> user
    {
      get { return _user; }
    }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"User")]
  public partial class User : global::ProtoBuf.IExtensible
  {
    public User() {}
    
    private bool _is_bank = default(bool);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"is_bank", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(default(bool))]
    public bool is_bank
    {
      get { return _is_bank; }
      set { _is_bank = value; }
    }
    private int _point = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"point", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int point
    {
      get { return _point; }
      set { _point = value; }
    }
    private string _player_nike_name = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"player_nike_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string player_nike_name
    {
      get { return _player_nike_name; }
      set { _player_nike_name = value; }
    }
    private uint _card_face = default(uint);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"card_face", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint card_face
    {
      get { return _card_face; }
      set { _card_face = value; }
    }
    private readonly global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.Card> _card = new global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.Card>();
    [global::ProtoBuf.ProtoMember(5, Name=@"card", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.Card> card
    {
      get { return _card; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserGameRecodeList")]
  public partial class UserGameRecodeList : global::ProtoBuf.IExtensible
  {
    public UserGameRecodeList() {}
    
    private bool _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool result
    {
      get { return _result; }
      set { _result = value; }
    }
    private readonly global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.UserGameRecodeList.UserName> _user_Name = new global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.UserGameRecodeList.UserName>();
    [global::ProtoBuf.ProtoMember(2, Name=@"user_Name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.UserGameRecodeList.UserName> user_Name
    {
      get { return _user_Name; }
    }
  
    private readonly global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.UserGameRecode> _user_game_recode = new global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.UserGameRecode>();
    [global::ProtoBuf.ProtoMember(3, Name=@"user_game_recode", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.UserGameRecode> user_game_recode
    {
      get { return _user_game_recode; }
    }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserName")]
  public partial class UserName : global::ProtoBuf.IExtensible
  {
    public UserName() {}
    
    private string _user_name = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"user_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string user_name
    {
      get { return _user_name; }
      set { _user_name = value; }
    }
    private int _point = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"point", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int point
    {
      get { return _point; }
      set { _point = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GameRecordList")]
  public partial class GameRecordList : global::ProtoBuf.IExtensible
  {
    public GameRecordList() {}
    
    private bool _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool result
    {
      get { return _result; }
      set { _result = value; }
    }
    private readonly global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.GameRecord> _game_record = new global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.GameRecord>();
    [global::ProtoBuf.ProtoMember(2, Name=@"game_record", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.GameRecord> game_record
    {
      get { return _game_record; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"NewsResponse")]
  public partial class NewsResponse : global::ProtoBuf.IExtensible
  {
    public NewsResponse() {}
    
    private string _rule_title = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"rule_title", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string rule_title
    {
      get { return _rule_title; }
      set { _rule_title = value; }
    }
    private string _news_message;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"news_message", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string news_message
    {
      get { return _news_message; }
      set { _news_message = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GameRuleResponse")]
  public partial class GameRuleResponse : global::ProtoBuf.IExtensible
  {
    public GameRuleResponse() {}
    
    private string _rule_title;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"rule_title", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string rule_title
    {
      get { return _rule_title; }
      set { _rule_title = value; }
    }
    private string _rule_message;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"rule_message", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string rule_message
    {
      get { return _rule_message; }
      set { _rule_message = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GameRuleResponseList")]
  public partial class GameRuleResponseList : global::ProtoBuf.IExtensible
  {
    public GameRuleResponseList() {}
    
    private readonly global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.GameRuleResponse> _game_rule_response = new global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.GameRuleResponse>();
    [global::ProtoBuf.ProtoMember(1, Name=@"game_rule_response", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.GameRuleResponse> game_rule_response
    {
      get { return _game_rule_response; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"CustomerResponse")]
  public partial class CustomerResponse : global::ProtoBuf.IExtensible
  {
    public CustomerResponse() {}
    
    private readonly global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.CustomerResponse.Customer> _customer = new global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.CustomerResponse.Customer>();
    [global::ProtoBuf.ProtoMember(1, Name=@"customer", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.CustomerResponse.Customer> customer
    {
      get { return _customer; }
    }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Customer")]
  public partial class Customer : global::ProtoBuf.IExtensible
  {
    public Customer() {}
    
    private string _name = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private string _value = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"value", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string value
    {
      get { return _value; }
      set { _value = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RechargeResponseList")]
  public partial class RechargeResponseList : global::ProtoBuf.IExtensible
  {
    public RechargeResponseList() {}
    
    private readonly global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.RechargeResponseList.RechargeResponse> _recharge_response = new global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.RechargeResponseList.RechargeResponse>();
    [global::ProtoBuf.ProtoMember(1, Name=@"recharge_response", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.RechargeResponseList.RechargeResponse> recharge_response
    {
      get { return _recharge_response; }
    }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RechargeResponse")]
  public partial class RechargeResponse : global::ProtoBuf.IExtensible
  {
    public RechargeResponse() {}
    
    private string _recharge_no = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"recharge_no", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string recharge_no
    {
      get { return _recharge_no; }
      set { _recharge_no = value; }
    }
    private uint _recharge_count = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"recharge_count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint recharge_count
    {
      get { return _recharge_count; }
      set { _recharge_count = value; }
    }
    private uint _recharge_money = default(uint);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"recharge_money", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint recharge_money
    {
      get { return _recharge_money; }
      set { _recharge_money = value; }
    }
    private string _invitation_code = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"invitation_code", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string invitation_code
    {
      get { return _invitation_code; }
      set { _invitation_code = value; }
    }
    private string _recharge_state = "";
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"recharge_state", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string recharge_state
    {
      get { return _recharge_state; }
      set { _recharge_state = value; }
    }
    private string _recharge_time = "";
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"recharge_time", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string recharge_time
    {
      get { return _recharge_time; }
      set { _recharge_time = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RoomList")]
  public partial class RoomList : global::ProtoBuf.IExtensible
  {
    public RoomList() {}
    
    private readonly global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.RoomList.Room> _room = new global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.RoomList.Room>();
    [global::ProtoBuf.ProtoMember(1, Name=@"room", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<com.sansanbbox.protobuf.lobby.RoomList.Room> room
    {
      get { return _room; }
    }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Room")]
  public partial class Room : global::ProtoBuf.IExtensible
  {
    public Room() {}
    
    private uint _room_number = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"room_number", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint room_number
    {
      get { return _room_number; }
      set { _room_number = value; }
    }
    private uint _all_round = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"all_round", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint all_round
    {
      get { return _all_round; }
      set { _all_round = value; }
    }
    private string _all_user = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"all_user", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string all_user
    {
      get { return _all_user; }
      set { _all_user = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}