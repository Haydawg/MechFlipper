using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float speed;

    [SerializeField] float timeInScene;

    [SerializeField] GameObject impactEffect;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HandleRemove(timeInScene));
    }

    // Update is called once per frame

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.GetComponentInParent<MechController>())
        {
            MechController hitMech = collision.gameObject.GetComponentInParent<MechController>();
            hitMech.TakeDamager(damage);
        }
        ContactPoint contact = collision.contacts[0];
        Instantiate(impactEffect, contact.point, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator HandleRemove(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

}
