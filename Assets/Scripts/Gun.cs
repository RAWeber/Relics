using UnityEngine;
using System.Collections;

public class Gun : Weapon {

    public enum FireMode { Auto, Burst, Single };
    public FireMode fireMode;
    public AnimationCurve accuracyCurve;
    public float maxInaccuracy;

    public Transform[] projectileSpawn;
    public Projectile projectile;
    public float timeBetweenShots = 100;
    public float shotSpeed = 35;
    public int burstCount;
    public int projectilesPerMag;
    public float reloadTime = .3f;

    float nextShotTime = 0;
    int shotsRemainingInBurst;
    int projectilesRemainingInMag;
    bool isReloading = false;

    void Start()
    {
        shotsRemainingInBurst = burstCount;
        projectilesRemainingInMag = projectilesPerMag;
    }

    void LateUpdate()
    {
        if (!isReloading && projectilesRemainingInMag == 0)
        {
            Reload();
        }
    }

    void Shoot()
    {

        if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMag > 0)
        {
            if (fireMode == FireMode.Burst)
            {
                if (shotsRemainingInBurst == 0)
                {
                    return;
                }
                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single)
            {
                if (!mainTriggerReleased)
                {
                    return;
                }
            }

            for (int i = 0; i < projectileSpawn.Length; i++)
            {
                if (projectilesRemainingInMag == 0)
                {
                    break;
                }
                projectilesRemainingInMag--;
                nextShotTime = Time.time + timeBetweenShots;
                Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                newProjectile.SetSpeed(shotSpeed);
                newProjectile.transform.parent = transform;
            }
        }
    }

    public void Reload()
    {
        if (!isReloading && projectilesRemainingInMag != projectilesPerMag)
        {
            StartCoroutine(AnimateReload());

        }
    }

    IEnumerator AnimateReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(.2f);

        float reloadSpeed = 1f / reloadTime;
        float percent = 0;
        Vector3 initialRot = transform.localEulerAngles;
        float maxReloadAngle = 50;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRot + Vector3.left * -reloadAngle;

            yield return null;
        }

        isReloading = false;
        projectilesRemainingInMag = projectilesPerMag;
    }

    public override void MainTriggerHold()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (secondaryTriggerRealeased)
        {
            if (Physics.Raycast(ray, out hit, 1000, layer))
            {
                transform.LookAt(hit.point + Random.insideUnitSphere * accuracyCurve.Evaluate(hit.distance / 10) * maxInaccuracy);
            }
            else
            {
                transform.LookAt((Camera.main.transform.forward + Random.insideUnitSphere * maxInaccuracy / 10) * 100);
            }
        }
        Shoot();
        base.MainTriggerHold();
    }

    public override void MainTriggerRelease()
    {
        shotsRemainingInBurst = burstCount;
        base.MainTriggerRelease();
    }

    public override void SecondaryTriggerHold()
    {
        StopCoroutine("Hold");
        StartCoroutine("Aim");
        base.SecondaryTriggerHold();
    }

    public override void SecondaryTriggerRelease()
    {
        StopCoroutine("Aim");
        StartCoroutine("Hold");
        base.SecondaryTriggerRelease();
    }

    IEnumerator Aim()
    {
        float percent = 0;
        float aimSpeed = 10;

        Transform camera = Camera.main.transform;
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = camera.position + camera.forward * 0.25f;

        while (percent <= 1)
        {
            percent += Time.deltaTime * aimSpeed;
            transform.position = Vector3.Lerp(originalPosition, camera.position + camera.transform.forward * 0.25f, percent);

            yield return null;
        }
        transform.LookAt(camera.forward * 100);
    }

    IEnumerator Hold()
    {
        float percent = 0;
        float aimSpeed = 5;

        Vector3 originalPosition = transform.position;

        while (percent <= 1)
        {
            percent += Time.deltaTime * aimSpeed;
            transform.position = Vector3.Lerp(originalPosition, transform.parent.position, percent);

            yield return null;
        }
    }
}
