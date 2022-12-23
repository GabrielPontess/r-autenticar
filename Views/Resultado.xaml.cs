using RAutenticar.Models;
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
using System.Windows.Shapes;
using RAutenticar.Views;
using RAutenticar.Shared;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Image = System.Drawing.Image;
using AForge.Controls;

namespace RAutenticar.Views
{
    /// <summary>
    /// Lógica interna para Resultado.xaml
    /// </summary>
    public partial class Resultado : Window
    {
        public Resultado()
        {
            InitializeComponent();
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(Global.Instance.DadosValidacao.Atendente.Imagem);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
            ImageSource imgSrc = biImg as ImageSource;
            PhotoAtendenteView.Source = imgSrc;
            TipoSolicitanteResultado.Content = "ATENDENTE";
            CpfSolicitanteResultado.Content = $"CPF: {Global.Instance.DadosValidacao.Atendente.Cpf}";
            DispositivoResultado.Content = $"DISPOSITIVO DA CAPTURA: {Global.Instance.DadosValidacao.DispositivoCaptura}";
            DataValidacaoResultado.Content = $"DATA DA CAPTURA: {string.Format("{0:dd/MM/yyyy}", Global.Instance.DadosValidacao.DataCaptura)}";
            HorarioValidacaoResultado.Content = $"HORÁRIO DA CAPTURA: {string.Format("{0:HH:mm}", Global.Instance.DadosValidacao.DataCaptura)}";
            ImagemBase64TextBox.Text = Global.Instance.DadosValidacao.Atendente.ImagemBase64;
        }

        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }   
}
