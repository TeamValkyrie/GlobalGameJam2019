using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private int connectedControllers;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private List<Transform> spawnPoints;

    public static PlayerManager instance;

    private List<GameObject> playerCharacters = new List<GameObject>();
    private CameraController cameraController;

    // Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();

        PollControllers();
    }

    // Update is called once per frame
    void Update()
    {
        PollControllers();
    }

    private void PollControllers()
    {
        string[] controllers = Input.GetJoystickNames();
        int polledControllers = 0;

        if (controllers.Length > 0)
        {
            for (int i = 0; i < controllers.Length; i++)
            {
                if (string.IsNullOrEmpty(controllers[i]) == false)
                {
                    polledControllers++;
                }
            }
        }

        if (polledControllers > connectedControllers)
        {
            connectedControllers = polledControllers;
            Debug.Log("Controlled connected!");
        }
        else if (polledControllers < connectedControllers)
        {
            connectedControllers = polledControllers;
            Debug.Log("Controlled disconnected!");
        }
    }

    private void SpawnPlayers(int id)
    {
        int randomNumber = Random.Range(0, spawnPoints.Count);
        GameObject newPlayer = Instantiate(playerPrefab, spawnPoints[randomNumber].position, Quaternion.identity);
        playerCharacters.Add(newPlayer);
        newPlayer.GetComponent<Player>().playerID = id;
        Debug.Log("Player spawned!");
        cameraController.FindTargets();
    }

    public int GetConnectedPlayers()
    {
        return connectedControllers;
    }
}
