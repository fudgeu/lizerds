using System.Collections.Generic;
using UnityEngine;

public class PlayerMutations : MonoBehaviour
{
    private MovementTargetController movementController;
    public List<Mutation> activeMutations = new List<Mutation>();

    private void Start()
    {
        movementController = GetComponent<MovementTargetController>();
    }

    public void AddMutation(Mutation newMutation)
    {
        if (!activeMutations.Contains(newMutation))
        {
            activeMutations.Add(newMutation);
            newMutation.Apply(movementController);
        }
    }

    public void RemoveMutation(Mutation mutationToRemove)
    {
        if (activeMutations.Contains(mutationToRemove))
        {
            mutationToRemove.Remove(movementController);
            activeMutations.Remove(mutationToRemove);
        }
    }

    public void ClearAllMutations()
    {
        foreach (var mutation in activeMutations)
        {
            mutation.Remove(movementController);
        }
        activeMutations.Clear();
    }
}
