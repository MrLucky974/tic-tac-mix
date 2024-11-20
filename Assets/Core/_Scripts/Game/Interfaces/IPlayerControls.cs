using UnityEngine.InputSystem;

namespace RapidPrototyping.TicTacMix
{
    public interface IPlayerControls
    {
        public void OnMovement(InputAction.CallbackContext ctx);
        public void OnPrimary(InputAction.CallbackContext ctx);
    }
}
