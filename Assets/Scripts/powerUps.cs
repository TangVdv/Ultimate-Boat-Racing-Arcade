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
	}

	private void evaluatePowerUp(Collider other){
		var manager = GameObject.FindWithTag("CheckpointController").GetComponent<CheckpointManager>();
		(int, int) playerProgress = manager.getPlayerProgress(other.gameObject);
		(int, int) averageProgress = manager.getAverageProgress();
		(int, int) highestProgress = manager.getHighestProgress();
		(int, int) lowestProgress = manager.getLowestProgress();

		//Evaluate PowerUp with some randomness and by weighing the percentile of the player from highest, lowest and average
		int score = 0;
		//TODO: create a formula for this
    }
}
