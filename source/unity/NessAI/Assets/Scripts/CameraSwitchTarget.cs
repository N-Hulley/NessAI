using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchTarget : MonoBehaviour
{
    
    public void clicked()
    {
        ChessManager.Instance.PlayerRig.transform.position = transform.position;

    }
}
