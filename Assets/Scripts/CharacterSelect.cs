using Alteruna.Multiplayer;
using Alteruna.Multiplayer.Core;
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
    public override void Possessed(bool isMe, User user)
    {
        // disables this script for remote players
        enabled = isMe;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!avatar.IsMe)
            return;

        int c = Multiplayer.GetUsers().Count;
        debug.text += ("players: " + c) +"\n";


    }
    public void ChangeMe(int which)
    {
        SpawnCharacter(which);
     
        BroadcastRemoteMethod("SpawnCharacter",which);
    }
    [SynchronizableMethod]  //is zero, it's first
    void SpawnCharacter(int which)
    {

        Debug.Log("hello SPAWN");
        {
            
            avatarChild.OverwritePrefab(avatarChild.Prefabs[which]);
        }

        return;

        //int c = Multiplayer.GetUsers().Count;
 
        //GameObject avy = spawner.Spawn(c - 1);

        //avy.transform.parent = avatar.transform;
        //avy.transform.position = Vector3.zero;

    }
    void InstanceCharacter() 
    {
        int c = Multiplayer.GetUsers().Count;

        Transform av = GameObject.Instantiate(Characters[c - 1]);

        av.parent = transform;
        av.position = Vector3.zero;
    }
}
