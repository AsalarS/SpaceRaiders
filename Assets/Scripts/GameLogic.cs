using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public int gamePhase = 0; // 0: Initial phase, 1: Arrow rotation, 2: Ball shooting
    public float shootingForce = 1000f; // Force used in phase 2

    // Method to transition to the next phase
    public void TransitionToNextPhase()
    {
        gamePhase++;
    }

    // Method to reset the game phase
    public void ResetGamePhase()
    {
        gamePhase = 0;
    }
}
