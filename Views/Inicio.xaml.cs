using RAutenticar.Models;
using RAutenticar.Views;
using RAutenticar.Shared;
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
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Diagnostics;
using System.Drawing;


namespace RAutenticar.Views
{
    public partial class Inicio : Window
    {
        public Inicio()
        {
            InitializeComponent();
        }
        private void ButtonIniciar_Click(object sender, RoutedEventArgs e)
        {
            Global.Instance.DadosValidacao.Atendente.Cpf = textCpf.Text;

            Captura Captura = new Captura();
            Captura.Show();
            this.Close();

        }

        private void textCpf_TextChanged(object sender, TextChangedEventArgs e)
        {
           var count = textCpf.Text.Length;

        }
    }
}
