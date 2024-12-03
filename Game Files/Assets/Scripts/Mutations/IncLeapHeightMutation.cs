using UnityEngine;

[CreateAssetMenu(fileName = "Leap Boost Mutation", menuName = "Mutations/Leap Boost")]
public class LeapBoostMutation : Mutation
{
    private void OnEnable()
    {
        mutationName = "Strong Legs";
        description = "Increases the player's leap force.";

        applyEffect = (player) => player.leapForce *= 1.5f;
        removeEffect = (player) => player.leapForce /= 1.5f;
    }
}