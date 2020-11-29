using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;


namespace FilesSearcher
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                System.Console.WriteLine("Please, enter more arguments.");
                System.Console.WriteLine("Usage: Find -options pattern1 pattern2");
            }
            else
            {
                // создание пустой перечисляемой коллекции сведений о файле
                IEnumerable<FileInfo> fiList = Enumerable.Empty<FileInfo>();
                
                // в этот список буду добавлять файлы, которые будут найдены по заданным параметрам поиска
                List<string> fileNames = new List<string>();
                //IEnumerable<FileInfo> findedFiles = Enumerable.Empty<FileInfo>();
                List<FileInfo> findedFiles = new List<FileInfo>();

                // создаю битовую ячейку для каждого из ключей(-R, -E, -T, -s)
                BitArray myBitArray = new BitArray(4);

                int strPatPlace = 0;       // место искомой строки среди параметров функции Find
                int regPatPlace = 0;       // место искомого шаблона среди параметров функции Find
                int sortParPlace = 0;      // место параметров сортировки среди параметорв функции Find

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "-R") myBitArray[0] = true;
                    if (args[i] == "-E") 
                    {
                        myBitArray[1] = true;
                        regPatPlace = i + 1;    // указывает на номер аргумета после опции -E
                    }
                    if (args[i] == "-T")
                    {
                        myBitArray[2] = true;
                        strPatPlace = i + 1;    // указывает на номер аргумета после опции -T
                    }
                    if (args[i] == "-s")
                    {
                        myBitArray[3] = true;
                        sortParPlace = i + 1;   // указывает на номер аргумета после опции -s
                    }
                }

                if (myBitArray[0]) // option -R
                    fiList = TextFilesSearher.TakeFiles(args[args.Length - 2], args[args.Length - 1], 1);     //int = 1 --> SearchOption.AllDirectories
                else
                    fiList = TextFilesSearher.TakeFiles(args[args.Length - 2], args[args.Length - 1], 0);     // int = 0 --> SearchOption.TopDirectoryOnly

              
                /*
                 Из документации:
                 Используйте методы класса System.String при поиске определенной строки.
                 Используйте класс Regex при поиске определенного шаблона в строке.
                 */
                if (myBitArray[2])      // задана опция -T для поиска строки
                {
                    /*
                     Этот метод выполняет сравнение по порядковым номерам
                     (с учетом регистра и без учета языка и региональных параметров).
                     */
                    String strPat = args[strPatPlace];
                    foreach (var fi in fiList)
                    {
                        // мой вариант конструктора public StreamReader(Stream stream, Encoding encoding)
                        using (StreamReader sr = new StreamReader(new FileStream(fi.FullName, FileMode.Open,
                                                                   FileAccess.Read), Encoding.Default))
                        {
                            String str;

                            while ((str = sr.ReadLine()) != null)
                            {
                                if (str.Contains(strPat))
                                {
                                    fileNames.Add(fi.FullName);     // добавляем элемент, соответствующий критерию поиска
                                    //Console.WriteLine("{0}", fi.FullName);
                                    findedFiles.Add(fi);
                                }
                            }
                        }

                    }
                }   // option -T
                               
                if (myBitArray[1])
                {
                    String regexPat = args[regPatPlace];
                                        
                    // Regex rgx = new Regex(regexPat, RegexOptions.IgnoreCase);
                    // Выше создание экземпляра, который представляет регулярное выражение. 
                    // Но буду использовать статический метод, так как он кэшируется
                    // например: public static bool IsMatch(string input, string pattern)

                    foreach (var fi in fiList)
                    {
                        // StreamReader sr = fi.OpenText() - если открывать так, то открывает в кодировке UTF-8
                        using (StreamReader sr = new StreamReader(new FileStream(fi.FullName, FileMode.Open,
                                                                   FileAccess.Read), Encoding.Default))
                        {
                            String str;

                            while ((str = sr.ReadLine()) != null)
                            {
                                if (Regex.IsMatch(str, regexPat))
                                {
                                    fileNames.Add(fi.FullName);     // добавляем элемент, соответствующий критерию поиска
                                    findedFiles.Add(fi);
                                    //Console.WriteLine("{0}", fi.Name);      // можно fi.FullName
                                    break;
                                }
                            }
                        }

                    }
                }   // option -E


                if (myBitArray[3])      // опция для порядка сортировки, параметр -s
                {
                    // убираю повторяющиеся имена, если при поиске будут заданы опции (-T && -E), 
                    // которые найдут одинаковые файлы
                    if (myBitArray[1] && myBitArray[2])
                    {
                        fileNames = TextFilesSearher.GetUniques(fileNames);
                        findedFiles = TextFilesSearher.GetUniquesFI(findedFiles);
                    }

                    String sortPar = args[sortParPlace];

                    for (int i = 0; i < sortPar.Length; i++)
                    {
                //        var sortedGroups =
                //from student in students
                //orderby student.Last, student.First
                //group student by student.Last[0] into newGroup
                //orderby newGroup.Key
                //select newGroup;

                        if (sortPar[i] == 'n')
                        {
                            var sortedByNames =
                            from files in findedFiles
                            orderby files.Name descending
                            select files;

                            foreach (var fi in sortedByNames)
                            {
                                Console.WriteLine("{0}", fi.Name);
                            }

                            //findedFiles.Sort(ComparebyName);
                            //if (sortPar[i + 1] == '+')
                            //                    i = i + 1;
                            //else if (sortPar[i + 1] == '-')
                            //{
                            //    findedFiles.Reverse();
                            //    i = i + 1;
                            //} 
                        }
                        
                        
                    }
                    

                    //Comparison<FileInfo> sortDelegate1 = ComparebyDate;
                    //Comparison<FileInfo> sortDelegate2 = ComparebyName;
                    //Comparison<FileInfo> sortDelegate3 = ComparebyExtension;
                    //Comparison<FileInfo> sortDelegate4 = ComparebySize;

                    //findedFiles.Sort(ComparebyDate);
                    //findedFiles.Reverse();


                   
                    //foreach (var fi in findedFiles)
                    //{
                    //    Console.WriteLine("{0}", fi);
                    //}

                }   // option -s
                               
                      
            }   // else

        }   // Main()

        
        private static int ComparebyDate(FileInfo x, FileInfo y)   // сравнение файлов по времени обновления  
        {
            // Использование времени в формате UTC рекомендовано, когда важна совместимость даты и времени между компьютерами
            return x.LastWriteTimeUtc.CompareTo(y.LastWriteTimeUtc);
        }

        private static int ComparebyName(FileInfo x, FileInfo y)   // сравнение файлов по имени
        {
            return x.FullName.CompareTo(x.FullName);
        }

        private static int ComparebyExtension(FileInfo x, FileInfo y)  // сравнение файлов по расширению
        {
            return x.Extension.CompareTo(y.Extension);
        }

        private static int ComparebySize(FileInfo x, FileInfo y)   // сравнение файлов по размеру файла
        {
            return x.Length.CompareTo(y.Length);
        }

    
    }   // Program

}   // namespace Find


