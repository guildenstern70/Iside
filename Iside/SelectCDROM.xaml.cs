using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Iside
{
    public class DriveItem
    {
        public ImageSource DriveImage { get; set; }
        public string DriveName { get; set; }
        public DirectoryInfo DriveDir { get; set; }
    }

    /// <summary>
    ///     Interaction logic for SelectCDROM.xaml
    /// </summary>
    public partial class SelectCDROM : Window
    {
        public SelectCDROM()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.Drives = new ObservableCollection<DriveItem>();
            this.InitializeDrives();
        }

        public ObservableCollection<DriveItem> Drives { get; }

        /// <summary>
        ///     Return selected drive
        /// </summary>
        public DirectoryInfo SelectedDrive
        {
            get
            {
                DriveItem di = this.cboDrive.SelectedItem as DriveItem;
                return di.DriveDir;
            }
        }

        private void AddItem(DriveInfo dInfo)
        {
            DriveItem di = new DriveItem();

            BitmapImage bitMap = new BitmapImage();
            bitMap.BeginInit();
            if (dInfo.DriveType == DriveType.CDRom)
            {
                bitMap.UriSource = new Uri("pack://application:,,,/Iside;component/Resources/disc-media_24.png");
            }
            else
            {
                bitMap.UriSource = new Uri("pack://application:,,,/Iside;component/Resources/USB1.png");
            }
            bitMap.EndInit();
            di.DriveImage = bitMap;
            di.DriveDir = new DirectoryInfo(dInfo.Name);
            di.DriveName = dInfo.Name;
            System.Diagnostics.Debug.WriteLine("Adding " + di.DriveName);
            this.Drives.Add(di);
        }

        private void InitializeDrives()
        {
            int firstCDIndex = -1;
            int index = -1;

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Removable)
                {
                    this.AddItem(drive);
                    index++;
                }
                else if (drive.DriveType == DriveType.CDRom)
                {
                    this.AddItem(drive);
                    index++;
                    firstCDIndex = index;
                }
            }

            if (firstCDIndex >= 0)
            {
                this.cboDrive.SelectedIndex = firstCDIndex;
            }
            else
            {
                this.cboDrive.SelectedIndex = 0;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}