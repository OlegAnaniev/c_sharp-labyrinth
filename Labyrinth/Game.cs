using System;
using System.Threading;

namespace HomeWork024___Game
{
    class Game
    {
        const byte STAY = 0;
        const byte LEFT = 1;
        const byte RIGHT = 2;
        const byte UP = 3;
        const byte DOWN = 4;

        const byte PLAYER = 0;
        const byte NPC = 1;
        const byte DEAD_PLAYER = 2;
        const byte DEAD_NPC = 3;
                
        GameSettings settings;
        Character[,] field;
        Player player;
                
        int exitPositionY;

        bool gameFinished = false;
        byte gameEnding;

        Thread getUserInput;

        delegate void ValidateProvocate(int x, int y);
        ValidateProvocate validateProvocate;

        public Game(GameSettings settings)
        {
            this.settings = settings;
        }
        
        public void Play()
        {
            if (settings.AggressiveNPCs)
            {
                validateProvocate += ProvocateNPC;
                validateProvocate += ValidateDirection;
            }
            else
            {
                validateProvocate += ValidateDirection;
            }
            
            CreateField();
            DrawFiled();

            do
            {
                if (settings.RealTime == false)
                {
                    GetUserInput();                        
                }
                else
                {
                    Thread.Sleep(1000);                
                }

                if (gameFinished == true) // Проверка выхода по Esc
                {
                    CheckGameState();
                    return;
                }
                                
                NextTurn();
                Console.Clear();
                DrawFiled();
                CheckGameState();
                
            } while (gameFinished == false);
        }

        private void CreateField()
        {
            const int playerXPosition = 0;
            int playerYPosition;
            
            field = new Character[settings.HorizontalSize, settings.VerticalSize];
            player = new Player(PLAYER, STAY);            
            Random random = new Random();
            
            playerYPosition = random.Next() % settings.VerticalSize;
            player.PositionX = playerXPosition;
            player.PositionY = playerYPosition;
            field[playerXPosition, playerYPosition] = player;

            exitPositionY = random.Next() % settings.VerticalSize;

            PlaceCharacters(settings.HorizontalEnemyCount, NPC, LEFT);
            PlaceCharacters(settings.VerticalEnemyCount, NPC, UP);
        }

        private void PlaceCharacters(int count, byte type, int dirBase)
        {
            Random random = new Random();

            for (int i = count, x, y; i > 0; i--)
            {
                do
                {
                    x = random.Next() % settings.HorizontalSize;
                    y = random.Next() % settings.VerticalSize;
                } while (field[x, y] != null);

                field[x, y] = new NPC(type, (byte)(random.Next() % 2 + dirBase));
            }
        }

