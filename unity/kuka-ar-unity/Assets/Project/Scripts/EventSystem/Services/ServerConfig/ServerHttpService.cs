using System.Collections;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;
using UnityEngine.UI;

public class ServerHttpService : MonoBehaviour
{
    public static ServerHttpService Instance;
    [SerializeField] private GameObject cloudComponent;
    [SerializeField] private int timeout;
    private Sprite pingSuccessIcon;
    private Sprite pingWaitIcon;
    private Sprite pingFailedIcon;
    private Image cloudIcon;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cloudIcon = cloudComponent.GetComponent<Image>();
        pingSuccessIcon = Resources.Load<Sprite>("Icons/cloudSuccessIcon");
        pingWaitIcon = Resources.Load<Sprite>("Icons/cloudWaiting");
        pingFailedIcon = Resources.Load<Sprite>("Icons/cloudFailedIcon");
        
        timeout /= 1000;
    }

    internal IEnumerator PingOperation(string ip)
    {
        var ping = new Ping(ip);
        var time = 0;
        while (!ping.isDone)
        {
            if (time > timeout)
            {
                break;
            }

            time -= ping.time;
            SwapCloud(ConnectionStatus.Connecting);
            yield return new WaitForSeconds(0.05f);
        }
        
        SwapCloud(time > timeout ? ConnectionStatus.Disconnected : ConnectionStatus.Connected);
    }

    private void SwapCloud(ConnectionStatus pingStatus)
    {
        switch (pingStatus)
        {
            case ConnectionStatus.Connected:
                cloudIcon.sprite = pingSuccessIcon;
                break;
            case ConnectionStatus.Connecting:
                cloudIcon.sprite = pingWaitIcon;
                break;
            case ConnectionStatus.Disconnected:
                cloudIcon.sprite = pingFailedIcon;
                break;
        }
    }
}
