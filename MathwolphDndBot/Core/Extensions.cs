using System;
using System.Windows;

namespace MathwolphDndBot.Core
{
    internal class Extensions
    {
        public static readonly DependencyProperty Icon = 
            DependencyProperty.RegisterAttached("Icon", typeof(string), typeof(Extensions), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty Label =
            DependencyProperty.RegisterAttached("Label", typeof(string), typeof(Extensions), new PropertyMetadata(default(string)));

        private static void OnProductTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static void SetIcon(UIElement element, string value)
        {
            element.SetValue(Icon, value);
        }

        public static string GetIcon(UIElement element)
        {
            return (string)element.GetValue(Icon);
        }

        public static void SetLabel(UIElement element, string value) 
        {
            element.SetValue(Label, value);
        }

        public static string GetLabel(UIElement element) 
        {
            return (string)element.GetValue(Label);
        }
    }
}