        private void DrawFiled()
        {
            for (int y = 0; y < settings.VerticalSize; y++)
            {
                for (int x = 0; x < settings.HorizontalSize; x++)
                {

                    if (field[x, y] != null) // Лишний if, не mvc, но альтернативы еще хуже =)
                    {
                        field[x, y].HasMoved = false;                         
                    }
                    
                    if (settings.FogOfWar == true &&
                            (x - player.PositionX > settings.FogOfWarRange ||
                            player.PositionX - x > settings.FogOfWarRange ||
                            y - player.PositionY > settings.FogOfWarRange ||
                            player.PositionY - y > settings.FogOfWarRange)                        
                        )
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("   ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        continue;
                    }

                    if (x == settings.HorizontalSize - 1 && y == exitPositionY)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Green;                        
                    }
                    
                    if (field[x, y] == null)
                    {
                        Console.Write(" _ ");
                    }
                    else
                    {
                        switch (field[x, y].Type)
                        {
                            case PLAYER:                                
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write(" X ");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case NPC:                                           
                                switch (field[x, y].Direction)
	                            {
                                    case LEFT:
                                        Console.Write(" {0} ", (char)27);
                                        break;
                                    case RIGHT:
                                        Console.Write(" {0} ", (char)26);
                                        break;
                                    case UP:
                                        Console.Write(" {0} ", (char)24);
                                        break;
                                    case DOWN:
                                        Console.Write(" {0} ", (char)25);
                                        break;
                                    default:
                                        break;
	                            }
                                break;
                            case DEAD_NPC:
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.BackgroundColor = ConsoleColor.Yellow;
                                Console.Write(" {0} ", (char)12);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.BackgroundColor = ConsoleColor.Black;
                                break;
                            case DEAD_PLAYER:
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.Write(" {0} ", (char)5);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.BackgroundColor = ConsoleColor.Black;
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (y == exitPositionY)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;                    
                }
                
                Console.Write(Environment.NewLine);                
            }
        }

        private void GetUserInput()
        {
            ConsoleKeyInfo choice;

            do
            {
                do
                {
                    choice = Console.ReadKey();

                    switch (choice.Key)
                    {
                        case ConsoleKey.DownArrow:
                            player.Direction = DOWN;
                            break;
                        case ConsoleKey.LeftArrow:
                            player.Direction = LEFT;
                            break;
                        case ConsoleKey.RightArrow:
                            player.Direction = RIGHT;
                            break;
                        case ConsoleKey.UpArrow:
                            player.Direction = UP;
                            break;
                        case ConsoleKey.Tab:
                            settings.FogOfWar = !settings.FogOfWar;
                            Console.Clear();
                            DrawFiled();
                            break;
                        case ConsoleKey.F1:
                            if (settings.AggressiveNPCs)
                            {
                                validateProvocate -= ProvocateNPC;
                            }
                            else
                            {
                                validateProvocate -= ValidateDirection;
                                validateProvocate += ProvocateNPC;
                                validateProvocate += ValidateDirection;
                            }
                            settings.AggressiveNPCs = !settings.AggressiveNPCs;
                            break;
                        case ConsoleKey.F2:
                            if (settings.RealTime == false)
                            {
                                getUserInput = new Thread(GetUserInput);
                                getUserInput.Start();
                            }
                            settings.RealTime = !settings.RealTime;
                            return;
                        case ConsoleKey.Escape:
                            gameEnding = 2;
                            gameFinished = true;
                            break;
                        default:
                            break;
                    }

                } while (choice.Key == ConsoleKey.Tab || choice.Key == ConsoleKey.F1);
            } while (settings.RealTime == true && gameFinished == false);
        }

        private void NextTurn()
        {
            for (int y = 0; y < settings.VerticalSize; y++)
            {
                for (int x = 0; x < settings.HorizontalSize; x++)
                {
                    if (field[x, y] != null)
                    {
                        MoveCharacter(x, y);
                    }
                }
            }
        }

        private void MoveCharacter(int x, int y)
        {
            int newX;
            int newY;

            if (field[x, y].HasMoved == true)
            {
                return;
            }
            else
            {
                field[x, y].HasMoved = true;
            }

            // Проверка нахождения на границе поля, провокация            
            validateProvocate(x, y); //Раньше необходимость провокации определялась if-ом. Теперь она или есть в делегате, или нет

            // Расчет новых координат объекта
            switch (field[x, y].Direction)
            {
                case STAY:
                    if (field[x, y].Type == DEAD_NPC)
                    {
                        field[x, y] = null;                        
                    }                    
                    return;
                case LEFT:
                    newX = x - 1;
                    newY = y;
                    break;
                case RIGHT:
                    newX = x + 1;
                    newY = y;                    
                    break;
                case UP:
                    newX = x;
                    newY = y - 1;
                    break;
                case DOWN:
                    newX = x;
                    newY = y + 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Invalid character direction");                    
            }

            // Сохраняем новые координаты игрока            
            if (field[x, y] == player) // По быстродействию лучше смотрелось в DrawField, но зато mvc =)
            {
                player.PositionX = newX;
                player.PositionY = newY;
            }

            // Если целевая клетка пустая, то просто переставляем объект
            if (field[newX, newY] == null)
            {
                field[newX, newY] = field[x, y];

            }
            else
            {
                // Если объект, находящийся в целевой клетке, еще не ходил
                if (field[newX, newY].HasMoved == false)
                {
                    // Если объекты движутся друг навстречу другу или второй объект стоит
                    if ((field[x, y].Direction == RIGHT && field[newX, newY].Direction == LEFT) ||
                        (field[x, y].Direction == LEFT && field[newX, newY].Direction == RIGHT) ||
                        (field[x, y].Direction == UP && field[newX, newY].Direction == DOWN) ||
                        (field[x, y].Direction == DOWN && field[newX, newY].Direction == UP) ||                        
                        field[newX, newY].Direction == STAY) 
                        //Возможно, если вынести обработку неподвижных объектов в отдельный if, то будет быстрее
                    {
                        // Если в целевой клетке стоит памятник, то игнорируем его
                        if (field[newX, newY].Type == DEAD_NPC)
                        {
                            field[newX, newY] = field[x, y];
                        }
                        else
                        {
                            field[newX, newY].HasMoved = true;
                            PlaceMonument(x, y, newX, newY);  
                        }                                              
                    }
                    // Если объект, находящийся в целевой клетке, освобождает ее
                    else
                    {
                        MoveCharacter(newX, newY);
                        field[newX, newY] = field[x, y];
                    }
                }
                // Если объект, находящийся в целевой клетке, уже ходил
                else
                {                    
                    PlaceMonument(x, y, newX, newY);
                }
            }

            field[x, y] = null;
        }

        private void ValidateDirection(int x, int y)
        {
            if ((x == 0 && field[x, y].Direction == LEFT) ||
                    (x == settings.HorizontalSize - 1 && field[x, y].Direction == RIGHT) ||
                    (y == 0 && field[x, y].Direction == UP) ||
                    (y == settings.VerticalSize - 1 && field[x, y].Direction == DOWN))
            {
                field[x, y].ChangeDirection();
            }
        }

        private void ProvocateNPC(int x, int y)
        {
            int horisontalDistance;
            int verticalDistance;
            int currentDistance;

            if (field[x, y] == player)
            {
                return;
            }

            horisontalDistance = Math.Abs(x - player.PositionX);
            verticalDistance = Math.Abs(y - player.PositionY);
            currentDistance = horisontalDistance + verticalDistance;

            if (currentDistance < settings.AggressionRange &&
                    (
                        (horisontalDistance != 0 && field[x, y].Direction != UP && field[x, y].Direction != DOWN) ||
                        (verticalDistance != 0 && field[x, y].Direction != LEFT && field[x, y].Direction != RIGHT)
                    )
               )
            {
                switch (field[x, y].Direction)
                {
                    case LEFT:
                        if (currentDistance < Math.Abs(x - 1 - player.PositionX) + verticalDistance)
                        {
                            field[x, y].ChangeDirection();
                        }
                        break;
                    case RIGHT:
                        if (currentDistance < Math.Abs(x + 1 - player.PositionX) + verticalDistance)
                        {
                            field[x, y].ChangeDirection();
                        }
                        break;
                    case UP:
                        if (currentDistance < horisontalDistance + Math.Abs(y - 1 - player.PositionY))
                        {
                            field[x, y].ChangeDirection();
                        }
                        break;
                    case DOWN:
                        if (currentDistance < horisontalDistance + Math.Abs(y + 1 - player.PositionY))
                        {
                            field[x, y].ChangeDirection();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        
        private void PlaceMonument(int x, int y, int newX, int newY)
        {
            if (field[newX, newY].Type == DEAD_PLAYER)
            {
                return;
            }

            if (field[x, y].Type == PLAYER || field[newX, newY].Type == PLAYER)
            {
                field[newX, newY].Type = DEAD_PLAYER;
                field[newX, newY].Direction = STAY;
                gameFinished = true;
                gameEnding = 1;
            }
            else
            {
                field[newX, newY].Type = DEAD_NPC;
                field[newX, newY].Direction = STAY;
            }
        }

        private void CheckGameState()
        {
            if (field[settings.HorizontalSize - 1, exitPositionY] == player)
            {
                gameFinished = true;
                gameEnding = 0;
            }
            
            if (gameFinished == true)
            {
                if (getUserInput != null && getUserInput.IsAlive == true)
                {
                    getUserInput.Abort();
                }                
                
                switch (gameEnding)
                {
                    case 0:
                        Console.WriteLine("\nYou have won!\n");
                        return;
                    case 1:
                        Console.WriteLine("\nYou have lost! Don't get upset, you're just a looser!\n");
                        return;
                    case 2:
                        Console.WriteLine("\nLeaver!\n");
                        return;
                    default:
                        break;
                }
            }
            else
            {
                player.Direction = STAY;
            }
        }                
    }    
}