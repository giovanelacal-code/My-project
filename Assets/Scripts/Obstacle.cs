using UnityEngine;

public class Obstacle : MonoBehaviour 
{
    
    void Start()
    {
        float ramdomSize = Random.Range(1f, 5f);
        transform.localScale = new Vector3( ramdomSize, ramdomSize, 1);
    }
   
    void Update()
    {
        
    }
}
