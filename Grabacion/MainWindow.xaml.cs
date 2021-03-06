﻿using System;
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

using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Microsoft.Win32;
using System.Windows.Threading;
using NAudio.Dsp;
using System.Threading;




namespace Grabacion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WaveIn waveIn;
        WaveFormat formato;
        WaveFileWriter writer;
        WaveOutEvent output;
        AudioFileReader reader;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            waveIn = new WaveIn();
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);
            formato = waveIn.WaveFormat;

            waveIn.DataAvailable += OnDataAvailable;
            waveIn.RecordingStopped += OnRecordingStopped;

            writer =
                new WaveFileWriter("sonido2.wav", formato);

            waveIn.StartRecording();
        }

        void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            writer.Dispose();

        }

        void OnDataAvailable (object sender, WaveInEventArgs e)
        {

            byte[] buffer = e.Buffer;
            int bytesGrabados = e.BytesRecorded;


            double acumulador = 0;
            double numMuestras = bytesGrabados / 2;
            int exponente = 1;
            int numeroMuestrasComplejas = 0;
            int bitsMaximos = 0;

            do //1,200
            {
                bitsMaximos = (int) Math.Pow(2, exponente);
                exponente++;
            } while (bitsMaximos < numMuestras);

            //bitsMaximos = 2048
            //exponente = 12

            //numeroMuestrasComplejas = 1024
            //exponente = 10

            exponente -= 2;
            numeroMuestrasComplejas = bitsMaximos / 2;

            Complex[] muestrasComplejas =
                new Complex[numeroMuestrasComplejas];

            for (int i=0; i < bytesGrabados; i+=2)
            {
                // byte i =  0 1 1 0 0 1 1 1
                //byte i+1 = 0 0 0 0 0 0 0 0 0 1 1 0 0 1 1 1
                // or      = 0 1 1 0 0 1 1 1 0 1 1 0 0 1 1 1
                short muestra =
                        (short)Math.Abs((buffer[i + 1] << 8)|buffer[i]);
                //lblMuestra.Text = muestra.ToString();
                //sldVolumen.Value = (double)muestra;

                float muestra32bits = (float)muestra / 32768.0f;
                sldVolumen.Value = Math.Abs(muestra32bits);

                if (i / 2 < numeroMuestrasComplejas)
                {
                    muestrasComplejas[i / 2].X = muestra32bits;
                }
                //acumulador += muestra;
                //numMuestras++;
            }
            //double promedio = acumulador / numMuestras;
            //sldVolumen.Value = promedio;
            //writer.Write(buffer, 0, bytesGrabados);

            FastFourierTransform.FFT(true, exponente, muestrasComplejas);
            float[] valoresAbsolutos = 
                new float[muestrasComplejas.Length];
            for(int i=0; i <muestrasComplejas.Length; i++)
            {
                valoresAbsolutos[i] = (float)
                    Math.Sqrt((muestrasComplejas[i].X * muestrasComplejas[i].X) +
                    (muestrasComplejas[i].Y * muestrasComplejas[i].Y));

            }

            int indiceMaximo =
                valoresAbsolutos.ToList().IndexOf(
                    valoresAbsolutos.Max());

            float frecuenciaFundamental =
                (float)(indiceMaximo * waveIn.WaveFormat.SampleRate) / (float)valoresAbsolutos.Length;

            lblFrecuencia.Text = frecuenciaFundamental.ToString();

            if (frecuenciaFundamental > 100 && frecuenciaFundamental < 199)
            {
                lblFrecuencia.Text = "A";
            }
            else if (frecuenciaFundamental > 200 && frecuenciaFundamental < 399)
            {
                lblFrecuencia.Text = "B";
            }
            else if (frecuenciaFundamental > 300 && frecuenciaFundamental < 399)
            {
                lblFrecuencia.Text = "C";
            }
            else if (frecuenciaFundamental > 400 && frecuenciaFundamental < 499)
            {
                lblFrecuencia.Text = "D";
            }
            else if (frecuenciaFundamental > 500 && frecuenciaFundamental < 599)
            {
                lblFrecuencia.Text = "E";
            }
            else if (frecuenciaFundamental > 600 && frecuenciaFundamental < 699)
            {
                lblFrecuencia.Text = "F";
            }
            else if (frecuenciaFundamental > 700 && frecuenciaFundamental < 799)
            {
                lblFrecuencia.Text = "G";
            }
            else if (frecuenciaFundamental > 800 && frecuenciaFundamental < 899)
            {
                lblFrecuencia.Text = "H";
            }
            else if (frecuenciaFundamental > 900 && frecuenciaFundamental < 999)
            {
                lblFrecuencia.Text = "I";
            }
            else if (frecuenciaFundamental > 1000 && frecuenciaFundamental < 1099)
            {
                lblFrecuencia.Text = "J";
            }
            else if (frecuenciaFundamental > 1100 && frecuenciaFundamental < 1199)
            {
                lblFrecuencia.Text = "K";
            }
            else if (frecuenciaFundamental > 1200 && frecuenciaFundamental < 1299)
            {
                lblFrecuencia.Text = "L";
            }
            else if (frecuenciaFundamental > 1300 && frecuenciaFundamental < 1399)
            {
                lblFrecuencia.Text = "M";
            }
            else if (frecuenciaFundamental > 1400 && frecuenciaFundamental < 1499)
            {
                lblFrecuencia.Text = "N";
            }
            else if (frecuenciaFundamental > 1500 && frecuenciaFundamental < 1599)
            {
                lblFrecuencia.Text = "O";
            }
            else if (frecuenciaFundamental > 1600 && frecuenciaFundamental < 1699)
            {
                lblFrecuencia.Text = "P";
            }
            else if (frecuenciaFundamental > 1700 && frecuenciaFundamental < 1799)
            {
                lblFrecuencia.Text = "Q";
            }
            else if (frecuenciaFundamental > 1800 && frecuenciaFundamental < 1899)
            {
                lblFrecuencia.Text = "R";
            }
            else if (frecuenciaFundamental > 1900 && frecuenciaFundamental < 1999)
            {
                lblFrecuencia.Text = "S";
            }
            else if (frecuenciaFundamental > 2000 && frecuenciaFundamental < 2099)
            {
                lblFrecuencia.Text = "T";
            }
            else if (frecuenciaFundamental > 2100 && frecuenciaFundamental < 2199)
            {
                lblFrecuencia.Text = "U";
            }
            else if (frecuenciaFundamental > 2200 && frecuenciaFundamental < 2299)
            {
                lblFrecuencia.Text = "V";
            }
            else if (frecuenciaFundamental > 2300 && frecuenciaFundamental < 2399)
            {
                lblFrecuencia.Text = "W";
            }
            else if (frecuenciaFundamental > 2400 && frecuenciaFundamental < 2499)
            {
                lblFrecuencia.Text = "X";
            }
            else if (frecuenciaFundamental > 2500 && frecuenciaFundamental < 2599)
            {
                lblFrecuencia.Text = "Y";
            }
            else if (frecuenciaFundamental > 2600 && frecuenciaFundamental < 2699)
            {
                lblFrecuencia.Text = "Z";
            }
            else if (frecuenciaFundamental > 2700 && frecuenciaFundamental < 2799)
            {
                lblFrecuencia.Text = " ";
            }

               
        }

        private void btnDetener_Click(object sender, RoutedEventArgs e)
        {
            waveIn.StopRecording();
        }

        

        

        private void button_Click(object sender, RoutedEventArgs e)
        {
            output = new WaveOutEvent();
            reader = new AudioFileReader("sonido2.wav");
            output.Init(reader);
            output.Play();

        }
    }
}
