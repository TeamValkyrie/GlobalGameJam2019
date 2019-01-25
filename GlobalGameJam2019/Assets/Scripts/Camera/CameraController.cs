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

    private Vector3 velocity;
    private List<Transform> targets;
    private Camera mainCamera;

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
        if (targets.Count < 1)
        {
            return;
        }

        Move();
        Zoom();
    }

    // Attempt to find targets with the right tag
    private void FindTargets()
    {
        GameObject[] foundTargets = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject foundTarget in foundTargets)
        {
            if (targets.Contains(foundTarget.transform) == false)
            {
                targets.Add(foundTarget.transform);
            }
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
        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }

    // Smoothly move the camera the the center of all players
    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
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
}
