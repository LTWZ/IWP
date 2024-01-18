using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolDoorLogic : MonoBehaviour
{

    public static PistolDoorLogic instance;
    public bool candoorbeopened = false;
    public GameObject door;
    // Start is called before the first frame update

    public static PistolDoorLogic GetInstance()
    {
        return instance;
    }

    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (candoorbeopened == true)
        {
            door.gameObject.SetActive(false);
            candoorbeopened = false;
        }
    }
}
