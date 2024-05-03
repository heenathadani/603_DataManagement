using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartListManager : MonoBehaviour
{

    public List<BodyPartData> initialBodyParts;
    // Start is called before the first frame update
    void Start()
    {
        CombatantData.InitializeCharacters(initialBodyParts);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
