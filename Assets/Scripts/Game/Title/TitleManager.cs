using Game.Title;
using UnityEngine;

namespace Game
{
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] private MultiRoomManager _multiRoomManager;

        public void Start()
        {
            Init();
        }

        private void Init()
        {
            _multiRoomManager.Init();
        }
    }
}
