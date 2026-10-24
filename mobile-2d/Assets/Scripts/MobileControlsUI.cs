using UnityEngine;
using UnityEngine.UI;

public class MobileControlsUI : MonoBehaviour
{
    [SerializeField] private Button attackButton;
    [SerializeField] private Button dodgeButton;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerMovement playerMovement;

    private void Start()
    {
        if (playerCombat == null) playerCombat = FindFirstObjectByType<PlayerCombat>();
        if (playerMovement == null) playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (attackButton != null)
        {
            attackButton.onClick.AddListener(() =>
            {
                if (playerCombat != null) playerCombat.TryAttack();
            });
        }

        if (dodgeButton != null)
        {
            dodgeButton.onClick.AddListener(() =>
            {
                if (playerMovement != null) playerMovement.TryDodge();
            });
        }
    }
}
