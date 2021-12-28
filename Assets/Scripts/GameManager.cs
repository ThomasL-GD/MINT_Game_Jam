using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    
    
    //Singleton time ! 	(˵ ͡° ͜ʖ ͡°˵)
    public static GameManager singleton { get; private set; }
        
    /// <summary>
    /// Is that... a singleton setup ?
    /// *Pokédex's voice* A singleton, a pretty common pokécode you can find in a lot of projects, it allows anyone to
    /// call it and ensure there is only one script of this type in the entire scene !
    /// </summary>
    private void Awake() {
        //base.Awake();
        // if the singleton hasn't been initialized yet
        if (singleton != null && singleton != this)
        {
            gameObject.SetActive(this);
            Debug.LogWarning("BROOOOOOOOOOOOOOOOOOO ! There are too many Singletons broda", this);
            return;
        }
 
        singleton = this;
    }
}
