﻿using System;
using System.Diagnostics;
using System.Reflection;
using TesJson;

namespace ProjetConsole
{
    public enum langueEnum { english, french, spanish };
    public class View
    {
        private string sourcePath = string.Empty;
        private string targetPath = string.Empty;
        private string targetFile = string.Empty;
        public langueEnum askLanguage()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string langageToPrint = string.Empty;
            foreach (var item in Enum.GetValues(typeof(langueEnum)))
            {
                langageToPrint += item + ", ";
            }
            Console.WriteLine("Select language: " + langageToPrint);
            string? inputLanguage = Console.ReadLine()?.ToLower();
            langueEnum language;

            switch (inputLanguage)
            {
                case "french":
                case "fr":
                case "français":
                case "francais":
                    language = langueEnum.french;
                    break;
                case "spanish":
                case "es":
                case "espagnol":
                    language = langueEnum.spanish;
                    break;
                default:
                    language = langueEnum.english;
                    break;
            }
            return language;
        }

        // --------------Get Info------------------
        public string getSourcePath()
        { return this.sourcePath; }
        public string getTargetPath()
        { return this.targetPath; }
        public string getTargetFile()
        { return this.targetFile; }


        // --------------Ask Info------------------
        public void askSourcePath()
        {
            Console.WriteLine("\n" + Traduction.Instance.Langue.EnterSourcePath);
            this.sourcePath = Console.ReadLine() ?? string.Empty;
        }
        public void askTargetFile()
        {
            Console.WriteLine("\n" + Traduction.Instance.Langue.EnterTargetFile);
            this.targetFile = Console.ReadLine() ?? string.Empty;
        }
        public void askTargetPath()
        {
            Console.WriteLine("\n" + Traduction.Instance.Langue.EnterTargetPath);
            this.targetPath = Path.Combine(Console.ReadLine() ?? string.Empty, targetFile);
        }

        // --------------Info Invalid------------------
        public void sourcePathIsInvalid()
        {
            Console.WriteLine(Traduction.Instance.Langue.SourcePathInvalid + "\n");
        }
        public void targetPathIsInvalid()
        {
            Console.WriteLine(Traduction.Instance.Langue.TargetPathInvalid + "\n");
        }
        public void targetDirInvalid()
        {
            Console.WriteLine(Traduction.Instance.Langue.targetDirInvalid + "\n");
        }

        // --------------------------------
        public void progress(bool state)
        {
            if (!state) { Console.WriteLine("\n" + Traduction.Instance.Langue.Buffering); }
            else if (state) { Console.WriteLine("\n" + Traduction.Instance.Langue.Complete); }

        }
        public void Display(string toDisplay)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(toDisplay);
            Console.ForegroundColor = ConsoleColor.White;
        }

    }

}