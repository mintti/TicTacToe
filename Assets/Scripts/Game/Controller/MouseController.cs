using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Controller
{
    public class MouseController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IController
    {
        private PlayerMovement _movement;
        private Vector2        _startPos;

        public void Init(PlayerMovement movement)
        {
            _movement = movement;
        }
    
        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPos = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _startPos = Vector2.zero;
            SendPos(_startPos);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var pos = eventData.position;
            SendPos(_startPos - pos);
        }

        private void SendPos(Vector3 movePos)
        {
            var posX = movePos.x;
            var eInput = posX == 0 ? EMoveType.None : (posX < 0 ? EMoveType.Right : EMoveType.Left);
            _movement.UpdateMovePos(eInput);
        }
    }
}

