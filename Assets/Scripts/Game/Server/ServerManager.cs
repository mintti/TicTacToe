using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Game.Server
{
    public class ServerManager : MonoBehaviour
    {
        [SerializeField] private PhotonView      _pv;
        [SerializeField] private TextMeshProUGUI _systemTMP;

        private StringBuilder _builder = new StringBuilder();
        

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
    }
}