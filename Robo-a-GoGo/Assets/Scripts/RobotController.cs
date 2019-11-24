using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RobotState
{
	Idle = 0,
	Punching = 1,
	Shooting = 2,
	Dodging = 3,
	Reflecting = 4,
	Posing = 5,
    Charging = 6,
    MovingForward = 7,
    MovingBackward = 8,
    Hurt = 9,
    Finisher = 10
}

public class RobotController : MonoBehaviour
{
    [SerializeField]
    private int robotNumber;

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
	private RobotState state;

    [SerializeField]
    private float speed;

    [SerializeField]
    private RobotController opponent;

    [SerializeField]
    private Sprite[] spriteSheet;

    private float lockTime;

    private Transform pos;
    private float xpos;

    private float style = 0f;
    private float power = 100f;

    private AudioSource dodgeSFX;
    public AudioClip[] dodgeSounds;


    // Start is called before the first frame update
    void Start()
    {
        lockTime = 0f;
        state = RobotState.Idle;
        pos = gameObject.GetComponent<Transform>();
        xpos = pos.position.x;
    }

    public float GetPosition()
    {
        return xpos;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == RobotState.Idle)
        {

            //Check for action commands
            if (Input.GetButtonDown("Dodge" + robotNumber.ToString()))
            {
                state = RobotState.Dodging;
                lockTime = 1f;
                TriggerAnimation();
                dodgeSFX.PlayOneShot(dodgeSounds[Random.Range(0, dodgeSounds.Length)]);
            }
            else if (Input.GetButtonDown("Punch" + robotNumber.ToString()))
            {
                //Debug.Log("Punch" + robotNumber.ToString());
                state = RobotState.Punching;
                lockTime = 1f;
                TriggerAnimation();
            }
            else if (Input.GetButtonDown("Flair" + robotNumber.ToString()))
            {
                state = RobotState.Posing;
                lockTime = 1f;
                TriggerAnimation();
            }
            else if (Input.GetButtonDown("Shoot" + robotNumber.ToString()))
            {
                state = RobotState.Shooting;
                lockTime = 1f;
                TriggerAnimation();
            }
            else if (Input.GetButtonDown("Reflect" + robotNumber.ToString()))
            {
                state = RobotState.Reflecting;
                lockTime = 1f;
                TriggerAnimation();
            }
            else if (Input.GetButtonDown("Charge" + robotNumber.ToString()))
            {
                state = RobotState.Charging;
                TriggerAnimation();
            }
            else
            {
                float deltaX = Input.GetAxis("Horizontal" + robotNumber.ToString());
                if (deltaX != 0f)
                {
                    xpos = pos.position.x;
                    xpos += Input.GetAxis("Horizontal" + robotNumber.ToString()) * Time.deltaTime *speed;

                    float enemyPos = opponent.GetPosition();

                    if (robotNumber == 1)
                    {
                        if (xpos + 4f > enemyPos)
                        {
                            xpos = enemyPos - 4f;
                        }
                        else if (xpos < -14f)
                        {
                            xpos = -14f;
                        }
                    }
                    else
                    {
                        if (xpos - 4f < enemyPos)
                        {
                            xpos = enemyPos + 4f;
                        }
                        else if (xpos > 14f)
                        {
                            xpos = 14f;
                        }
                    }

                    pos.position = new Vector3(xpos, pos.position.y, pos.position.z);
                    if (deltaX > 0)
                    {
                        //Move one way
                    }
                    else
                    {
                        //Move the other way
                    }
                }
                else
                {
                    //Idle
                }
            }
        }
        else if (state == RobotState.Charging)
        {
            if (Input.GetButtonDown("Charge" + robotNumber.ToString()))
            {
                state = RobotState.Idle;
                TriggerAnimation();
            }
        }
        else
        {
            lockTime -= Time.deltaTime;
            if (lockTime <= 0f)
            {
                state = RobotState.Idle;
                TriggerAnimation();
            }
        }
    }

    void TriggerAnimation()
    {
        sprite.sprite = spriteSheet[(int)state];
    }
}
