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
        public abstract void PickTarget();
    }

    public class Protagonist : aCombatant
    {
        private string _name;


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
        public override void PickTarget()
        {
            throw new System.NotImplementedException();
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
        public override void PickTarget()
        {
            throw new System.NotImplementedException();
        }
    }
}
