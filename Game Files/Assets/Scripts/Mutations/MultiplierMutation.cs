using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Exploit Mutation", menuName = "Mutations/Exploit")]
public class MultiplierMutation : MonoBehaviour
{
    private void OnEnable()
        {
            mutationName = "Exploitative";
            description = "Increases the effect of player hp on knockback.";

            applyEffect = (player) => player.knockbackMultiplier += .1f;
            removeEffect = (player) => player.knockbackMultiplier -= .1f;
        }
}
