using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace Lab2SPP
{
    public class Algorithms
    {
        public bool CheckPath(string path)
        {
            Regex reg = new Regex(@"^([A-Za-z]:(\\){1})((\\?\w*)*)?(\w*[^\\]$)");
            MatchCollection matches = reg.Matches(path);
            return (matches.Count == 1) ? true : false;
        }
        public Queue<FileInfo[]> GetGroupOfFiles(FileInfo[] source_files, int limit)
        {
            Queue<FileInfo[]> group = new Queue<FileInfo[]>();
            int files = 0;
            int size = source_files.Length;
            while (files != source_files.Length)
            {
                FileInfo[] sequence = null;
                if (size < limit)
                {
                    sequence = new FileInfo[size];
                }
                else
                {
                    sequence = new FileInfo[limit];
                }

                for (int i = 0; i < limit; i++)
                {
                    if (files == source_files.Length)
                        break;
                    sequence[i] = source_files[files];
                    files++;
                }
                size -= limit;
                group.Enqueue(sequence);
            }
            return group;
        }
        private Queue<FileInfo> file_queue = new Queue<FileInfo>();
        public Queue<FileInfo> GetFiles(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileSystemInfo[] mas = dir.GetFileSystemInfos();
            FindFiles(mas);
            return file_queue;
        }
        private void FindFiles(FileSystemInfo[] info)
        {
            for(int i = 0; i<info.Length; i++)
            {
                if((info[i].Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    DirectoryInfo subdir = (DirectoryInfo)info[i];
                    FindFiles(subdir.GetFileSystemInfos());
                }
                else
                {
                    file_queue.Enqueue((FileInfo)info[i]);
                }
            }
        }
    }
}
