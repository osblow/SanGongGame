using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Osblow.Game
{
    public class Cmd
    {
        public static short ClientRegisterRequest = 0x1001;
        public static short ServerRegisterResponse = 0x1002;
        public static short ClientHeartBeatRequest = 0x1003;
        public static short ServerHeartbeatResponse = 0x1004;
        public static short EnterRoomRequest = 0x1005;
        public static short EnterRoomResponse = 0x1006;
        public static short EnterRoomOtherResponse = 0x1007;
        public static short ExitRoomRequest = 0x1008;
        public static short ExitRoomToOtherResponse = 0x1009;
        public static short ExitRoomResultResponse = 0x1010;
        public static short DismissRoomRequest = 0x1011;
        public static short DismissRoomToOtherResponse = 0x1012;
        public static short PlayerVoteDismissRoomRequest = 0x1013;
        public static short PlayerVoteDismissRoomResponse = 0x1014;
        public static short DismissRoomResultResponse = 0x1015;
        public static short ReadyRequest = 0x1016;
        public static short ReadyResponse = 0x1017;
        public static short ReconnectRequest = 0x1018;
        public static short ReconnectResponse = 0x1019;
        public static short OnlineStatusResponse = 0x1020;
        public static short SynchroniseExpressionResponse = 0x1021;
        public static short AudioStreamUpload = 0x1022;
        public static short AudioStreamBroadcast = 0x1023;
        public static short ClientBetRequest = 0x1024;
        public static short ServerBetResponse = 0x1025;
        public static short ServerBankerNotice1Response = 0x1026;
        public static short ServerBankerNotice2Response = 0x1027;
        public static short ClientBetAgainRequest = 0x1028;
        public static short ServerBetAgainResponse = 0x1029;
        public static short ClientBankerRequest = 0x1030;
        public static short ServerBankerResponse = 0x1031;
        public static short ServerCardsResponse = 0x1032;
        public static short SynchroniseCardsResponse = 0x1033;
        public static short WinOrLoseResponse = 0x1034;
        public static short UserAllResult = 0x1035;
    }
}
