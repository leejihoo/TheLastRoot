using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(-0.1f, 0, 0) * moveSpeed * Time.deltaTime;
        if(this.transform.position.x <= -19.195)
        {
            this.transform.position = new Vector3(19.195f, 0, 0);
        }
    }
}
