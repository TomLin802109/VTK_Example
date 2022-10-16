using OpenTK;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Quadrep
{
    public class PointCloudIO
    {
        public IEnumerable<Vector3> ReadTXT(string filePath)
        {
            using (var sr = new StreamReader(filePath))
            {
                try
                {
                    var result = new List<Vector3>();
                    while (sr.Peek() > 0)
                    {
                        var strs = sr.ReadLine().Split(new char[] { '(', ',', ')' }).Where(p => p != "").ToArray();
                        result.Add(new Vector3(float.Parse(strs[0]),
                                               float.Parse(strs[1]),
                                               float.Parse(strs[2])));
                    }
                    return result;
                }
                catch
                {
                    return new List<Vector3>();
                }
            }
        }

        //public IEnumerable<Vector3> ReadPCD(string filePath)
        //{
        //    using (var reader = new PCDReader())
        //    using (var cloud = new PointCloudOfXYZ())
        //    {
        //        reader.Read(filePath, cloud);
        //        var effPoints = cloud.Points.Where(p => !float.IsNaN(p.Z)).ToArray();
        //        var result = new Vector3[cloud.Points.Count];
        //        Parallel.For(0, effPoints.Count(), i =>
        //        {
        //            result[i] = new Vector3(effPoints[i].X,
        //                                    effPoints[i].Y,
        //                                    effPoints[i].Z) * 1000f;
        //        });
        //        return result;
        //    }
        //}

        //public IEnumerable<Vector3> ReadPLY(string filePath)
        //{
        //    using (var cloud = new PointCloudOfXYZ())
        //    {
        //        var reader = new PLYReader();
        //        reader.Read(filePath, cloud);
        //        var effPoints = cloud.Points.Where(p => !float.IsNaN(p.Z)).ToArray();
        //        var result = new Vector3[cloud.Points.Count];
        //        Parallel.For(0, effPoints.Count(), i =>
        //        {
        //            result[i] = new Vector3(effPoints[i].X,
        //                                    effPoints[i].Y,
        //                                    effPoints[i].Z) * 1000f;
        //        });
        //        return result;
        //    }
        //}

        public IEnumerable<Vector3> ReadBinary(string filePath)
        {
            using (var br = new BinaryReader(File.OpenRead(filePath)))
            {

                var size = sizeof(float) * 3;
                var data = new Vector3[br.BaseStream.Length / size];
                for (var i = 0; i < data.Length; i++)
                {
                    var x = br.ReadSingle();
                    var y = br.ReadSingle();
                    var z = br.ReadSingle();
                    data[i] = new Vector3(x, y, z);
                }
                return data;
            }
        }

        //public void WriteTXT(string fileName, IEnumerable<Vector3> data)
        //{
        //    using (var sw = new StreamWriter(fileName))
        //    {
        //        foreach (var p in data)
        //            sw.WriteLine($"{p.X},{p.Y},{p.Z}");
        //    }
        //}

        //public void WritePCD(string fileName, IEnumerable<Vector3> data)
        //{
        //    using (var writer = new PCDWriter())
        //    using (var cloud = new PointCloudOfXYZ())
        //    {
        //        foreach (var p in data)
        //            cloud.Add(new System.Numerics.Vector3(p.X, p.Y, p.Z) / 1000f);
        //        writer.WriteBinaryCompressed(fileName, cloud);
        //    }
        //}

        //public void WritePLY(string fileName, IEnumerable<Vector3> data)
        //{
        //    using (var cloud = new PointCloudOfXYZ())
        //    {
        //        foreach (var p in data)
        //            cloud.Add(new System.Numerics.Vector3(p.X, p.Y, p.Z) / 1000f);
        //        var writer = new PLYWriter();
        //        writer.Write(fileName, cloud);
        //    }

        //}

        //public void WriteBinary(string fileName, IEnumerable<Vector3> data)
        //{
        //    using (var bw = new BinaryWriter(File.Open(fileName, FileMode.Create)))
        //    {
        //        foreach (var i in data)
        //        {
        //            bw.Write(i.X);
        //            bw.Write(i.Y);
        //            bw.Write(i.Z);
        //        }
        //    }
        //}
    }
}
