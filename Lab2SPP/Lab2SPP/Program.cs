using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Lab1SPP.Elements;
using System.IO;
namespace Lab2SPP
{
    class Program
    {
        private static Queue<FileInfo> file_queue = new Queue<FileInfo>();
        static void Main(string[] args)
        {
            Algorithms algorithms = new Algorithms();
            int files = 0;
            Console.WriteLine("Введите исходный путь к каталогу\n");
            string sourcePath = Console.ReadLine();
            Console.WriteLine("Введите целевой путь к каталогу\n");
            string targetPath = Console.ReadLine();
            if (algorithms.CheckPath(sourcePath) && algorithms.CheckPath(targetPath))
            {
               file_queue =  algorithms.GetFiles(sourcePath);
               TaskQueue pool = new TaskQueue(file_queue.Count);
                TaskDelegate task = delegate
                {
                    try
                    {
                        Monitor.Enter(file_queue);
                        FileInfo currFile = file_queue.Dequeue();
                        string destFilePath = string.Concat(targetPath, "\\", currFile.Name);
                        File.Copy(currFile.FullName, destFilePath);
                        Console.WriteLine(currFile.Name);
                        files++;
                    }
                    catch(IOException)
                    {
                        Console.WriteLine("Файл для копирования уже существует");
                        throw;
                    }
                    finally
                    {
                        Monitor.Exit(file_queue);
                    }
                    
                };
                int size = file_queue.Count;
                for(int i = 0; i<size; i++)
                {
                    pool.EnqueueTask(task);
                }
                pool.Dispose();
                Console.ReadLine();
                Console.WriteLine($"Количество скопированных файлов: {files}");
            }
        }
    }
}
/*
 *  static void Main(string[] args)
        {
            TaskQueue queue = new TaskQueue(10);
            int files = 0;
            Console.WriteLine("Введите исходный путь к каталогу\n");
            string targetPath = Console.ReadLine();
            Console.WriteLine("Введите целевой путь к каталогу\n");
            string sourcePath = Console.ReadLine();
            if (Algorithms.CheckPath(sourcePath) && Algorithms.CheckPath(targetPath))
            {
                DirectoryInfo target_dir = new DirectoryInfo(targetPath);
                FileInfo[] target_files = target_dir.GetFiles();
                TaskDelegate copying = delegate ()
                {
                    for (int i = 0; i < target_files.Length; i++)
                    {
                        string destFilePath = string.Concat(sourcePath, "\\", target_files[i].Name);
                        File.Copy(target_files[i].FullName, destFilePath);
                        Console.WriteLine(target_files[i].Name);
                        files++; 
                    }
                    Console.WriteLine("Files: {0}", files);
                };
                queue.EnqueueTask(copying);
            }
            queue.Dispose();
        }
 * 
 */

//Второй вариант
/*
 * if (algorithms.CheckPath(sourcePath) && algorithms.CheckPath(targetPath))
            {
                DirectoryInfo source_dir = new DirectoryInfo(sourcePath);
                Queue<FileInfo[]> group_of_files = algorithms.GetGroupOfFiles(source_dir.GetFiles(), LIMIT);
                TaskQueue pool = new TaskQueue(group_of_files.Count);
                int i = 0;
                object locker = new object();
                TaskDelegate task = delegate ()
                {
                    try
                    {
                        lock (group_of_files)
                        {
                            if (group_of_files.Count != 0)
                            {
                                FileInfo[] files_mas = group_of_files.Dequeue();
                                for (int j = 0; j < files_mas.Length; j++)
                                {
                                    string destFilePath = string.Concat(targetPath, "\\", files_mas[j].Name);
                                    File.Copy(files_mas[j].FullName, destFilePath);
                                    Console.WriteLine(files_mas[j].Name);
                                    files++;
                                }
                            }
                        }
                    }
                    catch (IOException)
                    {
                    }
                };
                while (group_of_files.Count!=0)
                {
                    try
                    {
                        if(Monitor.TryEnter(pool))
                        pool.EnqueueTask(task);
                    }
                    finally
                    {
                        Monitor.Exit(pool);
                    }
                }
                pool.Dispose();
                Console.WriteLine("\nКоличество скопированных файлов: {0}", files);
            }
 */
