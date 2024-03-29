﻿using System;
using System.Collections.Generic;
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
using PrintSelected.BLL;
using LoremNET;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using PrintSelected.API;

namespace PrintSelected
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> selectedIdList ;
        PrepareDocument recomendationDocument;
        ParodontRecommendationRepository repo = new ParodontRecommendationRepository();

        public MainWindow()
        {
            InitializeComponent();
            selectedIdList = new List<string>();
            recomendationDocument = new PrepareDocument();
            recommendationsList.ItemsSource = repo.GetAll();
        }

        private void Button_Click_AddRecommendation(object sender, RoutedEventArgs e)
        {
            AddEditNote addEditNoteWindow = new AddEditNote();
            addEditNoteWindow.Owner = this;
            addEditNoteWindow.Repo = repo;
            addEditNoteWindow.Show();

            #region Для тестирования
            //var testText = Lorem.Paragraph(10, 3);
            //repo.Create(testText);
            //repo.GetAll();
            #endregion
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = e.Source as CheckBox;

            selectedIdList.Add(chkBox.Uid);

        }

        private void Button_Click_Print(object sender, RoutedEventArgs e)
        {
            recomendationDocument.DocumentIds = selectedIdList;
            recomendationDocument.CreateDocumentWord(PatientNameTxbox.Text);
        }

        

        

    }
}
