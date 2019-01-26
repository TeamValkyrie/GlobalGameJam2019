using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionController : MonoBehaviour
{
    [System.Serializable]
    public struct PlayerUI
    {
        public List<GameObject> targets;
        public Image preview;
        public Text characterName;
        public Color color;
    }

    [SerializeField] private List<CharacterScriptableObject> characters;

    [SerializeField] private string StartupSceneName;
    [SerializeField] private float RequiredHeldCancelTime;
    [SerializeField] private float CurrentHeldBCancelTime;

    [SerializeField] private List<PlayerUI> playerInterfaces;

    [Header("Info")]
    public GameObject joinPanel;
    public GameObject startPanel;
    public GameObject startIndicator;
    public Sprite unknownCharacter;

    private PlayerManager playerManager;
    private List<int> playerCharacterIndex;

    // Start is called before the first frame update
    void Start()
    {
        if(StartupSceneName == "")
        {
            Debug.Log("[WARNING] - [CharacterSelectionController] - StartupSceneName not assiged");
        }

        playerManager = FindObjectOfType<PlayerManager>();
        playerCharacterIndex = new List<int>();
        
        foreach (PlayerUI playerUI in playerInterfaces)
        {
            playerUI.preview.sprite = unknownCharacter;
            playerUI.characterName.text = "Unknown";
            playerCharacterIndex.Add(0);

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
            foreach (GameObject targetUI in playerInterfaces[i].targets)
            {
                if (targetUI.GetComponent<Image>())
                {
                    targetUI.GetComponent<Image>().color = playerInterfaces[i].color;
                }

                if (targetUI.GetComponent<Text>())
                {
                    targetUI.GetComponent<Text>().color = playerInterfaces[i].color;
                }
            }

            if (Input.GetAxis("Horizontal" + (i + 1)) > 0)
            {
                SelectNextCharacter(i);
            }
            else if (Input.GetAxis("Horizontal" + (i + 1)) < 0)
            {
                SelectPreviousCharacter(i);
            }

            Image preview = playerInterfaces[i].preview;
            Text name = playerInterfaces[i].characterName;
            preview.sprite = characters[playerCharacterIndex[i]].preview;
            name.text = characters[playerCharacterIndex[i]].name;
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

    private void SelectNextCharacter(int playerIndex)
    {
        Image preview = playerInterfaces[playerIndex].preview;
        Text name = playerInterfaces[playerIndex].characterName;

        if (playerCharacterIndex[playerIndex] >= characters.Count - 1)
        {
            playerCharacterIndex[playerIndex] = 0;
        }
        else
        {
            playerCharacterIndex[playerIndex]++;
        }

        preview.sprite = characters[playerCharacterIndex[playerIndex]].preview;
        name.text = characters[playerCharacterIndex[playerIndex]].name;

        Debug.Log("Player " + playerIndex + " selected the next character");
    }

    private void SelectPreviousCharacter(int playerIndex)
    {
        Image preview = playerInterfaces[playerIndex].preview;
        Text name = playerInterfaces[playerIndex].characterName;

        if (playerCharacterIndex[playerIndex] <= 0)
        {
            playerCharacterIndex[playerIndex] = characters.Count - 1;
        }
        else
        {
            playerCharacterIndex[playerIndex]--;
        }

        preview.sprite = characters[playerCharacterIndex[playerIndex]].preview;
        name.text = characters[playerCharacterIndex[playerIndex]].name;

        Debug.Log("Player " + playerIndex + " selected the previous character");
    }
}
