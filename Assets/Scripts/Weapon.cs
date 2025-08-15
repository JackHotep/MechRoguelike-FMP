using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    public WeaponType weaponType;
    public ElementType elementType;
    public string label;
    public string description;
    public GameObject model;
    public Vector3 spawnPoint;
    public Vector3 spawnRot;

    //public ImpactType impactType;

    public WeaponConfig weaponConfig;
    public TrailConfig trailConfig;

    private MonoBehaviour activeBehaviour;
    private GameObject modelInst;
    private float lastShot;
    private ParticleSystem shotSyst;
    private ObjectPool<TrailRenderer> trailPool;

    private void Spawn(Transform parent, MonoBehaviour activeBehaviour)
    {
        this.activeBehaviour = activeBehaviour;
        lastShot = 0;
        trailPool = new ObjectPool<TrailRenderer>(SpawnTrail);
        modelInst = Instantiate(model);
        modelInst.transform.SetParent(parent, false);
        modelInst.transform.localPosition = spawnPoint;
        modelInst.transform.localRotation = Quaternion.Euler(spawnRot);

        shotSyst = modelInst.GetComponentInChildren<ParticleSystem>();
    }

    public void Shoot()
    {
        if (Time.time > weaponConfig.fireRate + lastShot)
        {
            lastShot = Time.time;
            shotSyst.Play();
            Vector3 shootDir = shotSyst.transform.forward +
                               new Vector3(Random.Range(-weaponConfig.spread.x, weaponConfig.spread.x),
                                   Random.Range(-weaponConfig.spread.y, weaponConfig.spread.y),
                                   Random.Range(-weaponConfig.spread.z, weaponConfig.spread.z));
            shootDir.Normalize();

            if (Physics.Raycast(shotSyst.transform.position, shootDir, out RaycastHit hit, float.MaxValue,
                    weaponConfig.targetMask)) { activeBehaviour.StartCoroutine(FireTrail(shotSyst.transform.position, hit.point, hit)); }
            else { activeBehaviour.StartCoroutine(FireTrail(shotSyst.transform.position, shotSyst.transform.position + (shootDir * trailConfig.flyDist), new RaycastHit())); }
        }
    }

    private IEnumerator FireTrail(Vector3 firePoint, Vector3 hitPoint, RaycastHit hit)
    {
        TrailRenderer inst = trailPool.Get();
        inst.gameObject.SetActive(true);
        inst.transform.position = firePoint;
        yield return null; // avoids visual bug

        inst.emitting = true;

        float dist = Vector3.Distance(firePoint, hitPoint);
        float distLeft = dist;
        while (distLeft > 0)
        {
            inst.transform.position = Vector3.Lerp(firePoint, hitPoint, Mathf.Clamp01(1 - (distLeft / dist)));
            distLeft -= trailConfig.tracerSpeed * Time.deltaTime;
            yield return null;
        }

        inst.transform.position = hitPoint;

        if (hit.collider != null)
        {
            //ImpactManager.Instance.Impact(hit.transform.gameObject, hitPoint, hit.normal, ImpactType, 0);
        }
        
        yield return new WaitForSeconds(trailConfig.duration);
        yield return null;
        inst.emitting = false;
        inst.gameObject.SetActive(false);
        trailPool.Release(inst);
    }
    
    private TrailRenderer SpawnTrail()
    {
        GameObject inst = new GameObject("Tracer");
        TrailRenderer trail = inst.AddComponent<TrailRenderer>();
        
        trail.colorGradient = trailConfig.colour;
        trail.material = trailConfig.material;
        trail.widthCurve = trailConfig.widthAnim;
        trail.time = trailConfig.duration;
        trail.minVertexDistance = trailConfig.minVert;
        
        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }

    //public int damageDealt;
    //public int ammoCount;

}
