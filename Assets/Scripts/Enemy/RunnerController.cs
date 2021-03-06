using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerController : EnemyController {
    private bool running = false;
    public int runTimer, cooldownTimer;
    float theta;
	int timer = 10;
    // Update is called once per frame
    void FixedUpdate()
    {
        timer--;
        if (!running)
            SlowDown(.92f);
        if (timer <= 0)
        {
            if (running)
            {
                timer = cooldownTimer;
                running = false;
            }
            else
            {
                timer = runTimer;
                if (GameController.Instance.player != null && CloseToPlayer(findDistance))
                {
                    theta = pickAngle();
                    MoveRadian(theta);
                    GetAnimator().SetFloat("X", Mathf.Cos(theta));
                    GetAnimator().SetFloat("Y", Mathf.Sin(theta));
                }
                running = true;
            }
        }
    }
}
