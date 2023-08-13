using System.Collections.Generic;
using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageRecognizer : MonoBehaviour
{
    private AnchorManager anchorManager;
    private ARTrackedImageManager trackedImageManager;
    private Dictionary<string, ARTrackedImage> trackedImages;

    private void Awake()
    {
        anchorManager = gameObject.GetComponent<AnchorManager>();
        trackedImageManager = gameObject.GetComponent<ARTrackedImageManager>();
    }

    private void Start()
    {
        trackedImages = new Dictionary<string, ARTrackedImage>();
        trackedImageManager.trackedImagesChanged += OnChange;
    }

    private void OnChange(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            trackedImages.Add(newImage);
            StartCoroutine(anchorManager.CreateAnchor(imageName, newImage));
            DebugLogger.Instance().AddLog($"Current tracked images count: {trackedImages.Count.ToString()} ");
        }
    }
}