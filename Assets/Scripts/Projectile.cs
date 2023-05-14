using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float speed;

    [SerializeField] float timeInScene;

    [SerializeField] GameObject impactEffect;

    [SerializeField] MechController[] hitmech;

    public MechController ownerMech;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HandleRemove(timeInScene));
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.GetComponentInParent<MechController>())
        {
            MechController hitMech = other.gameObject.GetComponentInParent<MechController>();
            if (hitMech == ownerMech) { return; }
            hitMech.TakeDamager(damage);
        }
        
        Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator HandleRemove(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

}
