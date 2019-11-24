using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int ownerNumber;
    private Transform pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = gameObject.GetComponent<Transform>();
    }

    public void SetOwner(int owner)
    {
        ownerNumber = owner;
    }

    public int GetOwner()
    {
        return ownerNumber;
    }

    // Update is called once per frame
    void Update()
    {
        if (ownerNumber == 1)
        {
            pos.position = new Vector3(pos.position.x + 10f*Time.deltaTime, pos.position.y, pos.position.z);
        }
        else
        {
            pos.position = new Vector3(pos.position.x - 10f*Time.deltaTime, pos.position.y, pos.position.z);
        }
    }
}
