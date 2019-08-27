using UnityEngine;
using UnityEngine.UI;


public enum WalkingMode //walking modes around the helix
{
    NormalWalk = 0,
    RandomDirectionWalk = 1,
    RandomDirectionFloatingWalk = 2
}


public class WalkingOnHelix : MonoBehaviour
{

    public float maxDirChangeTime = 3; //Limits to change direction in random durations
    public float minDirChangeTime= 1;

    public float maxFloatDirTime = 10; //Limits to change floating direction in random durations
    public float minFloatDirTime= 3;
    public float outOfPathSpeed = 5;  //Interpolation speed 
    Vector3 OutOfPathDirection;       //Random direction to float around the helix

    public float randomDirGap = 1.0f; //value gap for creating a random floating direction

    int Direction = 1; //current direction

    public float speed = 5; //speed of walk

    float Angle = 0; //current angle
    float AngleLimit; //max value an angle take to create movement, defines last point of the helix


    public WalkingMode WalkMode = WalkingMode.NormalWalk; //walk mode of the game
    GiveMeHelix GMH; //Helix maker reference to take core helix values 
    Dropdown ModeDropdown; //dropdown ui element to change modes 


    void Start()
    {
        GMH = transform.parent.gameObject.GetComponent<GiveMeHelix>(); //value is taken from parent's HelixMaker component
        AngleLimit = GMH.PointCount * GMH.DegreeIncerement * Mathf.Deg2Rad;
        Angle = -AngleLimit; //walk starts from down to top
        ModeDropdown = FindObjectOfType<Dropdown>();
    }

    void Update() 
    {
        CalculateCurrentAngle(); //current angle is calculated

        switch (WalkMode) //according to walk mode,the object moves
        {
            case WalkingMode.NormalWalk:
            case WalkingMode.RandomDirectionWalk:
                NormalWalk();
                break;
            case WalkingMode.RandomDirectionFloatingWalk:
                RandomDirectionFloatingWalk();
                break;
        }
    }
    void CalculateCurrentAngle()
    {
        Angle += GMH.PointCount * GMH.DegreeIncerement * Mathf.Deg2Rad * speed * Direction * Time.deltaTime;
        if (Angle > AngleLimit || Angle < -AngleLimit) //when the angle limit is exceeded direction is changed
        {
            Direction = -Direction;
        }
    }

    void NormalWalk() //used for normal walking and random direction walking on helix 
    {
        transform.localPosition = new Vector3(GMH.r * Mathf.Cos(Angle), GMH.c * Angle, GMH.r * Mathf.Sin(Angle));
    }

    void RandomDirectionFloatingWalk() //while float walking 
    {
        Vector3 helixMove = new Vector3(GMH.r * Mathf.Cos(Angle), GMH.c * Angle, GMH.r * Mathf.Sin(Angle));
        transform.localPosition = Vector3.Lerp(transform.localPosition, helixMove + OutOfPathDirection, Time.deltaTime * outOfPathSpeed);
    }


    void ChangeDirection() //direction changed and a random duration to change it again is calulated
    {
        Direction = -Direction;
        float nextTime = Random.Range(minDirChangeTime, maxDirChangeTime);
        Invoke("ChangeDirection", nextTime);
    }

    void ChangeFloatingDirection() //new direction is calculated to give floating effect the game object
    {
        OutOfPathDirection = new Vector3(Random.Range(-randomDirGap, randomDirGap), Random.Range(-randomDirGap, randomDirGap), Random.Range(-randomDirGap, randomDirGap));
        float nextTime = Random.Range(minFloatDirTime, maxFloatDirTime);
        Invoke("ChangeFloatingDirection", nextTime);
    }

    public void ChangeWalkingMode() //walking mode is changed with change of the dropdown's value change
    {
        WalkMode = (WalkingMode)ModeDropdown.value;
        CancelInvoke("ChangeDirection");  //When mode changes, current invokes are cancelled
        CancelInvoke("ChangeFloatingDirection");
        switch (WalkMode) //With the mode new invokes are called
        {
            case WalkingMode.NormalWalk:
                break;
            case WalkingMode.RandomDirectionWalk:
                Invoke("ChangeDirection", 0);
                break;
            case WalkingMode.RandomDirectionFloatingWalk:
                Invoke("ChangeFloatingDirection", 0);
                break;
        }
    }
}
