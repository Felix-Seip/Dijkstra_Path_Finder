using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Dijkstra
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random random;
        public List<Node> nodeList;
        private List<string> cityNames;
        bool leftCtrlKeyPressed = false;
        private bool startIsSelected = false;
        private Node startEdgeNode = null;
        private Node currentSelectedNode = null;
        private Point rightClickButtonDownPosition;

        private Node startNode = null;
        private Node endNode = null;
        private double edgeWeight = 1;
        private int nodeCount = 0;
        public MainWindow()
        {
            InitializeComponent();
            random = new Random();
            WindowState = WindowState.Maximized;
            nodeList = new List<Node>();
            cityNames = new List<string>();
            //MessageBox.Show(File.ReadAllLines(Properties.Resources.City_Names).ToString());
            string[] tempNames = Properties.Resources.City_Names.Split('\n');
            for (int i = 0; i < tempNames.Length; i++)
                cityNames.Add(tempNames[i].Replace("\r", ""));
        }

        private void AddNodes(object sender, MouseButtonEventArgs e)
        {
            bool startIsSet = false;
            Point currentMousePosition = Mouse.GetPosition(myGraph);
            if (!startIsSelected)
            {
                for (int i = 0; i < myGraph.Children.Count; i++)
                {
                    if (myGraph.Children[i].GetType() == typeof(Node))
                    {
                        if (((Node)myGraph.Children[i]).NodeRect.IntersectsWith(new Rect(currentMousePosition.X, currentMousePosition.Y, 5, 5)))
                        {
                            startEdgeNode = (Node)myGraph.Children[i];

                            if (currentSelectedNode != null)
                                currentSelectedNode.Fill = Brushes.Yellow;
                            if (endNode != null)
                                endNode.Fill = Brushes.Red;
                            if (startNode != null)
                                startNode.Fill = Brushes.Blue;
                            currentSelectedNode = (Node)myGraph.Children[i];
                            currentSelectedNode.DrawNodeInfo(myGraph);
                            currentSelectedNode.Fill = Brushes.Green;

                            startIsSet = true;
                        }
                    }
                }

                if (!startIsSet)
                    AddNode(new Node(currentMousePosition.X, currentMousePosition.Y, cityNames[nodeCount], myGraph));

                if (myGraph.Children.Count == 0)
                    AddNode(new Node(currentMousePosition.X, currentMousePosition.Y, cityNames[nodeCount], myGraph));
            }
            else
            {
                for (int i = 0; i < myGraph.Children.Count; i++)
                {
                    if (myGraph.Children[i].GetType() == typeof(Node))
                    {
                        if (((Node)myGraph.Children[i]).NodeRect.IntersectsWith(new Rect(currentMousePosition.X, currentMousePosition.Y, 5, 5)))
                        {
                            if (((Node)myGraph.Children[i]).NodeName.Equals(startNode.NodeName, StringComparison.InvariantCultureIgnoreCase))
                                return;
                            endNode = (Node)myGraph.Children[i];
                            ((Node)myGraph.Children[i]).Fill = Brushes.Red;
                            startIsSelected = false;
                        }
                    }
                }
            }
        }

        private void AddEdges(object sender, MouseButtonEventArgs e)
        {
            Node endNode = null;
            Point currentMousePosition = Mouse.GetPosition(myGraph);
            if (startEdgeNode != null)
            {
                for (int i = 0; i < myGraph.Children.Count; i++)
                {
                    if (myGraph.Children[i].GetType() == typeof(Node))
                    {
                        if (((Node)myGraph.Children[i]).NodeRect.IntersectsWith(new Rect(currentMousePosition.X, currentMousePosition.Y, 5, 5)))
                        {
                            endNode = (Node)myGraph.Children[i];
                        }
                    }
                }
            }

            if (startEdgeNode != null && endNode != null)
            {
                AddEdge(startEdgeNode, endNode, 0);
                startEdgeNode = null;
            }

            if (startEdgeNode != null)
            {
                endNode = new Node(currentMousePosition.X, currentMousePosition.Y, cityNames[nodeCount], myGraph);
                AddNode(endNode);
                AddEdge(startEdgeNode, endNode, 0);
                startEdgeNode = null;
            }
        }

        private void AddEdge(Node startNode, Node endNode, double weight)
        {
                if (startNode.NodeName.Equals(endNode.NodeName, StringComparison.InvariantCultureIgnoreCase))
                    return;
                Edge edge = new Edge(startNode, endNode, myGraph, weight);
                myGraph.Children.Add(edge);

                if (edge.Weight != 0)
                    startNode.ConnectedEdges.Add(edge);
                startNode.NeighborNodes.Add(endNode);

                endNode.NeighborNodes.Add(startNode);
                if(edge.Weight != 0)
                    endNode.ConnectedEdges.Add(edge);

                edgeWeight++;
                leftCtrlKeyPressed = false;
         
        }

        private void AddNode(Node node)
        {
            myGraph.Children.Add(node);
            nodeList.Add(node);
            nodeCount++;
        }

        private void OnSetStartPointClicked(object sender, RoutedEventArgs e)
        {
            if (myGraph.Children.Count >= 2)
            {
                for (int i = 0; i < myGraph.Children.Count; i++)
                {
                    if (myGraph.Children[i].GetType() == typeof(Node))
                    {
                        if (((Node)myGraph.Children[i]).NodeRect.IntersectsWith(new Rect(rightClickButtonDownPosition.X, rightClickButtonDownPosition.Y, 5, 5)))
                        {
                            ((Node)myGraph.Children[i]).Fill = Brushes.Blue;
                            startNode = (Node)myGraph.Children[i];
                            startIsSelected = true;
                        }
                    }
                }
            }
        }

        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            rightClickButtonDownPosition = Mouse.GetPosition(myGraph);
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.R:
                    startNode = null;
                    endNode = null;
                    myGraph.Children.Clear();
                    edgeWeight = 1;
                    nodeCount = 0;
                    break;
                case Key.S:
                    if (leftCtrlKeyPressed)
                    {
                        //Save XML
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Title = "Save Locations";
                        saveFileDialog.Filter = "XML Files|*.xml";
                        saveFileDialog.ShowDialog();

                        try
                        {
                            TextWriter WriteFileStream = new StreamWriter(saveFileDialog.FileName);

                            // Create a new XmlSerializer instance with the type of the test class
                            XmlSerializer SerializerObj = new XmlSerializer(typeof(List<Node>));

                            // Create a new file stream to write the serialized object to a file
                            SerializerObj.Serialize(WriteFileStream, nodeList);

                            // Cleanup
                            WriteFileStream.Close();
                        }
                        catch (Exception ex)
                        {
                            string bla = ex.ToString();
                        }

                        leftCtrlKeyPressed = false;
                    }
                    break;
                case Key.O:
                    if (leftCtrlKeyPressed)
                    {
                        //Read XML
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Title = "Open Points File";
                        openFileDialog.Filter = "Node Files|*.tree";
                        openFileDialog.ShowDialog();

                        GenerateNodesAndEdgesFromFile(openFileDialog.FileName);
                        leftCtrlKeyPressed = false;
                    }
                    break;
                case Key.P:
                    if (leftCtrlKeyPressed)
                    {
                        //Open Window Showing Points Table
                        PointsTable pt = new PointsTable(nodeList);
                        pt.ShowDialog();
                        leftCtrlKeyPressed = false;
                    }
                    break;
                case Key.D:
                    DijkstraAlgorithm.FindQuickestRouteToEndPoint(startNode, endNode, nodeList);
                    break;
                case Key.LeftCtrl:
                    leftCtrlKeyPressed = true;
                    break;
            }
        }

        public List<Node> GetAllUnvisitedNodes()
        {
            return nodeList.FindAll(node => node.HasBeenVisited == false);
        }

        public Node FindNodeByName(string name)
        {
            foreach (Node n in nodeList)
            {
                if (n.NodeName.Equals(name))
                    return n;
            }
            return null;
        }

        private void GenerateNodesAndEdgesFromFile(string pathToGraphFile)
        {
            if (String.IsNullOrEmpty(pathToGraphFile))
                return;

            if (!File.Exists(pathToGraphFile))
                return;

            bool nodeInLine = false;
            bool edgeInLine = false;
            string[] allLines = File.ReadAllLines(pathToGraphFile);
            foreach (string line in allLines)
            {
                line.Trim();

                if (line.Equals("<Nodes>"))
                {
                    nodeInLine = true;
                    edgeInLine = false;
                    continue;
                }
                if (line.Equals("<Edges>"))
                {
                    nodeInLine = false;
                    edgeInLine = true;
                    continue;
                }

                if (nodeInLine)
                {
                    try
                    {
                        string name = line.Substring(0, line.IndexOf('[') - 1);
                        string x = line.Substring(line.IndexOf('[') + 1, line.IndexOf(',') - 1 - line.IndexOf('['));
                        string y = line.Substring(line.IndexOf(',') + 1, line.IndexOf(']') - 1 - line.IndexOf(','));
                        //Node erzeugen
                        Node n = new Node(Double.Parse(x), Double.Parse(y), name, myGraph);
                        //if (n != null)
                        //    nodeList.Add(n);
                        AddNode(n);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
                else if (edgeInLine)
                {
                    try
                    {
                        string startNodeName = line.Substring(0, line.IndexOf('-'));
                        string endNodeName = line.Substring(line.IndexOf('-') + 1, line.IndexOf('(') - 2 - line.IndexOf('-'));
                        string weight = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - 1 - line.IndexOf('('));

                        //find Node Objekts
                        Node startNode = FindNodeByName(startNodeName);
                        Node endNode = FindNodeByName(endNodeName);
                        //Edge Objekt
                        if (startNode != null && endNode != null)
                        {
                            Edge e = new Edge(startNode, endNode, myGraph, double.Parse(weight));
                            if (e != null)
                                AddEdge(startNode, endNode, double.Parse(weight));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
            }
        }
    }
}
