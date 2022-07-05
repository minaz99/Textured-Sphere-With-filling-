using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
namespace project5
{
    public static class Drawing
    {

        

        public static double[,] getPoints(double X0, double Y0, double X1, double Y1)
        {
            //Bitmap img = Image.BitmapImage2Bitmap(image);
            double dx = X1 - X0;
            double dy = Y1 - Y0;

            int steps = (int)(Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy));
            double Xinc = dx / (double)steps;
            double Yinc = dy / (double)steps;

            double X = X0;
            double Y = Y0;
            double[,] points = new double[steps + 2, 2];
           
            for (int i = 0; i <= steps; i++)
            {
                //Image.draw(img, X, Y, c);
                (points[i, 0], points[i, 1]) = (X, Y);
                X += Xinc;
                Y += Yinc;
               
            }
            return points; 
        }

      
        public static void DDA(Canvas canvas, double X0, double Y0, double X1, double Y1,System.Windows.Media.Brush color)
        {
            Line line = new Line();

            if (Double.IsNaN(X0) == false && Double.IsInfinity(X0) == false)
            {
                line.X1 = RoundToSignificantDigits(X0, 3);
                line.X2 = RoundToSignificantDigits(X1, 3);
                line.Y1 = RoundToSignificantDigits(Y0, 3);
                line.Y2 = RoundToSignificantDigits(Y1, 3);
                line.Fill = color;
                line.Stroke = color;
                canvas.Children.Add(line);
            }
        }


        public static void drawTriangle(Canvas canvas, triangle triangle)
        {
            DDA(canvas, triangle.vertex1.pos[0, 0], triangle.vertex1.pos[1, 0], triangle.vertex2.pos[0, 0], triangle.vertex2.pos[1, 0],System.Windows.Media.Brushes.Black);
            DDA(canvas, triangle.vertex2.pos[0, 0], triangle.vertex2.pos[1, 0], triangle.vertex3.pos[0, 0], triangle.vertex3.pos[1, 0],System.Windows.Media.Brushes.Black);
            DDA(canvas, triangle.vertex3.pos[0, 0], triangle.vertex3.pos[1, 0], triangle.vertex1.pos[0, 0], triangle.vertex1.pos[1, 0],System.Windows.Media.Brushes.Black);

        }

