using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    public float damageRate;
    public int damage;

    private List<IDamageable> things = new List<IDamageable>();
    private WaitForSeconds dealWait;
    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        dealWait = new WaitForSeconds(damageRate);
    }

    private void OnEnable()
    {
        coroutine = StartCoroutine(DealDamage());
    }

    private void OnDisable()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    IEnumerator DealDamage()
    {
        while (true)
        {
            for (int i = 0; i < things.Count; i++)
            {
                things[i].TakePhysicalDamage(damage);
            }
            yield return dealWait;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            things.Add(damageable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            things.Remove(damageable);
        }
    }
}