using System;
using System.Collections.Generic;
using System.Linq;
using Boat.New;
using TMPro;
using UnityEngine;

//Use tuples

namespace Checkpoints
{
    public class CheckpointManager : MonoBehaviour
    {
        [SerializeField] private ConfigScript config;
        private TimerScript _timerScript;
        private FinishUI _finishUI;
        
        public List<GameObject> boats;
        public bool debug;

        public class PlayerProgress
        {
            public readonly GameObject player;
            public int lap;
            public int checkpoint;
            public int pos;
            public List<float> checkpointTime = new List<float>();
            public NewInputManagerInterface newInputManagerInterface;
            public PlayerUI playerUI;
            public PlayerProgress(GameObject player)
            {
                this.player = player;
                this.lap = 1;
                this.checkpoint = 0;
                this.pos = 1;
                this.newInputManagerInterface = player.GetComponent<NewInputManagerInterface>();
                this.playerUI = player.GetComponent<NewInputManagerInterface>().globalPlayerUI;
            }
        }
    
        private Checkpoint[] checkpoints;
    
        public int grace = 3;
        public int lapGoal = 1;

        private int _maxPos;
        private int _playerFinishedAmount = 0;
    
        private List<PlayerProgress> playerProgress = new List<PlayerProgress>();

        private ChronoScript chrono;
        private RaceModeScript race;
        
        private int[] pointsTable = {15, 12, 10, 8, 6, 4, 2};

        private void Awake()
        {
            _maxPos = config.PlayerAmount + config.AIAmount;
            _timerScript = GameObject.Find("TimerUI").GetComponent<TimerScript>();
            _finishUI = GameObject.Find("FinishUI").GetComponent<FinishUI>();
        }

        public void Setup()
        {
            // First time init
            if (checkpoints == null)
            {
                checkpoints = new Checkpoint[transform.childCount]; 
                foreach (Transform child in transform)
                {
                    Checkpoint checkpoint = child.GetComponent<Checkpoint>();
                    checkpoints[checkpoint.ID] = checkpoint;
                    checkpoint.SetCheckpointManager(this);
                }
            }

            _playerFinishedAmount = 0;
            if(debug) UpdateVisuals(playerProgress[0]);
            ResetProgress();
        }

        private void ResetProgress()
        {
            foreach (var progress in playerProgress)
            {
                progress.lap = 1;
                progress.checkpoint = 0;
                progress.checkpointTime = new List<float>();
                if(progress.playerUI) progress.playerUI.RaceModeScript.SetMaxLapText(lapGoal);
                progress.newInputManagerInterface.checkpointManager = this;
                progress.newInputManagerInterface.newBoatMovementManager.frozen = false;
                var aiInputManager = progress.player.GetComponent<NewAIInputManager>();
                if (aiInputManager) aiInputManager.ResetPathing();
            }
        }

        public void AddPlayer(GameObject player)
        {
            if(debug)Debug.Log("Add player : "+player); 
            boats.Add(player);
            playerProgress.Add(new PlayerProgress(player));

            if (playerProgress.Count == (config.PlayerAmount + config.AIAmount)) 
            {
                if (debug)
                {
                    Debug.Log("Start : ");
                    foreach (var p in playerProgress)
                    {
                        Debug.Log(p.player.name);
                    }
                }
                Setup();
            }
        }

        //Deprecated
        public Vector3 GetNextCheckpointCoordinates(GameObject player){
            PlayerProgress progress = playerProgress.Find(x => x.player == player);
            int index = progress.checkpoint + 1;
            if (index >= checkpoints.Length) index = 0;
            return checkpoints[index].core.transform.position;
        }
        
        public Collider GetNextCheckpointCollider(GameObject player, int i = 1){
            PlayerProgress progress = playerProgress.Find(x => x.player == player);
            if(lapGoal == progress.lap && progress.checkpoint+i >= checkpoints.Length) return checkpoints[0].core.GetComponent<Collider>();
            int index = (progress.checkpoint + i) % checkpoints.Length;
            return checkpoints[index].core.GetComponent<Collider>();
        }

