namespace AdventOfCode._2023.Modules;

public abstract class BaseDay
{
    public virtual void PartOne()
    {
        Console.WriteLine("Part 1 not implemented");
        return;
    }

    public virtual void PartTwo()
    {
        Console.WriteLine("Part 2 not implemented");
        return;
    }

    public virtual List<string?> GetFileData(string? fileName)
    {
        var def = new List<string?>();

        if (string.IsNullOrEmpty(fileName)) return def;

        var data = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), fileName)).ToList();

        if (data is null || data.Count == 0) return def;

        return data;
    }
}
