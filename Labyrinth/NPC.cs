namespace HomeWork024___Game
{
    class NPC : Character
    {
        public NPC (byte type, byte direction) : base(type, direction) {}

        public override void ChangeDirection()
        {
            switch (Direction)
            {
                case 1:
                    Direction = 2;
                    break;
                case 2:
                    Direction = 1;
                    break;
                case 3:
                    Direction = 4;
                    break;
                case 4:
                    Direction = 3;
                    break;
                default:
                    break;
            }
        }
    }
}
