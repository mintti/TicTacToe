using UnityEngine;

namespace Game.Controller
{
    public class DeathZone : MonoBehaviour
    {
        [SerializeField] private PlayerInfo _playerInfo;
        public PlayerInfo PlayerInfo => _playerInfo;
    }
}
