using UnityEngine;

public class TestListener : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoIt() 
    {
        Debug.Log("Listen Did It");
    }
    public void UnDoIt()
    {
        Debug.Log("Listen Un-Did It");
    }

    public void NewPlayer()
    {
        Debug.Log("New Player in");
    }
    public void OldPlayer()
    {
        Debug.Log("Old Player out");
    }
}
