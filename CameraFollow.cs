using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private BoxCollider2D cameraBox; //This is the BoxCollider of the camera
    private Transform player;//This is the position of the Player
    private BoxCollider2D boundary;

    // Use this for initialization
    void Start()
    {
        cameraBox = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update () {
        AspectRatioBoxChange();
        FollowPlayer();
	}

    void AspectRatioBoxChange()
    {
        //16:10 ratio
        if (Camera.main.aspect >= (1.6f) && Camera.main.aspect < 1.7f)
        {
            cameraBox.size = new Vector2(23f, 14.3f);
        }
        //16:9 ratio
        if (Camera.main.aspect >= (1.7f) && Camera.main.aspect < 1.8f)
        {
            cameraBox.size = new Vector2(25.47f, 14.3f);
        }
        //5:4 ratio
        if (Camera.main.aspect >= (1.25f) && Camera.main.aspect < 1.3f)
        {
            cameraBox.size = new Vector2(18f, 14.3f);
        }
        //4:3 ratio
        if (Camera.main.aspect >= (1.3f) && Camera.main.aspect < 1.4f)
        {
            cameraBox.size = new Vector2(19.13f, 14.3f);
        }
        //3:2 ratio
        if (Camera.main.aspect >= (1.5f) && Camera.main.aspect < 1.6f)
        {
            cameraBox.size = new Vector2(21.6f, 14.3f);
        }
    }

    void FollowPlayer()
    {
        if (GameObject.Find("Boundary"))
        {
            boundary = GameObject.Find("Boundary").GetComponent<BoxCollider2D>();
            transform.position = new Vector3(Mathf.Clamp(player.position.x, boundary.bounds.min.x + (cameraBox.size.x / 2), boundary.bounds.max.x - (cameraBox.size.x / 2)),
                                             Mathf.Clamp(player.position.y, boundary.bounds.min.y + (cameraBox.size.y / 2), boundary.bounds.max.y - (cameraBox.size.y / 2)),
                                             transform.position.z);
        }
    }
}
