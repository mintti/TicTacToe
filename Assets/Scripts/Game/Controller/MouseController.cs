using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Controller
{
    public class MouseController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IController
    {
        private MovementHandler _movementHandler;
        private Vector2        _startPos;

        public void Init(MovementHandler movementHandler)
        {
            _movementHandler = movementHandler;
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
            _movementHandler.SendData((eInput));
        }
    }
}

