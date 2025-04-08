using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml;
using System.Globalization; // 시간 측정을 위한

namespace ConsoleApp1
{
    internal class Program
    {


        //  배열 전체 출력 함수
        static void Print(int[,] array)
        {
            Console.WriteLine("번호\t성적1\t성적2\t성적3\t합계\t순위");
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                    Console.Write(array[i, j] + "\t");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        
        //기수정렬 
        static void Radix_func(int[,] array)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            int n = array.GetLength(0);
            int size = array.GetLength(1); 


            int p = 1; // 시작 자릿수 => 1의 자리부터
            int m = 6; // 최대 자릿수 (50만개 -> 6자리)

            for (int pos = 0; pos < m; pos++)
            {
                List<int>[] radix_arr = new List<int>[10]; 
                for (int i = 0; i < 10; i++)
                {
                    radix_arr[i] = new List<int>(); // 초기화

                }

               
                for (int i = 0; i < n; i++)
                {
                    int d = (array[i, 4] / p) % 10; 
                    radix_arr[d].Add(i); 
                }

                
                int[,] tmp = new int[n, size];
                int idx = 0;
                for (int d = 9; d >= 0; d--) 
                {
                    foreach (int i in radix_arr[d])
                    {
                        for (int j = 0; j < size; j++) 
                        {
                            tmp[idx, j] = array[i, j];
                        }
                        idx++;
                    }
                }

             
                for (int i = 0; i < n; i++)
                {
                    for (int col = 0; col < size; col++) 
                    {
                        array[i, col] = tmp[i, col];
                    }
                }

                p *= 10; 
            }

            // 순위 계산
            int rank = 1;
            array[0, 5] = rank;
            for (int i = 1; i < n; i++)
            {
                if (array[i, 4] != array[i - 1, 4]) 
                    rank = i + 1;
                array[i, 5] = rank;
            }


            sw.Stop();
            Console.WriteLine($"Radix Time: {sw.ElapsedMilliseconds}ms");


        }


        // Counting Sort
        static void Counting_sort_func(int[,] array)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int n = array.GetLength(0);
            int size = array.GetLength(1);

            int[] tmp = new int[301];
            // 값 담을 곳-> 점수 3개 최대값 100*3=300


            // 배열 초기화
            for (int i = 0; i < 301; i++)
            {
                tmp[i] = 0;
            }


            // 점수대 1씩 증가 .
            for (int i = 0; i < n; i++)
            {

                for (int j = 0; j < 301; j++)
                {
                    if (array[i, 4] == j)
                    {
                        tmp[j]++;
                    }
                }
            }

            int[,] sorted_arr = new int[n, size]; // 순위순으로 정렬된 배열
            int[] ranks = new int[n];

            int r = 1;// rank
            int idx = 0;

