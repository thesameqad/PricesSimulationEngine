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
using System.ComponentModel;
using PricesSimulator.Business;
using System.Threading;

namespace PricesSimulator
{    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker simulationWorker = new BackgroundWorker();

        private bool buttonState = false;
        public MainWindow()
        {
            InitializeComponent();
            simulationWorker.WorkerSupportsCancellation = true;
            simulationWorker.DoWork += simulation_DoWork;
            simulationWorker.WorkerReportsProgress = true;
            simulationWorker.ProgressChanged += simulationWorker_ProgressChanged;
            listViewPrices.ItemsSource = SimulatorEngine.Tickers;
        }

        void simulationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            listViewPrices.ItemsSource = SimulatorEngine.Tickers;
            listViewPrices.Items.Refresh();
        }

        private void simulation_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                if (simulationWorker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    SimulatorEngine.Simulate();
                    System.Threading.Thread.Sleep(500);
                    simulationWorker.ReportProgress(10);
                }
            }
        }

        private void runClick(object sender, RoutedEventArgs e)
        {
            buttonState = !buttonState;
            if (buttonState)
            {
                ((Button)sender).Content = "Stop Engine";
                simulationWorker.RunWorkerAsync();
            }
            else
            {
                ((Button)sender).Content = "Start Engine";
                simulationWorker.CancelAsync();
            }
        }

        private void subscribeClick(object sender, RoutedEventArgs e)
        {
            SimulatorEngine.Subscribe(tickerName.Text);
            listViewPrices.ItemsSource = SimulatorEngine.Tickers;
            listViewPrices.Items.Refresh();
        }        
    }
}
