using System;
using System.Windows;
using System.Windows.Controls;

namespace MethodicalService.ViewModel
{
    /// <summary>
    /// Логика взаимодействия для MenuItem.xaml
    /// </summary>
    public partial class MenuItem : UserControl
    {
        private MainWindow _context;

        public MenuItem(ItemMenu itemMenu, MainWindow context)
        {
            InitializeComponent();

            _context = context;

            ExpanderMenu.Visibility = itemMenu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
            ListViewItemMenu.Visibility = itemMenu.SubItems == null ? Visibility.Visible : Visibility.Collapsed;

            this.DataContext = itemMenu;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _context.SwitchScreen(((SubItem)((ListView)sender).SelectedItem).Screen);
        }
    }
}
