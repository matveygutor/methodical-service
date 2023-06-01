using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace MethodicalService
{
    public class SubItem
    {
        public SubItem(string name, PackIconKind icon, UserControl screen = null)
        {
            Name = name;
            Screen = screen;
            SubIcon = icon;
        }
        public string Name { get; private set; }
        public UserControl Screen { get; private set; }
        public PackIconKind SubIcon { get; private set; }
    }
}