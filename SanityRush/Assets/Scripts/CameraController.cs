using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject player;
    private Vector3 offset;

    public int pixPerUnit;
    public float zoom;

    // Use this for initialization
    void Start()
    {
        offset = new Vector3(0, 0, -10);

        // set orthographic size for pixel rendering
        Camera camera = GetComponent<Camera>();
        camera.orthographicSize = Screen.height / (2.0f * zoom * pixPerUnit);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
