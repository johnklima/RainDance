using Alteruna.Multiplayer;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSelect : CommunicationBridge
{
    public Transform[] Characters;
    public Text debug;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        int c = Multiplayer.GetUsers().Count;
        debug.text = ("players " + c);


        Transform av = GameObject.Instantiate(Characters[c-1]);
            
        av.parent = transform;
        av.position = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
