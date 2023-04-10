using UnityEngine;

namespace Boat.New
{
    public class NewFloater : MonoBehaviour
    {
        public NewFloatersManager Manager;

        private void FixedUpdate()
        {
            var position = transform.position;
            Manager.rigidBody.AddForceAtPosition(Physics.gravity / Manager.floaterCount, position, ForceMode.Acceleration);
            float waveHeight = WaveManager.instance.GetWaveHeight(position.x);
            if (transform.position.y < waveHeight)
            {
                var position1 = transform.position;
                float displacementMultiplier =
                    Mathf.Clamp01((waveHeight - position1.y) / Manager.depthBeforeSubmission) * Manager.displacementAmount;
                Manager.rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f),
                    position1, ForceMode.Acceleration);
                Manager.rigidBody.AddForce(-Manager.rigidBody.velocity * (displacementMultiplier * Manager.waterDrag * Time.fixedDeltaTime),
                    ForceMode.VelocityChange);
                Manager.rigidBody.AddTorque(
                    -Manager.rigidBody.angularVelocity * (displacementMultiplier * Manager.waterAngularDrag * Time.fixedDeltaTime),
                    ForceMode.VelocityChange);

            }
        }
    }
}