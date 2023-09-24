using System.Globalization;
using Project.Scripts.Connectivity.Models.KRLValues;
using Project.Scripts.Connectivity.WebSocket;
using Project.Scripts.TrackedRobots;
using UnityEngine;

namespace Project.Scripts.EventSystem.DebugEventSystem
{
    public class JogsTestController : MonoBehaviour
    {
        [SerializeField] private GameObject robotHandler;
        private TrackedRobotsHandler handler;
        
        private bool ClientConnected { get; set; }
        private bool FirstRobotConnected { get; set; }

        private const string FirstIp = "192.168.1.50";
        private const string SecondIp = "192.168.1.51";

        private string currentlyTrackedRobot;
        
        private void Start()
        {
            handler = robotHandler.GetComponent<TrackedRobotsHandler>();

            currentlyTrackedRobot = FirstIp;
            
            ClientConnected = false;

            JogsTestEvents.Current.OnPressButtonConnectToServer += ConnectToWebSocketServer;
            JogsTestEvents.Current.OnPressButtonConnectToTwoRobots += InitializeConnectionToRobots;
            JogsTestEvents.Current.OnPressButtonSwitchIP += SwitchCurrentlyTrackedVariable;
            handler.ActiveJointsUpdated += LogUpdate;
        }
        
        private void ConnectToWebSocketServer()
        {
            if (ClientConnected) return;
            ClientConnected = true;
            WebSocketClient.Instance.ConnectToWebsocket("ws://192.168.18.20:8080/kuka-variables");
        }
        
        private void InitializeConnectionToRobots()
        {
            if (FirstRobotConnected) return;
            FirstRobotConnected = true;
            WebSocketClient.Instance.SendToWebSocketServer("{ \"host\": \"192.168.1.50\", \"var\": \"JOINTS\" }");
            WebSocketClient.Instance.SendToWebSocketServer("{ \"host\": \"192.168.1.51\", \"var\": \"JOINTS\" }");
        }

        private void SwitchCurrentlyTrackedVariable()
        {
            handler.ChangeSelectedRobotIP(currentlyTrackedRobot);
            currentlyTrackedRobot = currentlyTrackedRobot == FirstIp ? SecondIp : FirstIp;
        }

        private void LogUpdate(object sender, KRLJoints joints)
        {
            Debug.Log(currentlyTrackedRobot + " : " + joints.J1.ToString(CultureInfo.InvariantCulture));
        }
    }
}
