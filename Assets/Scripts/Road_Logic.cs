using UnityEngine;

public class Road_Logic : MonoBehaviour
{
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        //CurrentSpeed --> speed for all Road instances
        float speed = ManageRoadSpeed.Instance.CurrentSpeed;
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
