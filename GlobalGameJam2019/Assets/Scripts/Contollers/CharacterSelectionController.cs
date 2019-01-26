using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionController : MonoBehaviour
{

    [SerializeField] private string StartupSceneName;
    [SerializeField] private float RequiredHeldCancelTime;
    [SerializeField] private float CurrentHeldBCancelTime;

    // Start is called before the first frame update
    void Start()
    {
        if(StartupSceneName == "")
        {
            Debug.Log("[WARNING] - [CharacterSelectionController] - StartupSceneName not assiged");
        }
    }

    // Update is called once per frame
    void Update()
    {

        

        ProcessExit();
    }

    private void ProcessExit()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel"))
        {
            //Play Cancel Audio?
        }

        if(Input.GetKey(KeyCode.Escape) || Input.GetButton("Cancel"))
        {
            CurrentHeldBCancelTime += Time.deltaTime;
            if(CurrentHeldBCancelTime >= RequiredHeldCancelTime)
            {
                SceneManager.LoadScene(StartupSceneName);
            }
        }
        else
        {
            CurrentHeldBCancelTime = 0.0f;
        }

    }
}
