using System.Windows;

namespace DijkstraAlgorithm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Graph.Graph mMainGraph;
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            mMainGraph = new Graph.Graph();
        }
    }
}
