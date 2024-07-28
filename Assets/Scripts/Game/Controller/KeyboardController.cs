using UnityEngine;

namespace Game.Controller
{
    public class KeyboardController : MonoBehaviour, IController
    {
        private PlayerMovement _movement;
        public void Init(PlayerMovement movement)
        {
            _movement = movement;
        }
        
        private void Update()
        {
            HandleKeyboardInput();
        }

        /// <summary>
        /// 키보드 입력 처리를 수행하는 함수
        /// </summary>
        private void HandleKeyboardInput()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                SendInputInfo(EMoveType.Left);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                SendInputInfo(EMoveType.Right);
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                SendInputInfo(EMoveType.None);
            }
        }

        private void SendInputInfo(EMoveType eType)
        {
            _movement.UpdateMovePos(eType);
        }
    }
}