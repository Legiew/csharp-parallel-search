using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;

namespace Parallel_Computing_Project
{
   /// <summary>
   /// Interaktionslogik für MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      private FolderBrowserDialog folderBrowseDialog;

      private object listLock = new object();

      private Stopwatch runWatch;
      private Stopwatch startWatch;

      public ViewModel ViewModel;

      public MainWindow()
      {
         InitializeComponent();

         this.ViewModel = new ViewModel();
         this.ViewModel.ResultTime = "";
         this.ViewModel.SearchDirectory = @"C:\Users\Sebastian\SolarCar\workspace\RoadsignRacer Desktop Old";
         this.ViewModel.SearchText = "cache";
         this.ViewModel.SearchResultList = new ObservableCollection<string>();

         this.windowMain.DataContext = this.ViewModel;

         this.comboBoxSearchType.ItemsSource = Enum.GetValues(typeof(SearchType));

         this.folderBrowseDialog = new FolderBrowserDialog();
         this.ViewModel.SearchResultList = new ObservableCollection<string>();

         BindingOperations.EnableCollectionSynchronization(this.ViewModel.SearchResultList, this.listLock);
      }

      private void buttonBrowse_Click(object sender, RoutedEventArgs e)
      {
         DialogResult result = this.folderBrowseDialog.ShowDialog();

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            this.ViewModel.SearchDirectory = this.folderBrowseDialog.SelectedPath;
         }
      }

      private void buttonStartSearch_Click(object sender, RoutedEventArgs e)
      {
         this.ViewModel.ResultTime = "";
         this.ViewModel.SearchResultList.Clear();

         SearchType searchType = (SearchType)Enum.Parse(typeof(SearchType), this.comboBoxSearchType.SelectedItem.ToString(), true);

         startWatch = Stopwatch.StartNew();
         this.runWatch = Stopwatch.StartNew();

         switch (searchType)
         {
            case SearchType.Synchron:
               this.ExecuteSynchSearch(this.ViewModel.SearchDirectory, this.ViewModel.SearchText);
               break;
            case SearchType.Thread:
               ExecuteThreadSearch(this.ViewModel.SearchDirectory, this.ViewModel.SearchText);
               break;
            case SearchType.Task:
               ExecuteTaskSearch(this.ViewModel.SearchDirectory, this.ViewModel.SearchText);
               break;
            case SearchType.ParallelLoop:
               ExecuteParallelForSearch(this.ViewModel.SearchDirectory, this.ViewModel.SearchText);
               break;
            case SearchType.AsyncAwait:
               ExecuteAsyncWait(this.ViewModel.SearchDirectory, this.ViewModel.SearchText);
               break;
            case SearchType.PLinq:
               ExecutePLinqForSearch(this.ViewModel.SearchDirectory, this.ViewModel.SearchText);
               break;
            default:
               break;
         }

         startWatch.Stop();
      }

      private void ExecuteThreadSearch(string directory, string text)
      {
         new Thread(() => this.ExecuteSynchSearch(this.ViewModel.SearchDirectory, this.ViewModel.SearchText)).Start();
      }

      private void ShowTimes()
      {
         this.ViewModel.ResultTime = "Start Time: " + this.startWatch.ElapsedMilliseconds + " ms"
            + " Run Time: " + this.runWatch.ElapsedMilliseconds + " ms";
      }

      private void ExecuteTaskSearch(string directory, string text)
      {
         Task.Factory.StartNew(() => ExecuteSynchSearch(directory, text));
      }

      private void ExecutePLinqForSearch(string directory, string text)
      {
         IEnumerable<string> files = Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories);

         var fileResults = from file in files.AsParallel()
                           where File.ReadAllText(file).Contains(text)
                           select file;

         foreach (var file in fileResults)
         {
            this.ViewModel.SearchResultList.Insert(0, file);
         }

         this.runWatch.Stop();

         this.ShowTimes();
      }

      private void ExecuteParallelForSearch(string directory, string text)
      {
         string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

         Parallel.ForEach(files, currentFile =>
         {
            string fileContent = File.ReadAllText(currentFile);

            if (fileContent.Contains(text))
            {
               this.ViewModel.SearchResultList.Insert(0, currentFile);
            }
         });

         this.runWatch.Stop();

         this.ShowTimes();
      }

      private void ExecuteAsyncWait(string directory, string text)
      {
          Task.Factory.StartNew(() => ExecuteSynchSearch(this.ViewModel.SearchDirectory, this.ViewModel.SearchText));
      }  

      private void ExecuteSynchSearch(string directory, string text)
      {
         string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

         List<string> foundFiles = new List<string>();

         foreach (string file in files)
         {
            string fileContent = File.ReadAllText(file);

            if (fileContent.Contains(text))
            {
               this.ViewModel.SearchResultList.Insert(0, file);
            }
         }

         this.runWatch.Stop();

         this.ShowTimes();
      }
   }
}