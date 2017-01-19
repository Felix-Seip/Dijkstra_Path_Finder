using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Dijkstra
{
    public static class DijkstraAlgorithm
    {
        private static Node StartNode = null;
        private static List<Node> NodeList = null;
        public static void FindQuickestRouteToEndPoint(Node startNode, Node endNode, List<Node> allNodesOnGraph)
        {
            StartNode = startNode;
            NodeList = allNodesOnGraph;
            List<Node> unvisitedNodes = NodeList.FindAll(node => node.HasBeenVisited == false);
            List<Node> path = new List<Node>();
            Node currentNode;

            foreach(Node node in NodeList)
            {
                node.HasBeenVisited = false;
                node.DistanceFromStart = double.MaxValue;
            }
            if(startNode == null)
            {
                MessageBox.Show("Please select a start node!");
                return;
            }

            startNode.DistanceFromStart = 0.0;
            currentNode = startNode;

            bool routeFound = false;

            while (!routeFound)
            {
                double alt = 0;
                foreach (Node neighborV in currentNode.GetUnvisitedNeighborNodes())
                { 
                    if (NodeList.Contains(neighborV) )
                    {
                        alt = currentNode.DistanceFromStart + GetDistanceBetweenNodes(currentNode, neighborV);
                        if (alt < neighborV.DistanceFromStart)
                        {
                            string neighborName = neighborV.NodeName;
                            neighborV.DistanceFromStart = alt;
                            neighborV.PreviousNode = currentNode;
                        }
                    }
                }

                currentNode.HasBeenVisited = true;
                unvisitedNodes = NodeList.FindAll(node => node.HasBeenVisited == false);
                if (unvisitedNodes.Count == 0)
                    routeFound = true;
                else
                {
                    routeFound = !unvisitedNodes.Any(node => node == endNode);
                    if (!routeFound)
                        currentNode = unvisitedNodes.First(node => node.DistanceFromStart == unvisitedNodes.Min(minDisNode => minDisNode.DistanceFromStart));
                }
    
            }

            while (currentNode.PreviousNode != null && currentNode != startNode)
            {
                path.Add(currentNode);
                //for(int i = 0; i < currentNode.ConnectedEdges.Count; i++)
                //{
                //    if (currentNode.ConnectedEdges[i].EndPoint == currentNode.PreviousNode
                //        || currentNode.ConnectedEdges[i].EndPoint == currentNode)
                //    {
                //        currentNode.ConnectedEdges[i].Stroke = Brushes.Green;
                //        currentNode.ConnectedEdges[i].StrokeThickness = 5;
                //    }
                //}
                currentNode = currentNode.PreviousNode;
            }

            path.Add(startNode);

            string bla = "";
            int totalDistance = 0;
            for (int i = 0; i < path.Count; i++)
            {
                bla += path[i].NodeName + "\n";
            }
            MessageBox.Show(bla);
        }

        private static Node FindSmallestDistance()
        {
            Node shortestDistanceNode = null;
            GetDistanceFromStartNode();
            for (int i = 1; i < NodeList.Count; i++)
            {
                Node temp = NodeList[i - 1];
                if (shortestDistanceNode == null)
                    shortestDistanceNode = temp;
                else if (temp.DistanceFromStart < shortestDistanceNode.DistanceFromStart)
                    shortestDistanceNode = temp;
            }
            return shortestDistanceNode;
        }

        private static void GetDistanceFromStartNode()
        {
        }

        private static double GetDistanceBetweenNodes(Node startNode, Node endNode)
        {
            for(int i = 0; i < startNode.ConnectedEdges.Count; i++)
            {
                if(startNode.ConnectedEdges[i].EndPoint == endNode || startNode.ConnectedEdges[i].StartPoint == endNode)
                {
                    return startNode.ConnectedEdges[i].Weight;
                }
            }
            return 0.0;
        }
    }
}
