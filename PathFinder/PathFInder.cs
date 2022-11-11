using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PathFinder
{
    // hello
    public class Node
    {
        // name property can be empty, but mainly important nodes will have
        //   names
        // ex: SLot, PLot, Gollisanno, etc.
        private string name;
        public string Name { get { return this.name; } }
        // id used to find the Node in a SortedList
        private int id;
        public int ID { get { return this.id; } }

        // holds the x position of the node. has a getter method
        private double x;
        public double X { get { return this.x; } }
        // y value and its getter method of the node
        private double y;
        public double Y { get { return this.y; } }

        // holds a list of all nodes that this instance can connect to
        //   has a getter method
        private SortedList<int, Node> nodeConnections = new SortedList<int, Node>();
        public SortedList<int, Node> NodeConnections { get { return this.nodeConnections; } }

        // holds the weight (distance) between each node using the same ID system
        //   has a getter method
        private SortedList<int, double> nodeWeights = new SortedList<int, double>();
        public SortedList<int, double> NodeWeights { get { return this.nodeWeights; } }

        // this method is used to set up the weights between all nodes connected
        //   to this node instance
        private SortedList<int, double> FindWeights()
        {
            // create a new SortedList to hold the weights generated
            SortedList<int, double> weights = new SortedList<int, double>();

            // iterate through all the contents of the connections list
            foreach ( KeyValuePair<int, Node> kvp in this.nodeConnections )
            {
                // find the distance between this Node instance, and the selected
                //   value in the loop
                double weight = Math.Sqrt(
                    Math.Pow((this.x - kvp.Value.x), 2) + Math.Pow((this.y - kvp.Value.y), 2));

                // store that distance in the weights list with the id of the node
                //   as the key in nodeWeights
                weights.Add(kvp.Key, weight);
            }

            // return newly generated nodeWeight list
            return weights;
        }

        public Node(int id, double x, double y, SortedList<int, Node> connections, string name = "")
        {
            this.name = name;
            this.id = id;
            this.x = x;
            this.y = y;
            this.nodeConnections = connections;

            // set up nodeWeights after initialization
            this.FindWeights();
        }
    }

    public class PathFinder
    {
        public SortedList<int, Node> nodes;
        // list that holds the weights of every node
        // Key - the id of a Node
        // Value - the weight of each connection the Node has
        public SortedList<int, SortedList<int, double>> distances;

        // Method: SetShortestDistance
        // Purpose: When given a set of Node id's and weights
        //          will determine which is the shortest path and return
        //          the id of said shortest path
        // Restrictions: Private, so should only be accessed by other
        //               methods within this class
        private int SetShortestDistance(SortedList<int, double> distances, SortedList<int, Node> visitedNodes)
        {
            // create a variable to hold the if of the node we are checking
            int shortest = -1;

            // iterate through all nearby nodes and check the space between them
            foreach (KeyValuePair<int, double> kvp in distances)
            {
                // assign the value of the current node we are checking
                int node = kvp.Key;

                //      // if no node has been assigned to the shortest node, set a flag
                //      // that the current node should be the shortest
                bool currentIsTheShortest = (shortest == -1 || (distances[node] < distances[shortest]));

                //      // check to see if the current node hasn't already been visited
                if (currentIsTheShortest && !visitedNodes.ContainsKey(node))
                {
                    // if it hasn't set the current node to shortest!
                    shortest = node;
                }
            }
            // return the shortest node
            return shortest;
        }

        // 
        public void FindPath(Node start, Node end)
        {

        }

        private void LoadJson(string dir)
        {
            // will hold all loaded Nodes from JSON so we can
            // access them for sorting into usable lists!
            List<Node> items;

            using (StreamReader sr = new StreamReader(dir))
            {
                string json = sr.ReadToEnd();
                items = JsonSerializer.Deserialize<List<Node>>(json);
            }

            foreach (Node node in items)
            {
                // add each node to the list of nodes
                nodes.Add(node.ID, node);
                // add the node's SortedList of weights to the weights list
                distances.Add(node.ID, node.NodeWeights);
            }
        }

        public PathFinder(string directory)
        {

        }
    }
}
