using UnityEngine.InputSystem;

namespace RapidPrototyping.TicTacMix
{
    public interface IPlayerPrimaryControls
    {
        public void OnPrimary(InputAction.CallbackContext ctx);
    }
}