using UnityEngine;

[CreateAssetMenu(fileName = "Leap Decrease Mutation", menuName = "Mutations/Leap Decrease")]
public class LeapDecreaseMutation : Mutation
{
    private void OnEnable()
    {
        mutationName = "Weak legs";
        description = "Decrease the player's leap force.";

        applyEffect = (player) => player.leapForce /= 1.5f;
        removeEffect = (player) => player.leapForce *= 1.5f;
    }
}