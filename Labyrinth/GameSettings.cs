using System;

namespace HomeWork024___Game
{    
    class GameSettings
    {        
        int horizontalSize;
        int verticalSize;
        int horizontalEnemyCount;
        int verticalEnemyCount;        
        int fogOfWarRange;
        int aggressionRange;
        
        public int HorizontalSize
        {
            get
            {
                return horizontalSize;
            }
        }

        public int VerticalSize
        {
            get
            {
                return verticalSize;
            }
        }

        public int HorizontalEnemyCount
        {
            get
            {
                return horizontalEnemyCount;
            }
        }

        public int VerticalEnemyCount
        {
            get
            {
                return verticalEnemyCount;
            }
        }

        public bool FogOfWar { get; set; }

        public int FogOfWarRange 
        {
            get
            {
                return fogOfWarRange;
            }
        }

        public bool AggressiveNPCs { get; set; }

        public int AggressionRange
        {
            get
            {
                return aggressionRange;
            }
        }

        public bool RealTime { get; set; }

        public GameSettings(int horizontalSize, int verticalSize, int horizontalEnemyCount, int verticalEnemyCount, 
            bool fogOfWar = false, int fogOfWarRange = 2, bool aggressiveNPCs = false, int aggressiveNPCsRange = 2, 
            bool realTime = false)
        {
            if (horizontalSize < 1 ||
                verticalSize < 1 || 
                horizontalEnemyCount < 0 || 
                verticalEnemyCount < 0 ||
                horizontalSize * verticalSize < horizontalEnemyCount + verticalEnemyCount + 1 || 
                fogOfWarRange < 1 ||
                aggressiveNPCsRange < 1)
            {
                throw new ArgumentOutOfRangeException("Invalid game settings");
            }

            this.horizontalSize = horizontalSize;
            this.verticalSize = verticalSize;
            this.horizontalEnemyCount = horizontalEnemyCount;
            this.verticalEnemyCount = verticalEnemyCount;
            FogOfWar = fogOfWar;
            this.fogOfWarRange = fogOfWarRange;
            this.AggressiveNPCs = aggressiveNPCs;
            this.aggressionRange = aggressiveNPCsRange;
            RealTime = realTime;
        }        
    }
}
