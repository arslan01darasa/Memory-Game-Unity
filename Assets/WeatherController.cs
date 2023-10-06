using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{

    

    

    // Start is called before the first frame update
    void Start()
    {

        
    }

    public void WeatherMan()
    {
    for (int i = 0; i < this.transform.childCount; i++)
     {
         this.transform.GetChild(i).gameObject.SetActive(false);
     }

     this.transform.GetChild(Random.Range(0, this.transform.childCount)).gameObject.SetActive(true);
     this.enabled = false;
        
    }

    public Transform Reference;

    private void FixedUpdate()
    {
        this.transform.position = Reference.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
