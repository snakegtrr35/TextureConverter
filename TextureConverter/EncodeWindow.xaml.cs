using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Microsoft.Win32;

using System.Windows.Forms;

namespace TextureConverter
{
    public class PrefList
    {
        public ObservableCollection<string> Data { get; }

        public PrefList()
        {
            Data = new ObservableCollection<string>();
            Data.Add("BC1");
            Data.Add("BC1 SRGB");
            Data.Add("BC2");
            Data.Add("BC2 SRGB");
            Data.Add("BC3");
            Data.Add("BC3 SRGB");
            Data.Add("BC4");
            Data.Add("BC4 SRGB");
            Data.Add("BC5");
            Data.Add("BC5 SRGB");
            Data.Add("BC6H");
            Data.Add("BC6H SRGB");
            Data.Add("BC7");
            Data.Add("BC7 SRGB");
        }
    }

    /// <summary>
    /// EncodeWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class EncodeWindow : Window
    {
        private string InputFile = "";          // 使用する画像ファイル
        private string FileName = "";           // 出力ファイル名
        private string Folder = "";             // 出力先
        private string OutputType = "";         // BCの種類
        private string EncodeType = "";         // エンコードの方法


        public EncodeWindow(string file)
        {
            InitializeComponent();

            InputFile = file;

            ComboBox.Items.Add("BC1_UNORM");
            ComboBox.Items.Add("BC1_UNORM_SRGB");
            ComboBox.Items.Add("BC2_UNORM");
            ComboBox.Items.Add("BC2_UNORM_SRGB");
            ComboBox.Items.Add("BC3_UNORM");
            ComboBox.Items.Add("BC3_UNORM_SRGB");
            ComboBox.Items.Add("BC4_UNORM");
            ComboBox.Items.Add("BC4_UNORM_SRGB");
            ComboBox.Items.Add("BC5_UNORM");
            ComboBox.Items.Add("BC5_UNORM_SRGB");
            ComboBox.Items.Add("BC6H_UF16");
            ComboBox.Items.Add("BC6H_SF16");
            ComboBox.Items.Add("BC7_UNORM");
            ComboBox.Items.Add("BC7_UNORM_SRGB");
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var radioButton = (System.Windows.Controls.RadioButton)sender;

            // エンコード方法の設定
            if("GPU 使用" == radioButton.Content.ToString())
            {
                EncodeType = "gpu";
            }
            else
            {
                EncodeType = "nogpu";
            }
        }

        private void FD(object sender, RoutedEventArgs e)
        {
            FolderDialog();
        }

        private void Run(object sender, RoutedEventArgs e)
        {
            var proc = new System.Diagnostics.Process();

            proc.StartInfo.FileName = "date\\texconv.exe";

            proc.StartInfo.Arguments = "-f " + OutputType + " -srgb " + EncodeType + " -o " + Folder + " -y " + InputFile;

            proc.Start();

            proc.WaitForExit();

            System.Windows.MessageBox.Show("終了しました");

            this.Close();
        }

        // <summary>
        // フォルダダイアログボックスを表示
        // </summary>
        private void FolderDialog()
        {
            // ダイアログのインスタンスを生成
            var browser = new System.Windows.Forms.FolderBrowserDialog();

            browser.Description = "フォルダーを選択してください";

            // ダイアログを表示する
            if (browser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Folder = browser.SelectedPath;

                this.OutputFolder.Text = Folder;

                var i = 0;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OutputType = ComboBox.SelectedValue.ToString();
        }
    }
}
