using UnityEngine;

public class ActivateBoss : MonoBehaviour
{
    public GameObject Boss;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Boss.SetActive(true);
            Destroy(gameObject);
        }
    }
}
