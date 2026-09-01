using UnityEngine;
using UnityEngine.UI;
public class Listener : MonoBehaviour
{

    public Text debugLog;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void possessed() 
    {
        debugLog.text += "heard possessed" + "\n";
    }
    public void NewPlayerIn()
    {
        debugLog.text += "heard NewPlayerIn" + "\n";
    }
    public void OldPlayerOut()
    {
        debugLog.text += "heard OldPlayerOut" + "\n";
    }
}
