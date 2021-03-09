using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Standalone_Controller : MonoBehaviour
{
    public static Standalone_Controller instance;

    public GameObject sprDot;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RayHit();
    }


    private void RayHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform objectHit = hit.transform;
            sprDot.transform.position = hit.point;
        }
    }
}
