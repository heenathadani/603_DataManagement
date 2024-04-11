using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public TextMeshProUGUI battleLog;
    public int maxQueueLength = 5;

    private Queue<string> battleUpdates;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //could be called from the battle hander to update the combat log ui
    //this is just one way that this could work and can be changed when we have a better idea of how we're doing things
    public void updateBattleLog(int damage, string damageType, string damagerName, string damagedName)
    {
        updateQueueSize();

        //Console.Log(Name.hit(target:name, type:type, points:int))
        battleUpdates.Enqueue("\nConsole.Log(" + damagerName + ".hit(target:" + damagedName + ", type:" + damageType + ", points:" + damage.ToString() + "))");

        battleLog.text = returnLog();
    }

    private void updateQueueSize()
    {
        if(battleUpdates.Count > maxQueueLength)
        {
            battleUpdates.Dequeue();//removes oldest item in battle log
        }
    }

    private string returnLog()
    {
        string log = "";
        foreach(string s in battleUpdates)
        {
            log = log + s;
        }
        return log;
    }
}
