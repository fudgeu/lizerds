using UnityEngine;

[CreateAssetMenu(fileName = "Mass Mutation", menuName = "Mutations/Change Mass")]
public class MassMutation : Mutation
{
    private void OnEnable()
    {
        mutationName = "Dense";
        description = "Increases the player's mass.";

        applyEffect = (player) =>
        {
            if (player.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.mass *= 2f; // Example: double the mass
            }
        };

        removeEffect = (player) =>
        {
            if (player.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.mass /= 2f; // Revert mass back to original
            }
        };
    }
}
