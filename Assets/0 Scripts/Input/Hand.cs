using UnityEngine;

namespace HandObject
{
    public class Hand : MonoBehaviour
    {
        public HandDetection handDetection;
        string prefabPath = "/HandPrefab";

        private void Start()
        {
            Debug.Log("Hand : " + name + GetComponent<SphereCollider>().isTrigger);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Hand>())
                handDetection.OpenMainMenu(gameObject);
        }
    }
}
