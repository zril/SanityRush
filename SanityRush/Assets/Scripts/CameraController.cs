using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;

    public int pixPerUnit;
    public float zoom;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;

        // set orthographic size for pixel rendering
        Camera camera = GetComponent<Camera>();
        camera.orthographicSize = Screen.height / (2.0f * zoom * pixPerUnit);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
