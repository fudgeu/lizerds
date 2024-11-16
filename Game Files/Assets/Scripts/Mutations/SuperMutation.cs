using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Super Mutation", menuName = "Mutations/Super Mutation")]
public class SuperMutation : Mutation
{
    private void OnEnable()
    {
        mutationName = "Super Mutation";
        description = "Boosts leap force and speed.";

        applyEffect = (player) =>
        {
            player.leapForce *= 2f;
            player.moveSpeed += 3f;
        };
        removeEffect = (player) =>
        {
            player.leapForce /= 2f;
            player.moveSpeed -= 3f;
        };
    }
}
