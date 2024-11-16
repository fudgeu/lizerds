using UnityEngine;

public class MutationsManager : MonoBehaviour
{
    public Mutation[] availableMutations;

    public void AssignRandomMutation(PlayerMutations player)
    {
        Mutation randomMutation = availableMutations[Random.Range(0, availableMutations.Length)];
        player.AddMutation(randomMutation);
    }

    public void AssignSpecificMutation(PlayerMutations player, Mutation mutation)
    {
        player.AddMutation(mutation);
    }
}
