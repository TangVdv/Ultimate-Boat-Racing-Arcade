using System.Collections.Generic;
using UnityEngine;

//Use tuples

namespace Checkpoints
{
    public class CheckpointManager : MonoBehaviour
    {

        [SerializeField] private ChronoScript chrono;
        
        public List<GameObject> boats;

        public class PlayerProgress
        {
            public readonly GameObject player;
            public int lap;
            public int checkpoint; 
            public PlayerProgress(GameObject player)
            {
                this.player = player;
                this.lap = 0;
                this.checkpoint = 0;
            }
        }
    
        private Checkpoint[] checkpoints;
    
        public int grace = 3;
    
        private List<PlayerProgress> playerProgress = new List<PlayerProgress>();

        void Start()
        {
            foreach (GameObject boat in boats)playerProgress.Add(new PlayerProgress(boat));

            Debug.Log(playerProgress);
        
            checkpoints = new Checkpoint[transform.childCount];
            foreach (Transform child in transform)
            {
                Checkpoint checkpoint = child.GetComponent<Checkpoint>();
                checkpoints[checkpoint.ID] = checkpoint;
                checkpoint.SetCheckpointManager(this);
            }
        
            UpdateVisuals(playerProgress[0]);
        
        }
    
        public void AddPlayer(GameObject player)
        {
            boats.Add(player);
            playerProgress.Add(new PlayerProgress(player));
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

            chrono.SaveCheckpointTime(0, checkpoint);

            if (checkpoint == 0)
            {
                if ( (progress.checkpoint == checkpoints.Length - 1) || (progress.checkpoint + grace >= checkpoints.Length) )
                {
                    progress.lap++;
                    Debug.Log("Lap "+ progress.lap +" completed !");
                    chrono.PauseTimer();
                    chrono.PrintCheckpointsTime(0);
                }
                else
                {
                    return;
                }


            }
            progress.checkpoint = checkpoint;
            UpdateVisuals(progress);
        }

        public void UpdateVisuals(PlayerProgress progress)
        {
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
