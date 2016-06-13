using System;

/*
 * Что можно улучшить: 
 * - Переворачивать стрелку сразу при достижении границы поля
 * - Переворачивать стрелку, если следующим ходом она провоцируется (это не всегда известно, может, лучше и не делать =))
 * 
 * Комментарии преподавателя:
 * паттерн Состояние - для персонажей
 * размещение объектов - выкидывать эксепшн после 1000 неудачных попыток
 * Дельта тайм!!!
 * Приватная переменая начинается с _
 * Название переменной - максимум 3 слова, лучше 2
 * Конструктор - лучше всего максимум 5 параметров, если очень надо, то можно до 8. Если больше - делаем структуру/класс
 * Строк кода в одном методе - один экран монитора (рекомендация, но не железное правило)
 * В интерфейсе, по возможности, до 5 методов.
*/ 


namespace HomeWork024___Game
{
    class Program
    {
        static void Main(string[] args)
        {
            int horizontalSize = 25;
            int verticalSize = 21;
            int horizontalEnemyCount = 50;
            int verticalEnemyCount = 50; 
            bool fogOfWar = true;
            int fogOfWarRange = 2;
            bool aggressiveNPCs = true; 
            int aggressiveNPCsRange = 4; 
            bool realTime = false;

            GameSettings gs;
            Game g;
            ConsoleKeyInfo choice;
            ConsoleKeyInfo settingsChoice;

            do
            {
                Console.Clear();
                Console.WriteLine("\"Crazy X\"");
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Controls:");
                Console.WriteLine(" - Arrows - move player");
                Console.WriteLine(" - Tab - toggle fog of war");
                Console.WriteLine(" - F1 - toggle NPC provocation mode");
                Console.WriteLine(" - F2 - toggle turn based/realtime mode");
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Settings:");
                Console.WriteLine(" - Field horizontal size: {0}", horizontalSize);
                Console.WriteLine(" - Field vertical size: {0}", verticalSize);
                Console.WriteLine(" - Enemies: {0} horizontal, {1} vertical", horizontalEnemyCount, verticalEnemyCount);
                Console.WriteLine(" - Fog of war: {0}, range {1}", fogOfWar, fogOfWarRange);
                Console.WriteLine(" - Provocation: {0}, range {1}", aggressiveNPCs, aggressiveNPCsRange);
                Console.WriteLine(" - Realtime: {0}", realTime);
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("1. Play");
                Console.WriteLine("2. Change settings");
                Console.WriteLine("3 .Quit");

                Console.Write("Choice: ");
                choice = Console.ReadKey();

                switch (choice.Key)
                {
                    case ConsoleKey.D1:
                        break;
                    case ConsoleKey.D2:

                        Console.Clear();
                        Console.WriteLine("Change settings:");
                        Console.WriteLine("1. Field horizontal size");
                        Console.WriteLine("2. Field vertical size");
                        Console.WriteLine("3. Horizontal enemies");
                        Console.WriteLine("4. Vertical enemies");
                        Console.WriteLine("5. Toggle fog of war");
                        Console.WriteLine("6. Fog of war range");
                        Console.WriteLine("7. Toggle provocation");
                        Console.WriteLine("8. Provocationa range");
                        Console.WriteLine("9. Toggle realtime");
                        Console.WriteLine("Press any other key to return to main menu");

                        settingsChoice = Console.ReadKey();

                        try 
	                    {
                            switch (settingsChoice.Key)
	                        {
		                    case ConsoleKey.D1:
                                Console.Write("\nValue: ");
                                horizontalSize = Int32.Parse(Console.ReadLine());
                                break;
                            case ConsoleKey.D2:
                                Console.Write("\nValue: ");
                                verticalSize = Int32.Parse(Console.ReadLine());
                                break;
                            case ConsoleKey.D3:
                                Console.Write("\nValue: ");
                                horizontalEnemyCount = Int32.Parse(Console.ReadLine());
                                break;
                            case ConsoleKey.D4:
                                Console.Write("\nValue: ");
                                verticalEnemyCount = Int32.Parse(Console.ReadLine());
                                break;
                            case ConsoleKey.D5:
                                fogOfWar = !fogOfWar;
                                break;
                            case ConsoleKey.D6:
                                Console.Write("\nValue: ");
                                fogOfWarRange = Int32.Parse(Console.ReadLine());
                                break;
                            case ConsoleKey.D7:
                                aggressiveNPCs = !aggressiveNPCs;
                                break;
                            case ConsoleKey.D8:
                                Console.Write("\nValue: ");
                                aggressiveNPCsRange = Int32.Parse(Console.ReadLine());
                                break;
                            case ConsoleKey.D9:
                                realTime = !realTime;
                                break;
                            default:
                                break;
	                        }
		
	                    }
	                    catch (FormatException)
	                    {
                            Console.WriteLine("Invalid value");
                            Console.ReadKey();
	                    }

                        break;
                    case ConsoleKey.D3:
                        return;
                    default:
                        break;
                }

            } while (choice.Key != ConsoleKey.D1);


            gs = new GameSettings(horizontalSize, verticalSize, horizontalEnemyCount, verticalEnemyCount,
                fogOfWar, fogOfWarRange, aggressiveNPCs, aggressiveNPCsRange, realTime);
            g = new Game(gs);
            Console.Clear();
            g.Play();
            
            Console.ReadKey();
        }
    }
}
