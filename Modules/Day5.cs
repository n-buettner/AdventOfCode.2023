using System.Diagnostics;
using System.Timers;

namespace AdventOfCode._2023.Modules;

public class Day5 : BaseDay
{
    public override void PartOne()
    {
        var data = GetFileData("Data/Day5/Data.txt");

        var parseTokens = new List<string> { "seeds", "seed-to-soil", "soil-to-fertilizer", "fertilizer-to-water", "water-to-light", "light-to-temperature", "-temperature-to-humidity", "humidity-to-location" };

        var dataMaps = new List<DataMap>();
        var currentMap = new DataMap();

        var seeds = data[0].Replace("seeds: ", "").Trim().Split(" ").Select(long.Parse).ToList();

        for (int i = 0; i < data.Count; i++)
        {
            if (string.IsNullOrEmpty(data[i])) 
                continue;

            if (data[i].Contains("seeds"))
                continue;

            if (data[i].Contains("map"))
            {
                if (!string.IsNullOrEmpty(currentMap.Name))
                    dataMaps.Add(currentMap);

                var dataMap = new DataMap()
                {
                    Name = data[i].Replace("map:", "").Trim(),
                    Offsets = new List<List<long>>()
                };

                currentMap = dataMap;
                continue;
            }

            currentMap.Offsets?.Add(data[i].Trim().Split(" ").Select(long.Parse).ToList());

            if (i == data.Count - 1)
                dataMaps.Add(currentMap);
        }

        var seedTracker = new List<long>();

        foreach (var seed in seeds)
            seedTracker.Add(TraverseMap(seed, dataMaps));

        Console.WriteLine($"Day 5 Part 1: {seedTracker.Min()}");
    }

    public override void PartTwo()
    {
        var data = GetFileData("Data/Day5/Data.txt");

        var parseTokens = new List<string> { "seeds", "seed-to-soil", "soil-to-fertilizer", "fertilizer-to-water", "water-to-light", "light-to-temperature", "-temperature-to-humidity", "humidity-to-location" };

        var dataMaps = new List<DataMap>();
        var currentMap = new DataMap();

        var seeds = data[0].Replace("seeds: ", "").Trim().Split(" ").Select(long.Parse).ToList();

        for (int i = 0; i < data.Count; i++)
        {
            if (string.IsNullOrEmpty(data[i]))
                continue;

            if (data[i].Contains("seeds"))
                continue;

            if (data[i].Contains("map"))
            {
                if (!string.IsNullOrEmpty(currentMap.Name))
                    dataMaps.Add(currentMap);

                var dataMap = new DataMap()
                {
                    Name = data[i].Replace("map:", "").Trim(),
                    Offsets = new List<List<long>>()
                };

                currentMap = dataMap;
                continue;
            }

            currentMap.Offsets?.Add(data[i].Trim().Split(" ").Select(long.Parse).ToList());

            if (i == data.Count - 1)
                dataMaps.Add(currentMap);
        }

        var seedTracker = new List<long>();
        var internalTracker = new List<long>();

        var timer = new Stopwatch();
        timer.Start();

        var timeTracker = 0.0;
        double prevLoops = 0.0;
        double timeRemaining = 0.0;

        for (int i = 0; i < seeds.Count; i += 2)
        {
            var lRange = seeds[i];
            var hRange = seeds[i] + seeds[i + 1] - 1;

            var range = hRange - lRange;
            var expectedLoops = Convert.ToDouble(range / 1000000);
            prevLoops = expectedLoops;

            Console.WriteLine($"Looping through range {lRange}-{hRange} for seed group {i}. Total values to loop: {range}");

            for (long j = lRange; j < hRange; j++)
            {
                if (j % 1000000 == 0)
                {
                    var loopsRemaining = (hRange - j) / 1000000;
                    if (timeTracker > 0)
                        timeRemaining = (prevLoops / timeTracker) * Convert.ToDouble(loopsRemaining);

                    Console.WriteLine($"Current value: {j} | Goal: {hRange} | Estimated Loops Remainining: {loopsRemaining} | Estimated time for current loop completion (seconds): {timeRemaining}");
                }

                internalTracker.Add(TraverseMap(j, dataMaps));
            }

            timeTracker = timer.Elapsed.TotalSeconds;
            Console.WriteLine($"Current timer elapsed seconds: {timeTracker}");

            //memory :)
            seedTracker.Add(internalTracker.Min());
            internalTracker.Clear();

            timer.Restart();
        }

        timer.Stop();
        Console.WriteLine($"Day 5 Part 2: {seedTracker.Min()}");
    }
    
    protected virtual long TraverseMap(long seed, List<DataMap> dataMaps)
    {
        var offsetTracker = seed;

        foreach (var dataMap in dataMaps)
        {
            var offsetList = dataMap.Offsets.FirstOrDefault(x => offsetTracker >= x[1] && offsetTracker <= x[1] + x[2] - 1);

            if (offsetList is null)
                continue;

            offsetTracker = offsetTracker + (offsetList[0] - offsetList[1]);
        }

        return offsetTracker;
    }
}

public class DataMap
{
    public string? Name { get; set; }
    public List<List<long>>? Offsets { get; set; }
}
