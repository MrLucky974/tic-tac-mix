using UnityEngine.InputSystem;

namespace RapidPrototyping.TicTacMix
{
    public interface IPlayerMovementControls
    {
        public void OnMovement(InputAction.CallbackContext ctx);
    }
}