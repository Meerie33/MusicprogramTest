﻿using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace MusikPlayer.FileManager
{
    public abstract class FilePathBase
    {

        public const string CONFIG_FILE = "\\Config.json";
        public const string SONG_DATA_FILE = "\\SongData.json";
        public const string PLAYLIST_FILE = "\\PlayList.json";

        public readonly string ConfigFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}{PROJECT_DIRECTORY}";
        //public readonly string SongDataFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}{PROJECT_DIRECTORY}";
        public readonly string SongDataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)); // Beispiel: Datei im Musik-Ordner
        public readonly string LogsFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}{PROJECT_DIRECTORY}";
        //public readonly string PlayListFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}{PROJECT_DIRECTORY}";
        public readonly string PlayListFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}{PROJECT_DIRECTORY}";

        private const string GLOBAL_FILTER = "Alle Dateien (*.*)|*.*";
        private const string PROJECT_DIRECTORY = "\\Mp3Player";


        public string GetFilePath(string dialogTitle, string filter = GLOBAL_FILTER)
        {
            return GetSaveFileInfo(filter, dialogTitle);
        }

        public async void SaveText(string data, string filePath)
        {
            // Hat kein Bock
            /*
            System.Diagnostics.Debug.WriteLine($"DEBUG -> LOG:\n{data}\n");

            FileInfo file = new System.IO.FileInfo(filePath);
            file.Directory.Create();
            await File.WriteAllTextAsync(filePath, data);
            */
        }

        public string LoadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public bool ExistFolder(string folder)
        {
            return Directory.Exists(folder);
        }

        public bool ExistFile(string file)
        {
            return File.Exists(file);
        }

        public void CreateFolder(string value)
        {
            Directory.CreateDirectory(value);
        }

        public void CreateFile(string value)
        {
            File.Create(value).Dispose();
        }

        private string GetSaveFileInfo(string filter, string dialogTitle)
        {
            string filePath = "";

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = dialogTitle;
            openFile.Filter = filter;
            openFile.ShowDialog();

            filePath = openFile.FileName;

            return filePath;
        }


    }
}
