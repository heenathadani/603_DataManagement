using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        private int _id;

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

        //Bind to UI hp slider
        private Slider _hpSlider;

        public Protagonist(string name, int id) {
            _name = name;
            _id = id;

            CombatUIManager cum = GameObject.FindAnyObjectByType<CombatUIManager>();
            _hpSlider = cum.characterHpSliderList[id];     
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
            }

            Debug.Log("Character Id:" + _id + "Max HP: " + _maxHp + ", Current HP: " + _currentHp);

            //Update Slider
            _hpSlider.value = _currentHp / _maxHp;
        }
    }

    public class Enemy : aCombatant
    {
        private string _name;
        private int _id;

        //Store the body part lists in the inventory -- Rin
        public List<BodyPart> _bodyPartsInventory = new List<BodyPart>();

        //Enemy Attributes -- Rin
        private float _maxHp;
        public float _currentHp;

        public float _attackPoint;

        //Bind to UI hp slider -- Rin
        private Slider _hpSlider;

        public Enemy(string name, int id)
        {
            _name = name;
            _id = id;

            CombatUIManager cum = GameObject.FindAnyObjectByType<CombatUIManager>();
            _hpSlider = cum.enemyHpSliderList[id];
        }

        public void AddBodyPart(BodyPart bp)
        {
            _bodyPartsInventory.Add(bp);
        }

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
            }

            Debug.Log("Enermy Id:" + _id + "Max HP: " + _maxHp + ", Current HP: " + _currentHp);

            //Update Slider
            _hpSlider.value = _currentHp / _maxHp;
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
