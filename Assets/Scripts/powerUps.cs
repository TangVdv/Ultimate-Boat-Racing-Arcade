using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUps : MonoBehaviour
{
	private int cooldowm = 0;
	// In Frames
	private int cooldownTime = 200;

	private bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldowm > 0) cooldowm--;

		if (cooldowm == 0) {
			gameObject.GetComponent<Renderer>().enabled = true;
			active = true;
		}
    }

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag != "Player" || !active) return;

		gameObject.GetComponent<Renderer>().enabled = false;
        cooldowm = cooldownTime;
        active = false;	
		evaluatePowerUp(other);
	}

	private void evaluatePowerUp(Collider other){
		var manager = GameObject.FindWithTag("CheckpointController").GetComponent<CheckpointManager>();

		//(lap, checkpoint)
		(int, int) playerProgress = manager.GetPlayerProgress(other.gameObject);
		(int, int) highestProgress = manager.GetHighestProgress();

		int checkpointCount = manager.GetCheckpointCount();
		int score = (highestProgress.Item1 - playerProgress.Item1) * checkpointCount + (highestProgress.Item2 - playerProgress.Item2);
		//Randomize score by X
		int x = 3;
		score += (int) Random.Range(-x, x);

	    //Debug.Log("Placement Score: " + score);

		//TODO: Evaluate powerup using score value
    }
}
