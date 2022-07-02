namespace Stopwatch
{
    class App
    {
        static void Main()
        {
            // Make the cursor invisible.
            Console.CursorVisible = false;

            // Make the console window smaller.
            Console.SetWindowSize(35, 8);
            Console.SetBufferSize(35, 8);
            
            // This boolean will influence the path the code takes depending
            // on if the user enters a valid value or not later on.
            // No questionable user inputs have been made so far, so this
            // boolean will be initialized as false.
            bool hasFormatException = false;
            
            // This will later determine how often the stopwatch is updated
            // per second. For now, it'll be set to 500 milliseconds.
            int pollingRate = 500;

            // We need to remember what time it is when we start the stopwatch.
            // We will be comparing the time difference between the start time
            // and the current time recurrently, and print that difference.
            DateTime startTime;
            DateTime currentTime;

            // Here, the console is "idling" while showing the time and awaiting
            // an ENTER press.
            Console.WriteLine("Press ENTER to continue.");
            do
            {
                // This loop writes out the current time every 100 milliseconds...
                currentTime = DateTime.Now;
                Console.Write(currentTime);
                Thread.Sleep(100);
                foreach (char letter in currentTime.ToString())
                    Console.Write("\r");
            } 
            // ...for as long as the ENTER key isn't pressed.
            while (!(Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Enter));

            Console.Clear();

        // This lets the user pick a value between 1 and 10.
        // The higher the value, the more often the stopwatch
        // will update per second.
        setrate:

            // Only ask the user to input a value if the format exception
            // boolean from earlier wasn't set to true.
            if (!hasFormatException)
            {
                Console.CursorVisible = true;
                Console.Write("Set polling rate (1-10): ");
            }

            // The program has to make sure the user input is an integer,
            // so we have to put this code into a try block.
            try
            {
                // If the value entered earlier was invalid,
                // we will skip asking the user again.
                if (hasFormatException) goto skip;
                
                // If not, convert the user input into an int right away.
                var userInput = Convert.ToInt32(Console.ReadLine());

                // If the value entered is lower than 1 or higher than 10,
                // use a default value and move onto the next instruction.
                // Otherwise, proceed as normal.
                if (userInput < 1 | userInput > 10)
                    goto skip; 
                else goto success;

                // For when the user entered an invalid value.
                skip:
                userInput = 6;

                // If the input is valid, divide pollingRate by it.
                // This gives us a custom refresh rate.
            success:
                pollingRate /= userInput;
                Console.CursorVisible = false;
            }

            // If the input is not an integer, we will skip asking
            // the user again.
            catch (FormatException)
            {
                hasFormatException = true;
                Console.Clear();
                goto setrate;
            }

        reset:
            
            // Let's prepare the actual stopwatch.
            Console.Clear();
            startTime = DateTime.Now;
            Console.WriteLine("Press ENTER to stop.");

            // The actual stopwatch operates inside this loop.
            // Basically, it recurrently subtracts the start time from
            // the current time and gives you the time difference.
            // This makes this stopwatch reasonably accurate.
            do
            {
                currentTime = DateTime.Now;
                
                Console.Write("Time: " + (currentTime - startTime));

                Thread.Sleep(pollingRate);

                foreach (char letter in (currentTime - startTime).ToString()) 
                    Console.Write("\r"); 
            }
            while (!(Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Enter));

            Console.Clear();
            Console.WriteLine($"Start time: {startTime}");
            Console.WriteLine($"Time: {currentTime - startTime}");
            
            // If the user wants another go, they can press R to reset
            // the stopwatch. If not, they can press any key to exit.
            Console.WriteLine("\nPress R to reset.");
            Console.WriteLine("Press any key to exit.");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.R:
                    goto reset;
                default:
                    break;
            }
        }
    }
}

