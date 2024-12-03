using UnityEngine;

[CreateAssetMenu(fileName = "Decrease Mass Mutation", menuName = "Mutations/Decrease Mass")]
public class DecMassMutation : Mutation
{
    private void OnEnable()
    {
        mutationName = "Light";
        description = "Decreases the player's mass.";

        applyEffect = (player) =>
        {
            if (player.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.mass /= 2f; // Example: double the mass
            }
        };

        removeEffect = (player) =>
        {
            if (player.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.mass *= 2f; // Revert mass back to original
            }
        };
    }
}
