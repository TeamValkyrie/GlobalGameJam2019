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
        if (other.transform.tag == "Weapon")
        {
            var weaponComponent = other.GetComponent<WeaponBase>();

            weaponComponent.GetComponent<Rigidbody2D>().AddForce((weaponComponent.gameObject.transform.position - transform.position) * knockbackForce, ForceMode2D.Impulse);
            print("Colliding with weapon");
            //if(weaponComponent.isHeld)
        }
    }
}