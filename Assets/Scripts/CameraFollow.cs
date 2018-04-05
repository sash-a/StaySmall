
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        var playerPos = player.GetComponent<Transform>().position;
        transform.position = new Vector3(playerPos.x, playerPos.y, -10);

        // only for camera 1
        var lr = GetComponentInChildren<LineRenderer>();
        if (lr != null)
        {
            lr.SetPosition(0, new Vector3(-30, 15 + playerPos.y, -1));
            lr.SetPosition(1, new Vector3(100, 15 + playerPos.y, -1));
        }
    }
}