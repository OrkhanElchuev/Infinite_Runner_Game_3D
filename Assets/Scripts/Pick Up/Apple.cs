using UnityEngine;

/// <summary>
/// Apple pickup that increases the chunk scrolling speed (and may adjust gravity/FOV via LevelGenerator).
/// Inherits pickup behavior (rotate + trigger + destroy) from PickUp.
/// </summary>

public class Apple : PickUp
{
    [Tooltip("How much to increase/decrease level move speed when picked up.")]
    [SerializeField] float adjustChangeMoveSpeedAmount = 3f;

    LevelGenerator levelGenerator;

    // Initialize the apple with required references.
    public void Init(LevelGenerator levelGenerator) 
    {
        this.levelGenerator = levelGenerator;
    }

    protected override void OnPickup()
    {
        if (levelGenerator == null) return;
        
        levelGenerator.ChangeChunkMoveSpeed(adjustChangeMoveSpeedAmount);
    }
}
