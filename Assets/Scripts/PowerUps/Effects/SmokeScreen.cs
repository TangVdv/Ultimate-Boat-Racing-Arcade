using System;
using System.Collections.Generic;
using Boat.New;
using UnityEngine;

namespace PowerUps.Effects
{
    public class SmokeScreen : MonoBehaviour
    {
        public int cooldownTimerRangeMin;
        public int cooldownTimerRangeMax;
        private float cooldownTimer;
        
        private List<NewAIInputManager> botsInSmokeScreen = new List<NewAIInputManager>();

        // Start is called before the first frame update
        void Start()
        {
            cooldownTimer = UnityEngine.Random.Range(cooldownTimerRangeMin, cooldownTimerRangeMax);
        }

        // Update is called once per frame
        void Update()
        {
            if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
            else RemoveSmokeScreen();
        }

        private void OnTriggerEnter(Collider other)
        {
            //Depending on the instance of input controller, we do different things
            //New AI input Manager VS New Player Input Manager
            
            var bot = other.gameObject.GetComponent<NewAIInputManager>();

            if (bot != null)
            {
                botsInSmokeScreen.Add(bot);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var botScript = other.gameObject.GetComponent<NewAIInputManager>();
            if (botScript == null) return;
            
            botScript.State.IsBlinded = false;
            botsInSmokeScreen.Remove(botScript);
        }

        private void RemoveSmokeScreen()
        {
            foreach (var bot in botsInSmokeScreen)
            {
                bot.State.IsBlinded = false;
            }
            Destroy(gameObject);
        }
    }
}
