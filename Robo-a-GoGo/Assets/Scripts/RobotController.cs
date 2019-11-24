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
    private GameObject punchBox;

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
        punchBox.SetActive(false);
        pos = gameObject.GetComponent<Transform>();
        xpos = pos.position.x;
    }

    public float GetPosition()
    {
        return xpos;
    }

    public RobotState GetState()
    {
        return state;
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
                punchBox.SetActive(true);
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
                punchBox.SetActive(false);
                TriggerAnimation();
            }
        }
    }

    /// <summary>
    /// When it is detected that players have interacted with one of the 5 options, they both determine their results here.
    /// </summary>
    private void ResolveInteraction()
    {
        switch (opponent.GetState())
        {
            case RobotState.Punching:
                if (state == RobotState.Dodging || state == RobotState.Shooting)
                {
                    lockTime = 1f;
                    GainStyle();
                }
                else if (state == RobotState.Posing || state == RobotState.Reflecting || state == RobotState.Idle)
                {
                    lockTime = 1f;
                    state = RobotState.Hurt;
                    TriggerAnimation();
                }
                else if (state == RobotState.Punching)
                {
                    punchBox.SetActive(false);
                    //Play impact sound
                }
                break;
            case RobotState.Shooting:
                if (state == RobotState.Dodging)
                {
                    lockTime = 1f;
                    GainStyle();
                }
                else if (state == RobotState.Reflecting)
                {
                    //REVERSE SHOT
                    lockTime = 1f;
                    GainStyle();
                }
                else if (state == RobotState.Posing || state == RobotState.Punching || state == RobotState.Idle)
                {
                    lockTime = 1f;
                    state = RobotState.Hurt;
                    TriggerAnimation();
                }
                break;
            case RobotState.Dodging:
                if (state == RobotState.Reflecting || state == RobotState.Posing)
                {
                    lockTime = 1f;
                    GainStyle();
                }
                else if (state == RobotState.Shooting || state == RobotState.Punching)
                {
                    lockTime = 1f;
                }
                break;
            case RobotState.Reflecting:
                if (state == RobotState.Posing || state == RobotState.Punching)
                {
                    lockTime = 1f;
                    GainStyle();
                }
                else if (state == RobotState.Dodging)
                {
                    lockTime = 1f;
                }
                else if (state == RobotState.Shooting)
                {
                    lockTime = 1f;
                    state = RobotState.Hurt;
                    TriggerAnimation();
                }
                break;
            case RobotState.Posing:
                if (state == RobotState.Punching || state == RobotState.Shooting)
                {
                    lockTime = 1f;
                    GainStyle();
                }
                else if (state == RobotState.Reflecting || state == RobotState.Dodging)
                {
                    lockTime = 1f;
                }
                break;
            case RobotState.Idle:
                if (state == RobotState.Punching || state == RobotState.Shooting)
                {
                    lockTime = 1f;
                    GainStyle();
                }
                break;
            case RobotState.Hurt:
                lockTime = 1f;
                GainStyle();
                break;
        }
    }

    private void GainStyle()
    {
        Debug.Log("Player " + robotNumber.ToString() + " gains style!");
        //GOOD STUFF
    }

    private void OnTriggerEnter(Collider other)
    {
        ResolveInteraction();
    }
    void TriggerAnimation()
    {
        sprite.sprite = spriteSheet[(int)state];
    }
}
