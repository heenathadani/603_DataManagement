using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BattleFeedback : MonoBehaviour
{
    public TextMeshProUGUI battleLog;

    /*public TextMeshProUGUI eDamage1;
    public TextMeshProUGUI eDamage2;
    public TextMeshProUGUI eDamage3;
    public TextMeshProUGUI pDamage1;
    public TextMeshProUGUI pDamage2;
    public TextMeshProUGUI pDamage3;*/

    public int maxQueueLength = 2;

    private Queue<string> battleUpdates = new Queue<string>();
    private TextMeshProUGUI[] eDamageArray;
    private TextMeshProUGUI[] pDamageArray;

    // Start is called before the first frame update
    void Start()
    {
        //eDamageArray = new TextMeshProUGUI[] { eDamage1, eDamage2, eDamage3 };
        //pDamageArray = new TextMeshProUGUI[] { pDamage1, pDamage2, pDamage3 };

        //test
        //updateBattleLog(4, "fire", "bot1", "bot2", 0, "p");
        //updateBattleLog(2, "force", "bot2", "bot1", 2, "e");
        //updateBattleLog(8, "slashing", "bot3", "bot2", 0, "p");
        clearAll();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //could be called from the battle hander to update the combat log ui
    //this is just one way that this could work and can be changed when we have a better idea of how we're doing things
    public void updateBattleLog(int damage, string damageType, string damagerName, string damagedName/*, int index, string targetType*/)
    {
        updateQueueSize();
        //clearAllFeedback();

        //Console.Log(Name.hit(target:name, type:type, points:int))
        battleUpdates.Enqueue("<" + damagerName + "> hit [" + damagedName + "] with " + damageType + " for " + damage.ToString() + " points\n");

        battleLog.text = returnLog();

        //hitFeedback(damage, index, targetType);
    }

    public void clearAll()
    {
        battleUpdates.Clear();
        battleLog.text = "";
        //clearAllFeedback();
    }

    private void updateQueueSize()
    {
        if (battleUpdates.Count > maxQueueLength - 1)
        {
            battleUpdates.Dequeue();//removes oldest item in battle log
        }

    }

    private string returnLog()
    {
        string log = "";
        foreach (string s in battleUpdates)
        {
            log = log + s;
        }
        return log;
    }

    private void hitFeedback(int damage, int i, string targetType)
    {
        //returnArray(targetType)[i].text = damage.ToString();
    }

    private void clearAllFeedback()
    {
        clearFeedbackArray(eDamageArray);
        clearFeedbackArray(pDamageArray);
    }

    private void clearFeedbackArray(TextMeshProUGUI[] feedback)
    {
        foreach (TextMeshProUGUI f in feedback)
        {
            f.text = "";
        }
    }

    private TextMeshProUGUI[] returnArray(string type)
    {
        if (type == "p")
        {
            return pDamageArray;
        }
        else
        {
            return eDamageArray;
        }
    }
}
