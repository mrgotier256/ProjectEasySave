﻿using CommonCode;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Windows.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace ProjetBureau
{
    public partial class MainWindow : Window, IView
    {
        ProgressWindow progressWindow = new ProgressWindow();
        public MainWindow()
        {
            InitializeComponent();
            Traduction.Instance.SetInterfaceLanguage(SelectLanguage.Text);
            TextEnterSourcePath.Content = Traduction.Instance.Langue.EnterSourcePath;
            TextLanguage.Content = Traduction.Instance.Langue.SelectLanguage;
            TextEnterTargetPath.Content = Traduction.Instance.Langue.EnterTargetPath;
            TextEnterTargetFile.Content = Traduction.Instance.Langue.EnterTargetFile;
            TextEnterLogType.Content = Traduction.Instance.Langue.EnterLogType;
        }

        private string sourcePath = string.Empty;
        private string targetPath = string.Empty;
        private string targetFile = string.Empty;

        public string typeOfMode => "Graphic";

        /// <summary>
        /// --------------Demande d'informations à l'utilisateur (méthode) ------------------
        /// --------------Ask Informations to user (methods) ------------------
        /// </summary>

        public langueEnum askLanguage()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string langageToPrint = string.Empty;
            foreach (var item in Enum.GetValues(typeof(langueEnum)))
            {
                langageToPrint += $"{item}, ";
            }
            Console.WriteLine("Select language: " + langageToPrint);
            string? inputLanguage = Console.ReadLine()?.ToLower();


            langueEnum selectedLanguage;
            switch (inputLanguage)
            {
                case "french":
                case "fr":
                case "français":
                case "francais":
                    selectedLanguage = langueEnum.french;
                    break;
                case "spanish":
                case "es":
                case "espagnol":
                    selectedLanguage = langueEnum.spanish;
                    break;
                default:
                    selectedLanguage = langueEnum.english;
                    break;
            }
            return selectedLanguage;
        }

        /// <summary>
        /// ---Méthodes informant l'utilisateurs que des informations sont invalides---
        /// ---methods informing the user that information is invalid---
        /// </summary>

        public void sourcePathIsInvalid()
        {
            string? messageBoxText = Traduction.Instance.Langue.SourcePathInvalid;
            string caption = "Source Path Invalid";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }
        public void targetPathIsInvalid()
        {
            string? messageBoxText = Traduction.Instance.Langue.TargetPathInvalid;
            string caption = "Source Path Invalid";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }
        public void targetDirInvalid()
        {
            string? messageBoxText = Traduction.Instance.Langue.targetDirInvalid;
            string caption = "Source Path Invalid";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }


        /// <summary>
        /// Affichage en temps réel des informations de la sauvegarde (Pourcentage | Nom du fichier | Nombre de fichier restant)
        /// Display in real time the informations of the back-up (Percentage | File's name | Number of remaining files)
        /// </summary>
        /// <param name="toDisplay"></param>

        public void display(string[] textToDisplay)
        {

            progressWindow.ContentCountsize.Dispatcher.Invoke(() => progressWindow.ContentCountsize.Text = textToDisplay[0], DispatcherPriority.Background);
            progressWindow.ContentFilename.Dispatcher.Invoke(() => progressWindow.ContentFilename.Text = textToDisplay[1], DispatcherPriority.Background);
            progressWindow.ContentHistory.Dispatcher.Invoke(() => progressWindow.ContentHistory.Text = textToDisplay[2], DispatcherPriority.Background);
            progressWindow.ProgressBarSave.Dispatcher.Invoke(() => progressWindow.ProgressBarSave.Value = Convert.ToDouble(textToDisplay[3]), DispatcherPriority.Background);
        }
        public progressState controlProgress(string fileName, double countfile, int totalFileToCopy, double percentage)
        {
            if (progressWindow.progress != progressState.pause)
            {
                string[] text = { $"{countfile}/{totalFileToCopy}", $" | {fileName}", $"{progressWindow.ContentHistory.Text}", $"{percentage}" };
                this.display(text);
            }
            progressWindow.ContentHistory.Text = $"{countfile}/{totalFileToCopy} | {fileName}\n {progressWindow.ContentHistory.Text}"; ;

            return progressWindow.progress;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IController controller = new Controller(this);
            controller.execute();
        }

        private void SelectLanguage_DropDownClosed(object sender, EventArgs e)
        {
            Traduction.Instance.SetInterfaceLanguage(SelectLanguage.Text);
            TextEnterSourcePath.Content = Traduction.Instance.Langue.EnterSourcePath;
            TextLanguage.Content = Traduction.Instance.Langue.SelectLanguage;
            TextEnterTargetPath.Content = Traduction.Instance.Langue.EnterTargetPath;
            TextEnterTargetFile.Content = Traduction.Instance.Langue.EnterTargetFile;
            TextEnterLogType.Content = Traduction.Instance.Langue.EnterLogType;
        }
        public void progress(bool state)
        {
            if (!state)
            {
                progressWindow.progress = progressState.play;
                progressWindow.Show();
            }
            else if (state) 
            {
                progressWindow.Hide();
            }
        }
        langueEnum IView.askLanguage() { return Traduction.Instance.convertLanguage(SelectLanguage.Text); }

        public string asklogType() { return "json"; }

        public string askSourcePath() {return textBoxSourcePath.Text;}

        public string askTargetFile() {return textBoxNameSave.Text;}

        public string askTargetPath() {return textBoxDestPath.Text;}

        private void btnBrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            var result = folderDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                textBoxSourcePath.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void btnBrowseFolder_Copy_Click(object sender, RoutedEventArgs e)
        {
            var folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            var result = folderDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                textBoxDestPath.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }
    }
}
