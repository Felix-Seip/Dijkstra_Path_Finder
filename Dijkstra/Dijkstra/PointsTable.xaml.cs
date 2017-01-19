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

namespace Dijkstra
{
    /// <summary>
    /// Interaction logic for PointsTable.xaml
    /// </summary>
    public partial class PointsTable : Window
    {
        List<Node> NodeList = null;
        public PointsTable(List<Node> nodeList)
        {
            InitializeComponent();

            NodeList = nodeList;
            LoadCollectionData();
            pointsListData.AutoGenerateColumns = false;
        }

        public List<Node> LoadCollectionData()
        {
            //for(int i = 0; i < NodeList.Count; i++)
            //{
            //    pointsListData.Columns.Add()
            //}
            return NodeList;
        }
    }
}
