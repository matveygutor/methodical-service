using MaterialDesignThemes.Wpf;
using MethodicalService.Forms;
using MethodicalService.Service;
using MethodicalService.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MethodicalService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User User { get; set; }

        public MainWindow(User user)
        {
            User = user;
            InitializeComponent();
            DrawMenu();
        }

        internal void SwitchScreen(object sender)
        {
            var screen = ((UserControl)sender);
            if (screen != null)
            {
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(screen);
            }
        }

        private void DrawMenu()
        {
            if (User.Role == "deputy")
            {
                List<SubItem> updDevelopment = new()
                {
                    new SubItem("Поступление УПД", new JournalReceipt()),
                    new SubItem("Пед. нагрузка", new SubjectTeacher()),
                };
                ItemMenu thirdItem = new("УПД", updDevelopment, PackIconKind.Teacher);

                Menu.Children.Add(new ViewModel.MenuItem(thirdItem, this));
            }
            if (User.Role == "methodist")
            {
                List<SubItem> teachingLoad = new()
                {
                    new SubItem("Пед. нагрузка", new SubjectTeacher()),
                };
                ItemMenu secondItem = new("Пед. нагрузка", teachingLoad, PackIconKind.Teacher);
                Menu.Children.Add(new ViewModel.MenuItem(secondItem, this));
            }
        }
    }
}
