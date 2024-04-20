using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        // Input if you want to play as the guesser or the opponent
        Console.Write("Do you want to play as the guesser (G) or the opponent (O)? ");
        string playerRole = Console.ReadLine();

        // Validate the input
        if (playerRole != "G" && playerRole != "O")
        {
            Console.WriteLine("Invalid input! Please enter 'G' to play as the guesser or 'O' to play as the opponent.");
            return;
        }

        if (playerRole == "G")
        {
            string DebugSecret = "9536";
            // Secret is now hidden and unknown. We need to guess until there is only one possibility left
            List<string> possibilities = GeneratePossibilities();
            string nextGuess = possibilities[0]; // Initialize the first guess
            Console.WriteLine(nextGuess);

            while (possibilities.Count > 1)
            {
                Console.Write($"Enter the response (e.g., {GenerateResponce(nextGuess, DebugSecret)}): ");
                string response = Console.ReadLine();

                List<string> numbersToRemove = new List<string>();
                foreach (string number in possibilities)
                {
                    int correctPositionCount = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (number[i] == nextGuess[i])
                        {
                            correctPositionCount++;
                        }
                    }

                    int correctDigitCount = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (nextGuess.Contains(number[i]) && number[i] != nextGuess[i])
                        {
                            correctDigitCount++;
                        }
                    }

                    string calculatedResponse = $"{correctPositionCount}R{correctDigitCount}W";
                    if (calculatedResponse != response)
                    {
                        numbersToRemove.Add(number);
                    }
                }

                foreach (string numberToRemove in numbersToRemove)
                {
                    possibilities.Remove(numberToRemove);
                }

                nextGuess = possibilities[0];
                Console.WriteLine(nextGuess);
            }
            // The last remaining possibility is the secret number
            Console.WriteLine("The secret number is: " + possibilities[0]);
        }
        else
        {   
            // Input the opponent's secret number
            Console.Write("Enter your secret number: ");
            string secretNumber = Console.ReadLine();

            // Check if the input is a number with 4 different digits
            if (secretNumber.Length != 4 || !IsNumber(secretNumber) || !IsDifferent(secretNumber))
            {
                Console.WriteLine("Invalid input! Please enter a 4-digit number with different digits.");
                return;
            }

            // Initialize the pool of possibilities (all 4-digit numbers with different digits)
            List<string> possibilities = GeneratePossibilities();

            Console.WriteLine("Let's play the game!");

            string nextGuess = possibilities[0]; // Initialize the first guess

            Console.WriteLine("My first guess is: " + nextGuess);

            // Repeat the process until the secret number is guessed
            while (true)
            {
                // Receive feedback from the opponent
                Console.Write($"Enter the response (e.g., {GenerateResponce(nextGuess, secretNumber)}): ");
                string response = Console.ReadLine();

                // Validate the response format
                if (!IsValidResponse(response))
                {
                    Console.WriteLine("Invalid response format! Please enter a response in the format 'NRNW'.");
                    continue; // Skip the current iteration and prompt the user to input the response again
                }

                // Eliminate numbers from the pool based on the feedback
                List<string> numbersToRemove = new List<string>();
                foreach (string number in possibilities)
                {
                    // Calculate the number of correct digits in the correct position (R)
                    int correctPositionCount = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (number[i] == nextGuess[i])
                        {
                            correctPositionCount++;
                        }
                    }

                    // Calculate the number of correct digits in the wrong position (W)
                    int correctDigitCount = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (nextGuess.Contains(number[i]) && number[i] != nextGuess[i])
                        {
                            correctDigitCount++;
                        }
                    }

                    // Compare the calculated R and W with the feedback
                    string calculatedResponse = $"{correctPositionCount}R{correctDigitCount}W";
                    if (calculatedResponse != response)
                    {
                        // If the calculated response does not match the actual response, add the number to the list of numbers to be removed
                        numbersToRemove.Add(number);
                    }
                }

                // Remove numbers from the pool based on the feedback
                foreach (string numberToRemove in numbersToRemove)
                {
                    possibilities.Remove(numberToRemove);
                }

                // Check if the correct number is guessed
                if (response == "4R0W")
                {
                    Console.WriteLine("Congratulations! I guessed your number: " + nextGuess);
                    break; // Exit the loop if the correct number is guessed
                }

                // Generate the next guess (simply pick the first number from the reduced pool)
                nextGuess = possibilities.Any() ? possibilities[0] : "No valid guesses left.";
                if (nextGuess == "No valid guesses left.")
                {
                    Console.WriteLine("There are no valid guesses left. Please ensure your feedback is correct or there was no mistake in the previous guesses.");
                    break; // Exit the loop as there are no valid guesses left
                }

                Console.WriteLine("My next guess is: " + nextGuess);
            }
        }

    }

    // Function that generates a response based on argumented guess vs secret number
    static string GenerateResponce(string guess, string secret)
    {
        // Check if the input is a number with 4 different digits
        if (secret.Length != 4 || !IsNumber(secret) || !IsDifferent(secret))
        {
            Console.WriteLine("Invalid input! Please enter a 4-digit number with different digits.");
            return "";
        }

        // Calculate the number of correct digits in the correct position (R)
        int correctPositionCount = 0;
        for (int i = 0; i < 4; i++)
        {
            if (secret[i] == guess[i])
            {
                correctPositionCount++;
            }
        }

        // Calculate the number of correct digits in the wrong position (W)
        int correctDigitCount = 0;
        for (int i = 0; i < 4; i++)
        {
            if (guess.Contains(secret[i]) && secret[i] != guess[i])
            {
                correctDigitCount++;
            }
        }

        // Generate the response
        return $"{correctPositionCount}R{correctDigitCount}W";
    }

    // Generate a list of all 4-digit numbers with different digits
    static List<string> GeneratePossibilities()
    {
        List<string> possibilities = new List<string>();
        for (int i = 123; i <= 9876; i++)
        {
            string number = i.ToString().PadLeft(4, '0');
            if (IsDifferent(number))
            {
                possibilities.Add(number);
            }
        }
        return possibilities;
    }

    // Check if the input is a number
    static bool IsNumber(string data)
    {
        foreach (char c in data)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }
        return true;
    }

    // Check if the input has 4 different digits
    static bool IsDifferent(string data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            for (int j = i + 1; j < data.Length; j++)
            {
                if (data[i] == data[j])
                {
                    return false;
                }
            }
        }
        return true;
    }

    // Validate the response format
    static bool IsValidResponse(string response)
    {
        // Check if the response has length 4 and contains valid characters
        if (response.Length != 4 || response[1] != 'R' || response[3] != 'W' ||
            !char.IsDigit(response[0]) || !char.IsDigit(response[2]))
        {
            return false;
        }

        // Optionally, you can add more validation rules here

        return true;
    }
}