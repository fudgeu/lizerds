using UnityEngine;

[CreateAssetMenu(fileName = "New Mutation", menuName = "Mutations/General Mutation")]
public class Mutation : ScriptableObject
{
    public string mutationName;
    public string description;

    // Define delegates for applying and removing mutations
    public System.Action<MovementTargetController> applyEffect;
    public System.Action<MovementTargetController> removeEffect;

    // Assign the effects dynamically
    public void Apply(MovementTargetController player)
    {
        applyEffect?.Invoke(player);
    }

    public void Remove(MovementTargetController player)
    {
        removeEffect?.Invoke(player);
    }
}
