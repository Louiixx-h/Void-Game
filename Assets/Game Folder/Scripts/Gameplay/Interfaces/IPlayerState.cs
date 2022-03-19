
using UnityEngine;

namespace Devflowbr.GameMecanics.States
{
    public interface IPlayerState
    {
        public void Idle();
        public void Run(Vector2 direction);
        public void Jump();
        public void Dead();
    }
}