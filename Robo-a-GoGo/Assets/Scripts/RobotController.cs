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
	Posing = 5
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
    private Sprite[] spriteSheet;

    private float lockTime;

    private Transform pos;
    private float xpos;



    // Start is called before the first frame update
    void Start()
    {
        lockTime = 0f;
        state = RobotState.Idle;
        pos = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == RobotState.Idle)
        {
            xpos = pos.position.x;
            xpos += Input.GetAxis("Horizontal" + robotNumber.ToString()) * Time.deltaTime;
            pos.position = new Vector3(xpos, pos.position.y, pos.position.z);

            //Check for action commands
            if (Input.GetButtonDown("Dodge" + robotNumber.ToString()))
            {
                state = RobotState.Dodging;
                lockTime = 1f;
                TriggerAnimation();
            }
            else if (Input.GetButtonDown("Punch" + robotNumber.ToString()))
            {
                state = RobotState.Punching;
                lockTime = 1f;
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
