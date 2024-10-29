using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;
    public string sfxPath = "Audio/SoundEffects/laser_01"; // Adjust this to your sound file's path
    private AudioClip sfx1;

    public override void Enter()
    {
        // Load the sound effect when entering the state
        sfx1 = Resources.Load<AudioClip>(sfxPath);
        if (sfx1 == null)
        {
            Debug.LogWarning("AudioClip not found at " + sfxPath);
        }
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);

            if (shotTimer > enemy.fireRate)
            {
                Shoot();
            }

            if (moveTimer > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
            enemy.LastKnowPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                // Change to search state
                stateMachine.ChangeState(new SearchState());
            }
        }
    }

    public void Shoot()
    {
        // Store reference to gun barrel
        Transform gunbarrel = enemy.gunBarrel;

        // Instantiate new bullet
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunbarrel.position, enemy.transform.rotation);

        // Calculate direction to player
        Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.transform.position).normalized;

        // Add force to the rigidbody of the bullet
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 40;

        // Log shoot event
        Debug.Log("Shoot");

        // Play the shooting sound
        PlayShootingSound();

        // Reset the shot timer
        shotTimer = 0;
    }

    private void PlayShootingSound()
    {
        // Get the AudioSource from the enemy object
        AudioSource audioSource = enemy.GetComponent<AudioSource>();
        if (audioSource != null && sfx1 != null)
        {
            audioSource.clip = sfx1; // Set the audio clip
            audioSource.Play(); // Play the sound effect
        }
        else
        {
            Debug.LogWarning("AudioSource is not found on enemy or AudioClip is null.");
        }
    }

    public override void Exit()
    {
        // Optional: Reset audio source or other cleanup if necessary
    }
}
