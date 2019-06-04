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
        public AddEditNote()
        {
            InitializeComponent();
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            var text = NoteTxbox.Text;
            var res = false;

           
                res = Repo.Create(text); 
            

            if (res)
            {
                MessageBox.Show("Рекоммендация успешно сохранена");
            }
            else
            {
                MessageBox.Show("Произошла ошибка при добавлении рекомендации");
            }

            this.DialogResult = true;
        }


    }
}
