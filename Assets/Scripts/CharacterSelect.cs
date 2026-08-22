using Alteruna.Multiplayer;
using UnityEngine;


public class CharacterSelect : CommunicationBridge
{
    public Transform[] Characters;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //based on who's in the scene, change to another, quick and dirty;
        Transform amy;
        amy = GameObject.Find("amy").transform;
        Transform claire;
        claire = GameObject.Find("claire").transform;

        int c = Multiplayer.GetUsers().Count;
        Debug.Log("players " +  c);
        if (c == 2)
        {
            //child amy
            amy.parent = transform;
            amy.position = Vector3.zero;
            
        }
        else 
        {
            //child clair
            claire.parent = transform;
            claire.position = Vector3.zero;

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
