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
        public Image preview;
        public Image title;
        public Image ready;
    }

   public List<CharacterScriptableObject> characters;
    
    [SerializeField] private float RequiredHeldCancelTime;
    [SerializeField] private float CurrentHeldBCancelTime;

    [SerializeField] private List<PlayerUI> playerInterfaces;

    [Header("Info")]
    public GameObject joinPanel;
    public GameObject startPanel;
    public GameObject startIndicator;
    public GameObject split;
    public Sprite unknownCharacter;
    public Sprite placeholder;

    private PlayerManager playerManager;
    private List<PlayerSelection> playerSelections;
    private bool playersAreReady = false;
    private int readyPlayers = 0;

    [SerializeField] AudioClip ChickAnouncer;
    [SerializeField] AudioClip KonineAnouncer;
    [SerializeField] AudioClip KattoAnouncer;
    [SerializeField] AudioClip FishkomAnouncer;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        playerSelections = new List<PlayerSelection>();

        foreach (PlayerUI playerUI in playerInterfaces)
        {
            playerUI.preview.sprite = unknownCharacter;
            playerUI.title.sprite = placeholder;
            PlayerSelection playerSelection = new PlayerSelection();
            playerSelection.characterIndex = -1;
            playerSelection.isReady = false;
            playerSelections.Add(playerSelection);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Submit"))
        {
            if (playersAreReady)
            {
                CurrentHeldBCancelTime += Time.deltaTime;

                float percentage = CurrentHeldBCancelTime / RequiredHeldCancelTime;

                startIndicator.GetComponent<Image>().fillAmount = percentage;

                if (CurrentHeldBCancelTime >= RequiredHeldCancelTime)
                {
                    List<string> finalSelections = new List<string>(playerSelections.Count);

                    foreach (PlayerSelection selection in playerSelections)
                    {
                        if (selection.characterIndex != -1)
                        {
                            finalSelections.Add(characters[selection.characterIndex].name);
                        }
                        else
                        {
                            finalSelections.Add("none");
                        }
                    }

                    playerManager.SetPlayers(finalSelections);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    return;
                }
            }
        }
        else
        {
            CurrentHeldBCancelTime = 0.0f;

            if (startIndicator.GetComponent<Image>().fillAmount > 0.0f)
            {
                startIndicator.GetComponent<Image>().fillAmount -= Time.deltaTime;
            }
        }

        for (int i = 0; i < playerManager.GetConnectedPlayers(); i++)
        {
            if (!playersAreReady)
            {
                if (Input.GetButtonUp("RB" + (i + 1)))
                {
                    SelectNextCharacter(i);
                }
                else if (Input.GetButtonUp("LB" + (i + 1)))
                {
                    SelectPreviousCharacter(i);
                }
            }

            if (Input.GetButtonUp("Submit" + (i + 1)))
            {
                if (playerSelections[i].characterIndex != -1)
                {
                    playerSelections[i].isReady = true;
                    PlayAnouncerVoice(characters[playerSelections[i].characterIndex].name);
                    if (playerSelections[i].isReady)
                    {
                        readyPlayers++;
                        playerInterfaces[i].ready.gameObject.SetActive(true);
                    }

                    if (readyPlayers == playerManager.GetConnectedPlayers())
                    {
                        playersAreReady = true;
                    }
                }
            }

            if (Input.GetButtonUp("Cancel" + (i + 1)))
            {
                if (playerSelections[i].isReady)
                {
                    readyPlayers--;
                    playerSelections[i].isReady = false;
                    playerInterfaces[i].ready.gameObject.SetActive(false);
                    playersAreReady = false;

                    if (split.activeInHierarchy)
                    {
                        split.SetActive(false);
                    }

                    if (startPanel.activeInHierarchy)
                    {
                        startPanel.SetActive(false);
                    }
                }
            }
        }

        if (playerManager.GetConnectedPlayers() > 0)
        {
            if (playersAreReady)
            {
                if (!split.activeInHierarchy)
                {
                    split.SetActive(true);
                }

                if (!startPanel.activeInHierarchy)
                {
                    startPanel.SetActive(true);
                }
            }

            if (joinPanel.activeInHierarchy)
            {
                joinPanel.SetActive(false);
            }
        }
        else
        {
            if (!joinPanel.activeInHierarchy)
            {
                joinPanel.SetActive(true);
            }

            if (split.activeInHierarchy)
            {
                split.SetActive(false);
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
        Image title = playerInterfaces[playerIndex].title;

        if (playerSelections[playerIndex].characterIndex >= characters.Count - 1)
        {
            playerSelections[playerIndex].characterIndex = 0;
        }
        else
        {
            playerSelections[playerIndex].characterIndex++;
        }
        
        preview.sprite = characters[playerSelections[playerIndex].characterIndex].preview;
        title.sprite = characters[playerSelections[playerIndex].characterIndex].title;

        Debug.Log("Player " + playerIndex + " selected the next character");
    }

    private void SelectPreviousCharacter(int playerIndex)
    {
        Image preview = playerInterfaces[playerIndex].preview;
        Image title = playerInterfaces[playerIndex].title;

        if (playerSelections[playerIndex].characterIndex <= 0)
        {
            playerSelections[playerIndex].characterIndex = characters.Count - 1;
        }
        else
        {
            playerSelections[playerIndex].characterIndex--;
        }

        preview.sprite = characters[playerSelections[playerIndex].characterIndex].preview;
        title.sprite = characters[playerSelections[playerIndex].characterIndex].title;

        Debug.Log("Player " + playerIndex + " selected the previous character");
    }

    void PlayAnouncerVoice(string name)
    {
        switch (name)
        {
            case "Fishy":
                GetComponent<AudioSource>().PlayOneShot(FishkomAnouncer);
                break;
            case "Chick":
                GetComponent<AudioSource>().PlayOneShot(ChickAnouncer);
                break;
            case "Konine":
                GetComponent<AudioSource>().PlayOneShot(KonineAnouncer);
                break;
            case "Catto":
                GetComponent<AudioSource>().PlayOneShot(KattoAnouncer);
                break;
        }
    }


}
