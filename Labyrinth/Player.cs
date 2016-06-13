namespace HomeWork024___Game
{
    class Player : Character
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        
        public Player (byte type, byte direction) : base(type, direction) {}

        public override void ChangeDirection()
        {
            Direction = 0;
        }
    }
}
