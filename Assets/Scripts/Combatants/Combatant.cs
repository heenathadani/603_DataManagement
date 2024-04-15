using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



namespace Combatant
{
    public struct CombatStats
    {
        public float maxHp;
        public float currentHp;
        public float attackValue;
        public float shieldValue;

        public CombatStats(float mHp, float cHp, float aV, float sV)
        {
            maxHp = mHp;
            currentHp = cHp;
            attackValue = aV;
            shieldValue = sV;
        }
    }

    public enum CombatantType
    {
        ALLIES,
        ENEMIES
    }

    public enum StatType
    {
        //Below are the combat related attributes -- Rin
        protected float _maxHp;
        public float _currentHp;
        public float _attackPoint;
        public float _shieldPoint;
        public abstract string Name { get; }
        HP,
        Energy,
        Attack,
        Shield
        public CombatStats GetStats()
        {
            return new CombatStats(_maxHp, _currentHp, _attackPoint, _shieldPoint);
        }

        public bool isAlive()
        {
            return _currentHp > 0;
        }
    }

    public abstract class aCombatant
    {
        private string _name;
        private int _id;

        //Below are the combat related attributes -- Rin

        //Store the body part lists in the inventory 
        public List<BodyPart> _bodyPartsInventory = new List<BodyPart>();

        private float _maxHp;
        protected CombatantType _side;
        public abstract string Name { get; }
        public float _attackPoint;
        public float _shieldPoint;
        //End

        public Protagonist(string name, int id) {
            _name = name;
        public float _attackPoint;
        public float _shieldPoint;

        public float GetStatByType(StatType type)
        {
            switch (type)
            {
                case StatType.Energy:
                    return _currentEnergy;
                case StatType.Attack:
                    return _attackPoint;
                case StatType.Shield:
                    return _shieldPoint;
                default:
                    return _currentHp;
            }
        }

        public void AffectStatByType(StatType type, float value)
        {
            switch (type)
            {
                case StatType.Energy:
                    Mathf.Clamp(_currentEnergy + value, 0, _maxEnergy);
                    break;
                case StatType.Attack:
                    if (_attackPoint + value < 0)
                    {
                        _attackPoint = 0;
                    } else
                    {
                        _attackPoint += value;
                    }
                    break;
                case StatType.Shield:
                    if (_shieldPoint + value < 0)
                    {
                        _shieldPoint = 0;
                    }
                    else
                    {
                        _shieldPoint += value;
                    }
                    break;
                default:
                    Mathf.Clamp(_currentHp + value, 0, _maxHp);
                    break;
            }
        }

        public CombatantType GetSide()
        {
            return _side;
        }

        public void AffectBodyPartByType(BodyPartType part, float value)
        {
            // For now, let's keep it so we can only diminish part HP
            foreach(BodyPart bodyPart in _bodyPartsInventory)
            {
                if (bodyPart.bodyPartData.type == part)
                {
                    bodyPart.UpdateCurrentHP(value);
                }
            }
        }

        public void AddBodyPart(BodyPart bp)
        {
            _bodyPartsInventory.Add(bp);
        }

        //Damage on the body part -- Rin
        public void AffectBodyPartByIndex(int index, float value)
        {
            _bodyPartsInventory[index].currentHp += value;

        }
    }

    public class Protagonist : aCombatant
    {
        private string _name;
        private int _id;

            if (uiManager.enemyHpSliderList.Count > 0)
            {
                _hpSlider = uiManager.characterHpSliderList[id];
            }
        }

        public override string Name
        {
            get 
            {
                return _name;
            }
        }

        //Add body part to an protagonist's inventory -- Rin


        //Update the status (HP) -- Rin
        public void UpdateStatus()
        {
            _maxHp = 0;
            _currentHp = 0;
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

            //Debug.Log("Character Id:" + _id + "Max HP: " + _maxHp + ", Current HP: " + _currentHp);

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

            CombatUIManager uiManager = GameObject.FindAnyObjectByType<CombatUIManager>();
            if (uiManager.enemyHpSliderList.Count > 0)
            {
                _hpSlider = uiManager.enemyHpSliderList[id];
            }
        }


        public void UpdateStatus()
        {
            _maxHp = 0;
            _currentHp = 0;
            //Calculate status -- Rin
            foreach (BodyPart bd in _bodyPartsInventory)
            {
                //Update Hp
                _maxHp += bd.bodyPartData.maxHp;
                _currentHp += bd.currentHp;

                //Update attack point
                _attackPoint += bd.bodyPartData.attackPoint;
            }

            //Debug.Log("Enermy Id:" + _id + "Max HP: " + _maxHp + ", Current HP: " + _currentHp);

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
