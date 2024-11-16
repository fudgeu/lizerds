using UnityEngine;

public class MutationTester : MonoBehaviour
{
    public Mutation massMutation;
    private PlayerMutations playerMutations;

    private void Start()
    {
        playerMutations = FindObjectOfType<PlayerMutations>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) // Press 'M' to apply the mass mutation
        {
            playerMutations.AddMutation(massMutation);
        }

        if (Input.GetKeyDown(KeyCode.R)) // Press 'R' to remove all mutations
        {
            playerMutations.ClearAllMutations();
        }
    }
}
