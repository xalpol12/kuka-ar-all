using System;
using UnityEngine;

namespace Project.Scripts.EventSystem.DebugEventSystem
{
    public class ConnectionTestEvents : MonoBehaviour
    {
        public static ConnectionTestEvents Current;

        private void Awake()
        {
            Current = this;
        }

        public event Action OnPressButtonConnectToServer;
        public event Action OnPressButtonConnectToFirstRobot;
        public event Action OnPressButtonConnectToSecondRobot;
        public event Action OnPressButtonConnectToThirdRobot;

        public void StartConnectionToServer()
        {
            OnPressButtonConnectToServer?.Invoke();
        }

        public void StartConnectionFirstRobot()
        {
            OnPressButtonConnectToFirstRobot?.Invoke();
        }
        
        public void StartConnectionSecondRobot()
        {
            OnPressButtonConnectToSecondRobot?.Invoke();
        }
        
        public void StartConnectionThirdRobot()
        {
            OnPressButtonConnectToThirdRobot?.Invoke();
        }
    }
}