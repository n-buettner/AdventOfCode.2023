using System.Text.RegularExpressions;

namespace AdventOfCode._2023.Modules;

public class Day3 : BaseDay
{
    public override void PartOne()
    {
        var data = GetFileData("Data/Day3/Data.txt");
        var counter = 0;

        var result = new List<int>();

        if (data is null || data.Count == 0)
        {
            Console.WriteLine("No data found");
            return;
        }

        //create 3 lists to use as offsets. row 1 and last row will need special handling
        var currentGroup = new List<string?>();
        var currentRowGroup = new List<RowDataDay3>();

        foreach (var row in data)
        {
            currentGroup.Clear();
            currentRowGroup.Clear();

            var rowPrev = new RowDataDay3();
            var rowCurrent = new RowDataDay3();
            var rowNext = new RowDataDay3();

            if (counter == 0)
            {
                var current = data.Skip(counter).Take(2).ToList();
                currentGroup.AddRange(current);

                rowPrev = new RowDataDay3() { OffsetRow = -1 };
                rowCurrent = new RowDataDay3() { Text = currentGroup[0], RowLength = currentGroup[0]?.Length ?? 0, OffsetRow = 0 };
                rowNext = new RowDataDay3() { Text = currentGroup[1], RowLength = currentGroup[1]?.Length ?? 0, OffsetRow = 1 };
            }
            else if (counter == data.Count - 1)
            {
                var current = data.Skip(counter - 1).Take(2).ToList();
                currentGroup.AddRange(current);

                rowPrev = new RowDataDay3() { Text = currentGroup[0], RowLength = currentGroup[0]?.Length ?? 0, OffsetRow = -1 };
                rowCurrent = new RowDataDay3() { Text = currentGroup[1], RowLength = currentGroup[1]?.Length ?? 0, OffsetRow = 0 };
                rowNext = new RowDataDay3() { OffsetRow = 1 };
            }
            else
            {
                var current = data.Skip(counter - 1).Take(3).ToList();
                currentGroup.AddRange(current);

                rowPrev = new RowDataDay3() { Text = currentGroup[0], RowLength = currentGroup[0]?.Length ?? 0, OffsetRow = -1 };
                rowCurrent = new RowDataDay3() { Text = currentGroup[1], RowLength = currentGroup[1]?.Length ?? 0, OffsetRow = 0 };
                rowNext = new RowDataDay3() { Text = currentGroup[2], RowLength = currentGroup[2]?.Length ?? 0, OffsetRow = 1 };
            }

            rowCurrent.Matches = GetMatches(rowCurrent, "[0-9]+");

            currentRowGroup.Add(rowPrev);
            currentRowGroup.Add(rowCurrent);
            currentRowGroup.Add(rowNext);

            if (rowCurrent.Matches is null || rowCurrent.Matches.Count == 0)
            {
                counter++;
                continue;
            }

            var validMatch = ReturnMatchIfValid(currentRowGroup);
            if (validMatch != 0)
                result.Add(validMatch);

            counter++;
        }

        Console.WriteLine($"Day 3 Part 1: {result.Sum()}");
    }

