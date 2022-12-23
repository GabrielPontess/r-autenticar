using RAutenticar.Shared;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;

using Path = System.IO.Path;
using System.Windows.Media;

namespace RAutenticar.Views
{

    public partial class Captura : Window, INotifyPropertyChanged
    {
        public ObservableCollection<FilterInfo> VideoDevices { get; set; }

        public FilterInfo CurrentDevice
        {
            get { return _currentDevice; }
            set { _currentDevice = value; this.OnPropertyChanged("CurrentDevice"); }
        }
        private FilterInfo _currentDevice;

        private IVideoSource _videoSource;
        private bool CapturaRealizada { get; set; } = false;

        public Captura()
        {
            InitializeComponent();
            this.DataContext = this;
            GetVideoDevices();
            this.Closing += MainWindow_Closing;
            StartCamera();
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopCamera();
        }

        private void btnCapturar_Click(object sender, RoutedEventArgs e)
        {
            if (_videoSource.IsRunning)
            {
                Global.Instance.DadosValidacao.DataCaptura = DateTime.Now;
                Global.Instance.DadosValidacao.HoraCaptura = DateTime.Now;
                Global.Instance.DadosValidacao.DispositivoCaptura = String.Format("{0}", CurrentDevice.Name);

                CapturaRealizadaLabel.Content = "CAPTURA REALIZADA COM SUCESSO!";
                CapturaRealizadaLabel.Foreground = new SolidColorBrush(Colors.Green);
                DataValidacao.Content = $"DATA DA CAPTURA: {string.Format("{0:dd/MM/yyyy}", Global.Instance.DadosValidacao.DataCaptura)}";
                HorarioValidacao.Content = $"HORÁRIO DA CAPTURA: {string.Format("{0:HH:mm}", Global.Instance.DadosValidacao.DataCaptura)}";
                DispositivoValidacao.Content = $"DISPOSITIVO DA CAPTURA: {Global.Instance.DadosValidacao.DispositivoCaptura}";
                PhotoScreenView.Source = VideoScreenView.Source;

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapImage)PhotoScreenView.Source));
                var filePath = Path.GetTempPath().ToString() + "Captura_R-Autenticar" + ".png";

                using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    encoder.Save(fileStream);
                }

                CapturaRealizada = true;
                btnCapturar.Content = "Recapturar";
                btnValidar.IsEnabled = CapturaRealizada;

                Global.Instance.DadosValidacao.Atendente.Imagem = System.IO.File.ReadAllBytes(filePath);
                Global.Instance.DadosValidacao.Atendente.ImagemBase64 = Convert.ToBase64String(Global.Instance.DadosValidacao.Atendente.Imagem);
            }
            else
            {
                MessageBox.Show("A câmera precisa estar ligada para captura!", "Falha ao realizar captura", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnValidar_Click(object sender, RoutedEventArgs e)
        {
            Resultado resultado = new Resultado();
            StopCamera();
            resultado.Show();
            this.Close();
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                BitmapImage bi;
                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    bi = bitmap.ToBitmapImage();
                }
                bi.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(delegate { VideoScreenView.Source = bi; }));
            }
            catch (Exception exc)
            {
                MessageBox.Show("Erro na captura dos frames em VideoSource:\n" + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StopCamera();
            }
        }

        private void GetVideoDevices()
        {
            VideoDevices = new ObservableCollection<FilterInfo>();
            foreach (FilterInfo filterInfo in new FilterInfoCollection(FilterCategory.VideoInputDevice))
            {
                VideoDevices.Add(filterInfo);
            }
            if (VideoDevices.Any())
            {
                CurrentDevice = VideoDevices[0];
            }
            else
            {
                MessageBox.Show("Nenhum dispositivo de captura de vídeo foi encontrado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartCamera()
        {
            if (CurrentDevice != null)
            {
                btnValidar.IsEnabled = CapturaRealizada;
                _videoSource = new VideoCaptureDevice(CurrentDevice.MonikerString);
                _videoSource.NewFrame += VideoSource_NewFrame;
                _videoSource.Start();
            }
        }

        private void StopCamera()
        {
            BitmapImage Background = new BitmapImage();
            VideoScreenView.Source =  Background;
            if (_videoSource != null && _videoSource.IsRunning)
            {
                cbDispositivos.IsEnabled = true;
                btnValidar.IsEnabled = CapturaRealizada;
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= new NewFrameEventHandler(VideoSource_NewFrame);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }


    }
}
