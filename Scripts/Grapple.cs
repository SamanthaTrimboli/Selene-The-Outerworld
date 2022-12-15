using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class Grapple : MonoBehaviour
    {
        // Start is called before the first frame update
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private StarterAssetsInputs _input;
        private LineRenderer lr;
        private Vector3 grapplePoint;
        public LayerMask whatIsGrappleable;
        public Transform gunTip, camera, player;
        private float maxDistance = 300f;
        private SpringJoint joint;

        void Awake()
        {
           // GameObject gun = GameObject.FindWithTag("GrappleGun");
            lr = GetComponent<LineRenderer>();
        }

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }
        void Start()
        {
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
            Debug.LogError("Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
            
        }

        // Update is called once per frame
        void Update()
        {
            if (_input.grapple)
            {
                StartGrapple();
                Debug.LogError("true");
            }
            StopGrapple();
            _input.grapple = false;
        }

        void LateUpdate()
        {
            DrawRope();
        }

        void StartGrapple()
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
            {
                grapplePoint = hit.point;
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;

                float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

                //The distance grapple will try to keep from grapple point.
                joint.maxDistance = distanceFromPoint * 0.08f;
                joint.minDistance = distanceFromPoint * 0.25f;

                //Adjust these values to fit your game.
                joint.spring = 12.5f;
                joint.damper = 7f;
                joint.massScale = 4.5f;

                lr.positionCount = 2;

            }
        }

        void DrawRope()
        {
            if (!joint) return;

            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, grapplePoint);
        }

        void StopGrapple()
        {
            lr.positionCount = 0;
            Destroy(joint);
        }

    }
}
