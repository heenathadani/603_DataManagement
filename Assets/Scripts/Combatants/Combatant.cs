using System;
using System.Collections.Generic;
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
        public abstract string Name { get; }
        public CombatEntityUI combatantUI;

        [SerializeField]
        protected string _name;
        public int _id;
        //Bind to UI hp slider
        protected Slider _hpSlider;


        //Store the body part lists in the inventory 
        [SerializeField]
        public List<BodyPart> _bodyPartsInventory = new List<BodyPart>();
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
            combatantUI.DisplayDamage((int)value);
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

        public void SetSlider(Slider s)
        {
            _hpSlider = s;
        }
        public abstract void UpdateStatus();

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

        //Add body part to an protagonist's inventory -- Rin


        //Update the status (HP) -- Rin
        public override void UpdateStatus()
        {
            _maxHp = 0;
            _currentHp = 0;
            _shieldPoint = 0;
            _attackPoint = 0;
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

            if (_currentHp <= 0)
            {
                CharacterDie();
            }

            //Update Slider
            if (combatantUI != null)
            {
                combatantUI.UpdateHPBar(_currentHp / _maxHp);
            }

        }

        public void CharacterDie()
        {
            // Hide character Ui -- Rin
            CombatUIManager uiManager = GameObject.FindAnyObjectByType<CombatUIManager>();
            //uiManager.characterHpSliderList[_id].gameObject.SetActive(false);

            bool ifEnd = true;
            // Check Combat End
            foreach (Protagonist e in CombatantData.partyCharacters)
            {
                if (e.isAlive())
                {
                    ifEnd = false;
                }
            }
            if (ifEnd)
            {
                uiManager.ShowGameOver();
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


        public override void UpdateStatus()
        {

            CombatUIManager uiManager = GameObject.FindAnyObjectByType<CombatUIManager>();
            if (uiManager.enemyHpSliderList.Count > 0)
            {
                _hpSlider = uiManager.enemyHpSliderList[_id];
            }


            _maxHp = 0;
            _currentHp = 0;
            _shieldPoint = 0;
            _attackPoint = 0;
            //Calculate status -- Rin
            foreach (BodyPart bd in _bodyPartsInventory)
            {
                if (bd.currentHp <= 0)
                {
                    _maxHp += bd.GetMaxHp();
                    _currentHp += bd.currentHp;
                    continue;
                }

                //Update Hp
                _maxHp += bd.bodyPartData.maxHp;
                _currentHp += bd.currentHp;

                //Update attack point
                _attackPoint += bd.bodyPartData.attackPoint;
                _shieldPoint += bd.bodyPartData.shieldPoint;
            }

            //Enemy Dies
            if(_currentHp <= 0)
            {
                Debug.Log("Enemy is dead");
                EnemyDie();
            }


            //Update Slider
            if(combatantUI != null)
            {
                combatantUI.UpdateHPBar(_currentHp / _maxHp);
            }
            
        }

        public void Reset()
        {
            foreach (BodyPart bd in _bodyPartsInventory)
            {
                //Update Hp
                bd.currentHp = bd.GetMaxHp();
            }
        }

        public Enemy Clone(int id)
        {
            Enemy e = new Enemy(_name, id);
            e._side = CombatantType.ENEMIES;
            e.aiType = aiType;
            e._bodyPartsInventory = new List<BodyPart>();
            e._powers = new List<Power>();
            foreach(BodyPart part in _bodyPartsInventory)
            {
                e._bodyPartsInventory.Add(part.Clone());
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

        // When the enemy dies
        public void EnemyDie()
        {
            // Hide enemy Ui -- Rin
            CombatUIManager uiManager = GameObject.FindAnyObjectByType<CombatUIManager>();
            //uiManager.enemyHpSliderList[_id].gameObject.SetActive(false);
            combatantUI.Disable();

            bool ifEnd = true;
            //Check Combat End
            foreach(Enemy e in CombatantData.enemies)
            {
                if(e.isAlive())
                {
                    ifEnd = false;
                }
            }

            if(ifEnd)
            {
                uiManager.ShowVictory();
            }
        }


        public void InitializeBodyPart()
        {

        }
    }
}
