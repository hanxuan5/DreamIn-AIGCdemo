using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    public float Speed;
    private Vector3 _pos;

    private void Update()
    {
        if (Player.active == true)
        {
            _pos = new Vector3(Player.transform.position.x, Player.transform.position.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, _pos, Speed);
        }
    }
}
