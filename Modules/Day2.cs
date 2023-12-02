namespace AdventOfCode._2023.Modules;

public class Day2
{
    public void PartOne()
    {
        var redMax = 12;
        var greenMax = 13;
        var blueMax = 14;

        var rowCounter = 0;

        var inputData = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "Data/Day2/Data.txt")).ToList();
        var rowDataList = new List<RowData>();

        foreach (var row in inputData)
        {
            var rowData = GetRowData(row.Trim());
            rowDataList.Add(rowData);

            if (rowData.MaxRed <= redMax && rowData.MaxGreen <= greenMax && rowData.MaxBlue <= blueMax)
                rowCounter += rowData.Id;
        }

        Console.WriteLine($"Day 2 Part 1 Sum: {rowCounter}");
    }

    public void PartTwo()
    {
        var rowCounter = 0;

        var inputData = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "Data/Day2/Data.txt")).ToList();
        var rowDataList = new List<RowData>();

        foreach (var row in inputData)
        {
            var rowData = GetRowData(row.Trim());
            rowDataList.Add(rowData);

            var power = rowData.MaxRed * rowData.MaxGreen * rowData.MaxBlue;

            rowCounter += power;
        }

        Console.WriteLine($"Day 2 Part 2 Sum: {rowCounter}");
    }

    protected static RowData GetRowData(string text)
    {
        //Game 61: 12 green, 1 red, 3 blue; 3 red, 4 blue, 19 green; 1 blue, 7 green

        var rowId = Convert.ToInt32(text.Trim().Split(":")[0].Split(" ")[1].Replace(":", ""));

        var rowColorData = text.Trim().Split(":")[1];
        var rowDataArray = rowColorData.Trim().Split(";");

        return ParseColorData(rowDataArray, rowId);
    }

    protected static RowData ParseColorData(string[] dataArray, int rowId)
    {
        int redMax = 0;
        int greenMax = 0;
        int blueMax = 0;

        foreach (var token in dataArray)
        {
            var colorArray = token.Trim().Split(",");

            foreach (var tokenColor in colorArray)
            {
                var count = Convert.ToInt32(tokenColor.Trim().Split(" ")[0]);
                var color = tokenColor.Trim().Split(" ")[1];

                switch (color)
                {
                    case "red":
                        if (count > redMax) redMax = count;
                        break;
                    case "green":
                        if (count > greenMax) greenMax = count;
                        break;
                    case "blue":
                        if (count > blueMax) blueMax = count;
                        break;
                };
            }
        }

        return new RowData() { Id = rowId, MaxRed = redMax, MaxGreen = greenMax, MaxBlue = blueMax };
    }
}

public class RowData
{
    public int Id { get; set; }
    public int MaxRed { get; set; }
    public int MaxGreen { get; set; }
    public int MaxBlue { get; set; }
}
