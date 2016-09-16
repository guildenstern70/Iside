using System.IO;
using System.Windows;
using System.Windows.Controls;
using ColorFont;
using Iside.Logic;
using Iside.Properties;
using IsideLogic;
using LLCryptoLib.Hash;
using LLCryptoLib.Utils;

namespace Iside
{
    /// <summary>
    ///     Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        private static string[] comboOptionsShell;
        private static string[] comboOptions1;
        private static string[] comboOptions2;
        private AvailableHash hash1;
        private AvailableHash hash2;

        // Options
        private RegistryOptions optionsInRegistry;
        private ShellIntegrationOption shellInt;
        private HexEnum style;

        public Options()
        {
            this.InitializeComponent();
            this.Init();
        }

        private void Init()
        {
            this.optionsInRegistry = new RegistryOptions();

            comboOptionsShell = SupportedHashAlgorithms.GetFastHashAlgorithms();
            comboOptions1 = SupportedHashAlgorithms.GetHashAlgorithms();
            comboOptions2 = SupportedHashAlgorithms.GetHashAlgorithmsWithNone();

            WPFUtils.GUI.ComboBoxItemsAdd(this.cboHash1, comboOptions1);
            WPFUtils.GUI.ComboBoxItemsAdd(this.cboHash2, comboOptions2);
            WPFUtils.GUI.ComboBoxItemsAdd(this.cboHashShell, comboOptions1);
            this.txtHashStyle.IsReadOnly = true;

            this.hash1 = Settings.Default.PrimaryHash;
            this.hash2 = Settings.Default.AlternativeHash;
            this.shellInt = new ShellIntegrationOption(this.optionsInRegistry.ShellIntegration);
            this.style = Settings.Default.HashStyle;

            this.SyncOptions();
        }

        private void SyncOptions()
        {
            FormWidth sizeForm = Settings.Default.SizeForm;

            // GUI Width
            if (sizeForm == FormWidth.SINGLE)
            {
                this.chkHalfSize.IsChecked = true;
            }
            else
            {
                this.chkHalfSize.IsChecked = false;
            }

            // Secondary Hash
            HashLogic.SyncHashCombo(this.hash2, this.cboHash2);
            // Primary Hash
            HashLogic.SyncHashCombo(this.hash1, this.cboHash1);

            // Shell Integration
            AvailableHash hashShell = (AvailableHash) this.optionsInRegistry.DefaultHash.RegistryValue;
            HashLogic.SyncHashCombo(hashShell, this.cboHashShell);

            // Style
            if (this.style == HexEnum.UNIX)
            {
                this.radStyleSpace.IsChecked = false;
                this.radStyleUnix.IsChecked = true;
                this.radStyleClassic.IsChecked = false;
                this.radStyleNetscape.IsChecked = false;
            }
            else if (this.style == HexEnum.SPACE)
            {
                this.radStyleSpace.IsChecked = true;
                this.radStyleUnix.IsChecked = false;
                this.radStyleClassic.IsChecked = false;
                this.radStyleNetscape.IsChecked = false;
            }
            else if (this.style == HexEnum.NETSCAPE)
            {
                this.radStyleSpace.IsChecked = false;
                this.radStyleUnix.IsChecked = false;
                this.radStyleClassic.IsChecked = false;
                this.radStyleNetscape.IsChecked = true;
            }
            else // default
            {
                this.radStyleSpace.IsChecked = false;
                this.radStyleUnix.IsChecked = false;
                this.radStyleClassic.IsChecked = true;
                this.radStyleNetscape.IsChecked = false;
            }

            // Fonts, colors and layout
            if (File.Exists(Config.FONTFILE))
            {
                System.Diagnostics.Debug.WriteLine("FontInfo serialization found. Deserializing.");
                FontInfo selectedFont = FontInfo.Deserialize(Config.FONTFILE);
                FontInfo.ApplyFont(this.txtHashStyle, selectedFont);
            }

            // ShellIntegration
            this.chkEnableFileIntegration.IsChecked = this.shellInt.FileIntegration;
            this.chkEnableMd5Association.IsChecked = this.shellInt.Md5FileAssociation;
            this.chkEnableFolderIntegration.IsChecked = this.shellInt.FolderShellIntegration;
        }

