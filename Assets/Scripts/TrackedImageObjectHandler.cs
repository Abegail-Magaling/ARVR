using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using NUnit.Framework.Constraints;
using UnityEngine.XR.ARSubsystems;
public class TrackedImageObjectHandler : MonoBehaviour
{

    [SerializeField] private GameObject prefab;

    private float yOffset = 0.05f;
    private ARTrackedImageManager _arTrackedImageManager;
    private Dictionary<string, GameObject> _arObject;

    private void Awake()
    {
        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        //_arObject = new Dictionary<string, GameObject>();
    }
    private void Start()
    {
        _arTrackedImageManager.trackedImagesChanged += OnTrackedImageChanged;

        //GameObject newARObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        //newARObject.name = prefab.name;
        //newARObject.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _arTrackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
    }

    public void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            GameObject pinkCube = GameObject.Instantiate(prefab);

            pinkCube.transform.parent = trackedImage.gameObject.transform;

            pinkCube.transform.localPosition = new Vector3(0, yOffset, 0);
           // UpdateTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            //UpdateTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            //_arObject[trackedImage.referenceImage.name].gameObject.SetActive(false);
        }
    }

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        if(trackedImage.trackingState is TrackingState.Limited or TrackingState.None)
        {
            //_arObject[trackedImage.referenceImage.name].gameObject.SetActive(false);

        }

        if(prefab != null)
        {
            //_arObject[trackedImage.referenceImage.name].gameObject.SetActive(true);
            _arObject[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        }
    }
}
