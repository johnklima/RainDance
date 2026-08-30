using Alteruna.Multiplayer;
using Alteruna.Multiplayer.Core;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSelect : AttributesSync
{
    public Transform[] Characters;
    public Text debug;
    Alteruna.Multiplayer.Avatar avatar;
    Alteruna.Multiplayer.Spawner spawner;
    UniqueAvatarChild avatarChild;
    private void Awake()
    {
        avatar = GetComponent<Alteruna.Multiplayer.Avatar>();
        spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
        avatarChild = GetComponent<UniqueAvatarChild>();    
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!avatar.IsMe)
            return;

        int c = Multiplayer.GetUsers().Count;
        //debug.text = ("player " + c);

        


        //SpawnCharacter();
        //BroadcastRemoteMethod(0);

    }

    [SynchronizableMethod]
    void SpawnCharacter()
    {
        int c = Multiplayer.GetUsers().Count;
        if (c == 1)
        {
            //GameObject avy = spawner.Spawn(2);
            avatarChild.OverwritePrefab(avatarChild.Prefabs[1]);
        }

        return;
       
        GameObject avy = spawner.Spawn(c - 1);

        avy.transform.parent = avatar.transform;
        avy.transform.position = Vector3.zero;

    }
    void InstanceCharacter() 
    {
        int c = Multiplayer.GetUsers().Count;

        Transform av = GameObject.Instantiate(Characters[c - 1]);

        av.parent = transform;
        av.position = Vector3.zero;
    }
}