    public override void PartTwo()
    {
        var data = GetFileData("Data/Day3/Data.txt");
        var counter = 0;

        var result = new List<int>();

        if (data is null || data.Count == 0)
        {
            Console.WriteLine("No data found");
            return;
        }

        //create 3 lists to use as offsets. row 1 and last row will need special handling
        var currentGroup = new List<string?>();
        var currentRowGroup = new List<RowDataDay3>();

        foreach (var row in data)
        {
            currentGroup.Clear();
            currentRowGroup.Clear();

            var rowPrev = new RowDataDay3();
            var rowCurrent = new RowDataDay3();
            var rowNext = new RowDataDay3();

            if (counter == 0)
            {
                var current = data.Skip(counter).Take(2).ToList();
                currentGroup.AddRange(current);

                rowPrev = new RowDataDay3() { OffsetRow = -1 };
                rowCurrent = new RowDataDay3() { Text = currentGroup[0], RowLength = currentGroup[0]?.Length ?? 0, OffsetRow = 0 };
                rowNext = new RowDataDay3() { Text = currentGroup[1], RowLength = currentGroup[1]?.Length ?? 0, OffsetRow = 1 };
            }
            else if (counter == data.Count - 1)
            {
                var current = data.Skip(counter - 1).Take(2).ToList();
                currentGroup.AddRange(current);

                rowPrev = new RowDataDay3() { Text = currentGroup[0], RowLength = currentGroup[0]?.Length ?? 0, OffsetRow = -1 };
                rowCurrent = new RowDataDay3() { Text = currentGroup[1], RowLength = currentGroup[1]?.Length ?? 0, OffsetRow = 0 };
                rowNext = new RowDataDay3() { OffsetRow = 1 };
            }
            else
            {
                var current = data.Skip(counter - 1).Take(3).ToList();
                currentGroup.AddRange(current);

                rowPrev = new RowDataDay3() { Text = currentGroup[0], RowLength = currentGroup[0]?.Length ?? 0, OffsetRow = -1 };
                rowCurrent = new RowDataDay3() { Text = currentGroup[1], RowLength = currentGroup[1]?.Length ?? 0, OffsetRow = 0 };
                rowNext = new RowDataDay3() { Text = currentGroup[2], RowLength = currentGroup[2]?.Length ?? 0, OffsetRow = 1 };
            }

            rowCurrent.Matches = GetMatches(rowCurrent, "[0-9]+");
            rowCurrent.AsteriskMatches = GetMatches(rowCurrent, "[*]");

            if (rowCurrent.AsteriskMatches is null || rowCurrent.AsteriskMatches.Count == 0)
            {
                //Console.WriteLine($"Skipping row as no asterisks");
                counter++;
                continue;
            }

            rowPrev.Matches = GetMatches(rowPrev, "[0-9]+");
            rowNext.Matches = GetMatches(rowNext, "[0-9]+");

            currentRowGroup.Add(rowPrev);
            currentRowGroup.Add(rowCurrent);
            currentRowGroup.Add(rowNext);

            var validMatch = ReturnAsteriskMatchesIfValid(currentRowGroup);
            if (validMatch != 0)
                result.Add(validMatch);

            counter++;
        }

        Console.WriteLine($"Day 3 Part 2: {result.Sum()}");
    }

    protected MatchCollection? GetMatches(RowDataDay3 rowData, string regex)
    {
        var matches = Regex.Matches(rowData.Text ?? "", regex);

        return matches;
    }

    protected int ReturnMatchIfValid(List<RowDataDay3> rowDataGroup)
    {
        //check if the surrounding values of the previous row are non periods, or if the current string left and right is non period. account for start/end of line too
        //always checking the row data that is offset 0
        var currentRow = rowDataGroup.FirstOrDefault(x => x.OffsetRow == 0);
        var rowMatches = new List<int>();

        if (currentRow is null || currentRow.Matches is null || currentRow.Matches.Count == 0) return 0;

        foreach (Match match in currentRow.Matches)
        {
            //can't be negative for row start
            var leftBound = match.Index - 1 == -1 ? 0 : match.Index - 1;
            //can't be more than row length
            var rightBound = match.Index + match.Length >= currentRow.RowLength ? match.Index + match.Length : match.Index + match.Length + 1;

            if (leftBound > 0)
            {
                if (currentRow.Text?[leftBound] != '.')
                {
                    rowMatches.Add(int.Parse(match.Value));
                    continue;
                }
            }

            if (rightBound < currentRow.RowLength)
            {
                if (currentRow.Text?[rightBound - 1] != '.')
                {
                    rowMatches.Add(int.Parse(match.Value));
                    continue;
                }
            }

            var validRegex = "[^.]";
            var previousSearchString = rowDataGroup.FirstOrDefault(x => x.OffsetRow == -1)?.Text?[leftBound..rightBound];

            if (!string.IsNullOrEmpty(previousSearchString))
            {
                var previousRowMatch = Regex.Match(previousSearchString, validRegex);
                if (previousRowMatch.Success)
                {
                    rowMatches.Add(int.Parse(match.Value));
                    continue;
                }

                //no match means the previous row is all periods and we need to check the next row instead
            }

            var nextSearchString = rowDataGroup.FirstOrDefault(x => x.OffsetRow == 1)?.Text?[leftBound..rightBound];

            if (!string.IsNullOrEmpty(nextSearchString))
            {
                var nextRowMatch = Regex.Match(nextSearchString, validRegex);
                if (nextRowMatch.Success)
                {
                    rowMatches.Add(int.Parse(match.Value));
                    continue;
                }

                //no match in next row means the match being checked can be ignored
            }
        }

        var result = rowMatches.Sum();

        return result;
    }

