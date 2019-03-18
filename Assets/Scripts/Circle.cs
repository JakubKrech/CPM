using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Circle : MonoBehaviour
{
    //public CPMLogic CPMData;
    public Vector3 leftAnchor, rightAnchor;
    public Text Left, Right, Top, Bottom;
    public SpriteRenderer circleSprite;

    void Start()
    {
        leftAnchor = new Vector3(this.transform.position.x - 20, this.transform.position.y, this.transform.position.z);
        rightAnchor = new Vector3(this.transform.position.x + 20, this.transform.position.y, this.transform.position.z);
    }
}
