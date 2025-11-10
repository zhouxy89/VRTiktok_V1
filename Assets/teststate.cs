using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teststate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GetComponent<Animator>().Play("ShakingExpressionFast", 0, 0f);
    }

}
