using UnityEngine;

public class BillboardEffectController : MonoBehaviour
{
    Transform _thisTransform;
    Vector3 _initialRotation;
    Transform _cameraTransform;

    void Start()
    {
        _thisTransform = transform;
        _initialRotation = _thisTransform.rotation.eulerAngles;
        print(_initialRotation);
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