        private void SaveOptions()
        {
            this.hash1 = GetHashFromCombo(this.cboHash1);
            this.hash2 = GetHashFromCombo(this.cboHash2);

            Settings.Default.PrimaryHash = this.hash1;
            Settings.Default.AlternativeHash = this.hash2;

            // Half size
            FormWidth guiWidth;
            if (this.chkHalfSize.IsChecked.GetValueOrDefault())
            {
                guiWidth = FormWidth.SINGLE;
            }
            else
            {
                guiWidth = FormWidth.DOUBLE;
            }
            Settings.Default.SizeForm = guiWidth;

            // Save hash style
            if (this.radStyleUnix.IsChecked.GetValueOrDefault())
            {
                this.style = HexEnum.UNIX;
            }
            else if (this.radStyleSpace.IsChecked.GetValueOrDefault())
            {
                this.style = HexEnum.SPACE;
            }
            else if (this.radStyleNetscape.IsChecked.GetValueOrDefault())
            {
                this.style = HexEnum.NETSCAPE;
            }
            else
            {
                this.style = HexEnum.CLASSIC;
            }
            Settings.Default.HashStyle = this.style;

            // Fonts, colors and layout
            string fontPath = Path.Combine(WPFUtils.Core.AppDataPath, Config.FONTFILE);
            FontInfo selectedFont = FontInfo.GetControlFont(this.txtHashStyle);
            selectedFont.Serialize(fontPath);
            System.Diagnostics.Debug.WriteLine("Font serialized to " + fontPath);

            // Shell Integration
            AvailableHash hashShell = GetHashFromCombo(this.cboHashShell);
            this.optionsInRegistry.DefaultHash.RegistryValue = hashShell;
            this.shellInt.FileIntegration = this.chkEnableFileIntegration.IsChecked.GetValueOrDefault();
            this.shellInt.Md5FileAssociation = this.chkEnableMd5Association.IsChecked.GetValueOrDefault();
            this.shellInt.FolderShellIntegration = this.chkEnableFolderIntegration.IsChecked.GetValueOrDefault();
            this.shellInt.WriteOptionsToRegistry();

            Settings.Default.Save();
        }

        private static AvailableHash GetHashFromCombo(ComboBox cbo)
        {
            ComboBoxItem cbi = cbo.SelectedItem as ComboBoxItem;
            return GetSelectedHashCode(cbi.Content.ToString());
        }

        private static AvailableHash GetSelectedHashCode(string selected)
        {
            SupportedHashAlgo selectedAlgo = SupportedHashAlgoFactory.FromName(selected);
            return selectedAlgo.Id;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ColorFontDialog fontDlg = new ColorFontDialog();
            fontDlg.Owner = this;
            fontDlg.Font = FontInfo.GetControlFont(this.txtHashStyle);
            if (fontDlg.ShowDialog() == true)
            {
                FontInfo.ApplyFont(this.txtHashStyle, fontDlg.Font);
            }
        }

        private void ShowHashDescription(string selected)
        {
            SupportedHashAlgo sha = SupportedHashAlgoFactory.FromName(selected);
            this.txtDescription.Text = sha.Name;
            this.txtDescription.Text += "\r\n\r\n";
            this.txtDescription.Text += sha.Description;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.SaveOptions();
            this.DialogResult = true;
        }

        private void cboHash1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedHash = this.cboHash1.SelectedItem as ComboBoxItem;
            this.ShowHashDescription(selectedHash.Content.ToString());
        }

        private void cboHash2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedHash = this.cboHash2.SelectedItem as ComboBoxItem;
            this.ShowHashDescription(selectedHash.Content.ToString());
        }
    }
}