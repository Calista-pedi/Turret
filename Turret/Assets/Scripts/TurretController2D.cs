using UnityEngine;
using System.Collections;

public class TurretController2D : MonoBehaviour
{
    public Transform firePoint;             
    public GameObject bulletPrefab;        
    public Transform target;               

    public float fireInterval = 0.5f;       // 子弹发射间隔
    public float detectDistance = 8f;       
    public float detectAngle = 45f;         

    private Coroutine fireCoroutine = null;

    void Update()
    {
        bool inSight = TargetInSight();

        if (inSight && fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!inSight && fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }


    IEnumerator FireContinuously()
    {
        while (true)
        {
            if (target == null)
            {
                yield break;  // 目标消失了，停止协程发射
            }

            Fire();
            yield return new WaitForSeconds(fireInterval);
        }
    }


    bool TargetInSight()
    {
        if (target == null)
            return false;

        Vector2 dirToTarget = target.position - transform.position;
        float distance = dirToTarget.magnitude;

        if (distance > detectDistance)
            return false;

        float angleToTarget = Vector2.Angle(-transform.up, dirToTarget);
        if (angleToTarget > detectAngle)
            return false;

        return true;
    }

    void Fire()
    {
        if (target == null || firePoint == null)
            return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 dir = (target.position - firePoint.position).normalized;
        bullet.GetComponent<Bullet2D>().direction = dir;
    }

}
