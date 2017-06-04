using Drawing.Models;
using Drawing.Services;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Drawing
{
    /// <summary>
    /// Interaction logic for SaveWindow.xaml
    /// </summary>
    public partial class SaveWindow
    {
        private IStorageService _storageService = new SqliteStorageService();

        private List<ShapeBase> _shapes;

        public SaveWindow(List<ShapeBase> shapes)
        {
            InitializeComponent();

            _shapes = shapes;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var diagram = _storageService.GetByName(Name.Text.Trim());
            if (diagram != null)
            {
                if (MessageBox.Show($"已经存在名为：'{Name.Text.Trim()}'的图形，确定覆盖现有图形？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    diagram.Shapes = _shapes;
                    _storageService.Update(diagram);
                    DialogResult = true;
                    Close();
                    MessageBox.Show("更新完成！");
                }
                return;
            }

            diagram = new Diagram
            {
                Id = Guid.NewGuid(),
                Name = Name.Text.Trim(),
                Shapes = _shapes
            };
            _storageService.Add(diagram);
            DialogResult = true;
            Close();
            MessageBox.Show("保存完成！");
        }
    }
}
