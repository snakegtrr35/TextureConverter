using Microsoft.Win32;
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

namespace TextureConverter
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage bitmap = null;

        private Point _start;

        public string FileName = "";

        public MainWindow()
        {
            InitializeComponent();

            bitmap = new BitmapImage(); // デコードされたビットマップイメージのインスタンスを作る。
        }

        private void event_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy; // ドラッグ中のカーソルを変える。
        }

        private void event_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) // ドロップされたものがファイルかどうか確認する。
            {
                FileName = ((string[])e.Data.GetData(DataFormats.FileDrop))[0]; // ドロップされた最初のファイルのファイル名を得る。

                if(null != bitmap)
                {
                    bitmap = null;
                    bitmap = new BitmapImage(); // デコードされたビットマップイメージのインスタンスを作る。
                }

                LoadTexture(bitmap, FileName);
            }
        }

        private void FileDialog()
        {
            // ダイアログのインスタンスを生成
            var dialog = new OpenFileDialog();

            // ファイルの種類を設定
            dialog.Filter = "画像ファイル (*.png)|*.png|(*.jpg)|*.jpg|(*.bmp)|*.bmp|(*.gif)|*.gif|(*.tiff)|*.tiff|全てのファイル (*.*)|*.*";

            // ダイアログを表示する
            if (dialog.ShowDialog() == true)
            {
                if (null != bitmap)
                {
                    bitmap = null;
                    bitmap = new BitmapImage(); // デコードされたビットマップイメージのインスタンスを作る。
                }

                FileName = dialog.FileName;

                LoadTexture(bitmap, FileName);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuitem = (MenuItem)sender; // オブジェクトをMenuItemクラスのインスタンスにキャストする。
            string header = menuitem.Header.ToString(); // Headerプロパティを取り出して、文字列に変換する。
            string tag = menuitem.Tag.ToString(); // Tagプロパティを取り出して、文字列に変換する。

            switch (tag) // Tag文字列毎の処理
            {
                case "file":
                    FileDialog();
                    break;

                case "file_close":
                    image.Source = null;
                    break;

                case "encode":
                    Open_EncodeWindow();
                    break;

                case "close":
                    this.Close();
                    break;

                default:
                    break;
            }
        }


        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // スケールの値を変えることでホイールを動かした時の拡大率を制御できます
            const float scale = 1.2f;

            var matrix = image.RenderTransform.Value;
            if (e.Delta > 0)
            {
                // 拡大処理
                matrix.ScaleAt(scale, scale, e.GetPosition(this).X, e.GetPosition(this).Y);
            }
            else
            {
                // 縮小処理
                matrix.ScaleAt(1.0f / scale, 1.0f / scale, e.GetPosition(this).X, e.GetPosition(this).Y);
            }

            image.RenderTransform = new MatrixTransform(matrix);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            image.CaptureMouse();
            _start = e.GetPosition(Border1);
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (image.IsMouseCaptured)
            {
                var matrix = image.RenderTransform.Value;

                Vector v = _start - e.GetPosition(Border1);
                matrix.Translate(-v.X, -v.Y);
                image.RenderTransform = new MatrixTransform(matrix);
                _start = e.GetPosition(Border1);
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            image.ReleaseMouseCapture();
        }



        private void LoadTexture(BitmapImage bitmap, string filename)
        {
            try
            {
                bitmap.BeginInit();

                if (null != bitmap.UriSource) bitmap.UriSource = null;
                bitmap.UriSource = new Uri(filename); // ビットマップイメージのソースにファイルを指定する。

                bitmap.EndInit();
                image.Source = bitmap; // Imageコントロールにバインディングする。
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Open_EncodeWindow()
        {
            if(null != bitmap.UriSource)
            {
                EncodeWindow encode_window = new EncodeWindow(FileName);
                encode_window.Owner = this;
                encode_window.Show();
            }
            else
            {
                MessageBox.Show("画像ファイルが読み込まれていません");
            }
        }
    }
}
