using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace ce103_hw5_alper_sahin
{
    public class classs
    {
        public const int SNAKE_ARRAY_SIZE = 310;
        public const ConsoleKey UP_ARROW = ConsoleKey.UpArrow;
        public const ConsoleKey LEFT_ARROW = ConsoleKey.LeftArrow;
        public const ConsoleKey RIGHT_ARROW = ConsoleKey.RightArrow;
        public const ConsoleKey DOWN_ARROW = ConsoleKey.DownArrow;
        public const ConsoleKey ENTER_KEY = ConsoleKey.Enter;
        public const ConsoleKey EXIT_BUTTON = ConsoleKey.Escape; // ESC
        public const ConsoleKey PAUSE_BUTTON = ConsoleKey.P; //p
        const char SNAKE_HEAD = (char)177;
        const char SNAKE_BODY = (char)178;
        const char WALL = (char)219;
        const char FOOD = (char)254;
        const char BLANK = ' ';

        /**
       *	  @name Get Game Speed (getGameSpeed)
       *
       *	  @brief Get Game Speed
       *
       *	  Print get game speed , and create selection
       **/
        public ConsoleKey waitForAnyKey()
        {
            ConsoleKey pressed;

            while (!Console.KeyAvailable) ;

            pressed = Console.ReadKey(false).Key;
            //pressed = tolower(pressed);
            return pressed;
        }
        /**
        *	  @name Get Game Speed (getGameSpeed)
        *
        *	  @brief Get Game Speed
        *
        *	  Print get game speed , and create selection
        **/
        public int getGameSpeed()
        {
            int speed = 0;
            Console.Clear();
            Console.SetCursorPosition(10, 5);
            Console.Write("Select The game speed between 1 and 9 and press enter\n");
            Console.SetCursorPosition(10, 6);
            int selection = Convert.ToInt32(Console.ReadLine());
            switch (selection)
            {
                case 1:
                    speed = 90;
                    break;
                case 2:
                    speed = 80;
                    break;
                case 3:
                    speed = 70;
                    break;
                case 4:
                    speed = 60;
                    break;
                case 5:
                    speed = 50;
                    break;
                case 6:
                    speed = 40;
                    break;
                case 7:
                    speed = 30;
                    break;
                case 8:
                    speed = 20;
                    break;
                case 9:
                    speed = 10;
                    break;

            }
            return speed;
        }

        /**
        *	  @name exit YN (exitYN)
        *
        *	  @brief exit YN
        *
        *	  Print exit yn , and create selection
        **/
        public void exitYN()
        {
            char pressed;
            Console.SetCursorPosition(9, 8);
            Console.Write("Are you sure you want to exit(Y/N)\n");

            do
            {
                pressed = (char)waitForAnyKey();
                pressed = char.ToLower(pressed);
            } while (!(pressed == 'y' || pressed == 'n'));

            if (pressed == 'y')
            {
                Console.Clear(); //clear the console
                Environment.Exit(0);
            }
            return;
        }

        /**
        *	  @name Pause Menu (pauseMenu)
        *
        *	  @brief Pause Menu
        *
        *	  Print pause menu , and create selection
        **/
        public void pauseMenu()
        {
            int i;
            Console.SetCursorPosition(28, 23);
            Console.Write("**Paused**");

            waitForAnyKey();
            Console.SetCursorPosition(28, 23);
            Console.Write("            ");
            return;
        }
        /**
       *
       *	  @name Check Keys Pressed (checkKeysPressed)
       *
       *	  @brief Check Keys Pressed
       *
       *	  Check Keys Pressed
       *	  
       *	  @param  [in] direction [\b ConsoleKey]  
       **/

        //This function checks if a key has pressed, then checks if its any of the arrow keys/ p/esc key. It changes direction acording to the key pressed.
        public ConsoleKey checkKeysPressed(ConsoleKey direction)
        {
            ConsoleKey pressed;

            if (Console.KeyAvailable == true) //If a key has been pressed
            {
                pressed = Console.ReadKey(false).Key;
                if (direction != pressed)
                {
                    if (pressed == DOWN_ARROW && direction != UP_ARROW)
                    {
                        direction = pressed;
                    }
                    else if (pressed == UP_ARROW && direction != DOWN_ARROW)
                    {
                        direction = pressed;
                    }
                    else if (pressed == LEFT_ARROW && direction != RIGHT_ARROW)
                    {
                        direction = pressed;
                    }
                    else if (pressed == RIGHT_ARROW && direction != LEFT_ARROW)
                    {
                        direction = pressed;
                    }
                    else if (pressed == EXIT_BUTTON || pressed == PAUSE_BUTTON)
                    {
                        pauseMenu();
                    }
                }
            }
            return (direction);
        }

        /**
       *
       *	  @name Generate Food (generateFood)
       *
       *	  @brief Generate Food
       *
       *	  Generate Food
       *	  
       *	  @param  [in] snakeXY [\b int[,]] snake coordinates
       *
       *	  @param  [in] x [\b int]  
       *	  
       *	  @param  [in] y [\b int] 
       *	  
       *	  @param  [in] snakeLength [\b int]  Length of the Snake 
       *	  
       *	  @param  [in] detect [\b int]  
       **/

        //Cycles around checking if the x y coordinates ='s the snake coordinates as one of this parts
        //One thing to note, a snake of length 4 cannot collide with itself, therefore there is no need to call this function when the snakes length is <= 4
        public bool collisionSnake(int x, int y, int[,] snakeXY, int snakeLength, int detect)
        {
            int i;
            for (i = detect; i < snakeLength; i++) //Checks if the snake collided with itself
            {
                if (x == snakeXY[0, i] && y == snakeXY[1, i])
                    return true;
            }
            return false;
        }

        /**
       *
       *	  @name Generate Food (generateFood)
       *
       *	  @brief Generate Food
       *
       *	  Generate Food
       *	  
       *	  @param  [in] snakeXY [\b int[,]] snake coordinates
       *
       *	  @param  [in] foodXY [\b int[,]]  food coordinates
       *	  
       *	  @param  [in] width [\b int]  
       *	  
       *	  @param  [in] height [\b int] 
       *	  
       *	  @param  [in] snakeLength [\b int]  Length of the Snake 
       *	  
       *	  @param  [in] direction [\b ConsoleKey]  
       **/

        //Generates food & Makes sure the food doesn't appear on top of the snake <- This sometimes causes a lag issue!!! Not too much of a problem tho
        public void generateFood(int[] foodXY, int width, int height, int[,] snakeXY, int snakeLength)
        {
            Random RandomNumbers = new Random();
            do
            {
                //RandomNumbers.Seed(time(null));
                foodXY[0] = RandomNumbers.Next() % (width - 2) + 2;
                //RandomNumbers.Seed(time(null));
                foodXY[1] = RandomNumbers.Next() % (height - 6) + 2;
            } while (collisionSnake(foodXY[0], foodXY[1], snakeXY, snakeLength, 0)); //This should prevent the "Food" from being created on top of the snake. - However the food has a chance to be created ontop of the snake, in which case the snake should eat it...

            Console.SetCursorPosition(foodXY[0], foodXY[1]);
            Console.Write(FOOD);
        }
        /**
        *
        *	  @name Move Snake Array (moveSnakeArray)
        *
        *	  @brief Move Snake Array
        *
        *	  Move Snake Array
        *
        *	  @param  [in] snakeXY [\b int[,]]  snake coordinates
        *	  
        *	  @param  [in] snakeLength [\b int]  Length of the Snake
        *	  
        *	  @param  [in] direction [\b ConsoleKey]  
        **/
        public void moveSnakeArray(int[,] snakeXY, int snakeLength, ConsoleKey direction)
        {
            int i;
            for (i = snakeLength - 1; i >= 1; i--)
            {
                snakeXY[0, i] = snakeXY[0, i - 1];
                snakeXY[1, i] = snakeXY[1, i - 1];
            }

            /*
            because we dont actually know the new snakes head x y, 
            we have to check the direction and add or take from it depending on the direction.
            */
            switch (direction)
            {
                case DOWN_ARROW:
                    snakeXY[1, 0]++;
                    break;
                case RIGHT_ARROW:
                    snakeXY[0, 0]++;
                    break;
                case UP_ARROW:
                    snakeXY[1, 0]--;
                    break;
                case LEFT_ARROW:
                    snakeXY[0, 0]--;
                    break;
            }

            return;
        }

        /**
        *
        *	  @name   Move Snake Body (move)
        *
        *	  @brief Move snake body
        *
        *	  Moving snake body
        *
        *	  @param  [in] snakeXY [\b int[,]] Snake's body coordinates
        *	  
        *	  @param  [in] snakeLength [\b int]  Length of the Snake
        *	  
        *	  @param  [in] direction [\b ConsoleKey]  
        **/
        public void move(int[,] snakeXY, int snakeLength, ConsoleKey direction)
        {
            int x;
            int y;

            //Remove the tail ( HAS TO BE DONE BEFORE THE ARRAY IS MOVED!!!!! )
            x = snakeXY[0, snakeLength - 1];
            y = snakeXY[1, snakeLength - 1];

            Console.SetCursorPosition(x, y);
            Console.Write(BLANK);

            //Changes the head of the snake to a body part
            Console.SetCursorPosition(snakeXY[0, 0], snakeXY[1, 0]);
            Console.Write(SNAKE_BODY);

            moveSnakeArray(snakeXY, snakeLength, direction);

            Console.SetCursorPosition(snakeXY[0, 0], snakeXY[1, 0]);
            Console.Write(SNAKE_HEAD);

            Console.SetCursorPosition(1, 1); //Gets rid of the darn flashing underscore.

            return;
        }
        /**
        *
        *	  @name   eatFood (eatFood)
        *
        *	  @brief eatFood
        *
        *	  eatFood
        *
        *	  @param  [in] snakeXY [\b int[,]] Snake's body coordinates
        *	  
        *	  @param  [in] foodXY [\b int[,]] food coordinates
        *	  
        **/

        //This function checks if the snakes head his on top of the food, if it is then it'll generate some more food...
        public bool eatFood(int[,] snakeXY, int[] foodXY)
        {
            if (snakeXY[0, 0] == foodXY[0] && snakeXY[1, 0] == foodXY[1])
            {
                foodXY[0] = 0;
                foodXY[1] = 0; //This should prevent a nasty bug (loops) need to check if the bug still exists...

                return true;
            }

            return false;
        }
        /**
        *
        *	  @name  collisionDetection (collisionDetection)
        *
        *	  @brief collisionDetection
        *
        *	  collisionDetection
        *
        *	  @param  [in] snakeXY [\b int[,]] Snake's body coordinates
        *	  
        *	  @param  [in] snakeLength [\b int]  Length of the Snake
        *	  
        *	  @param  [in] consoleWidth [\b int]  
        *	  
        *	  @param  [in] consoleHeight [\b int]  
        *	  
        **/

        public bool collisionDetection(int[,] snakeXY, int consoleWidth, int consoleHeight, int snakeLength) //Need to Clean this up a bit
        {
            bool colision = false;
            if ((snakeXY[0, 0] == 1) || (snakeXY[1, 0] == 1) || (snakeXY[0, 0] == consoleWidth) || (snakeXY[1, 0] == consoleHeight - 4)) //Checks if the snake collided wit the wall or it's self
                colision = true;
            else
                if (collisionSnake(snakeXY[0, 0], snakeXY[1, 0], snakeXY, snakeLength, 1)) //If the snake collided with the wall, theres no point in checking if it collided with itself.
                colision = true;

            return (colision);
        }

        public void refreshInfoBar(int score, int speed)
        {
            Console.SetCursorPosition(5, 23);
            Console.Write("Score: " + score);

            Console.SetCursorPosition(5, 24);
            switch (speed)
            {
                case 90:
                    Console.Write("Speed: 1");
                    break;
                case 80:
                    Console.Write("Speed: 2");
                    break;
                case 70:
                    Console.Write("Speed: 3");
                    break;
                case 60:
                    Console.Write("Speed: 4");
                    break;
                case 50:
                    Console.Write("Speed: 5");
                    break;
                case 40:
                    Console.Write("Speed: 6");
                    break;
                case 30:
                    Console.Write("Speed: 7");
                    break;
                case 20:
                    Console.Write("Speed: 8");
                    break;
                case 10:
                    Console.Write("Speed: 9");
                    break;
            }

            Console.SetCursorPosition(52, 23);
            Console.Write("Coder: Alper SAHIN");

            Console.SetCursorPosition(52, 24);
            Console.Write("Version: 0.5");

            return;
        }

        /**
        *	  @name You Win Screen (youWinScreen)
        *
        *	  @brief You Win Screen
        *
        *	  Print you win screen , and create selection
        **/
        public void youWinScreen()
        {
            int x = 6, y = 7;
            Console.SetCursorPosition(x, y++);
            Console.Write("'##:::'##::'#######::'##::::'##::::'##:::::'##:'####:'##::: ##:'####:");
            Console.SetCursorPosition(x, y++);
            Console.Write(". ##:'##::'##.... ##: ##:::: ##:::: ##:'##: ##:. ##:: ###:: ##: ####:");
            Console.SetCursorPosition(x, y++);
            Console.Write(":. ####::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ####: ##: ####:");
            Console.SetCursorPosition(x, y++);
            Console.Write("::. ##:::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ## ## ##:: ##::");
            Console.SetCursorPosition(x, y++);
            Console.Write("::: ##:::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ##. ####::..:::");
            Console.SetCursorPosition(x, y++);
            Console.Write("::: ##:::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ##:. ###:'####:");
            Console.SetCursorPosition(x, y++);
            Console.Write("::: ##::::. #######::. #######:::::. ###. ###::'####: ##::. ##: ####:");
            Console.SetCursorPosition(x, y++);
            Console.Write(":::..::::::.......::::.......:::::::...::...:::....::..::::..::....::");
            Console.SetCursorPosition(x, y++);

            waitForAnyKey();
            Console.Clear(); //clear the console
            return;
        }
        /**
        *	  @name Game Over Screen (gameOverScreen)
        *
        *	  @brief Game Over Screen
        *
        *	  Print game over screen , and create selection
        **/
        public void gameOverScreen()
        {
            int x = 17, y = 3;

            //http://www.network-science.de/ascii/ <- Ascii Art Gen

            Console.SetCursorPosition(x, y++);
            Console.Write(":'######::::::'###::::'##::::'##:'########:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("'##... ##::::'## ##::: ###::'###: ##.....::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::..::::'##:. ##:: ####'####: ##:::::::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##::'####:'##:::. ##: ## ### ##: ######:::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##::: ##:: #########: ##. #: ##: ##...::::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##::: ##:: ##.... ##: ##:.:: ##: ##:::::::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(". ######::: ##:::: ##: ##:::: ##: ########:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(":......::::..:::::..::..:::::..::........::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(":'#######::'##::::'##:'########:'########::'####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("'##.... ##: ##:::: ##: ##.....:: ##.... ##: ####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::: ##: ##:::: ##: ##::::::: ##:::: ##: ####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::: ##: ##:::: ##: ######::: ########::: ##::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::: ##:. ##:: ##:: ##...:::: ##.. ##::::..:::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::: ##::. ## ##::: ##::::::: ##::. ##::'####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(". #######::::. ###:::: ########: ##:::. ##: ####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(":.......::::::...:::::........::..:::::..::....::\n");

            waitForAnyKey();
            Console.Clear(); //clear the console
            return;
        }

        /**
        *
        *	  @name  startGame (startGame)
        *
        *	  @brief startGame
        *
        *	  startGame
        *
        *	  @param  [in] snakeXY [\b int[,]] Snake's body coordinates
        *	  
        *	  @param  [in] foodXY [\b int[,]] food coordinates
        *	  
        *	  @param  [in] snakeLength [\b int]  Length of the Snake
        *	  
        *	  @param  [in] score [\b int]  
        *	  
        *	  @param  [in] speed [\b int]  
        *	  
        *	  @param  [in] consoleWidth [\b int]  
        *	  
        *	  @param  [in] consoleHeight [\b int]  
        *	  
        *	  @param  [in] direction [\b ConsoleKey]  
        *	  
        **/

        //Messy, need to clean this function up
        public void startGame(int[,] snakeXY, int[] foodXY, int consoleWidth, int consoleHeight, int snakeLength, ConsoleKey direction, int score, int speed)
        {
            bool gameOver = false;
            ConsoleKey oldDirection = ConsoleKey.NoName;
            bool canChangeDirection = true;
            int gameOver2 = 1;
            do
            {
                if (canChangeDirection)
                {
                    oldDirection = direction;
                    direction = checkKeysPressed(direction);
                }

                if (oldDirection != direction)//Temp fix to prevent the snake from colliding with itself
                    canChangeDirection = false;

                if (true) //haha, it moves according to how fast the computer running it is...
                {
                    //Console.SetCursorPosition(1,1);
                    //Console.Write("%d - %d",clock() , endWait);
                    move(snakeXY, snakeLength, direction);
                    canChangeDirection = true;


                    if (eatFood(snakeXY, foodXY))
                    {
                        generateFood(foodXY, consoleWidth, consoleHeight, snakeXY, snakeLength); //Generate More Food
                        snakeLength++;
                        switch (speed)
                        {
                            case 90:
                                score += 5;
                                break;
                            case 80:
                                score += 7;
                                break;
                            case 70:
                                score += 9;
                                break;
                            case 60:
                                score += 12;
                                break;
                            case 50:
                                score += 15;
                                break;
                            case 40:
                                score += 20;
                                break;
                            case 30:
                                score += 23;
                                break;
                            case 20:
                                score += 25;
                                break;
                            case 10:
                                score += 30;
                                break;
                        }

                        refreshInfoBar(score, speed);
                    }
                    Thread.Sleep(speed);
                }

                gameOver = collisionDetection(snakeXY, consoleWidth, consoleHeight, snakeLength);

                if (snakeLength >= SNAKE_ARRAY_SIZE - 5) //Just to make sure it doesn't get longer then the array size & crash
                {
                    gameOver2 = 2;//You Win! <- doesn't seem to work - NEED TO FIX/TEST THIS
                    score += 1500; //When you win you get an extra 1500 points!!!
                }

            } while (!gameOver);

            switch (gameOver2)
            {
                case 1:
                    gameOverScreen();

                    break;
                case 2:
                    youWinScreen();
                    break;
            }
            return;
        }
        /**
        *
        *	  @name  Load Enviroment (loadEnviroment)
        *
        *	  @brief Load Enviroment
        *
        *	  Load Enviroment
        *	  
        *	  @param  [in] consoleWidth [\b int]  
        *	  
        *	  @param  [in] consoleHeight [\b int]
        *	  
        **/
        public void loadEnviroment(int consoleWidth, int consoleHeight)//This can be done in a better way... FIX ME!!!! Also i think it doesn't work properly in ubuntu <- Fixed
        {
            int i;
            int x = 1, y = 1;
            int rectangleHeight = consoleHeight - 4;
            Console.Clear(); //clear the console

            Console.SetCursorPosition(x, y); //Top left corner

            for (; y < rectangleHeight; y++)
            {
                Console.SetCursorPosition(x, y); //Left Wall 
                Console.Write("%c", WALL);

                Console.SetCursorPosition(consoleWidth, y); //Right Wall
                Console.Write("%c", WALL);
            }

            y = 1;
            for (; x < consoleWidth + 1; x++)
            {
                Console.SetCursorPosition(x, y); //Left Wall 
                Console.Write("%c", WALL);

                Console.SetCursorPosition(x, rectangleHeight); //Right Wall
                Console.Write("%c", WALL);
            }
            return;
        }
        /**
        *
        *	  @name  Load Snake (loadSnake)
        *
        *	  @brief Load Snake
        *
        *	  Load Snake
        *
        *	  @param  [in] snakeXY [\b int[,]] Snake's body coordinates
        *	  
        *	  @param  [in] snakeLength [\b int]  Length of the Snake
        *	  
        **/
        public void loadSnake(int[,] snakeXY, int snakeLength)
        {
            int i;
            /*
            First off, The snake doesn't actually have enough XY coordinates (only 1 - the starting location), thus we use
            these XY coordinates to "create" the other coordinates. For this we can actually use the function used to move the snake.
            This helps create a "whole" snake instead of one "dot", when someone starts a game.
            */
            //moveSnakeArray(snakeXY, snakeLength); //One thing to note ATM, the snake starts of one coordinate to whatever direction it's pointing...

            //This should print out a snake :P
            for (i = 0; i < snakeLength; i++)
            {
                Console.SetCursorPosition(snakeXY[0, i], snakeXY[1, i]);
                Console.Write("%c", SNAKE_BODY); //Meh, at some point I should make it so the snake starts off with a head...
            }

            return;
        }
        /**
        *
        *	  @name  Prepair SnakeArray (prepairSnakeArray)
        *
        *	  @brief Prepair Snake Array
        *
        *	  Prepair Snake Array
        *
        *	  @param  [in] snakeXY [\b int[,]] Snake's body coordinates
        *	  
        *	  @param  [in] snakeLength [\b int]  Length of the Snake
        *	  
        **/

        /* NOTE, This function will only work if the snakes starting direction is left!!!! 
        Well it will work, but the results wont be the ones expected.. I need to fix this at some point.. */
        public void prepairSnakeArray(int[,] snakeXY, int snakeLength)
        {
            int i, x;
            int snakeX = snakeXY[0, 0];
            int snakeY = snakeXY[1, 0];

            


            for (i = 1; i <= snakeLength; i++)
            {
                snakeXY[0, i] = snakeX + i;
                snakeXY[1, i] = snakeY;
            }

            return;
        }
        /**
        *	  @name Load Game (loadGame)
        *
        *	  @brief Load Game
        *
        *	  Print loadgame , and create selection
        **/

        //This function loads the enviroment, snake, etc
        public void loadGame()
        {
            int[,] snakeXY = new int[2, SNAKE_ARRAY_SIZE]; //Two Dimentional Array, the first array is for the X coordinates and the second array for the Y coordinates

            int snakeLength = 4; //Starting Length

            ConsoleKey direction = ConsoleKey.LeftArrow; //DO NOT CHANGE THIS TO RIGHT ARROW, THE GAME WILL INSTANTLY BE OVER IF YOU DO!!! <- Unless the prepairSnakeArray function is changed to take into account the direction....

            int[] foodXY = { 5, 5 };// Stores the location of the food

            int score = 0;
            //int level = 1;

            //Window Width * Height - at some point find a way to get the actual dimensions of the console... <- Also somethings that display dont take this dimentions into account.. need to fix this...
            int consoleWidth = 80;
            int consoleHeight = 25;

            int speed = getGameSpeed();

            //The starting location of the snake
            snakeXY[0, 0] = 40;
            snakeXY[1, 0] = 10;

            loadEnviroment(consoleWidth, consoleHeight); //borders
            prepairSnakeArray(snakeXY, snakeLength);
            loadSnake(snakeXY, snakeLength);
            generateFood(foodXY, consoleWidth, consoleHeight, snakeXY, snakeLength);
            refreshInfoBar(score, speed); //Bottom info bar. Score, Level etc
            startGame(snakeXY, foodXY, consoleWidth, consoleHeight, snakeLength, direction, score, speed);

            return;
        }

        /**
       *
       *	  @name Menu Selector (menuSelector)
       *
       *	  @brief Menu Selector
       *
       *	  Menu Selector
       *	  
       *	  
       *	  @param  [in] a [\b int]  
       *	  
       *	  @param  [in] b [\b int] 
       *	  
       *	  @param  [in] bStart [\b int]
       **/
        public int menuSelector(int a, int b, int bStart)
        {
            ConsoleKey keys;
            int v = 0;
            a = a - 2;
            Console.SetCursorPosition(a, bStart);

            Console.Write(">");

            Console.SetCursorPosition(1, 1);
            // Even if it is not char, it converts it to char, which is called explicit conversion.

            do
            {
                keys = waitForAnyKey();
                if (keys == UP_ARROW)
                {
                    Console.SetCursorPosition(a, bStart + v);
                    Console.Write(" ");

                    if (bStart >= bStart + v)
                        v = b - bStart - 2;
                    else
                        v--;
                    Console.SetCursorPosition(a, bStart + v);
                    Console.Write(">");
                }
                else
                    if (keys == DOWN_ARROW)
                {
                    Console.SetCursorPosition(a, bStart + v);
                    Console.Write(" ");

                    if (v + 2 >= b - bStart)
                        v = 0;
                    else
                        v++;
                    Console.SetCursorPosition(a, bStart + v);
                    Console.Write(">");
                }
            } while (keys != ENTER_KEY);
            return (v);
        }
        /**
        *	  @name Welcome Art (welcomeArt)
        *
        *	  @brief Welcome Art
        *
        *	  Print welcome art , and create selection
        **/
        public void welcomeArt()
        {
            Console.Clear(); //clear the console
                             //Ascii art reference: http://www.chris.com/ascii/index.php?art=animals/reptiles/snakes
            Console.Write("\n");
            Console.Write("\t\t    _________         _________ 			\n");
            Console.Write("\t\t   /         \\       /         \\ 			\n");
            Console.Write("\t\t  /  /~~~~~\\  \\     /  /~~~~~\\  \\ 			\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  | 			\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  | 			\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  |         /	\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  |       //	\n");
            Console.Write("\t\t (o  o)    \\  \\_____/  /     \\  \\_____/ / 	\n");
            Console.Write("\t\t  \\__/      \\         /       \\        / 	\n");
            Console.Write("\t\t    |        ~~~~~~~~~         ~~~~~~~~ 		\n");
            Console.Write("\t\t    ^											\n");
            Console.Write("\t		Welcome To The Snake Game!			\n");
            Console.Write("\t			    Press Any Key To Continue...	\n");
            Console.Write("\n");

            waitForAnyKey();
            return;
        }
        /**
        *	  @name Controls (controls)
        *
        *	  @brief Controls
        *
        *	  Print controls , and create selection
        **/
        public void controls()
        {
            int x = 10, y = 5;
            Console.Clear(); //clear the console
            Console.SetCursorPosition(x, y++);
            Console.Write("Controls\n");
            Console.SetCursorPosition(x++, y++);
            Console.Write("Use the following arrow keys to direct the snake to the food: ");
            Console.SetCursorPosition(x, y++);
            Console.Write("Right Arrow");
            Console.SetCursorPosition(x, y++);
            Console.Write("Left Arrow");
            Console.SetCursorPosition(x, y++);
            Console.Write("Top Arrow");
            Console.SetCursorPosition(x, y++);
            Console.Write("Bottom Arrow");
            Console.SetCursorPosition(x, y++);
            Console.SetCursorPosition(x, y++);
            Console.Write("P & Esc pauses the game.");
            Console.SetCursorPosition(x, y++);
            Console.SetCursorPosition(x, y++);
            Console.Write("Press any key to continue...");
            waitForAnyKey();
            return;
        }
        /**
        *	  @name Main (main)
        *
        *	  @brief Main
        *
        *	  Print main , and create selection
        **/
        public int main() //Need to fix this up
        {

            welcomeArt();

            do
            {
                switch (mainMenu())
                {
                    case 0:
                        loadGame();
                        break;
                    case 1:
                        displayHighScores();
                        break;
                    case 2:
                        controls();
                        break;
                    case 3:
                        exitYN();
                        break;
                }
            } while (true);    //


        }
        /**
        *	  @name Main Menu (mainMenu)
        *
        *	  @brief Main Menu
        *
        *	  Print main menu, and create selection
        **/
        public int mainMenu()
        {
            int x = 10, y = 5;
            int yStart = y;

            int selected;

            Console.Clear(); //clear the console
                             //Might be better with arrays of strings???
            Console.SetCursorPosition(x, y++);
            Console.Write("New Game\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("High Scores\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("Controls\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("Exit\n");
            Console.SetCursorPosition(x, y++);

            selected = menuSelector(x, y, yStart);

            return (selected);
        }
        /**
        *	  @name Display High Scores (displayHighScores)
        *
        *	  @brief Display High Scores
        *
        *	  Print display high scores, and create selection
        **/
        public void displayHighScores() //NEED TO CHECK THIS CODE!!!
        {
            string[] str = File.ReadAllLines(@"..\\..\\..\\highscores.txt");
            int y = 5;
            Console.Clear();

            if (File.ReadAllLines(@"..\\..\\..\\highscores.txt") == null)
            {
                //Create the file, then try open it again.. if it fails this time exit.
                createHighScores(); //This should create a highscores file (If there isn't one)
                if (File.ReadAllLines(@"..\\..\\..\\highscores.txt") == null)
                    Environment.Exit(1);
            }

            Console.SetCursorPosition(10, y++);
            Console.WriteLine("High Scores");
            Console.SetCursorPosition(10, y++);
            Console.WriteLine("Rank\tScore\t\t\tName");
            for (int i = 0; i < str.Length; i++)
            {
                Console.SetCursorPosition(10, y++);
                Console.Write(str[i]);
            }
            //Close the file
            Console.SetCursorPosition(10, y++);
            Console.Write("Press any key to continue...");
            waitForAnyKey();
            return;
        }
        /**
        *	  @name Create High Scores (createHighScores)
        *
        *	  @brief Create High Scores
        *
        *	  Print create high scores, and create selection
        **/
        public void createHighScores()
        {
            TextWriter file = new StreamWriter(@"..\\..\\..\\highscores.txt");
            int i;
            if (file == null)
            {
                Console.Write("FAILED TO CREATE HIGHSCORES!!! EXITING!");
                Environment.Exit(0);
            }
            for (i = 0; i < 5; i++)
            {
                file.Write(i + 1);
                file.Write("\t0\t\t\tEMPTY\n");
            }
            file.Flush();
            file.Close();

            return;
        }

    }
}
