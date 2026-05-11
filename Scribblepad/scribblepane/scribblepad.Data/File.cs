using scribblepad.Core;
using System.Drawing;
using System.Text.Json;
using System.IO;

namespace scribblepad.Data {
    public class File {
        public void SaveAsJson(string path, List<Stroke> strokes) {
            string json = JsonSerializer.Serialize(strokes);
            System.IO.File.WriteAllText(path, json); 
        }

        public List<Stroke>? LoadFromJson(string path) {
            string json = System.IO.File.ReadAllText(path); 
            return JsonSerializer.Deserialize<List<Stroke>>(json);
        }

        public void SaveAsBinary(string path, List<Stroke> strokes) {
            using var fs = new FileStream(path, FileMode.Create);
            using var bw = new BinaryWriter(fs);
            bw.Write(strokes.Count);
            foreach (var s in strokes) {
                bw.Write(s.Color);
                bw.Write(s.Thickness);
                bw.Write(s.Points.Count);
                foreach (var p in s.Points) {
                    bw.Write(p.X);
                    bw.Write(p.Y);
                }
            }
        }

        public List<Stroke> LoadFromBinary(string path) {
            var strokes = new List<Stroke>();
            using var fs = new FileStream(path, FileMode.Open);
            using var br = new BinaryReader(fs);
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++) {
                var stroke = new Stroke {
                    Color = br.ReadString(),
                    Thickness = br.ReadDouble()
                };
                int pointCount = br.ReadInt32();
                for (int j = 0; j < pointCount; j++) {
                    stroke.Points.Add(new Point(
                        (int)br.ReadDouble(),
                        (int)br.ReadDouble()
                    ));
                }
                strokes.Add(stroke);
            }
            return strokes;
        }
    }
}

