using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    float x;
    float y;
    float z;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.BGMPlay("BGM");
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Mouse ScrollWheel");
        z = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = Vector3.zero;
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(new Vector3(x, 0, z) * 20 * Time.deltaTime);
        transform.Translate(new Vector3(0, -y, 0) * 20);
    }
}
