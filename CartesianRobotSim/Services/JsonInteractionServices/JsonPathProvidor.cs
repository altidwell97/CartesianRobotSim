using CartesianRobotSim.Model;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Path = CartesianRobotSim.Model.Path;

namespace CartesianRobotSim.Services.JsonInteractionServices
{
    public class JsonPathProvidor : IPathProvider
    {
        public async Task<IEnumerable<Path>> GetAllPaths()
        {
            using (StreamReader r = new StreamReader("Paths.json"))
            {
                string json = await r.ReadToEndAsync();

                // Parse the JSON into a dictionary: PathName -> (PointName -> "(x,y,z)")
                var doc = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);
                var result = new List<Path>();

                if (doc == null) return result;

                foreach (var pathEntry in doc)
                {
                    var vertices = new List<Vertex>();

                    // Enumerate the points in insertion order to preserve ordering from the JSON file
                    foreach (var pointEntry in pathEntry.Value)
                    {
                        var pointString = pointEntry.Value;
                        if (string.IsNullOrWhiteSpace(pointString)) continue;

                        // Expect format like: "(0,0,0)" or "(0.0, 1.2, -3)"
                        var trimmed = pointString.Trim();
                        if (trimmed.StartsWith("(") && trimmed.EndsWith(")"))
                            trimmed = trimmed.Substring(1, trimmed.Length - 2);

                        var parts = trimmed.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length != 3) continue; // skip malformed

                        if (double.TryParse(parts[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double x)
                            && double.TryParse(parts[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double y)
                            && double.TryParse(parts[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double z))
                        {
                            vertices.Add(new Vertex(x, y, z));
                        }
                    }

                    // Create a Path object from the vertices and add to result
                    result.Add(new Path(vertices));
                }
                return result;
            }
        }
    }
}
