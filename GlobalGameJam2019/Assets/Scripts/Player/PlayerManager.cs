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

        spawnPoints = new List<Transform>();

        PollControllers();
    }

    // Update is called once per frame
    void Update()
    {
        PollControllers();
    }

    public void FindSpawnPoints()
    {
        GameObject[] foundSpawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawnPoint");

        foreach(GameObject foundSpawnPoint in foundSpawnPoints)
        {
            spawnPoints.Add(foundSpawnPoint.transform);
        }

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
        int randomNumber = Random.Range(0, spawnPoints.Count - 1);
        GameObject newPlayer = Instantiate(playerPrefab, spawnPoints[randomNumber].position, Quaternion.identity);
        spawnPoints.RemoveAt(randomNumber);
        newPlayer.GetComponent<Player>().SetModel(playerNames[id]);
        playerCharacters.Add(newPlayer);
        newPlayer.GetComponent<Player>().playerID = id + 1;
        newPlayer.transform.position = new Vector3(newPlayer.transform.position.x, newPlayer.transform.position.y, 0.0f);
        Debug.Log("Player spawned!");
    }

    public int GetConnectedPlayers()
    {
        return connectedControllers;
    }
}
