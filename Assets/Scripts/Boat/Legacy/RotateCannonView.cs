using UnityEngine;

namespace Boat.Legacy
{
    public class RotateCannonView : MonoBehaviour
    {
        [SerializeField] private Transform bodyCannon;
        [SerializeField] private Transform cameraCannon;
        [SerializeField] private Transform barrels;
        public float mouseSensitivity = 10.0f;
        public float scrollSensitivity = .01f;
        public float rotationSideSpeed = 100f;
        public float orbitDampening = 10f;
        public float topClamp = 25.0f;
        public bool cameraEnabled;
    
        private float _localRotation;
        private float _scrollAmount;
        //private float _cameraDistance = 20f;
        //private int _cameraRotation = 15;

        private void Start()
        {
            //cameraCannon.position = bodyCannon.position;
            //cameraCannon.Translate(new Vector3(0, _cameraDistance, -_cameraDistance));
            //cameraCannon.Rotate(new Vector3(_cameraRotation, 0, 0));
        }

        // Update is called once per frame
        private void Update()
        {
            BodyCannonRotation();
            BarrelCannonRotation();
        }

        // LateUpdate is called once per frame, after Update() on every game object in the scene
        private void LateUpdate()
        {
            CameraRotation();
        }

        // Rotation of the camera on the X axis based on Mouse Coordinates
        private void CameraRotation()
        {
            if (!cameraEnabled) return;
            if (Input.GetAxis("Mouse X") != 0)
            {
                _localRotation += Input.GetAxis("Mouse X") * mouseSensitivity;
            }
        
            Quaternion qt = Quaternion.Euler(0, _localRotation, 0);
            var parent = cameraCannon.parent;
            parent.localRotation = Quaternion.Lerp(parent.localRotation, qt, Time.deltaTime * orbitDampening);
        }

        // Smooth rotation of the cannon on the X axis based on the Camera Rotation Coordinates
        private void BodyCannonRotation()
        {
            /*Quaternion targetRotation = Quaternion.Euler(0, _localRotation.x, 0);
        Quaternion actualRotation = Quaternion.RotateTowards(bodyCannon.localRotation, targetRotation, rotationSideSpeed * Time.deltaTime);
        
        if (targetRotation != actualRotation)
        {
            bodyCannon.localRotation = actualRotation;
        }*/
            bodyCannon.localRotation = Quaternion.Euler(0, _localRotation, 0);
        }

        private void BarrelCannonRotation()
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                _scrollAmount += Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity; // [0.1 / -0.1] * scrollSensitivity
                _scrollAmount = Mathf.Clamp(_scrollAmount, 0, topClamp);

                Quaternion qt = Quaternion.Euler(_scrollAmount, 0, 0);
                barrels.localRotation = Quaternion.Lerp(barrels.localRotation, qt, Time.deltaTime * orbitDampening);
            }
        }
    }
}
