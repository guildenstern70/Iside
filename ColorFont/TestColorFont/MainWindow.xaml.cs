using System.Windows;
using ColorFont;

namespace TestColorFont
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ColorFont.ColorFontDialog fntDialog = new ColorFont.ColorFontDialog();
            fntDialog.Owner = this;
            fntDialog.Font = FontInfo.GetControlFont(this.txtText);
            if (fntDialog.ShowDialog() == true)
            {
                FontInfo selectedFont = fntDialog.Font;
                
                if (selectedFont != null)
                {
                    FontInfo.ApplyFont(this.txtText, selectedFont);
                }
            }
        }
    }
}
