using UnityEngine;

public class MoreOptionsBehavior : MonoBehaviour
{
    private MoreOptionsController controller;
    private void Start()
    {
        controller = GetComponent<MoreOptionsController>();

        controller.InternetText.SetActive(false);
        
        controller.BugReportButton.onClick.AddListener(() =>
        {
            var internet = Application.internetReachability == NetworkReachability.NotReachable;
            controller.HttpService.HasInternet = internet;
            controller.InternetText.SetActive(internet);
        });
    }
}
