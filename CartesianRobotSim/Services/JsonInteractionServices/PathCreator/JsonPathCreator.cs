using CartesianRobotSim.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace CartesianRobotSim.Services.JsonInteractionServices.PathCreator
{
    public class JsonPathCreator : IPathCreator
    {
        public async Task CreatePath(Model.Path path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var vertices = path.GetPath();

            // Read existing JSON (or start a new document)
            Dictionary<string, Dictionary<string, string>> doc = null;
            if (File.Exists("Paths.json"))
            {
                var existing = await File.ReadAllTextAsync("Paths.json");
                if (!string.IsNullOrWhiteSpace(existing))
                {
                    try
                    {
                        doc = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(existing);
                    }
                    catch
                    {
                        // If deserialization fails, start fresh
                        doc = new Dictionary<string, Dictionary<string, string>>();
                    }
                }
            }

            if (doc == null) doc = new Dictionary<string, Dictionary<string, string>>();

            // Determine next Path name (Path1, Path2, ...)
            int maxIndex = 0;
            foreach (var key in doc.Keys)
            {
                if (key.StartsWith("Path", StringComparison.OrdinalIgnoreCase))
                {
                    var tail = key.Substring(4);
                    if (int.TryParse(tail, out int n) && n > maxIndex) maxIndex = n;
                }
            }
            var newPathName = $"Path{maxIndex + 1}";

            // Build inner dictionary for points in insertion order
            var points = new Dictionary<string, string>();
            for (int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];
                var pointName = $"Point{ i + 1 }";
                // Format as (x,y,z) using invariant culture
                var pointValue = $"({v.XValue.ToString(System.Globalization.CultureInfo.InvariantCulture)},{v.YValue.ToString(System.Globalization.CultureInfo.InvariantCulture)},{v.ZValue.ToString(System.Globalization.CultureInfo.InvariantCulture)})";
                points[pointName] = pointValue;
            }

            doc[newPathName] = points;

            var options = new JsonSerializerOptions { WriteIndented = true };
            var outJson = JsonSerializer.Serialize(doc, options);
            await File.WriteAllTextAsync("Paths.json", outJson);
        }
    }
}
