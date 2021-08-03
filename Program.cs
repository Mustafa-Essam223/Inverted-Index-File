using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Inverted_Index
{
    class Record
    {
        public string FirstName, City, depart, grade;
        public int std_id;
        public Record(int std_id, string FirstName, string City, string depart, string grade)
        {
            this.City = City;
            this.grade = grade;
            this.std_id = std_id;
            this.depart = depart;
            this.FirstName = FirstName;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, Record> records = new Dictionary<int, Record>();

            Record record0 = new Record(2236789, "Mai", "Cairo", "CS", "Excellant");
            records.Add(0, record0);
            Record rec1 = new Record(2124596, "Sarah", "Giza", "IS", "Good");
            records.Add(1, rec1);
            Record rec2 = new Record(3096743, "Maya", "Alex", "BIO", "Good");
            records.Add(2, rec2);
            Record rec3 = new Record(1098765, "Alaa", "Alex", "IS", "VGood");
            records.Add(3, rec3);
            Record rec4 = new Record(3528790, "Ayah", "Giza", "SC", "Good");
            records.Add(4, rec4);
            Record rec5 = new Record(1234567, "Yasmien", "Giza", "SC", "Excellant");
            records.Add(5, rec5);
            Record rec6 = new Record(2131415, "Hassan", "Cairo", "CS", "Good");
            records.Add(6, rec6);
            Record rec7 = new Record(2222221, "Mohamed", "Cairo", "CS", "Good");
            records.Add(7, rec7);
            Record rec8 = new Record(3322114, "Rehab", "Suez", "SW", "Excellent");
            records.Add(8, rec8);
            Record rec9 = new Record(2231301, "Ahmed", "Suez", "CSYS", "Sufficient");
            records.Add(9, rec9);
            Record rec10 = new Record(2180363, "Hassan", "Banha", "CS", "Sufficient");
            records.Add(10, rec10);
            Record rec11 = new Record(2225647, "Mostafa", "Tanta", "CS", "VGood");
            records.Add(11, rec11);
            Record rec12 = new Record(1111110, "Reda", "Tanta", "CS", "Sufficient");
            records.Add(12, rec12);
            Record rec13 = new Record(1243528, "Ahmed", "Tanta", "SW", "Sufficient");
            records.Add(13, rec13);
            Record rec14 = new Record(0987643, "Aml", "Suez", "SC", "Excellent");
            records.Add(14, rec14);
            Record rec15 = new Record(1243554, "Khaled", "Alex", "SC", "VGood");
            records.Add(15, rec15);
            Record rec16 = new Record(3322105, "Oliver", "Banha", "SW", "VGood");
            records.Add(16, rec16);
            Record rec17 = new Record(2124600, "Yazan", "Tanta", "CS", "Excellant");
            records.Add(17, rec17);
            Record rec18 = new Record(1098800, "Peter", "Cairo", "CSYS", "good");
            records.Add(18, rec18);
            Record rec19 = new Record(2017170, "Mostafa", "El-Arish", "SC", "VGood");
            records.Add(19, rec19);



            FileStream DataFile = new FileStream("DataFile.txt", FileMode.OpenOrCreate
                , FileAccess.Write, FileShare.ReadWrite);
            StreamWriter DataFileWriter = new StreamWriter(DataFile);


            foreach (KeyValuePair<int, Record> item in records)
            {
                DataFileWriter.WriteLine(item.Key + "\t" + item.Value.std_id + "\t" + item.Value.FirstName
                    + "\t" + item.Value.City + "\t" + item.Value.depart + " " + item.Value.grade);
            }

            SortedDictionary<int, int> PrimaryKeyIndex = new SortedDictionary<int, int>();

            foreach (KeyValuePair<int, Record> item in records)
            {
                PrimaryKeyIndex.Add(item.Value.std_id, item.Key);
            }

            FileStream IndexFile = new FileStream("PrimaryKeyFile.txt", FileMode.Create);
            StreamWriter PrimaryKeyWriter = new StreamWriter(IndexFile);
            PrimaryKeyWriter = new StreamWriter(IndexFile);

            SortedDictionary<string, List<int>> SecondaryKey = new SortedDictionary<string, List<int>>();

            foreach (KeyValuePair<int, Record> item in records)
            {
                if (!SecondaryKey.ContainsKey(item.Value.City))
                {
                    SecondaryKey.Add(item.Value.City, new List<int>());
                }
                SecondaryKey[item.Value.City].Add(item.Key);

            }

            FileStream SecondaryKeyFile = new FileStream("SecondaryKeyFile.txt", FileMode.Create);
            StreamWriter SecondaryKeyWriter = new StreamWriter(SecondaryKeyFile);
            int[] ptrs = new int[100];
            Array.Fill(ptrs, -1);
            foreach (KeyValuePair<string, List<int>> item in SecondaryKey)
            {
                if (item.Value.Count > 1)
                {
                    for (int idx = 0; idx < item.Value.Count - 1; idx++)
                        ptrs[item.Value[idx]] = item.Value[idx + 1];
                }
            }
            Dictionary<int, KeyValuePair<int, int>> invertedlist =
                new Dictionary<int, KeyValuePair<int, int>>();
            int ii = 0;
            foreach (KeyValuePair<int, Record> item in records)
            {
                KeyValuePair<int, int> temp = new KeyValuePair<int, int>(item.Value.std_id, ptrs[ii]);
                invertedlist.Add(item.Key, temp);
                ii++;
            }
            FileStream InvertedIndexFile = new FileStream("InvertedIndexFile.txt", FileMode.Create);
            StreamWriter InvertedIndexWriter = new StreamWriter(InvertedIndexFile);
            while (true)
            {
                Console.WriteLine("[1] Add record ");
                Console.WriteLine("[2] Display tables ");
                Console.WriteLine("[3] Exit and save");

                int choice = int.Parse(Console.ReadLine());

                if (choice == 1)
                {
                    long timebefore = System.Environment.TickCount;
                    Console.Write("ID : ");
                    int id = int.Parse(Console.ReadLine());
                    Console.Write("First Name : ");
                    string FirstName = Console.ReadLine();
                    Console.Write("City : ");
                    string city = Console.ReadLine();
                    Console.Write("Department : ");
                    string depart = Console.ReadLine();
                    Console.Write("Grade : ");
                    string grade = Console.ReadLine();
                    Record newrecord = new Record(id, FirstName, city, depart, grade);
                    records.Add(records.Count(), newrecord);
                    DataFileWriter.WriteLine(records.Count() - 1 + "\t" + id + "\t" + FirstName + "\t" + city + "\t" + depart + "\t" + grade);
                    PrimaryKeyIndex.Add(id, records.Count - 1);

                    KeyValuePair<int, int> pair = new KeyValuePair<int, int>(id, -1);
                    invertedlist.Add(records.Count - 1, pair);

                    if (SecondaryKey.ContainsKey(city))
                    {
                        int last = SecondaryKey[city].Last();
                        ptrs[last] = records.Count - 1;
                        invertedlist[last] = new KeyValuePair<int, int>(invertedlist[last].Key, ptrs[last]);
                        SecondaryKey[city].Add(records.Count - 1);
                    }
                    if (!SecondaryKey.ContainsKey(city))
                    {
                        SecondaryKey.Add(city, new List<int>());
                        SecondaryKey[city].Add(records.Count - 1);
                    }

                    //  Save();
                    long TimeAfter = System.Environment.TickCount;
                    long totalTime = TimeAfter - timebefore;
                    Console.WriteLine("=======================================================================");
                    Console.WriteLine("Execution Time " + (totalTime / 1000) + " secs");
                    Console.WriteLine("=======================================================================");
                }
                else if (choice == 2)
                {

                    Console.WriteLine("=======================================================================");
                    Console.WriteLine("\t\t\t\t Data File \t\t\t\t");
                    Console.WriteLine("=======================================================================");
                    foreach (KeyValuePair<int, Record> item in records)
                    {
                        Console.WriteLine(item.Key + "]\t" + item.Value.std_id + "\t" +
                            item.Value.FirstName + "\t" + item.Value.City + "\t" + item.Value.grade + "\t" + item.Value.depart);
                    }
                    Console.WriteLine("=======================================================================");
                    Console.WriteLine("\t\t\t\t Primary Key \t\t\t\t");
                    Console.WriteLine("=======================================================================");
                    foreach (KeyValuePair<int, int> item in PrimaryKeyIndex)
                    {
                        Console.WriteLine(item.Key + "\t\t" + item.Value);
                    }

                    Console.WriteLine("=======================================================================");
                    Console.WriteLine("\t\t\t\t Secondary Key \t\t\t\t");
                    Console.WriteLine("=======================================================================");
                    foreach (KeyValuePair<string, List<int>> item in SecondaryKey)
                    {
                        Console.Write(item.Key + "\t\t");
                        for (int index = 0; index < item.Value.Count; index++)
                        {
                            Console.Write(item.Value.ElementAt(index) + " ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine("=======================================================================");
                    Console.WriteLine("\t\t\t\t Inverted List \t\t\t\t");
                    Console.WriteLine("=======================================================================");
                    foreach (KeyValuePair<int, KeyValuePair<int, int>> item in invertedlist)
                    {
                        Console.WriteLine(item.Key + "\t\t" + item.Value.Key + "\t\t" + item.Value.Value);
                    }
                }
                else if (choice == 3)
                {
                    Save();

                    break;
                }
                void Save()
                {
                    DataFileWriter.Close();
                    foreach (KeyValuePair<int, int> pk in PrimaryKeyIndex)
                    {
                        PrimaryKeyWriter.WriteLine(pk.Key + "\t" + pk.Value);
                    }
                    PrimaryKeyWriter.Close();
                    foreach (KeyValuePair<string, List<int>> sk in SecondaryKey)
                    {
                        SecondaryKeyWriter.WriteLine(sk.Key + "\t" + sk.Value.First());
                    }
                    SecondaryKeyWriter.Close();
                    foreach (KeyValuePair<int, KeyValuePair<int, int>> IL in invertedlist)
                    {
                        InvertedIndexWriter.WriteLine(IL.Key + "\t" + IL.Value.Key + "\t" + IL.Value.Value);
                    }
                    InvertedIndexWriter.Close();
                }

            }
        }


    }
}