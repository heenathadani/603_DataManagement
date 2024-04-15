using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Combatant
{
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
        protected CombatantType _side;
        public abstract string Name { get; }
        //Store the body part lists in the inventory 
        public List<BodyPart> _bodyPartsInventory = new List<BodyPart>();

        //Below are the combat related attributes -- Rin
        protected float _maxHp;
        public float _currentHp;

        private float _maxEnergy;
        public float _currentEnergy;

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

        //Bind to UI hp slider
        private Slider _hpSlider;

        public Protagonist(string name, int id) {
            _name = name;
            _id = id;
            _side = CombatantType.ALLIES;

            CombatUIManager uiManager = GameObject.FindAnyObjectByType<CombatUIManager>();
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

            Debug.Log("Character Id:" + _id + "Max HP: " + _maxHp + ", Current HP: " + _currentHp);

            //Update Slider
            _hpSlider.value = _currentHp / _maxHp;
        }
    }

    public class Enemy : aCombatant
    {
        private string _name;
        private int _id;

        //Bind to UI hp slider -- Rin
        private Slider _hpSlider;

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
