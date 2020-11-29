using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FilesSearcher
{
    /// <summary>
    /// Класс содержит методы для поиска файлов по заданным параметрам
    /// </summary>
    public class TextFilesSearher
    {
        /// <summary>
        /// Метод ищет файлы по заданному имени в текущем каталоге и подкаталогах
        /// </summary>
        /// <param name="str1">
        ///     Шаблон строки, по которому будет произведён поиск имен файлов.
        ///     Этот параметр может содержать сочетание допустимого литерального пути и подстановочного символа (* и ?), но не поддерживает регулярные выражения.
        ///     Пробел служит окончанием шаблона строки.
        /// </param>
        /// <param name="str2">
        ///     Другой шаблон строки, по которому будет произведён поиск имен файлов.
        ///     Этот параметр может содержать сочетание допустимого литерального пути и подстановочного символа (* и ?), но не поддерживает регулярные выражения.
        ///     Пробел служит окончанием шаблона строки.
        /// </param>
        /// <param name="n">поиск в подкаталогах: 0 -- поиск только в текущем каталоге; 1 -- поиск в текущем каталоге и подкаталогах</param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> TakeFiles(String str1, String str2, int n)
        {
            // Create a DirectoryInfo object of the starting directory.
            DirectoryInfo diTop = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            try
            {   
                return diTop.EnumerateFiles(str1, (SearchOption)n).Concat(
                                diTop.EnumerateFiles(str2, (SearchOption)n));
            }
            // Catch error in directory path.
            catch (DirectoryNotFoundException DirNotFound)
            {
                Console.WriteLine("{0}", DirNotFound.Message);
                return Enumerable.Empty<FileInfo>();
            }
            // Catch unauthorized access to a first tier directory. 
            catch (UnauthorizedAccessException UnAuthDir)
            {
                Console.WriteLine("UnAuthDir: {0}", UnAuthDir.Message);
                return Enumerable.Empty<FileInfo>();
            }

        }

        // обобщённый метод поиска дубликатов в списке
        public static List<T> GetUniques<T>(List<T> list) 
        { 
            // Для отслеживания элементов используйте словарь 
            Dictionary<T, bool> found = new Dictionary<T, bool>(); 
            List<T> uniques = new List<T>(); 
            // Этот алгоритм сохраняет оригинальный порядок элементов 
            foreach (T val in list) 
            { 
                if (!found.ContainsKey(val)) 
                { 
                    found[val] = true; 
                    uniques.Add(val) ; 
                } 
            } 
            return uniques; 
        }
        ///
        ///

        // метод для удаления дупликатов из списка FileInfo 
        public static List<FileInfo> GetUniquesFI(List<FileInfo> list)
        {
            // Для отслеживания элементов используйте словарь 
            Dictionary<string, bool> found = new Dictionary<string, bool>();
            List<FileInfo> uniques = new List<FileInfo>();
            // Этот алгоритм сохраняет оригинальный порядок элементов 
            foreach (FileInfo val in list)
            {
                if (!found.ContainsKey(val.FullName))
                {
                    found[val.FullName] = true;
                    uniques.Add(val);
                }
            }
            return uniques;
        } 

    }
}
