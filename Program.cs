class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("==================================");
        Console.WriteLine("Welcome to your Character Manager!");
        Console.WriteLine("==================================\n");

        while (true) { 
            //Display Menu at the top of each loop
            DisplayMenu();

            var userInput = Console.ReadLine();
            Console.WriteLine($"You chose: {userInput}\n");

            //option 1 displays characters from .csv
            if (userInput == "1")
            {
                DisplayCharacters();
            }

            //option 2 adds a new character based on user input and writes to .csv
            else if (userInput == "2")
            {
                using (StreamWriter writer = new StreamWriter("input.csv", true))
                {
                    var myUserData = AddCharacter();
                    writer.WriteLine(myUserData);
                }
            }

            //option 3 allows leveling of a character 
            else if (userInput == "3")
            {
                LevelCharacter();
            }

            //option 4 exits
            else if (userInput == "4")
            {
                Console.WriteLine("Exiting Program...");
                break;
            }
        }


    }
    static void DisplayMenu() //simple menu
    {
        Console.WriteLine("1. Display Characters");
        Console.WriteLine("2. Add Character");
        Console.WriteLine("3. Level up Character");
        Console.WriteLine("4. Exit");
        Console.Write("> ");
    }

    static void DisplayCharacters() //stores .csv in array, loops through displaying each character
    {

        var lines = File.ReadAllLines("input.csv");

        foreach (var line in lines)
        {
            var cols = line.Split(",");
            var pname = cols[0];
            var pclass = cols[1];
            var plevel = cols[2];
            var phealth = cols[3];
            var equipment = cols[4];


            Console.WriteLine($"Name: {pname}");
            Console.WriteLine($"Class: {pclass}");
            Console.WriteLine($"Level: {plevel}");
            Console.WriteLine($"Health: {phealth}");

            var equiplist = equipment.Split("|");
            Console.WriteLine($"Equipment: ");
            foreach (var eq in equiplist)
            {
                Console.WriteLine($"\t{eq}");
            }
            Console.WriteLine("==============================");
        }
        Console.WriteLine("\n");

    }

    static string AddCharacter() //takes user input and returns the character's info as a string
    {
        Console.Write("Enter your character's name: ");
        string name = Console.ReadLine();

        Console.Write("Enter your character's class: ");
        string characterClass = Console.ReadLine();

        Console.Write("Enter your character's level: ");
        int level = int.Parse(Console.ReadLine());

        Console.Write("Enter your character's Hitpoints: ");
        int hitpoints = int.Parse(Console.ReadLine());

        Console.Write("Enter your character's equipment (separate items with a '|'): ");
        string[] equipment = Console.ReadLine().Split('|');

        Console.WriteLine($"\nWelcome, {name} the {characterClass}! You are level {level} and your equipment includes: {string.Join(", ", equipment)}.\n");

        return $"{name},{characterClass},{level},{hitpoints},{string.Join("|", equipment)}";
    }

    static void LevelCharacter() //choose character, input level and hp to add, update .csv
    {
        string[] characters = File.ReadAllLines("input.csv");

        Console.WriteLine("Please choose your character: ");
        
        for (int i = 0; i < characters.Length; i++) //loops over character names
        {
            Console.WriteLine($"{i+1}. {characters[i].Split(",")[0]} the {characters[i].Split(",")[1]}");
        }

        Console.Write("> ");
        var playerChoice = int.Parse(Console.ReadLine()); //choose character
        Console.WriteLine();

        //pluck character and split it immediately into an array
        var player = characters[playerChoice - 1].Split(",");
        var baseLevel = int.Parse(player[2]);
        var baseHp = int.Parse(player[3]);

        Console.WriteLine($"{player[0]} is level {player[2]} with {player[3]} hitpoints.");

        Console.Write($"How many levels does {player[0]} gain? ");
        var levelGain = int.Parse(Console.ReadLine());
        Console.Write($"How many hitpoints does {player[0]} gain? ");
        var hpGain = int.Parse(Console.ReadLine());
        Console.WriteLine();

        var finalLevel = baseLevel + levelGain;
        var finalHp = baseHp + hpGain;

        //update level and hp in player
        player[2] = $"{finalLevel}";
        player[3] = $"{finalHp}";
         
        //reconnect player string
        var updatedPlayer = String.Join(",", player);

        Console.Write("Saving...    ");


        //write characters back to .csv, using updatedPlayer in correct position
        using (StreamWriter writer = new StreamWriter("input.csv", false))
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if (i == (playerChoice - 1))
                {
                    writer.WriteLine(updatedPlayer);
                }
                else
                    writer.WriteLine(characters[i]);
            }
           
        }

        Console.WriteLine("Saved\n");
    }
}