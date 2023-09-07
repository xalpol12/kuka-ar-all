using UnityEngine;

namespace Project.Scripts.Connectivity.Extensions
{
    public class NotificationAssetWatcher : MonoBehaviour
    {
        public static NotificationAssetWatcher Watcher;
        internal Sprite Wifi;
        internal Sprite NoWifi;
        internal Sprite AddedSuccess;
        internal Sprite AddedFailed;
        private void Awake()
        {
            Watcher = this;
        }

        private void Start()
        {
            Wifi = Resources.Load<Sprite>("Icons/wifi");
            NoWifi = Resources.Load<Sprite>("Icons/noWifi");
            AddedSuccess = Resources.Load<Sprite>("Icons/success");
            AddedFailed = Resources.Load<Sprite>("Icons/fail");
        }
    }
}