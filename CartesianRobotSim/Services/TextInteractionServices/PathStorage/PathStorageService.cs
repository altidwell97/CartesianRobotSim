using CartesianRobotSim.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CartesianRobotSim.Services.TextInteractionServices.PathStorage
{
    public class PathStorageService : IPathStorageService
    {
        private readonly string _filePath;

        public PathStorageService()
        {
            // Revert to previous behavior: always use LocalAppData Paths.txt for persistence.
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            // Use LocalAppData\CartesianRobotSim for Paths.txt
            var appDataDir = System.IO.Path.Combine(appData, "CartesianRobotSim");
            Directory.CreateDirectory(appDataDir);
            _filePath = System.IO.Path.Combine(appDataDir, "Paths.txt");
            if (!File.Exists(_filePath)) File.WriteAllText(_filePath, string.Empty);

            // No debug logging here to keep startup output clean.
        }

        public async Task AppendPathAsync(CartesianRobotSim.Model.Path path)
        {
            var line = SerializePath(path);
            await File.AppendAllTextAsync(_filePath, line + Environment.NewLine).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CartesianRobotSim.Model.Path>> ReadAllPathsAsync()
        {
            var lines = await File.ReadAllLinesAsync(_filePath).ConfigureAwait(false);
            var list = new List<CartesianRobotSim.Model.Path>();
            foreach (var line in lines)
            {
                var p = DeserializePath(line);
                if (p != null) list.Add(p);
            }
            return list;
        }

        public async Task RemovePathAsync(CartesianRobotSim.Model.Path path)
        {
            var lines = (await File.ReadAllLinesAsync(_filePath).ConfigureAwait(false)).ToList();
            var target = SerializePath(path);
            var idx = lines.FindIndex(l => l == target);
            if (idx >= 0)
            {
                lines.RemoveAt(idx);
                await File.WriteAllLinesAsync(_filePath, lines).ConfigureAwait(false);
            }
        }

        private string SerializePath(CartesianRobotSim.Model.Path path)
        {
            // Serialize using parentheses for vertices and ';' between vertices to match existing file format
            var verts = path.GetPath();
            return string.Join(";", verts.Select(v => $"({v.XValue.ToString(System.Globalization.CultureInfo.InvariantCulture)},{v.YValue.ToString(System.Globalization.CultureInfo.InvariantCulture)},{v.ZValue.ToString(System.Globalization.CultureInfo.InvariantCulture)})"));
        }

        private CartesianRobotSim.Model.Path? DeserializePath(string line)
        {
            try
            {
                // Only accept ';' as the vertex separator
                var parts = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var verts = new List<Vertex>();
                foreach (var part in parts)
                {
                    var trimmed = part.Trim();
                    if (trimmed.StartsWith("(") && trimmed.EndsWith(")"))
                        trimmed = trimmed.Substring(1, trimmed.Length - 2);

                    var nums = trimmed.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (nums.Length >= 3 &&
                        double.TryParse(nums[0].Trim(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double x) &&
                        double.TryParse(nums[1].Trim(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double y) &&
                        double.TryParse(nums[2].Trim(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double z))
                    {
                        verts.Add(new Vertex(x, y, z));
                    }
                }
                return new CartesianRobotSim.Model.Path(verts);
            }
            catch
            {
                return null;
            }
        }
    }
}
