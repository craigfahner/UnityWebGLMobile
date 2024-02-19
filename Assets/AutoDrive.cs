using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDrive : MonoBehaviour


{
    public RCC_Inputs newInputs = new RCC_Inputs();
    public float throttleValue = 0.5f;
    public RCC_CarControllerV3 targetVehicle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        newInputs.throttleInput = throttleValue;
        targetVehicle.OverrideInputs(newInputs);
        if(gameObject.transform.position.y < -10.0f)
        {
            Vector3 newPos = gameObject.transform.position;
            newPos.y = 10.0f;
            gameObject.transform.position = newPos;
        }
    }
}
