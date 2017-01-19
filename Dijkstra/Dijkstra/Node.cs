using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Dijkstra
{
    public class Node : Shape
    {
        public Point NodeLocation { get; private set; }
        public Geometry ClipShape { get; private set; }
        public Rect NodeRect { get; private set; }
        public List<Node> NeighborNodes { get; set; }
        public List<Edge> ConnectedEdges { get; set; }
        public string NodeName { get; private set; }
        public double DistanceFromStart { get; set; }

        public Node PreviousNode { get; set; }

        public bool HasBeenVisited { get; set; }

        [XmlIgnore]
        protected override Geometry DefiningGeometry
        {
            get
            {
                return this.ClipShape;
            }
        }

        public Node()
        { }

        public Node(double x, double y, string nodeName, Canvas canvas) 
        {
            NodeLocation = new Point(x, y);
            NeighborNodes = new List<Node>();
            ConnectedEdges = new List<Edge>();
            NodeName = nodeName;
            ClipShape = new EllipseGeometry(NodeLocation, 5, 5);
            Fill = Brushes.Yellow;
            Stroke = Brushes.Black;
            StrokeThickness = 1;

            HasBeenVisited = false;
            NodeRect = new Rect(x, y, 5, 5);
            DrawNodeInfo(canvas);
        }

        public bool IsConnectedToNode(Node node)
        {
            for(int i = 0; i < ConnectedEdges.Count; i++)
            {
                if (ConnectedEdges[i].EndPoint.NodeName.Equals( node.NodeName, System.StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        public void DrawNodeInfo(Canvas graph)
        {
            CustomLabel infoLabel = new CustomLabel(NodeName);
            for(int i = 0; i < graph.Children.Count; i++)
            {
                if (graph.Children[i].GetType() == typeof(CustomLabel))
                    graph.Children.RemoveAt(i);
            }
            //weightLabel.Content = Weight + "";

            infoLabel.Content += NodeName;
            //infoLabel.Content += "\nNeighbor Nodes:\n";
            //for(int i = 0; i < NeighborNodes.Count; i++)
            //    infoLabel.Content += "Node " + i + ": " + NeighborNodes[i].NodeName + "\n";
            //infoLabel.Content += "Connected Edges:\n";
            //for (int i = 0; i < ConnectedEdges.Count; i++)
            //    infoLabel.Content += "Edge " + i + ": " + ConnectedEdges[i].Weight + "\n";

            //weightTextBox.PointToScreen(new Point((StartPoint.NodeLocation.X + EndPoint.NodeLocation.X) / 2, (StartPoint.NodeLocation.Y + EndPoint.NodeLocation.Y) / 2));
            Canvas.SetLeft(infoLabel, ((NodeLocation.X + NodeLocation.X) / 2) + 10);
            Canvas.SetTop(infoLabel, (NodeLocation.Y + NodeLocation.Y) / 2);

            graph.Children.Add(infoLabel);
        }
        
        public void UpdateNodeInfo(Canvas canvas)
        {
            DrawNodeInfo(canvas);
        }

        public List<Node> GetUnvisitedNeighborNodes()
        {
            return NeighborNodes.FindAll(node => node.HasBeenVisited == false);
        }
    }
}
