using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAction
{
    public string ActionName;
    public Func<IEnumerator> ActionCoroutine;
    public float ActionDelay;

    public BossAction(string actionName, Func<IEnumerator> actionCoroutine, float actionDelay)
    {
        ActionName = actionName;
        ActionCoroutine = actionCoroutine;
        ActionDelay = actionDelay;
    }
}

public class FakeBossBehaviour : MonoBehaviour
{
    private int currentActionIndex = 0;
    private List<BossAction> bossActions;

    void Start()
    {
        // Initialize boss actions with coroutines for layered attack patterns
        bossActions = new List<BossAction>
        {
            new BossAction("FireSpreadBullet", FireSpreadBullet, 3f),
            new BossAction("FocusedShotgunBlast", FocusedShotgunBlast, 3f),
            new BossAction("CircularBarrage", SpinningBarrage, 3f),
            new BossAction("BulletRain", BulletRain, 3f)
        };

        PerformBossAction(currentActionIndex);
    }

    void PerformBossAction(int actionIndex)
    {
        BossAction currentAction = bossActions[actionIndex];
        StartCoroutine(PerformBossActionCoroutine(currentAction));
    }

    IEnumerator PerformBossActionCoroutine(BossAction currentAction)
    {
        // Execute the current action (layered shot pattern) as a coroutine
        yield return StartCoroutine(currentAction.ActionCoroutine());

        // Wait for the specified delay before the next action
        yield return new WaitForSeconds(currentAction.ActionDelay);

        // Move to the next action
        currentActionIndex = (currentActionIndex + 1) % bossActions.Count;
        PerformBossAction(currentActionIndex);
    }

    //-------------------------------------------------- Layered Attack Pattern Example --------------------------------------------------
    IEnumerator FireSpreadBullet()
    {
        Debug.Log("Layer 1: Fire Spread Bullet");

        // Layer 1: Initial spread
        float[] anglesLayer1 = { 135, 145, 155, -135, -145, -155 };
        foreach (float angle in anglesLayer1)
        {
            ObjectPoolManager.instance.SpawnBullet("Test", transform.position, Quaternion.Euler(0, 0, angle));
        }

        // Wait between layers
        yield return new WaitForSeconds(0.4f); // Delay before Layer 2

        Debug.Log("Layer 2: Fire Spread Bullet");

        // Layer 2: Another spread with different angles
        foreach (float angle in anglesLayer1)
        {
            ObjectPoolManager.instance.SpawnBullet("Test", transform.position, Quaternion.Euler(0, 0, angle));
        }

        // You can add more layers by adding more shooting sequences here
        yield return new WaitForSeconds(0.1f); // Delay before Layer 3 (optional)

        // End of the current action
        yield return null;
    }

    IEnumerator FocusedShotgunBlast()
    {
        Debug.Log("Focused Shotgun Blast");

        // Fire 5 bullets in a tight spread
        for (int i = -2; i <= 2; i++)
        {
            float angle = 10 * i; // Adjust spread angle
            ObjectPoolManager.instance.SpawnBullet("Test", transform.position, Quaternion.Euler(0, 0, angle));
        }

        yield return new WaitForSeconds(0.5f); // Delay before the next action
    }

    IEnumerator SpinningBarrage()
    {
        Debug.Log("Circular Barrage");

        int bulletCount = 50; // Number of bullets in the circle
        int currentAngle = 0;
        for (int i = 0; i < bulletCount; i++)
        {
            
            ObjectPoolManager.instance.SpawnBullet("Test", transform.position, Quaternion.Euler(0, 0, currentAngle));
            currentAngle += 12; // Increase angle for the next bullet
            yield return new WaitForSeconds(0.02f); // Delay between each bullet
        }

        yield return new WaitForSeconds(0.6f); // Delay before the next action
    }

    IEnumerator BulletRain()
    {
        Debug.Log("Bullet Rain");

        int bulletCount = 10; // Number of bullets
        for (int i = 0; i < bulletCount; i++)
        {
            float randomX = UnityEngine.Random.Range(-5f, 5f); // Random X position
            ObjectPoolManager.instance.SpawnBullet("Test", new Vector3(randomX, transform.position.y + 5, 0), Quaternion.identity);
        }

        yield return new WaitForSeconds(0.8f); // Delay before the next action
    }

}
