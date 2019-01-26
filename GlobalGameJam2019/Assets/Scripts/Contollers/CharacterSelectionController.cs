using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionController : MonoBehaviour
{
    [System.Serializable]
    struct PlayerUI
    {
        public List<GameObject> targets;
        public Color color;
    }

    [SerializeField] private string StartupSceneName;
    [SerializeField] private float RequiredHeldCancelTime;
    [SerializeField] private float CurrentHeldBCancelTime;

    [SerializeField] private List<PlayerUI> PlayerInterfaces;

    [Header("Info")]
    public GameObject joinPanel;
    public GameObject startPanel;
    public GameObject startIndicator;

    private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        if(StartupSceneName == "")
        {
            Debug.Log("[WARNING] - [CharacterSelectionController] - StartupSceneName not assiged");
        }

        playerManager = FindObjectOfType<PlayerManager>();

        foreach (PlayerUI playerUI in PlayerInterfaces)
        {
            foreach (GameObject targetUI in playerUI.targets)
            {
                if (targetUI.GetComponent<Image>())
                {
                    targetUI.GetComponent<Image>().color = Color.grey;
                }

                if (targetUI.GetComponent<Text>())
                {
                    targetUI.GetComponent<Text>().color = Color.grey;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Submit"))
        {
            CurrentHeldBCancelTime += Time.deltaTime;

            float percentage = CurrentHeldBCancelTime / RequiredHeldCancelTime;

            startIndicator.GetComponent<Image>().fillAmount = percentage;

            if (CurrentHeldBCancelTime >= RequiredHeldCancelTime)
            {
                SceneManager.LoadScene(StartupSceneName);
            }
        }
        else
        {
            CurrentHeldBCancelTime = 0.0f;
        }

        for (int i = 0; i < playerManager.GetConnectedPlayers(); i++)
        {
            foreach (GameObject targetUI in PlayerInterfaces[i].targets)
            {
                if (targetUI.GetComponent<Image>())
                {
                    targetUI.GetComponent<Image>().color = PlayerInterfaces[i].color;
                }

                if (targetUI.GetComponent<Text>())
                {
                    targetUI.GetComponent<Text>().color = PlayerInterfaces[i].color;
                }
            }
        }

        if (playerManager.GetConnectedPlayers() > 0)
        {
            joinPanel.SetActive(false);
            startPanel.SetActive(true);
        }
        else
        {
            if (!joinPanel.activeInHierarchy)
            {
                joinPanel.SetActive(true);
            }

            if (startPanel.activeInHierarchy)
            {
                startPanel.SetActive(false);
            }
        }
    }
}
