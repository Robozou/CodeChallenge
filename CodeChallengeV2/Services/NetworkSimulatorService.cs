using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallengeV2.Models;

namespace CodeChallengeV2.Services
{
    public class NetworkSimulatorService : INetworkSimulatorService
    {
        /// <summary>
        /// Returns all nodes of type Gateway that if removed together with their connected edges would leave nodes of type Device, without edges to any Gateway. 
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public Task<List<Node>> FindCritialGateways(NetworkGraph graph)
        {
            List<Node> critGateways = new List<Node>();
            foreach (Graph g in graph.Graphs)
            {
                Dictionary<string, List<string>> edges = new Dictionary<string, List<string>>();
                Dictionary<string, Node> devices = new Dictionary<string, Node>();
                Dictionary<string, Node> gateways = new Dictionary<string, Node>();
                Dictionary<string, Node> crit = new Dictionary<string, Node>();
                foreach (Node n in g.Nodes)
                {
                    if (n.Type.Equals("Device")) devices.Add(n.Id, n);
                    if (n.Type.Equals("Gateway")) gateways.Add(n.Id, n);
                }
                foreach (string dev in devices.Keys)
                {
                    List<string> targets = new List<string>();
                    foreach (Edge e in g.Edges)
                        if (e.Source.Equals(dev))
                        {
                            targets.Add(e.Target);
                        }
                    edges.Add(dev, targets);
                }
                foreach (string key in devices.Keys)
                {
                    if (edges[key].Count == 1)
                    {
                        crit.TryAdd(gateways[edges[key][0]].Id, gateways[edges[key][0]]);
                    }
                }
                foreach (Node n in crit.Values)
                {
                    critGateways.Add(n);
                }
            }
            return Task.FromResult(critGateways);
        }
    }
}