            //내림차순 
            for (int s = 300; s >= 0; s--)
            {
                if (tmp[s] > 0) // 숫자가 들어간 곳만 남기려고
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (array[i, 4] == s)
                        {
                            // tmp 인덱스 값이랑 같을 때 
                            for (int j = 0; j < size; j++)
                            {
                                sorted_arr[idx, j] = array[i, j];
                            }
                            ranks[idx] = r;
                            idx++;
                        }
                    }
                    r = idx+1;
                }
            }


            for (int i = 0; i < n; i++)
            {

                sorted_arr[i, 5] = ranks[i];
            }

            //원본 업데이트
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    array[i, j] = sorted_arr[i, j];
                }
            }


            sw.Stop();
            Console.WriteLine($"Counting sort: {sw.ElapsedMilliseconds}ms");




            /* 작은 갯수일때는 정렬이 되는데 수가 커지니까 시간 복잡도 때문에 못쓰는 코드 됐어요,,, 만든 시간이 아까워서.. 첨부합니다..
                Stopwatch sw = new Stopwatch();
                sw.Start();

                Console.WriteLine("함수 시작");
                int n = array.GetLength(0);
                int size = array.GetLength(1);


                int[] tmp = new int[301]; // 점수 합계 범위 0~300 , 점수를 셀 배열.


                // 배열 0으로 초기화
                for (int i = 0; i < 301; i++)
                {
                    tmp[i] = 0;
                }

                // 점수대 1씩 증가 .
                for (int i = 0; i < n; i++)
                {

                    for (int j = 0; j < 301; j++)
                    {
                        if (array[i, 4] == j)
                        {
                            tmp[j]++;
                        }
                    }
                }

                int[] sorted_arr = new int[n];

                int idx = 0;
                for (int i = 300; i >= 0; i--)
                {
                    if (tmp[i] != 0)
                    {
                        if (tmp[i] > 1)
                        {
                            for (int j = 1; j < tmp[i] + 1; j++)
                            {
                                sorted_arr[idx++] = i;
                            }
                        }
                        else
                        {
                            sorted_arr[idx++] = i;
                        }
                    }
                }
                

                int rank = 1;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (sorted_arr[i] == array[j, 4]) // 정렬된 배열과 같은 값이 있으면
                        {
                            if (j > 0 && array[j, 4] == array[j - 1, 4])
                            {
                                array[j, 5] = array[j - 1, 5]; // 이전 행과 같은 순위 부여
                            }
                            else // 값이 다르면
                            {
                                array[j, 5] = rank;
                                rank++;
                            }
                        }
                    }
                }
                sw.Stop();
                Console.WriteLine($" Counting sort: {sw.ElapsedMilliseconds} ms");
            */


        }


        // 여기부터 퀵정렬.
        static int Partition(int[,] arr, int low, int high)
        {


            int pivot = arr[(low + high)/2, 4]; 

            // 150~155 범위 _ 300이니까 중간 값 쯤을 피봇으로 설정.
            for (int idx = low; idx <= high; idx++) 
            {
                if (arr[idx, 4]<= 150 && arr[idx, 4] <= 155) 
                {
                    pivot = arr[idx, 4];
                    break;
                }
            }
                
                
           
            //int pivot = arr[high, 4]; // 시간 너무 오래걸려서 피봇 바꿈.
            int i = low-1;

            for (int j = low; j<high; j++)
            {
                if (arr[j, 4]< pivot)
                {
                    i++;
                    for (int k = 0; k<arr.GetLength(1); k++)// 값 교체
                    {

                        int tmp = arr[i, k];
                        arr[i, k] = arr[j, k];
                        arr[j, k] = tmp;
                    }

                }
            }
            for (int m = 0; m<arr.GetLength(1); m++)// 값 교체
            {

                int temp = arr[i+1, m];
                arr[i+1, m] = arr[high, m];
                arr[high, m] = temp;
            }
            return i+1;

        }

        // 스택 오버 플로우 -> 꼬리 재귀 형태로 바꾸기 
        
        static void Quick_sort_func(int[,]arr, int low, int high)
        {

            /* 갯수 작을 때는 돌아가는데 50만개는 스택 오버 플로우 남. -> 스택이 처리할 수 있는 자료양 넘어섬
            if (low < high)
            {
                int p = Partition(arr, low, high);

                Quick_sort_func(arr, low, p-1);
                Quick_sort_func(arr, p+1, high);
            }

            */

            while (low < high)
            {
                int p = Partition(arr, low, high);

                Quick_sort_func(arr, low, p-1);
                low=p+1;


            }




        }


        static void Rank_func(int[,] arr, int low, int high)
        {
           
            while(low < high)
    {
                for (int k = 0; k < arr.GetLength(1); k++)
                {
                    int temp = arr[low, k];
                    arr[low, k] = arr[high, k];
                    arr[high, k] = temp;
                }
                low++;
                high--;
            }


            int rank = 1;
            arr[0, 5] = rank;
            for (int i = 1; i < arr.GetLength(0); i++)
            {
                if (arr[i, 4] != arr[i - 1, 4])
                {
                    rank = i + 1;
                }
                arr[i, 5] = rank;
            }
        }
        

        // 퀵정렬에 시간 넣으면 재귀 때문에 여러번 출력되서
        static void Quick_time_func(int[,]array)
        {

            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            Quick_sort_func(array, 0, array.GetLength(0)-1);
            Rank_func(array,0,array.GetLength(0)-1);
            sw.Stop();
            Console.WriteLine($"Quick sort Time: {sw.ElapsedMilliseconds}ms");


        }




        // 여기부터 머지 정렬
         static void Merge_sort_func(int[,]array, int low, int high)
        {

            if(low<high) {
                int mid = (low+high)/2;

                Merge_sort_func(array, low, mid);

                Merge_sort_func(array, mid + 1, high);

                Merge_func(array, low, mid, high);
            }
            
        }
       
        static void Merge_func(int[,]array, int low, int mid ,int high)
        {

            int l = low;
            int r = mid+1;
           

            int n=high-low+1; 
            // 병합하는 배열의 크인데 쪼갤때 갯수가 달라서 크기가 다 다름.
           

            int [,] merge_arr =new int[n,6];
            int idx = 0; // 임시 배열 인덱스


            while (l<=mid && r<=high)
            {
                if (array[l, 4] <= array[r, 4])
                {
                    for (int m = 0; m < 6; m++) 
                    {
                        merge_arr[idx, m] = array[l, m];
                    }
                    l++;

                }
                else
                {
                        for (int m = 0; m < 6; m++)
                        {
                            merge_arr[idx, m] = array[r, m];
                        }

                        r++;
                }
                idx++;  
               
            }

            while (l<=mid)// 왼쪽합치기
            {
                for (int m = 0; m < 6; m++)
                {
                    merge_arr[idx, m] = array[l, m];
                }

                l++;
                idx++;
                
            }

            while (r<=high) // 오른쪽 합치기
            {

                for (int m = 0; m < 6; m++)
                {
                    merge_arr[idx, m] = array[r, m];
                }

                r++;
                idx++;
            }



            for (int x = 0; x < n; x++)// 정렬한 배열 -> 원본으로
            {    for (int m = 0; m < 6; m++)
                {
                    array[low+x, m] = merge_arr[x, m];

                   
                }
            }

        }

        
        static void merge_time_func(int[,] array)
        {

            Stopwatch sw = Stopwatch.StartNew();
            

            Merge_sort_func(array, 0, array.GetLength(0)-1);
            Rank_func(array, 0, array.GetLength(0)-1);
            sw.Stop();
            Console.WriteLine($"Merge sort Time: {sw.ElapsedMilliseconds}ms");


        }

        //힙 정렬 
        static void Heapify(int[,] array, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2; // 오른쪽이 왼쪽보다 큰 값으로 하려고. 2*i대신 이거 씀. 

            // 왼쪽이 더 클떄
            if (left < n && array[left, 4] > array[largest, 4])
                largest = left;

            // 오른쪽이 더 클때
            if (right < n && array[right, 4] > array[largest, 4])
                largest = right;

            if (largest != i)
            {

                for (int k = 0; k < 6; k++)
                {
                    int temp = array[i, k];
                    array[i, k] = array[largest, k];
                    array[largest, k] = temp;
                }

                Heapify(array, n, largest);
            }
        }

        static void HeapSort_func(int[,] array)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            int n = array.GetLength(0);

            // 최대 힙
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(array, n, i);

            // 정렬 수행
            for (int i = n - 1; i >= 0; i--)
            {
                // 최댓값이라 끝 비교 후 교환
                for (int k = 0; k < 6; k++)
                {
                    int temp = array[0, k];
                    array[0, k] = array[i, k];
                    array[i, k] = temp;
                }

                Heapify(array, i, 0); // 루트에서 다시 힙 구성
            }

            // 내림차순으로 역순 + 순위 계산
            Rank_func(array, 0, n - 1);

            sw.Stop();
            Console.WriteLine($"Heap sort Time: {sw.ElapsedMilliseconds}ms");
        }





        static void Main(string[] args)
        {
            int[,] a = new int[500000, 6]; // 500000명의 데이터 (번호, 성적1, 성적2, 성적3, 합계, 순위)
            Random r = new Random();

            // 랜덤 점수 생성 및 번호 부여
            for (int i = 0; i < a.GetLength(0); i++)
            {
                a[i, 0] = i + 1;   // 번호 (1~50)
                a[i, 1] = r.Next(101);  // 성적1 (0~100)
                a[i, 2] = r.Next(101);  // 성적2 (0~100)
                a[i, 3] = r.Next(101);  // 성적3 (0~100)
                a[i, 4] = a[i, 1] + a[i, 2] + a[i, 3]; // 합계 계산

            }


            int[,] arr_counting = (int[,])a.Clone();
            int[,] arr_radix = (int[,])a.Clone();
            int[,] arr_quick = (int[,])a.Clone();
            int[,] arr_merge = (int[,])a.Clone();
            int[,] arr_heap = (int[,])a.Clone();

            Console.WriteLine("             처리시간 (ms)");

            Counting_sort_func(arr_counting);

            //Print(a);

            Radix_func(arr_radix);
            

            merge_time_func(arr_merge);
           HeapSort_func(arr_heap);
           Quick_time_func(arr_quick);

           
           



        }
    }
}

