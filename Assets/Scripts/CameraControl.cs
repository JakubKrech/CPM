using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            this.transform.position = new Vector3(this.transform.position.x + 0.06f, this.transform.position.y, this.transform.position.z); 
        }
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            this.transform.position = new Vector3(this.transform.position.x - 0.06f, this.transform.position.y, this.transform.position.z); 
        }
    }
}
