// Moth Harper
// 9/11/2025
// This script is meant for sprites, to make them always face the player's camera.
// This script should be applied to a sprite's parent, not the sprite itself.

using UnityEngine;

public class BillboardController : MonoBehaviour
{
    // cache the transform of this object
    Transform _thisTransform;
    
    // store the initial rotation
    Vector3 _initialRotation;

    // cache the player's camera's transform
    Transform _cameraTransform;

    void Start()
    {
        // cache transforms and rotation
        _thisTransform = transform;
        _initialRotation = _thisTransform.rotation.eulerAngles;
        _cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // rotate to face the player (main camera)
        _thisTransform.LookAt(_cameraTransform.position);
        // correct x and z rotation
        _thisTransform.rotation = Quaternion.Euler(new Vector3(_initialRotation.x, _thisTransform.rotation.eulerAngles.y, _initialRotation.z));
    }
}