        public int GetCheckpointCount(){
            return checkpoints.Length;
        }

        public void CheckPointPassed(int checkpoint, GameObject player)
        {
            PlayerProgress progress = playerProgress.Find(x => x.player == player);
            if (progress.playerUI)
            {
                race = progress.playerUI.RaceModeScript;
                chrono = progress.playerUI.ChronoScript;   
            }

            // Ignore if same checkpoint
            if (progress.checkpoint == checkpoint) return;


            // Ignore if checkpoint is not within grace
            if (progress.checkpoint + grace < checkpoint) return;


            // If checkpoint is a previous checkpoint, ignore
            if (progress.checkpoint > checkpoint && checkpoint != 0)
            {
                // progress.checkpoint = checkpoint;
                // UpdateVisuals(progress);
                return;
            }

            progress.newInputManagerInterface.lastCheckpoint = checkpoints[checkpoint].transform;
            progress.checkpoint = checkpoint;

            progress.checkpointTime.Add(_timerScript.TimerChrono);
            
            if (config.GameMode == 1)
            {
                if (progress.newInputManagerInterface.playerType == NewInputManagerInterface.PlayerType.Player)
                {
                    //CHRONO MODE
                    chrono.ShowCheckpointTimeDifference(
                        ChronoTimeDifference(progress.checkpointTime.Count - 1, progress).Item1, 
                        ChronoTimeDifference(progress.checkpointTime.Count - 1, progress).Item2);   
                }
            }
            else if (config.GameMode == 0)
            {
                //RACE MODE
                HandleRaceMode(checkpoint, progress);
            }

            if (checkpoint == 0)
            {
                //if (progress.checkpoint == checkpoints.Length - 1 || progress.checkpoint + grace >= checkpoints.Length)
                if(debug) Debug.Log("Lap "+ progress.lap +" completed !");
                progress.lap++;
                if (progress.lap > lapGoal)
                {
                    if (progress.newInputManagerInterface.playerType == NewInputManagerInterface.PlayerType.Bot)
                    {
                        progress.player.GetComponent<NewBoatMovementManager>().frozen = true; 
                        return;
                    }
                    
                    if(progress.newInputManagerInterface.playerType == NewInputManagerInterface.PlayerType.Player)
                    {
                        _playerFinishedAmount++;
                    }

                    if (_playerFinishedAmount == config.PlayerAmount)
                    {
                        HandleFinishUI(progress);
                        _timerScript.PauseTimer();
                        return;
                    }
                }
                else
                {
                    // Next lap
                    if (config.GameMode == 0)
                    {
                        race.SetCurrentLapText(progress.lap);
                    }
                }
            }
            if(debug) UpdateVisuals(progress);
        }

        private void HandleFinishUI(PlayerProgress progress)
        {
            if (config.GameMode == 1)
            {
                //CHRONO MODE
                if(debug) Debug.Log("Chrono mode finished");
                int i = 0;
                string text = "";
                Color color = new Color();
                _finishUI.ClearCheckpointInfo();
                float timerDiff = 0;
                foreach (var time in progress.checkpointTime)
                {
                    string timerText =  ChronoTimeDifference(i, progress).Item2;
                    timerDiff =  ChronoTimeDifference(i, progress).Item1;
                    _finishUI.InstantiateCheckpointInfo(
                        i+1, 
                        _timerScript.ConvertTimerToString(time), 
                        chrono.GetTimerDiffValues(timerDiff, timerText).Item1, 
                        chrono.GetTimerDiffValues(timerDiff, timerText).Item2);
                    i++;
                }
                            
                _finishUI.SetChronoInfo(_timerScript.ConvertTimerToString(_timerScript.TimerChrono), text, color);
                _finishUI.ChronoInfoPanel.SetActive(true);

                // Save checkpoint times to config
                if (timerDiff < 0 || config.CheckpointTimes[config.Level] == null)
                {
                    config.BestTimePlayerName.Insert(config.Level, progress.newInputManagerInterface.playerName);
                    config.CheckpointTimes[config.Level] = progress.checkpointTime;
                }
                            
            }
            else if (config.GameMode == 0)
            {
                //RACE MODE
                if(debug) Debug.Log("Race mode finished");
                // if this is the last level, show final scoreboard
                CalculatePlayerScore();
                if (config.Level == config.LastLevelIndex)
                {
                    CalculateFinalScoreboard();
                    _finishUI.FinalScoreboardPanel.SetActive(true);
                    Time.timeScale = 0f;
                }
                else
                {
                    CalculateScoreboard();
                    _finishUI.ScoreBoardPanel.SetActive(true);
                }
            }
                        
            _finishUI.FinishUIPanel.SetActive(true);
        }

