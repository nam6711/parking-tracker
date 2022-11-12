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
    // Class: Node
    // Author: Noah/Nat Manoucheri
    // Purpose: Contains data to create a Node for pathfinding. This
    //          includes x, y positions, an optional name value, a list
    //          of nodes that are connected to a node, and after initialization,
    //          the node calculates the weights of every node it is connected to
    // Construction: int id - id of the Node
    //               double x - x position of the Node
    //               double y - y position of the Node
    //               SortedList<int, Node> connections - a list of connected nodes, and their id's
    //               string? name - optional value that holds a name value for the Node. If no name
    //                              is given, it will recieve an empty string
    // Restrictions: All data is read-only. A Node should be static and unchanging after
    //               being initialized
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

    // Class: PathFinder
    // Author: Noah/Nat Manoucheri
    // Purpose: Initializes a set of nodes from a JSON file, and
    //          then allows a user to infinitely call a pathfinding method
    //          with a given start and end index
    // Construction: string directory - the directory of a JSON file to load Nodes from
    // Restrictions: The only data someone can grab from this is the FindPath method
    //               as NOTHING should be altered after initialization of JSON file
    public class PathFinder
    {
        // used to hold all Nodes loaded from a JSON file
        // public so that the x and y positions from JSON can be accessed outside the program
        private SortedList<int, Node> nodes;
        // list that holds the weights of every node
        // Key - the id of a Node
        // Value - the weight of each connection the Node has
        private SortedList<int, SortedList<int, double>> graph;

        // Method: SetShortestDistance
        // Purpose: When given a set of Node id's and weights
        //          will determine which is the shortest path and return
        //          the id of said shortest path
        // Parameters: SortedList<int, double> distances - a list of distances for nearby nodes to where
        //                                                 we are currently on the graph
        //             SortedList<int, bool> visitedNodes - holds a list of booleans for each Node on the 
        //                                                  graph that states if we have visited them yet or not
        // Returns: int - the index of the closest Node to where we currently are
        // Restrictions: Private, so should only be accessed by other
        //               methods within this class
        private int SetShortestDistance(SortedList<int, double> distances, SortedList<int, bool> visitedNodes)
        {
            // create a variable to hold the if of the node we are checking
            int shortest = -1;
            // set a starting shortest distance to basically infinity
            double shortestDist = double.MaxValue;

            // iterate through all nearby nodes and check the space between them
            foreach (KeyValuePair<int, double> kvp in distances)
            {
                // assign the value of the current node we are checking
                int node = kvp.Key;

                //      // if no node has been assigned to the shortest node, set a flag
                //      // that the current node should be the shortest
                bool currentIsTheShortest = (shortest == -1 || (distances[node] < shortestDist));

                //      // check to see if the current node hasn't already been visited
                if (currentIsTheShortest && !visitedNodes[node])
                {
                    // if it hasn't set the current node to shortest!
                    shortest = node;
                    shortestDist = distances[node];
                }
            }
            // return the shortest node
            return shortest;
        }

        // Method: FindPath
        // Purpose: Runs through the entire node list to find a path from
        //          a start index to the given end index using an adapted version
        //          of dijkstra's algorithm
        // Parameters: int start - index to start at
        //             int end - index to find the shortest path of
        // Returns: SortedList<int, Node> - a list detailing the shortest path
        //                                  where int is the step in our path, and Node
        //                                  is the next Node to step into
        // Restrictions: none? i think?
        public SortedList<int, Node> FindPath(int start, int end)
        {
            // Sorted List that maps the distance each node is from the current point
            SortedList<int, double> distances = new SortedList<int, double>();
            // SortedList holding whether or not we have already been to a node
            SortedList<int, bool> visited = new SortedList<int, bool>();
            // SortedList holding our shortest path
            SortedList<int, Node> shortestPath = new SortedList<int, Node>();

            // loop through both lists and initialize SortedList to start values
            foreach (KeyValuePair<int, Node> kvp in nodes )
            {
                // all distances are max value since we dont know what they are rn
                distances.Add(kvp.Key, double.MaxValue);
                // set all spaces to not being visited
                visited.Add(kvp.Key, false);
            }

            // set the initial point we start at to having a distance of 0 from us
            distances[start] = 0;

            // iterate a number of times equal to the size of the nodes list
            //  and every loop, find the next closest node to where we left off
            //  while making sure we don't accidently retread the same spaces
            // every time we find a new shortest node add it to an array of spaces
            //      treaded
            // once we hit the end index, break out
            // used to keep track of which index on our path we're on
            for (int i = 0; i < nodes.Count; i++)
            {
                // find the point closest to this current node
                // **on the first loop, the value of u starts as the start index
                //   as it has a weight of 0, as set above!
                // also add the current closest node to the path list
                int closestNode = SetShortestDistance(distances, visited);
                shortestPath.Add(i, nodes[closestNode]);

                // if the current node we are on is the end index, then
                // break out!
                if (closestNode == end)
                {
                    break;
                }

                // set the node we just found to having been visited
                visited.Add(closestNode, true);    

                // update the nearby spaces and add how far away they are from
                // the current index we are checking ( we check to find nearby spaces by if they're connected to the shortest node)
                // what will happen, is next loop there will be even more indeces to
                // check the distance of, and we loop over until we eventually hit the
                // end index
                foreach (KeyValuePair<int, Node> node in nodes)
                {
                    // if the current index is able to be accessed by the node we are checking
                    // and hasn't already been visited:
                    //   and if the weight from the current node to itself is smaller than infinity 
                    //   (meaning it still has its initial weight) then set its weight so we can check 
                    //   to see how far it is for next loop
                    if (!visited[node.Key] && // checks if a space was visited
                        graph[closestNode].Values.Contains(node.Key) && // checks if the closestNode is connected to the current node
                        distances[closestNode] + graph[closestNode][node.Key] < distances[node.Key]) // checks if the current value of the node in the distances array
                        //                                                                           // is greater than the graph's weight on the closest node
                    {
                        // if this node has not been visited, and we haven't even looked at it yet, then assign
                        // it a distance from the current node so we can potentially visit it next loop
                        distances[node.Key] = distances[closestNode] + graph[closestNode][node.Key];
                    }
                }
            }

            return shortestPath;
        }

        // Method: LoadJson
        // Purpose: Using a given JSON directory, load in a file of nodes and map
        //          them into a List. Using that list, initialize of Graph and SortedList
        //          of nodes
        // Parameters: string dir - the directory to load Nodes from
        // Returns: nothing
        // Restrictions: Should only be called within the constructor
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
                graph.Add(node.ID, node.NodeWeights);
            }
        }

        // Constructor: PathFinder
        // Purpose: Initialize the starting Nodes lists so that the
        //          path finding methods can be used infinitely without need
        //          for a new instance of the class
        // Parameters: string directory - the directory of a JSON file to load Nodes from
        // Restrictions: Runs first so that all dependent data is loaded prior
        //               to any pathfinding
        public PathFinder(string directory)
        {

        }
    }
}
