using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Knockback Mutation", menuName = "Mutations/Knockback")]
public class DamageMutation : MonoBehaviour
{
    private void OnEnable()
        {
            mutationName = "Hard Hitter";
            description = "Increases the player's knockback.";

            applyEffect = (player) => player.knockbackForce += 1f;
            removeEffect = (player) => player.knockbackForce -= 1f;
        }
}
