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
        
        public List<GameObject> boats;
        public bool debug;

        public class PlayerProgress
        {
            public readonly GameObject player;
            public int lap;
            public int checkpoint;
            public int pos;
            public bool isFinished;
            public List<float> checkpointTime = new List<float>();
            public NewInputManagerInterface newInputManagerInterface;
            public PlayerUI playerUI;
            public PlayerProgress(GameObject player)
            {
                this.player = player;
                this.lap = 1;
                this.checkpoint = 0;
                this.pos = 1;
                this.isFinished = false;
                this.newInputManagerInterface = player.GetComponent<NewInputManagerInterface>();
                this.playerUI = player.GetComponent<NewPlayerInputManager>().PlayerUI;
            }
        }
    
        private Checkpoint[] checkpoints;
    
        public int grace = 3;
        public int lapGoal = 1;

        private int _maxPos;
    
        private List<PlayerProgress> playerProgress = new List<PlayerProgress>();

        private void Awake()
        {
            _maxPos = config.PlayerAmount + config.AIAmount;
            _timerScript = GameObject.Find("TimerUI").GetComponent<TimerScript>();
        }

        private void Setup()
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
                progress.isFinished = false;
                progress.playerUI.RaceModeScript.SetMaxLapText(lapGoal);   
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

        public Vector3 GetNextCheckpointCoordinates(GameObject player){
            PlayerProgress progress = playerProgress.Find(x => x.player == player);
            int index = progress.checkpoint + 1;
            if (index >= checkpoints.Length) index = 0;
            return checkpoints[index].transform.position;
        }

        public int GetCheckpointCount(){
            return checkpoints.Length;
        }

        public void CheckPointPassed(int checkpoint, GameObject player)
        {
            PlayerProgress progress = playerProgress.Find(x => x.player == player);
            RaceModeScript race = progress.playerUI.RaceModeScript;

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

            if (config.GameMode == 1)
            {
                //CHRONO MODE
                progress.checkpointTime.Add(_timerScript.TimerChrono);
                ChronoTimeDifference(progress); 
            }
            else if (config.GameMode == 0)
            {
                //RACE MODE
                HandleRaceMode(checkpoint, progress);
            }

            if (checkpoint == 0)
            {
                //if (progress.checkpoint == checkpoints.Length - 1 || progress.checkpoint + grace >= checkpoints.Length)
                if(true)
                {
                    //TODO : FIX EVERYTHING
                    if(debug) Debug.Log("Lap "+ progress.lap +" completed !");
                    progress.lap++;
                    if (progress.lap > lapGoal)
                    {
                        progress.isFinished = true;
                        if(debug)Debug.Log("Player arrived : "+progress.isFinished);
                        
                        if (progress.newInputManagerInterface.playerType == NewInputManagerInterface.PlayerType.Bot)
                        {
                            //TODO : Deactivate bot inputs instead
                            progress.player.SetActive(false);    
                            return;
                        }
                        
                        if (playerProgress.All(p => p.isFinished && p.newInputManagerInterface.playerType == NewInputManagerInterface.PlayerType.Player))
                        {
                            if (config.GameMode == 1)
                            {
                                //CHRONO MODE
                                if(debug) Debug.Log("Chrono mode finished");
                                // Save checkpoint times to config
                                config.CheckpointTimes[config.Level] = progress.checkpointTime;
                            }
                            else if (config.GameMode == 0)
                            {
                                //RACE MODE
                                if(debug) Debug.Log("Race mode finished");
                            }
                            _timerScript.ResetTimer();
                            Time.timeScale = 0f;
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
            }
            if(debug) UpdateVisuals(progress);
        }

        private void ChronoTimeDifference(PlayerProgress progress)
        {
            ChronoScript chrono = progress.playerUI.ChronoScript;
            string timerText;
            float timerDiff;
            if (config.CheckpointTimes[config.Level] != null)
            {
                float checkPointTimer = config.CheckpointTimes[config.Level][progress.checkpointTime.Count - 1];
                timerDiff = _timerScript.TimerChrono - checkPointTimer;
                timerText = _timerScript.ConvertTimerToString(timerDiff);
            }
            else
            {
                timerDiff = 0;
                timerText = _timerScript.ConvertTimerToString(_timerScript.TimerChrono);
            }

            chrono.ShowCheckpointTimeDifference(timerDiff, timerText);
        }
        
        private void GetAllPos(int checkpoint)
        {
            int i = 1;
            Dictionary<string, float> dictionary = checkpoints[checkpoint].PlayerTimer;
            if(debug) Debug.Log("Final ranking : ");
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
            RaceModeScript race = progress.playerUI.RaceModeScript;
            checkpoints[checkpoint].PlayerTimer[progress.newInputManagerInterface.playerName] = _timerScript.TimerChrono;
            Dictionary<string, float> timerDictionary = checkpoints[checkpoint].PlayerTimer;

            foreach (var playerProg in playerProgress)
            {
                race = playerProg.playerUI.RaceModeScript;
                SetPlayerPos(timerDictionary, playerProg);
                race.SetCurrentPosText(playerProg.pos);
                if (playerProg.newInputManagerInterface.playerType == NewInputManagerInterface.PlayerType.Player && playerProg.checkpoint == checkpoint)
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

        public void UpdateVisuals(PlayerProgress progress)
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
