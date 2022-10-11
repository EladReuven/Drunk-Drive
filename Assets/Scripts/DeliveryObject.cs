using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryObject : MonoBehaviour
{
    public GameObject[] objects;
    private GameObject currentObject;

    private void Start()
    {
        int rand = Random.Range(0, objects.Length);
        currentObject = Instantiate(objects[rand], this.transform.position, objects[rand].transform.rotation, this.transform);
    }

    [ContextMenu("Swap Object")]
    public void SwapObject()
    {
        Destroy(currentObject);
        int rand = Random.Range(0, objects.Length);
        currentObject = Instantiate(objects[rand], this.transform.position, objects[rand].transform.rotation, this.transform);
    }
}
