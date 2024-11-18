using System.Collections.Generic;
using UnityEngine;

public class PlayerMutations : MonoBehaviour
{
    private MovementTargetController movementController;
    public List<Mutation> activeMutations = new List<Mutation>();

    private void Start()
    {
        movementController = GetComponentInChildren<MovementTargetController>();
    }

    public void AddMutation(Mutation newMutation)
    {
        if (!activeMutations.Contains(newMutation))
        {
            activeMutations.Add(newMutation);
        }
    }

    public void RemoveMutation(Mutation mutationToRemove)
    {
        if (activeMutations.Contains(mutationToRemove))
        {
            if (movementController) mutationToRemove.Remove(movementController);
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

    public void EnableAllMutations()
    {
        movementController = GetComponentInChildren<MovementTargetController>();
        if (!movementController) return;
        foreach (var mutation in activeMutations)
        {
            mutation.Apply(movementController);
        }
    }
    
    public void DisableAllMutations()
    {
        movementController = GetComponentInChildren<MovementTargetController>();
        if (!movementController) return;
        foreach (var mutation in activeMutations)
        {
            mutation.Remove(movementController);
        }
    }
}
