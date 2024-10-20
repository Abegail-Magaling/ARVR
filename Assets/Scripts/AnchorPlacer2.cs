using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class AnchorPlacer : MonoBehaviour
{
    private ARAnchorManager arAnchorManager;
    [SerializeField] private float offset = 0f;

    [Header("UI Elemets")]
    [SerializeField] private Text sliderText;
    [SerializeField] private Toggle pointCloudToggle;
    [SerializeField] private Button clearAllButton;
    [SerializeField] private Button[] prefabButtons;
    [SerializeField] private Slider distanceSlider;

    [Header("Game Objects")]
    [SerializeField] private GameObject[] anchorPrefabs;
    [SerializeField] private GameObject pointCloud;

    private List<GameObject> anchors = new List<GameObject>();
    private GameObject currentPrefab;

// Start is called before the first frame update
    private void Start()
    {
        arAnchorManager = GetComponent<ARAnchorManager>();
        currentPrefab = anchorPrefabs[0];

        UpdateSliderText(distanceSlider.value);

        InitializeListeners();

        for (int i = 0; i < prefabButtons.Length; i++)
        {
            int index = i;
            prefabButtons[i].onClick.AddListener(() => SelectPrefab(index));
        }
    }

    private void InitializeListeners()
    {
        pointCloudToggle.onValueChanged.AddListener(TogglePointCloud);
        clearAllButton.onClick.AddListener(RemoveAllAnchors);
        distanceSlider.onValueChanged.AddListener(ChangeSpawnDistance);
    }

   // Update is called once per frame
    private void Update()
    {
        UpdateSliderText(distanceSlider.value);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 touchPosition = Camera.main.ScreenPointToRay(Input.GetTouch(0).position).GetPoint(offset);
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                GameObject hitObject = hitInfo.collider.gameObject;
                if (hitObject.CompareTag("AnchoredObject"))
                {
                    RemoveAnchor(hitObject);
                    return;
                }
            }

            PlaceAnchor(touchPosition);
        }
    }

    private void PlaceAnchor(Vector3 position)
    {
        GameObject newAnchor = new GameObject("AR_Anchor");
        newAnchor.transform.position = position;
        newAnchor.AddComponent<ARAnchor>();

        GameObject instantiatedObject = Instantiate(currentPrefab, newAnchor.transform);
        instantiatedObject.transform.localPosition = Vector3.zero;
        instantiatedObject.tag = "AnchoredObject";

        anchors.Add(newAnchor);
    }

    private void RemoveAnchor(GameObject anchor)
    {
        anchors.Remove(anchor);
        Destroy(anchor);
    }

    private void RemoveAllAnchors()
    {
        foreach (GameObject anchor in anchors)
        {
            Destroy(anchor);
        }
        anchors.Clear();
    }

    private void ChangeSpawnDistance(float distance)
    {
        offset = distance;
    }

    private void TogglePointCloud(bool isActive)
    {
        pointCloud.SetActive(isActive);
    }

    private void SelectPrefab(int index)
    {
        currentPrefab = anchorPrefabs[index];
        for (int i = 0; i <prefabButtons.Length; i++)
        {
            prefabButtons[i].interactable = i != index;
        }
    }

    private void UpdateSliderText(float value)
    {
        sliderText.text = value.ToString("F2");
    }
}
