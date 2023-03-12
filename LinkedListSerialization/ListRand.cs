using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Saber.Collections
{
    class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand; // произвольный элемент внутри списка
        public string Data;
    }

    class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        /// <summary>
        /// First write Count, then - nodes. If Node.Rand == null, -1 will be serialized.
        /// </summary>
        /// <param name="s"></param>
        public void Serialize(FileStream s)
        {
            if (!s.CanWrite)
            {
                throw new Exception("The stream for file not is writable.");
            }

            var randIndexesMap = new Dictionary<ListNode, int>();
            var index = 0;

            ForEach(x =>
            {
                randIndexesMap.Add(x, index++);
            });

            using (var bw = new BinaryWriter(s))
            {
                bw.Write(Count);

                ForEach(x =>
                {
                    bw.Write(x.Data);
                    bw.Write(x.Rand != null ? randIndexesMap[x.Rand] : -1);
                });
            }
        }

        /// <summary>
        /// First reads (int Count) from binary file, after nodes in format(string data, int randIndex)
        /// </summary>
        /// <param name="s"></param>
        public void Deserialize(FileStream s)
        {
            if (!s.CanRead)
            {
                throw new Exception("The stream for file not is readable.");
            }

            Clear();

            var randIndexesMap = new Dictionary<ListNode, int>();
            var nodesArray = default(ListNode[]);

            //Deserializing
            using (var br = new BinaryReader(s))
            {
                var count = br.ReadInt32();
                nodesArray = new ListNode[count];

                for (var i = 0; i < count; i++)
                {
                    Add(new ListNode());
                    Tail.Data = br.ReadString();

                    randIndexesMap[Tail] = br.ReadInt32();
                    nodesArray[i] = Tail;
                }
            }

            //Looking for matches
            ForEach(x =>
            {
                var randIndex = randIndexesMap[x];

                if (randIndex != -1)
                {
                    x.Rand = nodesArray[randIndex];
                }
            });
        }

        public ListNode ElementAt(int index)
        {
            if (index >= Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            var node = Head;
            for (var i = 0; i < index; i++)
            {
                node = node.Next;
            }

            return node;
        }

        public void Add(ListNode newNode)
        {
            var node = Tail;
            Tail = newNode;
            Tail.Prev = node;

            if (Head == null)
            {
                Head = Tail;
            }
            else
            {
                Tail.Prev.Next = Tail;
            }

            Count++;
        }

        public void ForEach(Action<ListNode> action)
        {
            var node = Head;

            do
            {
                action(node);
                node = node.Next;

            } while (node != null);
        }

        public void Clear()
        {
            Count = 0;
            Head = null;
            Tail = null;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            ForEach(x =>
            {
                sb.AppendLine($"Data: {x.Data}, Rand Data: {x.Rand?.Data}");
            });

            return sb.ToString();
        }
    }
}
