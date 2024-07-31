using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Game.Server
{
    public class ServerManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PhotonView      _pv;
        [SerializeField] private TextMeshProUGUI _systemTMP;

        private StringBuilder _builder = new StringBuilder();

        public void SendGameStart()
        {
            _pv.RPC(nameof(ReceiveGameStart), RpcTarget.All);
        }

        [PunRPC]
        public void ReceiveGameStart()
        {
            GameManager.Instance.GameStart();
        }

        public void SendMessage(string msg)
        {
            _pv.RPC(nameof(ReceiveMessage), RpcTarget.All, msg);
        }


        [PunRPC]
        private void ReceiveMessage(string msg)
        {
            var stringBuilder = _builder.AppendLine(msg);
            _systemTMP.SetText(_builder);
        }
        
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.Log($"{newPlayer.NickName} entered the room.");
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Debug.Log($"{otherPlayer.NickName} left the room.");
        }
    }
}