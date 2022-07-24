using Microsoft.Win32;
using Paint.Controller;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
   
    public partial class MainWindow : Window
    {
        InkCanvas inkCanvas;
        ControllerColor controllerColor;
        public MainWindow()
        {
            inkCanvas = new InkCanvas();
            InitializeComponent();
        }
        
        private void Red_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorFunc(255, 0, 0);
        }

        private void Green_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorFunc(0, 255, 0);
        }

        private void Blue_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorFunc(0, 0, 255);
        }

        private void Black_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorFunc();
        }

        private void UserColor_Button_Click(object sender, RoutedEventArgs e)
        {
            byte byteValue;
            bool succes = byte.TryParse(this.textBoxR.Text, out byteValue);
            controllerColor = new ControllerColor(TryParseByte(textBoxR.Text),TryParseByte(textBoxG.Text),
                                                        TryParseByte(textBoxB.Text));
            ColorFunc(controllerColor.colorRGB.Red, controllerColor.colorRGB.Green, controllerColor.colorRGB.Blue);
        }
        /// <summary>
        /// изменение  размера кисточки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Brush_Size_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        { 
                inkcanavas.DefaultDrawingAttributes.Height = slider.Value;
                inkcanavas.DefaultDrawingAttributes.Width = slider.Value;
        }
       /// <summary>
       /// очищений поверхности
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void Clear_Surface_Button_Click(object sender, RoutedEventArgs e)
        {
            inkcanavas.Strokes.Clear();
            myImage.Source = null;

        }
        /// <summary>
        /// при нажатии сохраняет изображение в формате .png
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var saveDlg = new SaveFileDialog
            {
                FileName = "Masterpiece",
                DefaultExt = ".png",
                Filter = "PNG (.png)|*.png"
            };
            if (saveDlg.ShowDialog() == true)
            {
                SaveInkCanvas(inkcanavas, 96, saveDlg.FileName);
            }

        }
        
        private void SaveInkCanvas(InkCanvas _inkcanvas, int dpi, string filename)
        {
            var width = _inkcanvas.ActualWidth;
            var height = _inkcanvas.ActualHeight;
            var size = new Size(width, height);
            _inkcanvas.Measure(size);

            var rtb = new RenderTargetBitmap(
                (int)width, (int)height,
                dpi, //dpi x 
                dpi, //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                );
            rtb.Render(_inkcanvas);

            SaveAsPng(rtb, filename);
        }

        private static void SaveAsPng(RenderTargetBitmap bmp, string filename)
        {
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bmp));

            using (FileStream stm = File.Create(filename))
            {
                enc.Save(stm);
            }
        }

        /// <summary>
        /// Меняет кисточку на ластик
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        /// <summary>
        /// добовляет выделение элементов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            if (inkcanavas.EditingMode != InkCanvasEditingMode.Select)
            {
                inkcanavas.EditingMode = InkCanvasEditingMode.Select;
            }
            else{ inkcanavas.EditingMode = InkCanvasEditingMode.Ink; }
        }
        private void laundry_Click(object sender, RoutedEventArgs e)
        {

            if (inkcanavas.EditingMode != InkCanvasEditingMode.EraseByPoint)
            {
                inkcanavas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            }
            else { inkcanavas.EditingMode = InkCanvasEditingMode.Ink; }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
       
        /// <summary>
        /// Открывает изображения формата .png  в данном проектк
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".png";
            dialog.Filter = "PNG (.png)|*.png";
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(dialog.FileName, UriKind.Relative);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                Image img = new Image();
                myImage.Source = bitmap;
            }
        }
        private byte TryParseByte(string s)
        {
            bool succes = byte.TryParse(s, out byte byteValue);
            if (succes) { return byteValue; }
            MessageBox.Show("Значение RGB может быть в диапазоне [0,255]");
            return 0;
        }
        private void ColorFunc(byte r = 0, byte g = 0, byte b = 0)
        {
            inkcanavas.EditingMode = InkCanvasEditingMode.Ink;
            this.inkcanavas.DefaultDrawingAttributes.Color = System.Windows.Media.Color.FromRgb(r, g, b);
        }

        
    }
}
