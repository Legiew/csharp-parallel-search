﻿using Catel.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Computing_Project
{
   public class ViewModel : ObservableObject
   {
      private string searchText;
      private string searchDirectory;
      private string resultTime;
      private ObservableCollection<string> searchResultList;

      public string SearchText
      {
         get { return searchText; }
         set
         {
            RaisePropertyChanging(() => SearchText);

            var oldValue = searchText;
            searchText = value;
            RaisePropertyChanged(() => SearchText, oldValue, value);
         }
      }

      public string SearchDirectory
      {
         get { return searchDirectory; }
         set
         {
            RaisePropertyChanging(() => SearchDirectory);

            var oldValue = searchDirectory;
            searchDirectory = value;
            RaisePropertyChanged(() => SearchDirectory, oldValue, value);
         }
      }

      public string ResultTime
      {
         get { return resultTime; }
         set
         {
            RaisePropertyChanging(() => ResultTime);

            var oldValue = resultTime;
            resultTime = value;
            RaisePropertyChanged(() => ResultTime, oldValue, value);
         }
      }

      public ObservableCollection<string> SearchResultList
      {
         get { return searchResultList; }
         set
         {
            RaisePropertyChanging(() => SearchResultList);

            var oldValue = searchResultList;
            searchResultList = value;
            RaisePropertyChanged(() => SearchResultList, oldValue, value);
         }
      }
   }
}