        private void CalculatePlayerScore()
        {
            foreach (var progress in playerProgress)
            {
                progress.newInputManagerInterface.score += pointsTable[progress.pos - 1];   
            }
        }

        private void CalculateScoreboard()
        {
            playerProgress.Sort((x, y) => x.pos.CompareTo(y.pos));
            _finishUI.ClearPlayerScoreboard();
            foreach (var progress in playerProgress)
            {
                _finishUI.InstantiatePlayerScore(
                    progress.pos,
                    progress.newInputManagerInterface.playerName,
                    _timerScript.ConvertTimerToString(progress.checkpointTime[progress.checkpointTime.Count - 1]), 
                    progress.newInputManagerInterface.score, 
                    progress.newInputManagerInterface.playerType == NewInputManagerInterface.PlayerType.Player);
            }
        }
        
        private void CalculateFinalScoreboard()
        {
            playerProgress.Sort((x, y) => y.newInputManagerInterface.score.CompareTo(x.newInputManagerInterface.score));
            _finishUI.ClearFinalScoreboard();
            int index = 0;
            foreach (var progress in playerProgress)
            {
                if (index < 3)
                {
                    _finishUI.SetTop3Scoreboard(index,
                        progress.newInputManagerInterface.playerName,
                        progress.newInputManagerInterface.score);
                }
                else
                {
                    _finishUI.InstantiateFinalScoreboard(
                        index+1,
                        progress.newInputManagerInterface.playerName,
                        progress.newInputManagerInterface.score);
                }
                _finishUI.InstantiatePlayerScore(
                    progress.pos,
                    progress.newInputManagerInterface.playerName,
                    _timerScript.ConvertTimerToString(progress.checkpointTime[progress.checkpointTime.Count - 1]), 
                    progress.newInputManagerInterface.score, 
                    progress.newInputManagerInterface.playerType == NewInputManagerInterface.PlayerType.Player);
                index++;
            }
        }

        private (float, string) ChronoTimeDifference(int id, PlayerProgress progress)
        {
            float timerDiff;
            string timerText;
            if (config.CheckpointTimes[config.Level] != null)
            {
                float checkPointTimer = config.CheckpointTimes[config.Level][id];
                timerDiff = progress.checkpointTime[id] - checkPointTimer;
                if(timerDiff<0) timerText = _timerScript.ConvertTimerToString(-timerDiff);
                else timerText = _timerScript.ConvertTimerToString(timerDiff);
            }
            else
            {
                timerDiff = 0;
                timerText = _timerScript.ConvertTimerToString(_timerScript.TimerChrono);
            }

            return (timerDiff, timerText);
        }
        
        private void GetAllPos(int checkpoint)
        {
            int i = 1;
            Dictionary<string, float> dictionary = checkpoints[checkpoint].PlayerTimer;
            foreach(KeyValuePair<string, float> entry in dictionary)
            {
                if(debug) Debug.Log("name : "+entry.Key+" ; Pos : "+i);
                i++;
            }
        }

        private void SetPlayerPos(Dictionary<string, float> dictionary, PlayerProgress progress)
        {
            int currentPos = 1;
            if (!dictionary.ContainsKey(progress.newInputManagerInterface.playerName))
            {
                progress.pos = _maxPos;
            }
            foreach (KeyValuePair<string, float> entry in dictionary)
            {
                if (entry.Key == progress.newInputManagerInterface.playerName)
                {
                    progress.pos = currentPos;
                }
                currentPos++;
            }
        }

