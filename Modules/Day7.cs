using System.Xml;

namespace AdventOfCode._2023.Modules;

public class Day7 : BaseDay
{
    public override void PartOne()
    {
        var data = GetFileData("Data/Day7/Data.txt");

        var cardRanking = new Dictionary<char, int>()
        {
            {'2', 2},
            {'3', 3},
            {'4', 4},
            {'5', 5},
            {'6', 6},
            {'7', 7},
            {'8', 8},
            {'9', 9},
            {'T', 10},
            {'J', 11},
            {'Q', 12},
            {'K', 13},
            {'A', 14}
        };

        var rankList = new List<string>() { "High Card", "One Pair", "Two Pair", "Three of a Kind", "Full House", "Four of a Kind", "Five of a Kind" };

        var dataSet = new List<CardHand>();

        foreach (var row in data)
        {
            var card = new CardHand()
            {
                Score = Convert.ToInt32(row.Split(" ")[1].Trim()),
                Cards = row.Split(" ")[0].Trim().ToCharArray().ToList(),
            };

            SetHandType(card);

            dataSet.Add(card);
        }

        var sortedDataSet = new List<CardHand>();

        foreach (var rank in rankList)
        {
            var group = dataSet.Where(x => x.HandType == rank).ToList();

            if (group.Count == 0) continue;

            var ret = SortGroup(group, cardRanking);

            sortedDataSet.AddRange(ret);
        }

        var runningSum = 0;
        for (int i = 1; i <= sortedDataSet.Count; i++)
            runningSum += sortedDataSet[i - 1].Score * i;

        Console.WriteLine($"Day 7 Part 1: {runningSum}");
    }

    public override void PartTwo()
    {
        var data = GetFileData("Data/Day7/Data.txt");

        var cardRanking = new Dictionary<char, int>()
        {
            {'J', 1},
            {'2', 2},
            {'3', 3},
            {'4', 4},
            {'5', 5},
            {'6', 6},
            {'7', 7},
            {'8', 8},
            {'9', 9},
            {'T', 10},
            {'Q', 12},
            {'K', 13},
            {'A', 14}
        };

        var rankList = new List<string>() { "High Card", "One Pair", "Two Pair", "Three of a Kind", "Full House", "Four of a Kind", "Five of a Kind" };

        var dataSet = new List<CardHand>();

        foreach (var row in data)
        {
            var card = new CardHand()
            {
                Score = Convert.ToInt32(row.Split(" ")[1].Trim()),
                Cards = row.Split(" ")[0].Trim().ToCharArray().ToList(),
            };

            SetHandTypePartTwo(card);

            dataSet.Add(card);
        }

        var sortedDataSet = new List<CardHand>();

        foreach (var rank in rankList)
        {
            var group = dataSet.Where(x => x.HandType == rank).ToList();

            if (group.Count == 0) continue;

            var ret = SortGroup(group, cardRanking);

            sortedDataSet.AddRange(ret);
        }

        var runningSum = 0;
        for (int i = 1; i <= sortedDataSet.Count; i++)
            runningSum += sortedDataSet[i - 1].Score * i;

        Console.WriteLine($"Day 7 Part 2: {runningSum}");
    }

    protected virtual void SetHandType(CardHand hand)
    {
        var cardGroups = hand.Cards.GroupBy(x => x).Select(x => new {Card = x.Key, Count = x.Count()}).ToList();

        if (cardGroups.Count == 1)
        {
            hand.HandType = "Five of a Kind";
        }
        else if (cardGroups.Count == 2)
        {
            if (cardGroups.Any(x => x.Count == 4))
                hand.HandType = "Four of a Kind";
            else
                hand.HandType = "Full House";
        }
        else if (cardGroups.Count == 3)
        {
            if (cardGroups.Any(x => x.Count == 3))
                hand.HandType = "Three of a Kind";
            else 
                hand.HandType = "Two Pair";
        }
        else if (cardGroups.Count == 4)
        {
            hand.HandType = "One Pair";
        }
        else if (cardGroups.Count == 5)
        {
            hand.HandType = "High Card";
        } 
    }

    protected virtual void SetHandTypePartTwo(CardHand hand)
    {
        var cardGroups = hand.Cards.GroupBy(x => x).Select(x => new { Card = x.Key, Count = x.Count() }).ToList();
        var countJokers = hand.Cards.Count(x => x == 'J');

        if (cardGroups.Count == 1)
        {
            //same
            hand.HandType = "Five of a Kind";
        }
        else if (cardGroups.Count == 2)
        {
            //JJJJ2 or JJJ22 or JJ222 or J2222
            if (countJokers > 0)
                hand.HandType = "Five of a Kind";
            else if (cardGroups.Any(x => x.Count == 4))
                hand.HandType = "Four of a Kind";
            else
                hand.HandType = "Full House";
        }
        else if (cardGroups.Count == 3) //QQQJA
        {
            //JJJ23 of JJ223 or J2223 or J3223
            if (countJokers == 1 && cardGroups.Any(x => x.Count == 3))
                hand.HandType = "Four of a Kind";
            else if (countJokers >= 2)
                hand.HandType = "Four of a Kind";
            else if (countJokers > 0)
                hand.HandType = "Full House";
            else if (cardGroups.Any(x => x.Count == 3))
                hand.HandType = "Three of a Kind";
            else
                hand.HandType = "Two Pair";
        }
        else if (cardGroups.Count == 4)
        {
            //J2234
            if (countJokers > 0)
                hand.HandType = "Three of a Kind";
            else
                hand.HandType = "One Pair";
        }
        else if (cardGroups.Count == 5)
        {
            //914JQ
            if (countJokers > 0)
                hand.HandType = "One Pair";
            else
                hand.HandType = "High Card";
        }
    }

    protected List<CardHand> SortGroup(List<CardHand> dataSet, Dictionary<char, int> ranking)
    {
        var ret = new List<CardHand>();

        var sortedSet = dataSet.OrderBy(a => ranking[a.Cards[0]]).ThenBy(a => ranking[a.Cards[1]]).ThenBy(a => ranking[a.Cards[2]]).ThenBy(a => ranking[a.Cards[3]]).ThenBy(a => ranking[a.Cards[4]]);

        return sortedSet.ToList();
    }
}

public class CardHand
{
    public virtual int Score { get; set; }
    public virtual List<char>? Cards { get; set; }
    public virtual string? HandType { get; set; }
}