/*
 class Student
    {
        public int Number;
        public int Score1;
        public int Score2;
        public int Score3;
        public int Total;
        public int Rank;

        public Student(int number, Random r)
        {
            Number = number;
            Score1 = r.Next(101);
            Score2 = r.Next(101);
            Score3 = r.Next(101);
            Total = Score1 + Score2 + Score3;
        }

        public Student(int number, int s1, int s2, int s3)
        {
            Number = number;
            Score1 = s1;
            Score2 = s2;
            Score3 = s3;
            Total = s1 + s2 + s3;
        }
    }

    class Program
    {
        static void Main()
        {
            int studentCount = 500000;
            Random r = new Random();

            // List<T> 컬렉션
            // 형식 : List<T> list = new List<T>();
            //List<Student> originalStudents = new List<Student>();

            Student[] originalStudents = new Student[studentCount];
            for (int i = 0; i < studentCount; i++)
            {
                originalStudents[i] = new Student(i + 1, r);
            }

            //List<Student> quick = CopyList(originalStudents);
            //List<Student> heap = CopyList(originalStudents);
            //List<Student> radix = CopyList(originalStudents);
            //List<Student> counting = CopyList(originalStudents);
            //List<Student> merge = CopyList(originalStudents);

            // 정렬은 리스트를 실제로 바꿈
            // originalStudents 깊은 복사 -> 각 정렬에 적용
            // 같은 데이터를 정렬들에 쓰려면 별도의 복사본을 생성 필요
            Student[] quick = CopyArray(originalStudents);
            Student[] heap = CopyArray(originalStudents);
            Student[] radix = CopyArray(originalStudents);
            Student[] counting = CopyArray(originalStudents);
            Student[] merge = CopyArray(originalStudents);

            SortingTime("Quick sort Time", quick, QuickSort);
            SortingTime("Heap sort Time", heap, HeapSort);
            SortingTime("Radix sort Time", radix, RadixSort);
            SortingTime("Counting sort Time", counting, CountingSort);
            SortingTime("Merge sort Time", merge, MergeSort);
        }

        // copyArray 정의
        // 깊은 복사 
        // : 이터를 복사할 때, 원본과 완전히 독립된 새로운 복사본을 만드는 것, 
        // 원본을 수정해도 복사본은 영향을 받지 않고, 복사본을 수정해도 원본은 그대로
        static Student[] CopyArray(Student[] src)
        {
            Student[] copy = new Student[src.Length];
            for (int i = 0; i < src.Length; i++)
            {
                Student s = src[i];
                copy[i] = new Student(s.Number, s.Score1, s.Score2, s.Score3);
            }
            return copy;
        }

        //SortingTime 정의
        static void SortingTime(string name, Student[] students, Func<Student[], Student[]> sortFunc)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Student[] sorted = sortFunc(students);
            sw.Stop();

            for (int i = 0; i < sorted.Length; i++)
            {
                sorted[i].Rank = i + 1;
            }

            Console.WriteLine($"{name} {sw.Elapsed.TotalMilliseconds}ms");
        }

        // CountingSort란?
        // 숫자들이 몇 번 나왔는지 세고 , 순서대로 꺼내는 방식으로 정렬
        // ex. 1, 3, 2, 4, 3, 2, 5, 3, 1
        // 1: 2번, 2: 2번, 3: 3번, 4: 1번, 5: 1번
        // 1, 1, 2, 2, 3, 3, 3, 4, 5
        static Student[] CountingSort(Student[] students)
        {
            int maxTotal = 300; // 총점은 최대 300점 (100 + 100 + 100)

            // 0~300까지 총 301개
            Student[][] buckets = new Student[maxTotal + 1][];
            int[] counts = new int[maxTotal + 1];

            // 0~300까지 빈 리스트 생성
            for (int i = 0; i < students.Length; i++)
            {
                int total = students[i].Total;
                counts[total]++;
            }

            for (int i = 0; i <= maxTotal; i++)
            {
                buckets[i] = new Student[counts[i]];
                counts[i] = 0; // 재사용 위해 0으로 초기화
            }

            // 점수 높은 순서로 리스트에 담기
            for (int i = 0; i < students.Length; i++)
            {
                int total = students[i].Total;
                buckets[total][counts[total]++] = students[i];
            }

            Student[] sortedList = new Student[students.Length];
            int idx = 0;

            for (int score = maxTotal; score >= 0; score--)
            {
                for (int j = 0; j < buckets[score].Length; j++)
                {
                    sortedList[idx++] = buckets[score][j];
                }
            }

            return sortedList;
        }

        // QuickSort란?
        // pivot을 기준으로 작은 값은 왼쪽, 큰 값은 오른쪽으로 나누어 정렬
        // ex. 38 27 43 3 9 82 10
        // pivot = 38
        // 27 3 9 10 / 38 / 43 82
        // pivot = 27
        // 3 9 10 / 27 / 38 / 43 82
        // pivot = 3
        // 3 9 10 / 27 / 38 / 43 82
        // pivot = 43
        // 3 9 10 / 27 / 38 / 43 82
        // pivot = 82
        // 3 9 10 / 27 / 38 / 43 82
        static Student[] QuickSort(Student[] students)
        {
            //Quick Sort가 더 이상 쪼갤 수 없는 base case 처리
            // 위에서 정의한 const 500,000명을 1까지 쪼갬
            // 없으면 무한 루프
            if (students.Length <= 1)
                return students;

            Student pivot = students[0];

            Student[] less = new Student[students.Length];
            Student[] more = new Student[students.Length];
            int lessCount = 0, moreCount = 0;

            // pivot 기준으로 divide
            for (int i = 1; i < students.Length; i++)
            {
                if (students[i].Total >= pivot.Total)
                    less[lessCount++] = students[i];
                else
                    more[moreCount++] = students[i];
            }

            Student[] sortedLess = QuickSort(TrimArray(less, lessCount));
            Student[] sortedMore = QuickSort(TrimArray(more, moreCount));

            return CombineArrays(sortedLess, new Student[] { pivot }, sortedMore);
        }

        // MergeSort란?
        // 리스트를 반으로 나누어 정렬하고 합침
        // ex. 38 27 43 3 9 82 10
        // 38 27 43 3 / 9 82 10
        // 38 27 / 43 3 / 9 82 / 10
        // 38 / 27 / 43 / 3 / 9 / 82 / 10
        // 27 38 / 3 43 / 9 82 / 10
        // 3 27 38 43 / 9 10 82
        // 3 9 10 27 38 43 82
        static Student[] MergeSort(Student[] students)
        {
            if (students.Length <= 1)
                return students;

            int mid = students.Length / 2;   //전체 학생 수의 절반을 계산해서 중간 인덱스를 구함
            Student[] left = new Student[mid];
            Student[] right = new Student[students.Length - mid];

            Array.Copy(students, 0, left, 0, mid);
            Array.Copy(students, mid, right, 0, students.Length - mid);

            return Merge(MergeSort(left), MergeSort(right));//하나의 정렬된 리스트로 만들어서 반환
        }

        static Student[] Merge(Student[] left, Student[] right)
        {
            Student[] result = new Student[left.Length + right.Length];
            int i = 0, j = 0, k = 0;

            while (i < left.Length && j < right.Length)
            {
                if (left[i].Total >= right[j].Total)
                    result[k++] = left[i++];
                else
                    result[k++] = right[j++];
            }

            // 남은 요소들 추가
            while (i < left.Length) result[k++] = left[i++];
            while (j < right.Length) result[k++] = right[j++];

            return result;
        }

        // HeapSort란?
        // 최댓값 or 최솟값을 빠르게 꺼내는 구조
        // ex. 38 27 43 3 9 82 10
        // 38 27 43 3 9 82 10
        // 38 27 43 3 9 82 10
        // 43 27 38 3 9 82 10
        // 43 27 38 3 9 82 10
        // 82 43 38 27 9 3 10
        // 82 43 38 27 10 9 3

        //Heap, Heapify, Swap을 따로 나눈 이유
        //중복 감소,가독성을 높이기 위해

        static Student[] HeapSort(Student[] students)
        {
            Student[] arr = (Student[])students.Clone();
            int n = arr.Length;

            // 1. 힙 만들기
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(arr, n, i);

            // 2. 힙 정렬
            for (int i = n - 1; i > 0; i--)
            {
                Swap(arr, 0, i);         // 루트와 맨 끝 요소 교환
                Heapify(arr, i, 0);      // 줄어든 배열에 대해 재정렬
            }

            Array.Reverse(arr); // 내림차순 정렬을 위해 Reverse
            return arr;
        }

        //Heapify() : 노드 한 개를 기준으로 아래 트리를 힙 구조로 재정렬하는 함수

        static void Heapify(Student[] arr, int size, int i)   //list: 정렬할 학생 리스트
        {
            int largest = i;    //largest: 현재 가장 큰 값의 인덱스를 저장할 변수 (처음엔 자기 자신)
            int left = 2 * i + 1;   //left: 왼쪽 자식 노드의 인덱스
            int right = 2 * i + 2;  //right: 오른쪽 자식 노드의 인덱스

            if (left < size && arr[left].Total > arr[largest].Total)
                largest = left;
            if (right < size && arr[right].Total > arr[largest].Total)
                largest = right;

            if (largest != i)
            {
                Swap(arr, i, largest);  // 자리 바꾸기
                Heapify(arr, size, largest);
            }
        }

        static void Swap(Student[] arr, int i, int j)// 두 요소의 위치를 바꾸는 함수
        {
            Student temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        // RadixSort란?
        // 각 자리수(1의 자리, 10의 자리, 100의 자리...)를 기준으로 여러 번 정렬해서 전체 정렬을 완성하는 정렬
        // ex. 38 27 43 3 9 82 10
        // 10 27 38 43 3 82 9
        // 3 9 10 27 38 43 82
        static Student[] RadixSort(Student[] students)
        {
            Student[] arr = (Student[])students.Clone();
            int exp = 1;
            int max = 300;

            while (max / exp > 0)
            {
                arr = DigitSort(arr, exp);
                exp *= 10;
            }

            Array.Reverse(arr); // 내림차순 정렬
            return arr;
        }

        static Student[] DigitSort(Student[] students, int exp)
        {
            Student[] output = new Student[students.Length];
            int[] count = new int[10];

            // 자릿수별 등장 횟수 세기
            for (int i = 0; i < students.Length; i++)
            {
                int digit = (students[i].Total / exp) % 10;
                count[digit]++;
            }

            //자릿수별 등장 횟수를 누적합으로 변경
            for (int i = 1; i < 10; i++)
                count[i] += count[i - 1];

            for (int i = students.Length - 1; i >= 0; i--)
            {
                int digit = (students[i].Total / exp) % 10;
                output[--count[digit]] = students[i];
            }

            return output;
        }

        // 배열 잘라내는 함수 (less/more 배열 등)
        static Student[] TrimArray(Student[] arr, int size)
        {
            Student[] result = new Student[size];
            Array.Copy(arr, result, size);
            return result;
        }

        // 배열 3개 합치는 함수
        static Student[] CombineArrays(Student[] a, Student[] b, Student[] c)
        {
            Student[] result = new Student[a.Length + b.Length + c.Length];
            Array.Copy(a, 0, result, 0, a.Length);
            Array.Copy(b, 0, result, a.Length, b.Length);
            Array.Copy(c, 0, result, a.Length + b.Length, c.Length);
            return result;
        }
    } 
 */