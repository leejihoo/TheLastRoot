using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laboratory : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 fpos;
    private bool istrue;
    void Start()
    {
        Invoke("moveLabo",83f);
        fpos = new Vector2(this.transform.position.x - 10f, this.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (istrue == true)
        {
            this.gameObject.transform.position = Vector2.Lerp(transform.position, fpos, 0.5f * Time.deltaTime);
        }
    }

    void moveLabo()
    {
        istrue = true;
    }
}
