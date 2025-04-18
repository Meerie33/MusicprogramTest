﻿using MusikPlayer.Logs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusikPlayer.FileManager
{
    public class FileDirector : FilePathBase
    {
        private static FileDirector instance;
        private static object locker = new object();

        private List<DirectoryInfo> directorys = new List<DirectoryInfo>();

        private FileDirector()
        {
        }

        public static FileDirector GetFileDirector()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new FileDirector();
                    }
                }
            }
            return instance;
        }

        /// <param name="mediaExtensions">Liste der DateiTypen</param>
        public List<string> GetAllFilesFromDevice(List<string> mediaExtensions)
        {
            List<string> filesFound = new List<string>();

            var directorys = this.GetAllDirectorys(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)));

            foreach (DirectoryInfo directory in directorys)
            {
                filesFound.AddRange(this.GetFilesPath(directory, mediaExtensions));
            }

            return filesFound;
        }

        /// <param name="directory"></param>
        /// <param name="mediaExtensions"></param>
        private List<string> GetFilesPath(DirectoryInfo directory, List<string> mediaExtensions)
        {
            List<string> filesFound = new List<string>();
            try
            {
                foreach (string filePath in Directory.GetFiles(directory.FullName, "*.*"))
                {
                    if (mediaExtensions.Contains(Path.GetExtension(filePath).ToLower()))
                        filesFound.Add(filePath);
                }
            }
            catch (UnauthorizedAccessException UAex)
            {
                Logger.Instance.ExceptionLogg(nameof(FileDirector), nameof(GetFilesPath), UAex, $"Access denied to [{directory.Name}]");
            }
            catch (Exception ex)
            {
                Logger.Instance.ExceptionLogg(nameof(FileDirector), nameof(GetFilesPath), ex);
            }
            return filesFound;
        }

        /// <param name="root"></param>
        /// <returns></returns>
        private List<DirectoryInfo> GetAllDirectorys(DirectoryInfo root)
        {
            foreach (var item in root.GetDirectories())
            {
                if (this.CheckDirectoryAttributesBy(item))
                {
                    this.directorys.Add(item);
                    this.GetAllDirectorys(item);
                }
            }
            return this.directorys;
        }

        /// <param name="directoryToCheck"></param>
        /// <returns></returns>
        private bool CheckDirectoryAttributesBy(DirectoryInfo directoryToCheck)
        {
            string directory = $"{FileAttributes.Directory}";
            string readOnlyDirectory = $"{FileAttributes.ReadOnly}, {FileAttributes.Directory}";
            string currentDirectoryToCheck = directoryToCheck.Attributes.ToString();

            if (currentDirectoryToCheck == directory)
            {
                return true;
            }
            else if (currentDirectoryToCheck == readOnlyDirectory)
            {
                return true;
            }

            return false;

        }
    }
}
