using Drawing.Models;
using Drawing.Services;
using System.Windows;
using System.Windows.Controls;

namespace Drawing
{
    /// <summary>
    /// Interaction logic for OpenWindow.xaml
    /// </summary>
    public partial class OpenWindow
    {
        private IStorageService _storageService = new SqliteStorageService();

        public Diagram SelectedDiagram { get; private set; }

        public OpenWindow()
        {
            InitializeComponent();

            Diagrams.ItemsSource = _storageService.GetAll();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDiagram != null)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("请选择需要操作的图形。");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDiagram != null)
            {
                if (MessageBox.Show($"确定要删除名为“{SelectedDiagram.Name}({SelectedDiagram.Id.ToUpperString()})”的图形吗？","警告",MessageBoxButton.YesNo,MessageBoxImage.Warning) 
                    == MessageBoxResult.Yes)
                {
                    var id = SelectedDiagram.Id;
                    _storageService.Delete(id);
                    Diagrams.ItemsSource = _storageService.GetAll();
                }
            }
            else
            {
                MessageBox.Show("请选择需要操作的图形。");
            }
        }

        private void Diagrams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Diagrams.SelectedIndex != -1)
            {
                SelectedDiagram = (Diagram)Diagrams.SelectedItem;
            }
            else
            {
                SelectedDiagram = null;
            }
        }
    }
}
