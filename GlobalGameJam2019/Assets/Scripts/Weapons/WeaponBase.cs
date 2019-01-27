using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("Weapon setup")]
    [SerializeField] private Collider2D[] KillColliders;
    [SerializeField] private Collider2D[] BlockColliders;
    [SerializeField] private Collider2D[] PhyiscalColliders;

    [Header("Weapon properties")]
    public float knockbackForce = 10.0f;
    public bool isHeld = false;
    public Player carrier;
    public bool isFlying = false;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isFlying)
        {
             if (other.transform.tag == "Player") 
             {
                 other.GetComponentInParent<Player>().KillPlayer();
             }
             isFlying = false;
             SetCombatCollidersActive(false);
             SetPhysicalCollidersActive(true);
        }

        if (other.transform.tag == "Weapon")
        {
            var weaponComponent = other.GetComponent<WeaponBase>();
            if(isHeld && weaponComponent.isHeld)
            {
                carrier.KnockBackPlayer(knockbackForce);
                weaponComponent.carrier.KnockBackPlayer(knockbackForce);
                return;
            }
        }

        if (carrier != null)
        {
            if (other.transform.tag == "Player" && other.gameObject != carrier.gameObject)
            {
                if (!other.GetComponentInParent<Player>().isDead)
                {
                    other.GetComponentInParent<Player>().KillPlayer();
                }
            }
        }
    }
}