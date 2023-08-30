using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Connectivity.Mapping;
using Project.Scripts.Connectivity.Models.AggregationClasses;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;

namespace Project.Scripts.Connectivity.Http.Requests
{
    public class ServerInvoker : MonoBehaviour
    {
        public static ServerInvoker Invoker;

        private const int Timeout = 1;
        private HttpClientWrapper http;
        private WebDataStorage storage;
        private RobotsMapper robotsMapper;
        private ConfiguredRobotsMapper configuredRobotsMapper;
        private StickersMapper stickersMapper;

        private void Start()
        {
            http = HttpClientWrapper.Instance;
            storage = WebDataStorage.Instance;
            robotsMapper = RobotsMapper.Instance;
            configuredRobotsMapper = ConfiguredRobotsMapper.Instance;
            stickersMapper = StickersMapper.Instance;
        }

        private void Awake()
        {
            Invoker = this;
        }

        public void GetFullData()
        {
            StartCoroutine(GetRobots());
            StartCoroutine(GetConfiguredRobots());
            StartCoroutine(GetStickers());
        }
        
        public IEnumerator GetRobots()
        {
            var newRobotsTask = http.ExecuteRequest(new GetSavedRobotsRequest());
            while (!newRobotsTask.IsCompleted)
            {
                yield return null;
            }

            storage.Robots = newRobotsTask.Result;
            yield return null;
        }

        public IEnumerator GetConfiguredRobots()
        {
            var newConfiguredRobotsTask = http.ExecuteRequest(new GetRobotConfigDataRequest());

            while (!newConfiguredRobotsTask.IsCompleted)
            {
                yield return null;
            }

            var configured = newConfiguredRobotsTask.Result;
            storage.ConfiguredRobots = configuredRobotsMapper.MapToConfiguredRobots(configured);
            storage.CategoryNames = configuredRobotsMapper.MapStringsToUniqueNames(storage.ConfiguredRobots);
            yield return null;
        }

        public IEnumerator GetStickers()
        {
            var newStickersTask = http.ExecuteRequest(new GetTargetImagesRequest());

            while (!newStickersTask.IsCompleted)
            {
                yield return null;
            }

            var stickers = newStickersTask.Result;
            storage.Stickers = stickersMapper.MapBytesToSprite(stickers);
            storage.AvailableIps = robotsMapper.MapStringToIpAddress(stickers);
            yield return null;
        }

        public IEnumerator<ConnectionStatus> PingRobot(string ip)
        {
            yield return http.ExecuteRequest(new PingChosenIpRequest(ip)).Result;
        }

        public IEnumerator PostRobot(Robot? robot)
        {
            if (robot != null) http.ExecuteRequest(new PostNewRobotRequest(robot.Value));
            yield return null;
        }
    }
}
