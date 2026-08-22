using Alteruna.Multiplayer;
using Alteruna.Multiplayer.Core;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSelect : CommunicationBridge
{
    public Transform[] Characters;
    public Text debug;
    Alteruna.Multiplayer.Avatar avatar;
    Alteruna.Multiplayer.Spawner spawner;

    private void Awake()
    {
        avatar = GetComponent<Alteruna.Multiplayer.Avatar>();
        spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!avatar.IsMe)
            return;

        int c = Multiplayer.GetUsers().Count;
        debug.text = ("players " + c);

        SpawnCharacter();       

    }
    void SpawnCharacter()
    {
        int c = Multiplayer.GetUsers().Count;
        GameObject avy = spawner.Spawn(c - 1);

        avy.transform.parent = avatar.transform;

    }
    void InstanceCharacter() 
    {
        int c = Multiplayer.GetUsers().Count;

        Transform av = GameObject.Instantiate(Characters[c - 1]);

        av.parent = transform;
        av.position = Vector3.zero;
    }
}
