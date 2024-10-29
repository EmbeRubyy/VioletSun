using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Keypad : Interactable


{
    public AudioSource src;
    public AudioClip sfx1;
    [SerializeField]
    private GameObject door;
    private bool doorOpen;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    protected override void Interact()
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
        src.clip = sfx1;
        src.Play();
    }
}
