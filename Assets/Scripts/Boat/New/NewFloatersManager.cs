using UnityEngine;

namespace Boat.New
{
    public class NewFloatersManager: MonoBehaviour
    {
        public Rigidbody rigidBody;
        public float depthBeforeSubmission = 1f;
        public float displacementAmount = 3f;
        public int floaterCount = 1;
        public float waterDrag = 0.99f;
        public float waterAngularDrag = 0.5f;
        public NewBoatMovementManager movementManager;
    }
}