using UnityEngine;
public class GiveMeHelix : MonoBehaviour
{
    public GameObject point; //prefab to create the points that constructs the helix
    public int PointCount = 18; //#points/2 that represents the helix 
    public float DegreeIncerement = 20; //Degree between each point

    public int r = 2; //radius of the helix 
    public float c = 0.5f; //height of the helix
    float angleT; //angle to place the points of the helix

    void Start()
    {   
        for(float i = -PointCount; i < PointCount; i += 1)
        {
            angleT = Mathf.Deg2Rad * i * DegreeIncerement; 
            GameObject g =  Instantiate(point,  transform); //As a child of the game object create helix points
            g.transform.localPosition = new Vector3(r * Mathf.Cos(angleT), c * angleT, r * Mathf.Sin(angleT)); //y oriented helix                  
        }
    }
}
