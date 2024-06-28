using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float retreatDistance;
    [SerializeField] private Transform player;

    [SerializeField] private float startTimeBetweenShots;
    [SerializeField] private GameObject projectile;

    private float timeBetweenShots;

    void Start()
    {
        timeBetweenShots = startTimeBetweenShots;
    }

    void Update()
    {
        // If enemy is far away it will move close to the player
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance) {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // If it is near but not to near it will stop moving
        } else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance) {
            transform.position = this.transform.position;
        }

        // If it is to near it will back away
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance) {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);  
        }

        if (timeBetweenShots <= 0) {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
        } else {
            timeBetweenShots -= Time.deltaTime;
        }
    }
}
