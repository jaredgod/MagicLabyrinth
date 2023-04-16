using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;
    private float ypos;

    // Start is called before the first frame update
    void Start()
    {
        ypos = transform.position.y;
        Screen.SetResolution(1920, 1080, false);
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move()
    {
        float zpos = transform.position.z;
        float xpos = transform.position.x;

        float speedUp = Input.GetKey(KeyCode.LeftShift) ? 2.0f : 1.0f;

        if (Input.GetKey(KeyCode.W)) zpos += speedUp * cameraSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) xpos += speedUp * cameraSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) zpos -= speedUp * cameraSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) xpos -= speedUp * cameraSpeed * Time.deltaTime;

        transform.position = new Vector3(xpos, ypos, zpos);
    }
}
