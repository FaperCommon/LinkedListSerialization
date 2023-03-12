using Saber.Collections;
using System;
using System.IO;

namespace Saber
{
    class Program
    {
        // * Insert filename
        private const string filename = "filename";

        static void Main(string[] args)
        {
            Console.WriteLine("Create and serialize random list:");
            using (var s = new FileStream(filename, FileMode.OpenOrCreate))
            {
                var list = CreateRandomList(20);
                list.Serialize(s);
                Console.WriteLine(list.ToString());
            }

            Console.WriteLine("Deserialize random list:");
            using (var s = new FileStream(filename, FileMode.Open))
            {
                var list = new ListRand();
                list.Deserialize(s);
                Console.WriteLine(list.ToString());
            }
        }

        private static ListRand CreateRandomList(int count)
        {
            const int randPercent = 60;
            var list = new ListRand();


            for(var i = 0; i < count; i++)
            {
                list.Add(new ListNode()
                {
                    Data = $"{i}"
                });
            }

            var rnd = new Random();

            //Test n^2
            list.ForEach(x =>
            {
                var hasRandom = rnd.Next(0, 100) < randPercent;

                if(hasRandom)
                {
                    var randIndex = rnd.Next(0, count);

                    x.Rand = list.ElementAt(randIndex);
                }
            });

            return list;
        }
    }
}
