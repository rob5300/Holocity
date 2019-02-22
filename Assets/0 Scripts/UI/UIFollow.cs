using UnityEngine;

public class UIFollow : MonoBehaviour {

    Transform player;

    private void Awake()
    {
        player = Camera.main.transform;
    }

    void Update ()
    {
        FollowPlayer();
	}

    void FollowPlayer()
    {

    }
}
