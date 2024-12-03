using UnityEngine;

[CreateAssetMenu(fileName = "Speed Boost Mutation", menuName = "Mutations/Speed Boost")]
public class SpeedBoostMutation : Mutation
{
    private void OnEnable()
    {
        mutationName = "Speedy";
        description = "Increases the player's movement speed.";

        applyEffect = (player) => player.moveSpeed += 2f;
        removeEffect = (player) => player.moveSpeed -= 2f;
    }
}
