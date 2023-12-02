using System.Text.RegularExpressions;

namespace AdventOfCode._2023.Modules;

public class Day1
{
    public void PartOne()
    {
        var data = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "Data/Day1/Day1.txt")).ToList();

        int sum = 0;

        foreach (var row in data)
        {
            if (row is null) continue;

            var rowDigits = row.Where(c => char.IsDigit(c)).ToList();

            if (rowDigits.Count == 0) continue;

            var rowDigitCombine = $"{rowDigits[0]}{rowDigits[^1]}";

            sum += Convert.ToInt32(rowDigitCombine);
        }

        Console.WriteLine($"Day 1 Part 1: {sum}");
    }

    public void PartTwo()
    {
        var data = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "Data/Day1/Day1.txt")).ToList();

        int sum = 0;

        foreach (var row in data)
        {
            if (row is null) continue;

            var convertedRow = ConvertTextNumbersToNumbers(row);

            //Console.WriteLine($"{row} | {convertedRow}");

            var rowDigits = convertedRow?.Where(c => char.IsDigit(c))?.ToList();

            if (rowDigits is null || rowDigits.Count == 0) continue;

            var rowDigitCombine = $"{rowDigits[0]}{rowDigits[^1]}";

            sum += Convert.ToInt32(rowDigitCombine);
        }

        Console.WriteLine($"Day 1 Part 2: {sum}");
    }

    public static string? ConvertTextNumbersToNumbers(string? text)
    {
        if (string.IsNullOrEmpty(text)) return null;

        var searchVal = text.ToLower();
        var baseDictionary = new Dictionary<string, List<int>>();

        do
        {
            var dictionary = GetMatchingDictionary(searchVal);
            baseDictionary = dictionary;

            searchVal = ReplaceLowestIndexValue(searchVal, dictionary);

        } while (baseDictionary.Values.Count > 0);

        return searchVal;
    }

    protected static Dictionary<string, List<int>> GetMatchingDictionary(string text)
    {
        var numbers = new List<string> { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        var searchVal = text.ToLower();

        var matching = new Dictionary<string, List<int>>();

        for (int i = 0; i < numbers.Count; i++)
        {
            var indexList = new List<int>();

            for (int index = 0; ; index += numbers[i].Length)
            {
                index = searchVal.IndexOf(numbers[i], index);

                if (index == -1) break;

                indexList.Add(index);
            }

            if (indexList.Count > 0)
                matching.Add(numbers[i], indexList);
        }

        return matching;
    }

    protected static string ReplaceLowestIndexValue(string searchVal, Dictionary<string, List<int>> dictionary)
    {
        if (dictionary.Values.Count == 0) return searchVal;

        var numbers = new List<string> { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        var numberReplacer = new List<string> { "o1e", "t2o", "thre3e", "fou4r", "fiv5e", "s6x", "seve7n", "eigh8t", "nin9e" };

        //start replacing values with the lowest index from the dictionary. check to make sure the string after the index still matches the key
        var lowestIndex = dictionary.Values.SelectMany(x => x).Min();
        var lowestIndexKey = dictionary.First(x => x.Value.Contains(lowestIndex)).Key;

        if (searchVal.Substring(lowestIndex, lowestIndexKey.Length).Equals(lowestIndexKey))
        {
            var regex = new Regex(Regex.Escape(lowestIndexKey));
            searchVal = regex.Replace(searchVal, numberReplacer[numbers.IndexOf(lowestIndexKey)], 1);
        }

        return searchVal;
    }
}