        private void HandleRaceMode(int checkpoint, PlayerProgress progress)
        {
            //RACE MODE
            checkpoints[checkpoint].PlayerTimer[progress.newInputManagerInterface.playerName] = _timerScript.TimerChrono;
            Dictionary<string, float> timerDictionary = checkpoints[checkpoint].PlayerTimer;

            foreach (var playerProg in playerProgress)
            {
                SetPlayerPos(timerDictionary, playerProg);
                if (playerProg.newInputManagerInterface.playerType == NewInputManagerInterface.PlayerType.Player)
                {
                    race = playerProg.playerUI.RaceModeScript;
                    race.SetCurrentPosText(playerProg.pos);
                    if (playerProg.checkpoint == checkpoint)
                    {
                        race.ResetRanking();
                
                        KeyValuePair<string, float> firstEntry = timerDictionary.FirstOrDefault();
                
                        foreach (KeyValuePair<string, float> entry in timerDictionary)
                        {
                            bool isPlayer = entry.Key == playerProg.newInputManagerInterface.playerName ? true : false;
                            float timerDiff = entry.Key == firstEntry.Key ? entry.Value : entry.Value - firstEntry.Value;
                            race.InstantiateRanking(entry.Key, _timerScript.ConvertTimerToString(timerDiff), isPlayer);
                        }
                    }   
                }
            }
        }

        private void UpdateVisuals(PlayerProgress progress)
        {
            if(!debug) return;
            
            //Debug.Log("Checkpoint " + progress.checkpoint + " passed, lap " + progress.lap);
            foreach (Checkpoint checkpoint in checkpoints)
            {
                // Match id with progress.checkpoint
                // Current: Green, in grace: Yellow, else: Red, checkpoint 0: Blue            

                int id = checkpoint.ID;
            
                //Get the material of the checkpoint
                Material material = checkpoint.GetComponent<Renderer>().material;
            
                Color green = new Color(0, 1, 0, 0.3f);
                Color yellow = new Color(1, 1, 0, 0.3f);
                Color red = new Color(1, 0, 0, 0.3f);
                Color blue = new Color(0, 0, 1, 0.3f);
            
                //Set the color of the material
                if (id == progress.checkpoint) material.color = green;
                else if (id > progress.checkpoint && id <= progress.checkpoint + grace) material.color = yellow;
                else if (id == 0) material.color = blue;
                else material.color = red;
            }
        }

        public (int,int) GetPlayerProgress(GameObject player){
            PlayerProgress progress = playerProgress.Find(x => x.player == player);
            return (progress.lap, progress.checkpoint);
        }

        public (int,int) GetAverageProgress(){
            int lap = 0;
            int checkpoint = 0;
            foreach (PlayerProgress progress in playerProgress){
                lap += progress.lap;
                checkpoint += progress.checkpoint;
            }
            lap /= playerProgress.Count;
            checkpoint /= playerProgress.Count;
            return (lap, checkpoint);
        }

        public (int, int) GetLowestProgress(){
            int lap = 0;
            int checkpoint = 0;
            foreach (PlayerProgress progress in playerProgress){
                //Lap takes precedence oer checkpoint
                if (progress.lap < lap) {
                    lap = progress.lap;
                    checkpoint = progress.checkpoint;
                }
                else if (progress.lap == lap && progress.checkpoint < checkpoint) checkpoint = progress.checkpoint;
            }
            return (lap, checkpoint);
        }

        public (int, int) GetHighestProgress(){
            int lap = 0;
            int checkpoint = 0;
            foreach (PlayerProgress progress in playerProgress){
                //Lap takes precedence oer checkpoint
                if (progress.lap > lap) {
                    lap = progress.lap;
                    checkpoint = progress.checkpoint;
                }
                else if (progress.lap == lap && progress.checkpoint > checkpoint) checkpoint = progress.checkpoint;
            }
            return (lap, checkpoint);
        }
    }
}
