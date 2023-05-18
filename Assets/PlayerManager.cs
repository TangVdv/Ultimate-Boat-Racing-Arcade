using System.Collections;
using System.Collections.Generic;
using Checkpoints;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private CheckpointManager checkpointManager;

    public void AddPlayer(GameObject player)
    {
        checkpointManager.AddPlayer(player);
    }
}
