using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    [Tooltip("Tag to to find targets")]
    [SerializeField]
    private string targetTag = "Player";

    [Tooltip("Offset from the targets")]
    [SerializeField]
    private Vector3 offset;

    [Tooltip("Approximately the time it will take to reach the target. A smaller value will reach the target faster.")]
    [SerializeField]
    private float smoothTime = 0.5f;

    [Tooltip("Minimum FOV when zooming")]
    [SerializeField]
    private float minZoom = 40.0f;

    [Tooltip("Maximum FOV when zooming")]
    [SerializeField]
    private float maxZoom = 10.0f;

    [Tooltip("Limiting value when zooming")]
    [SerializeField]
    private float zoomLimiter = 5.0f;

    [SerializeField]
    private bool enableZooming = true;

    private Vector3 velocity;
    private Vector3 targetPosition;
    private List<Transform> targets;
    private Camera mainCamera;

    private float previewTime = 4.0f;
    private float currentPreviewTime = 0.0f;
    private int currentPreviewIndex = 0;
    private bool isInPreview;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogWarning("No camera found!");
        }

        targets = new List<Transform>();
        FindTargets();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // LateUpdate is called every frame, if the Behaviour is enabled
    void LateUpdate()
    {
        if (!isInPreview)
        {
            if (targets.Count < 1)
            {
                return;
            }

            targetPosition = GetCenterPoint();
            Move();

            if (enableZooming)
            {
                Zoom();
            }
        }
        else
        {
            currentPreviewTime += Time.deltaTime;
            targetPosition = targets[currentPreviewIndex].position;

            Move();
            Zoom();

            if(currentPreviewTime >= previewTime)
            {
                currentPreviewIndex++;
                currentPreviewTime = 0;
                if (currentPreviewIndex > targets.Count - 1)
                {
                    isInPreview = false;
                    currentPreviewIndex--;
                }
            }
        }
    }

    // Attempt to find targets with the right tag
    public void FindTargets()
    {
        GameObject[] foundTargets = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject foundTarget in foundTargets)
        {
            if (foundTarget == null || targets.Contains(foundTarget.transform))
                continue;

            targets.Add(foundTarget.transform);
        }
    }

    // Change the FOV of the camera to make it zoom in or out
    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, newZoom, Time.deltaTime);
    }

    // Find greatest possible distance between all players
    private float GetGreatestDistance()
    {
        if (!isInPreview)
        {
            var bounds = new Bounds(targets[0].position, Vector3.zero);

            for (int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }

            return bounds.size.x;
        }
        else
        {
            var bounds = new Bounds(targetPosition, Vector3.zero);
            bounds.Encapsulate(targetPosition);

            return bounds.size.x;
        }
    }

    // Smoothly move the camera the the center of all players
    private void Move()
    {
        Vector3 newPosition = targetPosition + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    // Calculate the center point between all players
    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

    public void PreviewPlayer(float Time)
    {
        previewTime = Time;
        currentPreviewIndex = 0;
        currentPreviewTime = 0;
        isInPreview = true;
    }
}
