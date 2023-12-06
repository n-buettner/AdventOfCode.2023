namespace AdventOfCode._2023.Modules;

public class Day6 : BaseDay
{
    public override void PartOne()
    {
        var data = GetFileData("Data/Day6/Data.txt");

        var maxTimeList = data[0].Replace("Time:","").Trim().Split(" ").Select(int.Parse).ToList();
        var maxDistanceList = data[1].Replace("Distance:","").Trim().Split(" ").Select(int.Parse).ToList();

        var runningSum = 0;

        for (int i = 0; i < maxTimeList.Count; i++)
        {
            var ret = GetCountOverMax(maxTimeList[i], maxDistanceList[i]);

            if (runningSum == 0)
                runningSum = ret;
            else
                runningSum *= ret;
        }

        Console.WriteLine($"Day 6 Part 1: {runningSum}");
    }

    public override void PartTwo()
    {
        var data = GetFileData("Data/Day6/Data.txt");

        var maxTime = Convert.ToUInt64(data[0].Replace("Time:", "").Replace(" ", "").Trim());
        var maxDistance = Convert.ToUInt64(data[1].Replace("Distance:", "").Replace(" ", "").Trim());

        ulong ret = GetCountOverMax(maxTime, maxDistance);

        Console.WriteLine($"Day 6 Part 2: {ret}");
    }

    protected virtual int GetCountOverMax(int time, int currentMaxDistance)
    {
        int countOverMax = 0;
        
        for (int i = 0; i < time; i++)
        {
            var distance = (time - i) * i;
            if (distance > currentMaxDistance)
                countOverMax++;
        }

        return countOverMax;
    }

    protected virtual ulong GetCountOverMax(ulong time, ulong currentMaxDistance)
    {
        ulong countOverMax = 0;

        for (ulong i = 0; i < time; i++)
        {
            ulong distance = (time - i) * i;
            if (distance > currentMaxDistance)
                countOverMax++;
        }

        return countOverMax;
    }
}
