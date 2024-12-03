using UnityEngine;

[CreateAssetMenu(fileName = "Speed Decrease Mutation", menuName = "Mutations/Speed Decrease")]
public class SpeedDecreaseMutation : Mutation
{
    private void OnEnable()
    {
        mutationName = "Slow";
        description = "Decrease the player's movement speed.";

        applyEffect = (player) => player.moveSpeed -= 2f;
        removeEffect = (player) => player.moveSpeed += 2f;
    }
}
