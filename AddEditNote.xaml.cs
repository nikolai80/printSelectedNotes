using PrintSelected.BLL;
using System;
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
using System.Windows.Shapes;

namespace PrintSelected
{
    /// <summary>
    /// Interaction logic for AddEditNote.xaml
    /// </summary>
    public partial class AddEditNote : Window
    {
        public ParodontRecommendationRepository Repo { get; set; }
        public string RecommendationGuid { get; set; }
        public AddEditNote()
        {
            InitializeComponent();
            this.Loaded += AddEditNoteWindow_Loaded;
            
        }

        private void AddEditNoteWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(RecommendationGuid))
            {
                NoteTxbox.Text = Repo.GetById(Guid.Parse(RecommendationGuid)).Text;
            }
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            var text = NoteTxbox.Text;
            var res = false;


            if (!String.IsNullOrEmpty(RecommendationGuid))
            {
                res = Repo.Update(Guid.Parse(RecommendationGuid), text);
                MessageBox.Show("Рекоммендация успешно обновлена");
            }
            else
            {
                res = Repo.Create(text);
                MessageBox.Show("Рекоммендация успешно добавлена");
            }

            if (!res)
            {
                MessageBox.Show("Произошла ошибка при добавлении рекомендации");
            }
            
            this.DialogResult = true;
        }


    }
}
