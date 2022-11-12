using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// loads pathfinding library by Nat :]
using PathFinder;

namespace PathFinderLibraryTestProgram
{
    internal class Program
    {
        // Method: Main
        // Purpose: This is a testing ground for the PathFinding library
        //          and is not a part of the main program
        // Notes: Loads in a JSON file stored within the bin, and is then
        //        passed into the PathFinder instance so we can use the loaded Nodes
        //        and pathfind between them
        // Building a JSON Node file: Nat made this app: https://people.rit.edu/nam6711/parking-tracker/
        //                            You can load Node JSON files and create/save them. Open the console to see how
        //                            to use it
        // Restrictions: Meant for testing and seeing how the library
        //               and seeing how data is returned
        static void Main(string[] args)
        {
            // test to make sure the JSON file is correctly being loaded from the bin/debug folder
            //using (StreamReader sr = new StreamReader("nodeList.json"))
            //{
            //    // Note from Nat: It works!
            //    string json = sr.ReadToEnd();
            //    Console.WriteLine(json); 
            //    sr.Close();
            //}

            // create a new PathFinder instince and run a path finding method
            PathCreate pc = new PathCreate("nodeList.json");
            // run the path finding method and store the output into a sorted list
            // for the given nodes, i've decided to path between nodes 1 to 4
            SortedList<int, Node> path = pc.FindPath(1, 8);
            // iterate through the path, node by node. each node has a name, id, x, y value you can connect
            Console.WriteLine("Step             Node");
            foreach (KeyValuePair<int, Node> kvp in path)
            {
                Console.WriteLine($"{kvp.Key}             {kvp.Value.Index}");
            }
        }
    }
}
