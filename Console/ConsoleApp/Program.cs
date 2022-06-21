using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp
{
    public class RankModel
    {
        public int RankUpperLimit { get; set; }
        public int RankLowerLimit { get; set; }
        public int RankWeight { get; set; }
        public int RankPosition { get; set; }
    }

    public class Program
    {
        public static List<RankModel> PrepareRankObjectList(List<int> rankFilteredList)
        {
            var rankList = new List<RankModel>();
            int rankWeight = 0;
            int rankUpperLimit = 0;
            int rankLowerLimit = 0;
            int rankPosition = 0;

            for (int rankIndex = 0; rankIndex < rankFilteredList.Count; rankIndex++)
            {
                rankWeight = rankFilteredList[rankIndex];

                if (!rankList.Any(a => a.RankWeight == rankWeight))
                {
                    if (rankWeight == rankFilteredList.Max())
                    {
                        rankUpperLimit = int.MaxValue;
                        rankLowerLimit = rankWeight;
                        rankPosition = 1;
                    }
                    else if (rankWeight == rankFilteredList.Min())
                    {
                        rankUpperLimit = rankFilteredList[rankIndex - 1] - 1;
                        rankLowerLimit = rankWeight;
                        rankPosition = rankFilteredList.Count();
                    }
                    else
                    {
                        rankUpperLimit = rankFilteredList[rankIndex - 1] - 1;
                        rankLowerLimit = rankWeight;
                        rankPosition = rankIndex + 1;
                    }

                    rankList.Add(new RankModel
                    {
                        RankWeight = rankWeight,
                        RankUpperLimit = rankUpperLimit,
                        RankLowerLimit = rankLowerLimit,
                        RankPosition = rankPosition
                    });
                }
            }

            rankList.Add(new RankModel
            {
                RankWeight = 0,
                RankUpperLimit = rankFilteredList.Min() - 1,
                RankLowerLimit = int.MinValue,
                RankPosition = rankFilteredList.Count() + 1
            });

            return rankList;
        }

        public static List<int> GetRank(List<int> rankCollection, List<int> playerCollection)
        {
            var result = new List<int>();

            var rankFilteredList = rankCollection.Distinct().OrderByDescending(o => o).ToList();
            var rankObjectList = PrepareRankObjectList(rankFilteredList);

            foreach (var player in playerCollection)
            {
                foreach (var rank in rankObjectList)
                {
                    if (player >= rank.RankLowerLimit && player <= rank.RankUpperLimit)
                    {
                        result.Add(rank.RankPosition);
                    }
                }
            }

            return result;
        }

        public static void Main(string[] args)
        {
            //TODO: Change this input file path before start
            var filePath = "D:\\DEV_ENV\\FaqInterview\\Console\\ConsoleApp\\Input.txt";

            var lines = File.ReadLines(filePath).ToList();

            int rankedCount = Convert.ToInt32(lines[0].Trim());

            List<int> ranked = lines[1].TrimEnd().Split(' ').ToList().Select(rankedTemp => Convert.ToInt32(rankedTemp)).ToList();

            int playerCount = Convert.ToInt32(lines[2].Trim());

            List<int> player = lines[3].TrimEnd().Split(' ').ToList().Select(playerTemp => Convert.ToInt32(playerTemp)).ToList();

            List<int> output = GetRank(ranked, player);
            
            Console.WriteLine(String.Join("\n", output));

            //string outputString = string.Join(" ", output);
            //Console.WriteLine(outputString);
        }

    }
}
