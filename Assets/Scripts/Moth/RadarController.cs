// Moth Harper
// This script creates a pulse radar system that uses a trigger collider to scan the area around 
// the player, and create a ui display the reflects nearby enemies and pickups.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarController : MonoBehaviour
{
    // how far the radar scans
    [SerializeField] private float _scanRadius;
    // how quickly the radar scans (from 0 to max radius)
    [SerializeField] private float _scanSpeed;
    // how often the radar scans
    [SerializeField] private float _scanInterval;

    // the ui element in the scene for the radar
    [SerializeField] private RectTransform _radarUI;
    // the ui element in the scene for the circle that grows when it scans
    [SerializeField] private RectTransform _sweepCircleUI;
    // the prefab for the ping
    [SerializeField] private GameObject _pingUIPrefab;
    // the sprites to use for different pings
    [SerializeField] private Sprite _enemyPingSprite;
    [SerializeField] private Sprite _pickUpPingSprite;

    // the sphere collider that grows to detect enemies and pickups
    private SphereCollider _detectionSphere;
    // bool used to say when the radar is currently growing and scanning
    private bool _isScanning = true;

    // the player's transform
    private Transform _playerTransform;


    void Start()
    {
        _playerTransform = FindFirstObjectByType<Player_Controller>().transform;
        
        _detectionSphere = GetComponent<SphereCollider>();
        
        StartCoroutine(nameof(TimeScan));
    }

    void Update()
    {
        if (_isScanning)
        {
            // expand the scan
            if (_detectionSphere.radius < _scanRadius)
            {
                _detectionSphere.radius += _scanSpeed * Time.deltaTime;
            }
            // end the scan
            else
            {
                _detectionSphere.radius = 0f;
                _isScanning = false;
            }
            // match ui scanning circle to scan's current size
            _sweepCircleUI.localScale = Vector3.one * (_detectionSphere.radius / _scanRadius);
        }
    }

    IEnumerator TimeScan()
    {
        while (true)
        {
            yield return new WaitForSeconds(_scanInterval);
            _isScanning = true;
        }
    }

    void LateUpdate()
    {
        // match the radar's rotation to the player's
        _radarUI.rotation = Quaternion.Euler(0, 0, _playerTransform.rotation.eulerAngles.y);
        // move to the player's position
        transform.position = _playerTransform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        // if not currently scanning, early return
         if (!_isScanning) return;

        if (other.CompareTag("Enemy") || other.CompareTag("Pickup"))
        {
            // convert enemy's position to one relative to the player
            Vector3 relativePosition = other.transform.position - transform.position;
            // scale the position to match the UI's scale (relative position multiplied by ratio of ui diameter to scan radius diameter)
            Vector3 scaledPosition = relativePosition * (_radarUI.rect.width / (_scanRadius * 2));
            
            // create a "ping" on the radar for where the enemy is
            GameObject ping = Instantiate(_pingUIPrefab, _radarUI);
            ping.transform.localPosition = new Vector2(scaledPosition.x, scaledPosition.z);

            // set the scale based on the overall distance
            float distance = Vector3.Distance(other.transform.position, transform.position);
            ping.transform.localScale = Vector3.one * (1.5f - (distance / _scanRadius));

            // cache the ping's image component
            Image pingImage = ping.GetComponent<Image>();
            // set the sprite based on the type
            pingImage.sprite = other.CompareTag("Enemy") ? _enemyPingSprite : _pickUpPingSprite;
            // start fading the ping out
            pingImage.CrossFadeAlpha(0, _scanInterval, true);

            // destroy the ping eventually
            Destroy(ping, _scanInterval);
        }
    }

}
