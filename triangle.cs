using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project5
{
    public class vertex
    {
       public double x, y, z;
       public double d4 = 1;
       public double[,] pos;
        double nx, ny, nz,nd4;
       public vertex(double pointX, double pointY, double pointZ, double radius)
        {
            x = pointX;
            y = pointY;
            z = pointZ;
            d4 = 1;
            nx = pointX / radius;
            ny = pointY / radius;
            nz = pointZ / radius;
            nd4 = 0;
            pos = new double[4, 1];
        }
    }
   public class triangle
    {
       public vertex vertex1,vertex2,vertex3;
        
        public triangle(vertex v1, vertex v2, vertex v3)
        {
            vertex1 = v1;
            vertex2 = v2;
            vertex3 = v3;
        }
    }
}
