using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Dijkstra
{
    public class Edge : Shape
    {
        public Geometry LineShape{ get; private set; }
        public Node StartPoint { get; private set; }
        public Node EndPoint { get; private set; }
        public double Weight { get; private set; }

        [XmlIgnore]
        protected override Geometry DefiningGeometry
        {
            get
            {
                return this.LineShape;
            }
        }

        public Edge()
        { }

        public Edge(Node startPoint, Node endPoint, Canvas graph, double weight = 0) 
        {
            LineShape = new LineGeometry(startPoint.NodeLocation, endPoint.NodeLocation);
            Fill = Brushes.Yellow;
            Stroke = Brushes.Black;
            StrokeThickness = 1;

            StartPoint = startPoint;
            EndPoint = endPoint;
            if (weight != 0)
                Weight = weight;
            AddWeightTextBox(graph);
        }

        private void AddWeightTextBox(Canvas graph)
        {
            Label weightLabel = new Label();
            if (Weight != 0)
                weightLabel.Content = Weight + "";
            Canvas.SetLeft(weightLabel, (StartPoint.NodeLocation.X + EndPoint.NodeLocation.X) / 2);
            Canvas.SetTop(weightLabel, (StartPoint.NodeLocation.Y + EndPoint.NodeLocation.Y) / 2);

            graph.Children.Add(weightLabel);
        }
    }
}
