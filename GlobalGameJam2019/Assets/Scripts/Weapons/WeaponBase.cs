using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("Weapon setup")]
    [SerializeField] private Collider2D[] KillColliders;
    [SerializeField] private Collider2D[] BlockColliders;
    [SerializeField] private Collider2D[] PhyiscalColliders;

    [Header("Weapon properties")]
    public bool isHeld = false;

    private void Start()
    {
        SetCombatCollidersActive(false);
        SetPhysicalCollidersActive(true);
    }

    public void SetCombatCollidersActive(bool newState)
    {
        for (int i = 0; i < KillColliders.Length; i++)
        {
            KillColliders[i].enabled = newState;
        }

        for (int i = 0; i < KillColliders.Length; i++)
        {
            BlockColliders[i].enabled = newState;
        }
    }

    public void SetPhysicalCollidersActive(bool newState)
    {
        for (int i = 0; i < PhyiscalColliders.Length; i++)
        {
            PhyiscalColliders[i].enabled = newState;
        }
    }
}