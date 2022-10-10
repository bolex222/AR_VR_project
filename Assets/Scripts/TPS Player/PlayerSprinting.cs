using TPS_Player;
using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(Player))]
    public class PlayerSprinting : MonoBehaviour
    {

        [SerializeField] float speedMultiplier = 2f;

        Player m_Player;
        private InputManager m_InputManager;

        void Awake()
        {
            m_InputManager = InputManager.Instance;
            m_Player = GetComponent<Player>();
        }

        void OnEnable() => m_Player.OnBeforeMove += OnBeforeMove;
        void OnDisable() => m_Player.OnBeforeMove -= OnBeforeMove;

        void OnBeforeMove() {
            float sprinInput = m_InputManager.GetShiftValue();
            m_Player.MovmentSpeedMultiplier *= sprinInput > 0 ? speedMultiplier : 1f;
        }
    }
}
