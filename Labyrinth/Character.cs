using System;

namespace HomeWork024___Game
{
    abstract class Character
    {
        static readonly byte typeMaxValue = 4;
        byte type; /// 0 - player, 1 - npc, 3 - dead npc, 4 - dead player
        byte direction; /// 0 - stay, 1 - left, 2 - right, 3 - up, 4 - down
        
        public bool HasMoved { get; set; }

        public byte Type
        {
            get
            {
                return type;
            }
            set
            {
                if (value < 0 || value > typeMaxValue)
                {
                    throw new ArgumentOutOfRangeException("Character type value may be set between 0 and " + typeMaxValue + " only");
                }

                type = value;
            }
        }

        public byte Direction 
        { 
            get
            {
                return direction;
            }
            set
            {
                if (value > 4)
                {
                    throw new ArgumentOutOfRangeException("Movement direction value may be set between 0 and 4 only");
                }

                direction = value;
            }
        }

        public Character(byte type, byte direction)
        {
            Type = type;
            Direction = direction;
        }

        abstract public void ChangeDirection();
    }
}
