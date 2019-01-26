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

    public List<GameObject> playerCharacters = new List<GameObject>();

    [SerializeField] List<string> playerNames;
    private CameraController cameraController;

    private static PlayerManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
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
    
    public void SetPlayers(List<string> playerSelections)
    {
        playerNames = playerSelections;
    }

    public void SpawnPlayer(int id)
    {
        int randomNumber = Random.Range(0, spawnPoints.Count);
        GameObject newPlayer = Instantiate(playerPrefab, new Vector2(50.0f,50.0f), Quaternion.identity);
        newPlayer.GetComponent<Player>().SetModel(playerNames[id]);
        playerCharacters.Add(newPlayer);
        newPlayer.GetComponent<Player>().playerID = id + 1;
        Debug.Log("Player spawned!");
        cameraController = FindObjectOfType<CameraController>();
        cameraController.FindTargets();
    }

    public int GetConnectedPlayers()
    {
        return connectedControllers;
    }
}
