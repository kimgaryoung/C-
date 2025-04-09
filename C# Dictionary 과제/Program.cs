using System.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;


namespace word2
{
  


    internal class Program
    {
        static void Main(string[] args)
        {
            string read_path = @"C:\work2\poem.txt"; // 읽을 파일 경로
            string write_path = @"C:\work2\result.txt"; // 결과를 저장할 파일 경로


            
            // 파일에서 모든 줄을 읽음
            string[] lines = File.ReadAllLines(read_path);

            // 파일 내용을 소문자로 변환
            string text = File.ReadAllText(read_path).ToLower();

            // 단어를 구분하기 위한 문자 배열
            char[] delimiters = { ' ', '\r', '\n', ':', ';', ',', '.', '\'', '\"', '(', ')', '-', '?', '!', '\t' };

            string[] words = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);



           

            

            Dictionary<string, int> wordCounts = new Dictionary<string, int>();

            foreach (var word in words)
            {
                //word = word.ToLower();
                if (wordCounts.ContainsKey(word))
                    wordCounts[word]++;
                else
                    wordCounts[word] = 1;
            }










            int max = 0;
            int min = 10;

            var sortedByValue = wordCounts.OrderBy(x => x.Key);
            foreach (KeyValuePair<string, int> entry in sortedByValue)
            {

                if (max< entry.Value)
                {

                    max = entry.Value;

                }

                if (min>entry.Value)
                {
                    min = entry.Value;
                }
            }




            using (StreamWriter writer = new StreamWriter(write_path))
            {

                foreach (KeyValuePair<string, int> entry in sortedByValue)
                {
                    if (entry.Value == max)
                    {
                        writer.WriteLine("Max: " + entry.Value + "개 \t " + entry.Key);
                        //Console.WriteLine("Max_Key: " + entry.Key + ", Max_ Value: " + entry.Value);
                    }



                }

                writer.WriteLine("-----------------------");

                foreach (KeyValuePair<string, int> entry in sortedByValue)
                {
                    

                    if (entry.Value ==min)
                    {
                        writer.WriteLine("Min: " + entry.Value + "개 \t " + entry.Key);
                        //Console.WriteLine("Min_Key: " + entry.Key + ", Min_Value: " + entry.Value);
                    }


                }




            }



        }
    }

}
