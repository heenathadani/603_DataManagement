using System.Collections.Generic;
using UnityEngine;

namespace Combatant
{
    public enum CombatantType
    {
        ALLIES,
        ENEMIES
    }

    public abstract class aCombatant
    {
        public abstract string Name { get; }
    }

    public class Protagonist : aCombatant
    {
        private string _name;

        //Below are the combat related attributes -- Rin

        //Store the body part lists in the inventory 
        public List<BodyPart> _bodyPartsInventory = new List<BodyPart>();

        private float _maxHp;
        public float _currentHp;

        private float _maxEnergy;
        public float _currentEnergy;

        public float _attackPoint;
        public float _shieldPoint;
        //End

        public Protagonist(string name) {
            _name = name;
        }

        public override string Name
        {
            get 
            {
                return _name;
            }
        }

        //Add body part to an protagonist's inventory -- Rin
        public void AddBodyPart(BodyPart bp)
        {
            _bodyPartsInventory.Add(bp);
        }


        //Update the status (HP) -- Rin
        public void UpdateStatus()
        {
            //Calculate status -- Rin
            foreach (BodyPart bd in _bodyPartsInventory)
            {
                //Update Hp
                _maxHp += bd.bodyPartData.maxHp;
                _currentHp += bd.currentHp;

                //Update attack point
                _attackPoint += bd.bodyPartData.attackPoint;

                //Update shield point
                _shieldPoint += bd.bodyPartData.shieldPoint;

                //Debug.Log("Max HP: " + _maxHp + ", Current HP: " + _currentHp);
            }
        }
    }

    public class Enemy : aCombatant
    {
        private string _name;
        public Enemy(string name)
        {
            _name = name;
        }

        public override string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
