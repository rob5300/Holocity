using UnityEngine;

namespace HandObject
{
    public class Hand : MonoBehaviour
    {
        public HandDetection handDetection;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Hand>())
                handDetection.OpenMainMenu(gameObject);
        }
    }
}
