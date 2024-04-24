using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



namespace Combatant
{
    public struct CombatStats
    {
        public float maxHp;
        public float currentHp;
        public float maxEnergy;
        public float currentEnergy;
        public float attackValue;
        public float shieldValue;

        public CombatStats(float mHp, float cHp, float aV, float sV, float mE, float cE)
        {
            maxHp = mHp;
            currentHp = cHp;
            attackValue = aV;
            shieldValue = sV;
            maxEnergy = mE;
            currentEnergy = cE;
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
        protected float _maxHp;
        [HideInInspector]
        public float _currentHp;
        public float _currentEnergy;
        public float _maxEnergy;
        public float _attackPoint;
        public float _shieldPoint;
        protected float _shieldDecrease;
        public abstract string Name { get; }
        public CombatEntityUI combatantUI;

        [SerializeField]
        protected string _name;
        public int _id;
        //Bind to UI hp slider
        protected Slider _hpSlider;


        //Store the body part lists in the inventory 
        [SerializeField]
        public Dictionary<BodyPartType, BodyPart> _equipment = new Dictionary<BodyPartType, BodyPart>();
        protected List<ConditionData> _conditions = new List<ConditionData>();

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
            return new CombatStats(_maxHp, _currentHp, _attackPoint, _shieldPoint, _maxEnergy, _currentEnergy);
        }

        public void SetStats(CombatStats values)
        {
            _maxHp = values.maxHp;
            _currentHp = values.currentHp;
            _attackPoint = values.attackValue;
            _shieldPoint = values.shieldValue;
            _currentEnergy = values.currentEnergy;
            _maxEnergy = values.maxEnergy;
        }

        public bool isAlive()
        {
            return _currentHp > 0;
        }

        public bool isCritical()
        {
            return _currentHp <= (_maxHp * 0.1);
        }

        public bool hasPowersToCast()
        {
            bool result = false;
            foreach(Power p in _powers){
                if (p.cost < _currentHp)
                {
                    return true;
                }
            }

            return result;
        }

        public CombatantType GetSide()
        {
            return _side;
        }

        public void SetSlider(Slider s)
        {
            _hpSlider = s;
        }

        // Conditions stuff
        public void AddCondition(ConditionType condition)
        {

            _conditions.Add(ConditionManager.CreateConditionData(condition));
        }

        public void ApplyConditions()
        {
            // For every condition, apply its effect to the player
            for (int i = 0; i < _conditions.Count; i++)
            {
                ConditionManager.ApplyConditionEffect(this, _conditions[i]);
            }
        }

        public void UpdateActiveConditions()
        {
            // Evaluate which conditions have expired and should no longer affect a combatant
            List<int> expiredConditions = new List<int>();
            for (int i = 0; i < _conditions.Count; i++)
            {
                _conditions[i].currentDuration++;
                if (_conditions[i].currentDuration == _conditions[i].maxDuration)
                {
                    expiredConditions.Add(i);
                }
            }
            foreach(int i in expiredConditions)
            {
                // Conditions might modify a character's stats. We need to reset those stats when the condition is done
                ConditionManager.CleanUpCondition(this, _conditions[i]);
                _conditions.RemoveAt(i);
            }
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

        public Enemy Clone(int id)
        {
            Enemy e = new Enemy(_name, id);
            e._side = CombatantType.ENEMIES;
            e.aiType = aiType;
            e._equipment = new Dictionary<BodyPartType,BodyPart>();
            e._powers = new List<Power>();
            foreach(KeyValuePair<BodyPartType, BodyPart> kvp in _equipment)
            {
                e._equipment.Add(kvp.Key, kvp.Value.Clone());
            }

            foreach(Power p in _powers)
            {
                e._powers.Add(p);
            }

            return e;
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
