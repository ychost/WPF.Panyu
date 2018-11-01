using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaWeiUtils
{
    /// <summary>
    /// 文件提取器，用于从指定目录获取按指定排序方式所筛选的文件信息
    /// 两种比较方式：
    ///     1）按文件的创建时间进行比较
    ///     2）按文件名上面的时间进行比较
    /// 两种排序方式：
    ///     1）升序
    ///     2）降序
    /// </summary>
    public class FileExtractor
    {
        private string directory;
        private FileCompareMode compareMode;
        private FileSortMode sortMode;

        /// <summary>
        /// 文件提取器构造函数
        /// </summary>
        /// <param name="directory">搜寻文件的目录</param>
        public FileExtractor(string directory)
            : this(directory, FileCompareMode.FileName)
        {
        }

        /// <summary>
        /// 文件提取器构造函数
        /// </summary>
        /// <param name="directory">搜寻文件的目录</param>
        /// <param name="compareMode">指定文件的比较方式，用于排序</param>
        public FileExtractor(string directory, FileCompareMode compareMode)
            : this(directory, compareMode, FileSortMode.Descending)
        {
        }

        /// <summary>
        /// 文件提取器构造函数
        /// </summary>
        /// <param name="directory">搜寻文件的目录</param>
        /// <param name="compareMode">指定文件的比较方式，用于排序</param>
        /// <param name="sortMode">排序方式，升序或降序</param>
        public FileExtractor(string directory, FileCompareMode compareMode, FileSortMode sortMode)
        {
            this.directory = directory;
            if(!this.directory.EndsWith("\\"))
            {
                this.directory += "\\";
            }
            this.compareMode = compareMode;
            this.sortMode = sortMode;
        }

        /// <summary>
        /// 获取指定数量的文件信息，如果当前文件夹文件不足，则全部返回
        /// </summary>
        /// <param name="number">需要返回的文件信息数</param>
        /// <returns></returns>
        public FileInfo[] GetFileInfos(int number)
        {
            DirectoryInfo di = new DirectoryInfo(directory);
            FileInfo[] files = di.GetFiles();
            if(compareMode == FileCompareMode.CreateTime)
            {
                CreationTimeComparer ctc = new CreationTimeComparer(sortMode);
                Array.Sort(files, ctc);
            }
            else
            {
                FileNameComparer fnc = new FileNameComparer(sortMode);
                Array.Sort(files, fnc);
            }

            if(number >= files.Length)
            {
                return files;
            }
            else
            {
                FileInfo[] resultFiles = new FileInfo[number];
                Array.Copy(files, resultFiles, number);
                return resultFiles;
            }
        }
    }

    public enum FileCompareMode
    {
        CreateTime,
        FileName
    }

    public enum FileSortMode
    {
        Ascending,
        Descending
    }

    /// <summary>
    /// 按文件从创建时间进行排序
    /// </summary>
    internal class CreationTimeComparer : IComparer
    {
        private FileSortMode sortMode;

        public CreationTimeComparer() : this(FileSortMode.Descending)
        {
        }

        public CreationTimeComparer(FileSortMode sortMode)
        {
            this.sortMode = sortMode;
        }

        int IComparer.Compare(Object o1, Object o2)
        {
            FileInfo fi1 = o1 as FileInfo;
            FileInfo fi2 = o2 as FileInfo;

            if(sortMode == FileSortMode.Ascending)
            {
                return fi1.CreationTime.CompareTo(fi2.CreationTime);
            }
            else
            {
                return fi2.CreationTime.CompareTo(fi1.CreationTime);
            }
        }
    }

    /// <summary>
    /// 按文件名上面的时间进行排序
    /// </summary>
    internal class FileNameComparer : IComparer
    {
        private FileSortMode sortMode;

        public FileNameComparer() : this(FileSortMode.Descending)
        {
        }

        public FileNameComparer(FileSortMode sortMode)
        {
            this.sortMode = sortMode;
        }

        public int Compare(object o1, object o2)
        {
            FileInfo fi1 = o1 as FileInfo;
            FileInfo fi2 = o2 as FileInfo;

            string fileName1 = fi1.Name.Substring(0, fi1.Name.LastIndexOf('.'));
            string fileName2 = fi2.Name.Substring(0, fi2.Name.LastIndexOf('.'));

            DateTime dt1 = DateTime.ParseExact(DigitFilter(fileName1), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
            DateTime dt2 = DateTime.ParseExact(DigitFilter(fileName2), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

            if(sortMode == FileSortMode.Ascending)
            {
                return dt1.CompareTo(dt2);
            }
            else
            {
                return dt2.CompareTo(dt1);
            }
        }

        /// <summary>
        /// 过滤并提取字符串中的所有数字 
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        private string DigitFilter(string sourceString)
        {
            string number = null;
            foreach(char item in sourceString)
            {
                if(item >= 48 && item <= 58)
                {
                    number += item;
                }
            }
            return number;
        }
    }
}
