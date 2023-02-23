using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Use tuples

public class CheckpointManager : MonoBehaviour
{

    public class PlayerProgress
    {
        public GameObject player;
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
        GameObject[] boats = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject boat in boats) playerProgress.Add(new PlayerProgress(boat));
        
        checkpoints = new Checkpoint[transform.childCount];
        foreach (Transform child in transform)
         {
             Checkpoint checkpoint = child.GetComponent<Checkpoint>();
             checkpoints[checkpoint.ID] = checkpoint;
             checkpoint.SetCheckpointManager(this);
             
         }
        
    }

    public void CheckPointPassed(int checkpoint, GameObject player)
    {
        PlayerProgress progress = playerProgress.Find(x => x.player == player);
        
        // Ignore if same checkpoint
        if (progress.checkpoint == checkpoint) return;
        
        
        // Ignore if checkpoint is not within grace
        if (progress.checkpoint + grace < checkpoint) return;
        
        
        // If checkpoint is a previous checkpoint, lower checkpoint
        if (progress.checkpoint > checkpoint && checkpoint != 0)
        {
            progress.checkpoint = checkpoint;
            updateVisuals(progress);
            return;
        }
        

        if ( checkpoint == 0 && ( (progress.checkpoint == checkpoints.Length - 1) || (progress.checkpoint + grace >= checkpoints.Length) ) )
        {
            progress.lap++;
            progress.checkpoint = 0;
            
            Debug.Log("Lap "+ progress.lap +" completed !");
            updateVisuals(progress);
        }
        else
        {
            progress.checkpoint = checkpoint;
            updateVisuals(progress);
        }
    }

    public void updateVisuals(PlayerProgress progress)
    {
		Debug.Log("Checkpoint " + progress.checkpoint + " passed, lap " + progress.lap);
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

	public (int,int) getPlayerProgress(GameObject player){
        PlayerProgress progress = playerProgress.Find(x => x.player == player);
        return (progress.lap, progress.checkpoint);
    }

	public (int,int) getAverageProgress(){
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

	public (int, int) getLowestProgress(){
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

	public (int, int) getHighestProgress(){
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