    protected int ReturnAsteriskMatchesIfValid(List<RowDataDay3> rowDataGroup)
    {
        var currentRow = rowDataGroup.FirstOrDefault(x => x.OffsetRow == 0);
        var rowMatches = new List<int>();

        var digitRegex = "[0-9]";
        var currentSum = 0;

        //Console.WriteLine("Starting Row Grouping...");

        if (currentRow is null || currentRow.AsteriskMatches is null || currentRow.AsteriskMatches.Count == 0) return 0;

        foreach (Match match in currentRow.AsteriskMatches)
        {
            //can't be negative for row start
            var leftBound = match.Index - 1 == -1 ? 0 : match.Index - 1;
            //can't be more than row length
            var rightBound = match.Index + match.Length >= currentRow.RowLength ? match.Index + match.Length : match.Index + match.Length + 1;

            if (leftBound > 0)
            {
                if (Regex.Match(currentRow.Text?[leftBound].ToString(), digitRegex).Success)
                {
                    var buddyMatch = currentRow.Matches?.FirstOrDefault(x => x.Index <= leftBound && x.Index + x.Length >= leftBound);
                    if (buddyMatch is not null)
                    {
                        rowMatches.Add(int.Parse(buddyMatch.Value));
                    }
                }
            }

            if (rightBound < currentRow.RowLength)
            {
                if (Regex.Match(currentRow.Text?[rightBound - 1].ToString(), digitRegex).Success)
                {
                    var buddyMatch = currentRow.Matches?.FirstOrDefault(x => x.Index <= rightBound && x.Index + x.Length >= rightBound);
                    if (buddyMatch is not null)
                    {
                        rowMatches.Add(int.Parse(buddyMatch.Value));
                    }
                }
            }

            var previousSearchString = rowDataGroup.FirstOrDefault(x => x.OffsetRow == -1)?.Text?[leftBound..rightBound];

            if (!string.IsNullOrEmpty(previousSearchString))
            {
                var previousRowMatch = Regex.Match(previousSearchString, digitRegex);
                if (previousRowMatch.Success)
                {
                    //these could match x.x or xxx or .xx or xx.
                    var matches = rowDataGroup.FirstOrDefault(x => x.OffsetRow == -1)?.Matches?
                        .Where(
                                x => x.Index <= leftBound && x.Index + x.Length - 1 >= leftBound
                                || x.Index >= leftBound && x.Index + x.Length <= rightBound
                                || x.Index < rightBound && x.Index + x.Length >= rightBound
                            )
                        .ToList();
                    if (matches is not null && matches.Count > 0)
                    {
                        rowMatches.AddRange(matches.Select(x => int.Parse(x.Value)));
                    }
                }
            }

            var nextSearchString = rowDataGroup.FirstOrDefault(x => x.OffsetRow == 1)?.Text?[leftBound..rightBound];

            if (!string.IsNullOrEmpty(nextSearchString))
            {
                var nextRowMatch = Regex.Match(nextSearchString, digitRegex);
                if (nextRowMatch.Success)
                {
                    //these could match x.x or xxx or .xx or xx.
                    var matches = rowDataGroup.FirstOrDefault(x => x.OffsetRow == 1)?.Matches?
                        .Where(
                                x => x.Index <= leftBound && x.Index + x.Length - 1 >= leftBound
                                || x.Index >= leftBound && x.Index + x.Length <= rightBound
                                || x.Index < rightBound && x.Index + x.Length >= rightBound
                            )
                        .ToList();
                    if (matches is not null && matches.Count > 0)
                    {
                        rowMatches.AddRange(matches.Select(x => int.Parse(x.Value)));
                    }
                }
            }

            if (rowMatches is not null && rowMatches.Count == 2)
            {
                //Console.WriteLine($"Found two matches for current row: {rowMatches[0]} and {rowMatches[1]}");
                currentSum += rowMatches[0] * rowMatches[1];
            }

            rowMatches?.Clear();
        }

        return currentSum;
    }
}

public class RowDataDay3
{
    public string? Text { get; set; }
    public int RowLength { get; set; }
    public int OffsetRow { get; set; }
    public MatchCollection? Matches { get; set; }
    public MatchCollection? AsteriskMatches { get; set; }
}
