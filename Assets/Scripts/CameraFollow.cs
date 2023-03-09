using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;

    private void Update()
    {
        if (Player.active == true)
        {
            Vector3 temp = Player.GetComponent<Transform>().position;
            GetComponent<Transform>().position = new Vector3(temp.x, temp.y, GetComponent<Transform>().position.z);
        }
    }
}
