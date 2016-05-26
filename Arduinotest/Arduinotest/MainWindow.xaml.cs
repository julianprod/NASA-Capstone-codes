using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;

namespace Arduinotest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        /// <summary> Current status text to display </summary>
        private string statusText = null;

        

        SerialPort sp = new SerialPort();
        public MainWindow()
        {
           


            this.InitializeComponent();

        }

        private void test(object sender, RoutedEventArgs e)
        {
            sp.Write("Z");
        }

        private void FORWARD1(object sender, RoutedEventArgs e)
        {            
            sp.Write("A");   
        }
        
        private void BACK1(object sender, RoutedEventArgs e)
        {
            sp.Write("B");
        }

        private void RIGHT1(object sender, RoutedEventArgs e)
        {

            sp.Write("C");
        }

        private void LEFT1(object sender, RoutedEventArgs e)
        {
            
            sp.Write("D");
            
        }

        private void BACKRIGHT1(object sender, RoutedEventArgs e)
        {
            sp.Write("E");
        }
        

        private void BACKLEFT1(object sender, RoutedEventArgs e)
        {
            sp.Write("F");
        }

        private void t360(object sender, RoutedEventArgs e)
        {
            sp.Write("G");
        }
        
        private void right360(object sender, RoutedEventArgs e)
        {
            sp.Write("H");
        }

        private void left360(object sender, RoutedEventArgs e)
        {
            sp.Write("I");
        }

        private void align(object sender, RoutedEventArgs e)
        {
            sp.Write("J");
        }

        private void sup(object sender, RoutedEventArgs e)
        {
            sp.Write("K");
        }

        private void sdown(object sender, RoutedEventArgs e)
        {
            sp.Write("L");
        }
        
        private void eup(object sender, RoutedEventArgs e)
        {
            sp.Write("M");
        }

        private void edown(object sender, RoutedEventArgs e)
        {
            sp.Write("N");
        }

        private void wup(object sender, RoutedEventArgs e)
        {
            sp.Write("O");
        }

        private void wdown(object sender, RoutedEventArgs e)
        {
            sp.Write("P");
        }

        private void gopen(object sender, RoutedEventArgs e)
        {
            sp.Write("Q");
        }

        private void gclose(object sender, RoutedEventArgs e)
        {
            sp.Write("R");
        }

        private void sout(object sender, RoutedEventArgs e)
        {
            sp.Write("S");
        }

        private void sin(object sender, RoutedEventArgs e)
        {
            sp.Write("T");
        }

       



        
    

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String portName = comportno.Text;
                sp.PortName = "COM8";
                sp.BaudRate = 9600;
                sp.Open();
                status.Text = "Connected";

                sp.Write("b");
            }
            catch (Exception)
            {

                MessageBox.Show("Please give a valid port number or check your connection");
            }
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sp.Close();
                status.Text = "Disconnected";
            }
            catch (Exception)
            {

                MessageBox.Show("First Connect and then disconnect");
            }
        }

        private void BACKLEFT_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
        
    

