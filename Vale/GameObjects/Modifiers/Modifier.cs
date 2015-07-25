using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vale.GameObjects.Modifiers
{
    /// <summary>
    /// A modifier applies positive or negative arithmatic operations on combat unit attributes, such as speed and damage.
    /// </summary>
    class Modifier:IUpdate
    {
        private readonly float modifierValue;
        public AttributeType AttributeModified { get; private set; }
        public ModifierOperationType Operation { get; private set; }
        
        public float ModifierValue
        {
            get { return modifierValue; }

            set
            {}
        }

        public enum CommonOperations
        {
            Double,
            Halve
        }

        public enum AttributeType
        {
            MaxHealth,
            MovementSpeed,
            Damage,
            Defense,
            AreaRadius,
            Range,
            ActionSpeed,
            None
        }

        public enum ModifierOperationType
        {
            Flat,
            /// <summary>
            /// The modifier applies the buff by a percentage increase/decrease. Characters base percentage is "1". Any value will add or subtract from that 1f. Expects floats. 10%= .1f, 25% = .25f, 50% = .5f
            /// A 10% damage increase would require a value of  .1f. Doubling your damage would be 1f.
            /// Use negatives for percent reductions. The opposite of doubling your damage (+100%) is halving your damage (-50%). A modifier that halves damage would have a value of -.5f
            /// </summary>
            Percent,
            FixedSet,
            None
        }

        public Modifier(AttributeType attributeModified, ModifierOperationType operation, float modifierValue)
        {
            AttributeModified = attributeModified;
            Operation = operation;
            this.modifierValue = modifierValue;
        }

        public Modifier(AttributeType attribureModified, CommonOperations operation)
        {
            AttributeModified = attribureModified;

            switch (operation)
            {
                case CommonOperations.Double:
                    Operation = ModifierOperationType.Percent;
                    ModifierValue = 1f;
                    break;
                case CommonOperations.Halve:
                    Operation = ModifierOperationType.Percent;
                    ModifierValue = -.5f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("operation", operation, null);
            }
        }

        public void Update(GameTime gameTime)
        {
            //uhhhhhhh nothing?
        }
    }
}
