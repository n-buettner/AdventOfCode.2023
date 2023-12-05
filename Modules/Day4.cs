namespace AdventOfCode._2023.Modules;

public class Day4 : BaseDay
{
    public override void PartOne()
    {
        var data = GetFileData("Data/Day4/Data.txt");
        var runningTotal = 0;

        foreach (var row in data)
        {
            if (string.IsNullOrEmpty(row)) continue;

            var winningNumbers = row.Split(":")[1].Split("|")[0].Trim().Replace("  "," ").Split(" ").ToList();
            var ourNumbers = row.Split(":")[1].Split("|")[1].Trim().Replace("  ", " ").Split(" ").ToList();

            var matches = 0;

            var firstMatch = winningNumbers.FindIndex(x => ourNumbers.Contains(x));
            Console.WriteLine($"Firstmatch found is {firstMatch}");

            if (firstMatch != -1)
                matches++;

            for (var i = firstMatch + 1; i < winningNumbers.Count; i++)
            {
                if (ourNumbers.Contains(winningNumbers[i]))
                    matches++;
            }

            Console.WriteLine($"Found {matches} in current row. Result would be {(int)Math.Pow(2, matches - 1)}");
            runningTotal += matches == 0 ? 0 : (int)Math.Pow(2, matches - 1);
        }

        Console.WriteLine($"Day 4 Part 1: {runningTotal}");
    }

    public override void PartTwo()
    {
        var data = GetFileData("Data/Day4/Data.txt");

        var totalsArray = new List<int>();

        //count the first card
        for (var index = 0; index < data.Count; index ++)
            totalsArray.Add(1);

        for (var index = 0; index < data.Count ; index ++)
        {
            var row = data[index];

            if (string.IsNullOrEmpty(row)) continue;

            var winningNumbers = row.Split(":")[1].Split("|")[0].Trim().Replace("  ", " ").Split(" ").ToList();
            var ourNumbers = row.Split(":")[1].Split("|")[1].Trim().Replace("  ", " ").Split(" ").ToList();

            var matches = winningNumbers.FindAll(x => ourNumbers.Contains(x)).Count;

            for (var i = index + 1; i < index + matches + 1; i++)
                totalsArray[i] += 1 * totalsArray[index];
        }

        Console.WriteLine($"Day 4 Part 2: {totalsArray.Sum()}");
    }
}
