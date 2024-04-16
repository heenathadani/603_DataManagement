using System;
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
        
        HP,
        Energy,
        Attack,
        Shield
        
    }

    public abstract class aCombatant
    {
        //Below are the combat related attributes -- Rin
        [SerializeField]
        protected float _maxHp;
        public float _currentHp;
        public float _currentEnergy;
        public float _maxEnergy;
        public float _attackPoint;
        public float _shieldPoint;
        public abstract string Name { get; }

        [SerializeField]
        protected string _name;
        protected int _id;
        //Bind to UI hp slider
        protected Slider _hpSlider;

        //Store the body part lists in the inventory 
        [SerializeField]
        public List<BodyPart> _bodyPartsInventory = new List<BodyPart>();

        //Store the powers in a list - need to figure out how to populate this
        public List<Power> _powers = new List<Power>();

        protected CombatantType _side;

        public List<Power> FilterPowerByTargetType(CombatActionTargets targetType)
        {
            List<Power> powers = new List<Power>();
            foreach(Power p in _powers)
            {
                if (p.targetType == targetType)
                {
                    powers.Add(p);
                }
            }
            return powers;
        }

        public CombatStats GetStats()
        {
            return new CombatStats(_maxHp, _currentHp, _attackPoint, _shieldPoint);
        }

        public bool isAlive()
        {
            return _currentHp > 0;
        }

        public bool isCritical()
        {
            return _currentHp <= (_maxHp * 0.1);
        }
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
                    }
                    else
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
            foreach (BodyPart bodyPart in _bodyPartsInventory)
            {
                if (bodyPart.bodyPartData.type == part)
                {
                    bodyPart.UpdateCurrentHP(value);
                }
            }
        }

        public void AddBodyPart(BodyPart bp)
        {
            Debug.Log(_bodyPartsInventory);
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
        public override string Name
        {
            get 
            {
                return _name;
            }
        }

        public Protagonist(string name, int id)
        {
            _name = name;
            _id = id;
            _side = CombatantType.ALLIES;

            CombatUIManager uiManager = GameObject.FindAnyObjectByType<CombatUIManager>();
            if (uiManager.enemyHpSliderList.Count > 0)
            {
                _hpSlider = uiManager.characterHpSliderList[id];
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
            if (_hpSlider != null)
            {
                _hpSlider.value = _currentHp / _maxHp;
            }
            
        }
    }

    [Serializable]
    public class Enemy : aCombatant
    {
        [SerializeField]
        public AITypes aiType;

        public Enemy(string name, int id)
        {
            _name = name;
            _id = id;
            _side = CombatantType.ENEMIES;

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
            if (_hpSlider != null)
            {
                _hpSlider.value = _currentHp / _maxHp;
            }
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