        public static vertex[] generateVertices(int m, int n, int radius)
        {


            //  double theta = Math.PI / 1.7;
            vertex[] vertices = new vertex[m * n + 2];
            vertices[0] = (new vertex(0, radius, 0, radius));
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    double p1 = RoundToSignificantDigits(radius * Math.Cos((2 * Math.PI * j) / m) * Math.Sin((Math.PI * (i + 1)) / (n + 1)),3);
                    double p2 = RoundToSignificantDigits(radius * Math.Cos((Math.PI * (i + 1)) / (n + 1)),3);
                    double p3 = RoundToSignificantDigits(radius * Math.Sin((2 * Math.PI * j) / m) * Math.Sin((Math.PI * (i + 1)) / (n + 1)),3);
                    vertices[i * m + j + 1] = (new vertex(p1, p2, p3, radius));
                }
            }
            vertices[m * n + 1] = (new vertex(0, -radius, 0, radius));
            return vertices;
        }
        public static double[][] getLinePoints(double X0, double Y0, double X1, double Y1)
        {
            double[][] cords;
            if (double.IsNaN(X0) == false && double.IsInfinity(X0) == false)
            {
                double dx = X1 - X0;
                double dy = Y1 - Y0;

                int steps = (int)(Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy));
                double Xinc = dx / (double)steps;
                double Yinc = dy / (double)steps;   

                double X = X0;
                double Y = Y0;

                 cords = new double[steps + 1][];
                for (int i = 0; i <= steps; i++)
                {
                    cords[i] = new double[2];
                    cords[i][0] = (int)X;
                    cords[i][1] = (int)Y;
                    X += Xinc;
                    Y += Yinc;
                }
                return cords;
            }
            return new double[1][];
        }

        public static line createLine(double x0, double y0, double x1, double y1)
        {
            double dx = x1 - x0;
            double dy = y1 - y0;

            int steps = (int)(Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy));
           return new line(0,x0,y0,x1,y1, Color.FromArgb(0, 0, 0), getLinePoints(x0, y0, x1, y1), steps + 1);
           
        }


       public static double RoundToSignificantDigits(this double d, int digits)
        {
            if (d == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }

        public static Polygon createPolygon(vertex v1, vertex v2, vertex v3)
        {
            Polygon p = new Polygon(2, Color.FromArgb(0, 0, 0));
            p.addLine(createLine(RoundToSignificantDigits(v1.pos[0, 0],3),RoundToSignificantDigits(v1.pos[1, 0],3),RoundToSignificantDigits(v2.pos[0, 0],3),RoundToSignificantDigits(v2.pos[1, 0],3)));
            p.addLine(createLine(RoundToSignificantDigits(v2.pos[0, 0],3),RoundToSignificantDigits(v2.pos[1, 0],3),RoundToSignificantDigits(v3.pos[0, 0],3),RoundToSignificantDigits(v3.pos[1, 0],3)));
            p.addLine(createLine(RoundToSignificantDigits(v3.pos[0, 0],3),RoundToSignificantDigits(v3.pos[1, 0],3),RoundToSignificantDigits(v1.pos[0, 0],3),RoundToSignificantDigits(v1.pos[1, 0],3)));             
            return p;
        }

        public static (triangle[],List<Polygon>) createTriangles(vertex[] vertices, int m, int n)
        {
            triangle[] triangles = new triangle[2 * m * n];
            List<Polygon> polygons = new List<Polygon>();
            //int m = 15;
            //int n = 15;
            // int radius = 10;

            for (int i = 0; i <= m - 2; i++)
            {
                triangles[i] = (new triangle(vertices[0], vertices[i + 2], vertices[i + 1]));                            
                polygons.Add(createPolygon(vertices[0], vertices[i + 2], vertices[i + 1]));
                triangles[2 * (n - 1) * m + i + m] = (new triangle(vertices[m * n + 1], vertices[(n - 1) * m + i + 1], vertices[(n - 1) * m + i + 2]));
                polygons.Add(createPolygon(vertices[m * n + 1], vertices[(n - 1) * m + i + 1], vertices[(n - 1) * m + i + 2]));
            }
            triangles[m - 1] = (new triangle(vertices[0], vertices[1], vertices[m]));
            triangles[2 * (n - 1) * m + m - 1 + m] = (new triangle(vertices[m * n + 1], vertices[m * n], vertices[(n - 1) * m + 1]));
            polygons.Add(createPolygon(vertices[0], vertices[1], vertices[m]));
            polygons.Add(createPolygon(vertices[m * n + 1], vertices[m * n], vertices[(n - 1) * m + 1]));
            for (int i = 0; i <= n - 2; i++)
            {
                for (int j = 1; j <= m - 1; j++)
                {
                    triangles[(2 * i + 1) * m + j - 1] = (new triangle(vertices[i * m + j], vertices[i * m + j + 1], vertices[(i + 1) * m + j]));
                    triangles[(2 * i + 2) * m + j - 1] = (new triangle(vertices[i * m + j], vertices[(i + 1) * m + j + 1], vertices[(i + 1) * m + j]));
                    polygons.Add(createPolygon(vertices[i * m + j], vertices[i * m + j + 1], vertices[(i + 1) * m + j]));
                    polygons.Add(createPolygon(vertices[i * m + j], vertices[(i + 1) * m + j + 1], vertices[(i + 1) * m + j]));
                }
                triangles[(2 * i + 1) * m + m - 1] = (new triangle(vertices[(i + 1) * m], vertices[i * m + 1], vertices[(i + 1) * m + 1]));
                triangles[(2 * i + 2) * m + m - 1] = (new triangle(vertices[(i + 1) * m], vertices[(i + 1) * m + 1], vertices[(i + 2) * m]));
                polygons.Add(createPolygon(vertices[(i + 1) * m], vertices[i * m + 1], vertices[(i + 1) * m + 1]));
                polygons.Add(createPolygon(vertices[(i + 1) * m], vertices[(i + 1) * m + 1], vertices[(i + 2) * m]));
            }
            return (triangles,polygons);
        }



        public static double[][] initializaMatrix()
        {
            double[][] c = new double[4][];

            for (int i = 0; i < 4; i++)
            {
                c[i] = new double[4];
            }
            return c;
        }

        public static double[,] MultiplyMatrix(double[,] A, double[,] B)
        {
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);
            double temp = 0;
            double[,] kHasil = new double[rA, cB];
            if (cA != rB)
            {
                Console.WriteLine("matrik can't be multiplied !!");
                return null;
            }
            else
            {
                for (int i = 0; i < rA; i++)
                {
                    for (int j = 0; j < cB; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            temp += A[i, k] * B[k, j];
                        }
                        kHasil[i, j] = temp;
                    }
                }
                return kHasil;
            }
        }

        public static void project_vertices(vertex[] vertices, double[,] transformationMatrix)
        {
            int height = 600;
            int width = 600;
            double theta = Math.PI / 1.7; //distance 
            double s = (width / 2) * (1 / Math.Tan(theta / 2));

            /* double[,] transformationMatrix = new double[4, 4]
             {
               {1,0,0,0},
               {0,1,0,0},
               {0,0,1,camera},
               {0,0,0,1}
             };*/
            double[,] PrespectiveProjection = new double[4, 4]
           {
              {-s,0,width/2,0},
              {0,s,height/2,0},
              {0,0,0,1},
              {0,0,1,0}
           };


            for (int i = 0; i < vertices.Length; i++)
            {
                vertex v = vertices[i];

                double[,] resultM = MultiplyMatrix(PrespectiveProjection, transformationMatrix);

                double[,] vertexPoints = new double[4, 1] { { v.x }, { v.y }, { v.z }, { v.d4 } };
                double[,] vertexPos = MultiplyMatrix(resultM, vertexPoints);
                for (int r = 0; r < vertexPoints.GetLength(0); r++)
                {
                    for (int o = 0; o < vertexPoints.GetLength(1); o++)
                        vertexPos[r, o] = vertexPos[r, o] / vertexPos[3, 0];
                }
                vertices[i].pos = vertexPos;
            }
        }

        public static double[,] transformations(double alphaX, double alphaY, double cameraZ)
        {

            double[,] transformationMatrix = new double[4, 4]
           {
              {1,0,0,0},
              {0,1,0,0},
              {0,0,1,cameraZ},
              {0,0,0,1}
           };
            double[,] rotationX = new double[4, 4]
            {
              {1,0,0,0},
              {0,Math.Cos(alphaX),-Math.Sin(alphaX),0},
              {0,Math.Sin(alphaX),Math.Cos(alphaX),0},
              {0,0,0,1}
            };

            double[,] rotationY = new double[4, 4]
            {
              {Math.Cos(alphaY),0,Math.Sin(alphaY),0},
              {0,1,0,0},
              {-Math.Sin(alphaY),0,Math.Cos(alphaY),0},
              {0,0,0,1}
            };

            double[,] TR = MultiplyMatrix(MultiplyMatrix(transformationMatrix, rotationX), rotationY);
            return TR;
        }
        public static double[,] crossProd(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            double[,] crossProduct = new double[1, 3];
            crossProduct[0, 0] = (x2 * z2) - (z1 * y2);
            crossProduct[0, 1] = ((x1 * z2) - (x2 * z1)) * -1;
            crossProduct[0, 2] = (x1 * y2) - (y1 * x2);
            return crossProduct;
        }


        public static void drawMesh(triangle[] tringles, Canvas canvas)
        {
            Bitmap img = new Bitmap(1920, 1080);
            for (int i = 0; i < tringles.Length; i++)
            {
                triangle t = tringles[i];
                double x1 = t.vertex1.pos[0, 0];
                double y1 = t.vertex1.pos[1, 0];

                double x2 = t.vertex2.pos[0, 0];
                double y2 = t.vertex2.pos[1, 0];
                double x3 = t.vertex3.pos[0, 0];
                double y3 = t.vertex3.pos[1, 0];
                double[,] crossProduct = new double[1, 3];

                double nx = x2 - x1;
                double ny = y2 - y1;
                double nz = 0;
                double nxb = x3 - x1;
                double nyb = y3 - y1;
                double nzb = 0;
                crossProduct = crossProd(nx, ny, nz, nxb, nyb, nzb);
                if (crossProduct[0, 2] > 0)
                {
                    drawTriangle(canvas, t);
                    
                }
            }

        }

        public static vertex[] generateTexturedCordinates(vertex[] vertices, double radius, int m, int n)
        {
            vertex[] texturedCoordinates = new vertex[vertices.Length];
            texturedCoordinates[0] = new vertex(1, 0.5, 1, radius);
            texturedCoordinates[m * n + 1] = new vertex(0, 0.5,1,radius);

            for(int i = 0; i < n - 1; i++)
            {
                for(int j = 0; j < m - 1; j++)
                {
                    texturedCoordinates[i * m + j + 1] = new vertex(j / (m - 1), (i + 1) / (n + 1),1,radius);
                }
            }
             //don't forget to project vertices
            return texturedCoordinates;
        }

      /*  public vertex void interpolate(vertex textured)
        {
            double xNew = textured.x + 2;

            double yNew = 
        }
  */
    }
}
