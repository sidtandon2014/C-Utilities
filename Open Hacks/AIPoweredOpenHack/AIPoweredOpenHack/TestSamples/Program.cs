using System;
using System.Collections.Generic;
using System.Linq;

namespace TestSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var wordDict = new Dictionary<string, int>();
            wordDict.Add("Sid", 10);
            wordDict.Add("sab", 14);
            wordDict.Add("ran", 8);
            wordDict.Add("tar", 6);
            wordDict.Add("sad1", 100);

            var total = wordDict.OrderByDescending(Key => Key.Value).Take(3);
            Console.WriteLine (total.Select(row => row.Key).ToArray());
            Console.Read();
        }
    }
}
