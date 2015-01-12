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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Parallel_Computing_Project
{
   /// <summary>
   /// Interaktionslogik für MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      private FolderBrowserDialog folderBrowseDialog;

      public MainWindow()
      {
         InitializeComponent();

         this.folderBrowseDialog = new FolderBrowserDialog();
      }

      private void buttonBrowse_Click(object sender, RoutedEventArgs e)
      {
         DialogResult result = this.folderBrowseDialog.ShowDialog();

         if (result == System.Windows.Forms.DialogResult.OK)
         {
            this.textBoxDirectory.Text = this.folderBrowseDialog.SelectedPath;
         }
      }

      private void buttonStartSearch_Click(object sender, RoutedEventArgs e)
      {
         this.listBoxSearchResults.ItemsSource = null;

         List<string> files = this.GetFilesInDirectory(this.textBoxDirectory.Text);

         List<string> foundFiles = new List<string>();

         foreach (string file in files)
         {
            string fileContent = File.ReadAllText(file);

            if (fileContent.Contains(this.textBoxSearchText.Text))
            {
               foundFiles.Add(file);
            }
         }

         this.listBoxSearchResults.ItemsSource = files;
      }
 
      private List<string> GetFilesInDirectory(string directory)
      {
         List<string> filesCurrentDirectory = new List<string>();

         filesCurrentDirectory.AddRange(Directory.GetFiles(directory));

         foreach (string directoryPath in Directory.GetDirectories(directory))
         {
            filesCurrentDirectory.AddRange(GetFilesInDirectory(directoryPath));
         }

         return filesCurrentDirectory;
      }
   }
}