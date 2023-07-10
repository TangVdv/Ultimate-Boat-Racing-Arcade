using System.Collections;
using System.Collections.Generic;
using Boat.New;
using Checkpoints;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class CheatUIScript : MonoBehaviour
{
    [SerializeField] private TMP_Text currentCheckpoint;
    [SerializeField] private TMP_Dropdown dropdownCheckpoints;

    private NewInputManagerInterface _newInputManagerInterface;
    private List<Transform> _checkpoints = new List<Transform>();
    private List<string> _checkpointsName = new List<string>();
    private Transform _currentCheckpoint;
    private int _currentID = 0;
    public void SetupCheatPanel(List<Transform> checkpoints)
    {
        _checkpoints.Clear();
        _checkpointsName.Clear();
        dropdownCheckpoints.ClearOptions();
        _checkpoints = checkpoints;
        foreach (var checkpoint in _checkpoints)
        {
            _checkpointsName.Add(checkpoint.name);
        }
        dropdownCheckpoints.AddOptions(_checkpointsName);
        _currentID = 0;
        _currentCheckpoint = _checkpoints[_currentID];
        SetCurrentCheckpointText();
    }
    
    public void SetPlayer(NewInputManagerInterface newInputManagerInterface)
    {
        _newInputManagerInterface = newInputManagerInterface;
    }
    
    public void NextCheckpoint()
    {
        _currentID = Mathf.Clamp(_currentID+1, 0, _checkpoints.Count-1);
        _currentCheckpoint = _checkpoints[_currentID];
        Teleport();
    }

    public void PreviousCheckpoint()
    {
        _currentID = Mathf.Clamp(_currentID-1, 0, _checkpoints.Count-1);
        _currentCheckpoint = _checkpoints[_currentID];
        Teleport();
    }

    public void OnDropdownChange()
    {
        for (int i = 0; i <= dropdownCheckpoints.value; i++)
        {
            _currentID = i;
            _currentCheckpoint = _checkpoints[_currentID];
            Teleport();
        }
    }

    private void Teleport()
    {
        Checkpoint checkpoint = _currentCheckpoint.GetComponent<Checkpoint>();
        if (checkpoint)
        {
            _newInputManagerInterface.checkpointManager.CheckPointPassed(checkpoint.ID, transform.parent.gameObject);
            if (checkpoint.ID == 0)
            {
                gameObject.SetActive(false);
                transform.parent.GetComponent<NewPlayerInputManager>().Frozen = false;
            };
        }
        _newInputManagerInterface.Respawn(_currentCheckpoint);
        SetCurrentCheckpointText();

    }

    private void SetCurrentCheckpointText()
    {
        currentCheckpoint.text = _currentCheckpoint.name;
    }
}
