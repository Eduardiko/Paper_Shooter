using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : Mob
{

    public void SetPhase(int phase)
    {
        switch (phase)
        {
            case 1:
                break;
            case 2:
                health = 50;
                foreach (Transform child in transform)
                {
                    Pufferfish pufferChild = child.GetComponent<Pufferfish>();

                    if(pufferChild != null)
                        pufferChild.gameObject.SetActive(true);
                }
                break;
            default:
                health = 100;
                foreach (Transform child in transform)
                {
                    CrabClaw crabClawChild = child.GetComponent<CrabClaw>();
                    Pufferfish pufferChild = child.GetComponent<Pufferfish>();

                    if (pufferChild != null)
                        pufferChild.gameObject.SetActive(true);
                    else if (crabClawChild != null)
                        crabClawChild.gameObject.SetActive(true);
                }
                break;
        }
    }

    public override void Die()
    {
        foreach (Transform child in transform)
        {
            Mob childMob = child.GetComponent<Mob>();

            if(childMob != null && childMob.gameObject.activeSelf)
                childMob.Die();
        }

        ThrowDeadAway();
    }
}
