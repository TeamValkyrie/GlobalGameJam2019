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
    
    private List<GameObject> playerCharacters = new List<GameObject>();
 
    // Start is called before the first frame update
    void Start()
    {
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
            int delta = polledControllers - connectedControllers;

            for (int i = 0; i < delta; i++)
            {
                SpawnPlayers(connectedControllers + i);
            }

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
    }
}
