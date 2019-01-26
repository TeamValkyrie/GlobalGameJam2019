using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XboxButtonData", menuName = "UI/Xbox/Button", order = 1)]
public class XboxButtonObject : ScriptableObject
{
    public Color color = Color.white;
    public string button;
}
