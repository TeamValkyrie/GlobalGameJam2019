using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CharacterData", menuName = "UI/Character", order = 1)]
public class CharacterScriptableObject : ScriptableObject
{
    public Sprite preview;
    public Sprite title;
}
