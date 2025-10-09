using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarController : MonoBehaviour
{

    [SerializeField] private float scanRadius;
    [SerializeField] private float scanSpeed;
    [SerializeField] private float scanInterval;

    [SerializeField] private RectTransform radarUI;
    [SerializeField] private RectTransform sweepCircleUI;
    [SerializeField] private GameObject pingUIPrefab;

    private Transform _playerTransform;
    private SphereCollider _detectionSphere;

    private bool isScanning = true;


    void Start()
    {
        _playerTransform = FindAnyObjectByType<Player_Controller>().transform;
        _detectionSphere = GetComponent<SphereCollider>();
        StartCoroutine(nameof(TimeScan));
    }

    void Update()
    {
        if (isScanning)
        {
            if (_detectionSphere.radius < scanRadius)
            {
                _detectionSphere.radius += scanSpeed * Time.deltaTime;
            }
            else
            {
                _detectionSphere.radius = 0f;
                isScanning = false;
            }

            sweepCircleUI.localScale = Vector3.one * (_detectionSphere.radius / scanRadius);
        }
    }

    IEnumerator TimeScan()
    {
        while (true)
        {
            yield return new WaitForSeconds(scanInterval);
            isScanning = true;
        }
    }

    void LateUpdate()
    {
        // match the radar's rotation to the player's
        radarUI.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.y);

        // sweepCircleUI.localScale = Vector3.one * (_detectionSphere.radius / scanRadius);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // convert enemy's position to one relative to the player
            Vector3 relativePosition = other.transform.position - transform.position;
            // scale the position to match the UI's scale (relative position multiplied by ratio of ui diameter to scan radius diameter)
            Vector3 scaledPosition = relativePosition * (radarUI.rect.width / (scanRadius * 2));
            // create a "ping" on the radar for where the enemy is
            GameObject ping = Instantiate(pingUIPrefab, radarUI);
            ping.transform.localPosition = scaledPosition;
            // start fading the ping out
            ping.GetComponent<Image>().CrossFadeAlpha(0, scanInterval, true);
            // destroy the ping eventually
            Destroy(ping, scanInterval);
        }
    }

}
