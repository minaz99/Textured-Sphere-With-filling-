using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace project5
{
   public static class Filling
    {
        public static (double, double) getMin(line x, line y)
        {
            if (x.pointsI[1] > y.pointsI[1]) return (y.pointsI[1], y.pointsI[0]);
            else return (x.pointsI[1], x.pointsI[0]);
        }
        public static double getMax(double x, double y)
        {
            if (x > y) return x;
            else return y;
        }
        public static double[] globalEdgeTableInitializing(line pointA, line pointB, int val)
        {
            double[] components = new double[3];
            (components[0], components[1], components[2]) = (-1, -1, -1);
            var minItems = getMin(pointA, pointB);
            double maxY = getMax(pointA.pointsI[1], pointB.pointsI[1]);
            double minY = minItems.Item1;
            double xForMinY = minItems.Item2;
            //Console.WriteLine($"({pointA.pointsI[0]}, {pointA.pointsI[1]})");
            //Console.WriteLine($"({pointB.pointsI[0]}, {pointB.pointsI[1]})");           
            if (val == -1) return components;
            else
            {
                double inverseGradient = (pointB.pointsI[0] - pointA.pointsI[0]) / (pointB.pointsI[1] - pointA.pointsI[1]);
                components[0] = maxY;
                components[1] = xForMinY;
                components[2] = inverseGradient;
                return components;
            }
        }

        public static bool pointAlreadyExists(List<int[]> alphabets, double x, double y)
        {
            foreach (int[] edge in alphabets)
            {
                if ((edge[0] == x && edge[1] == y) || (edge[0] == y && edge[1] == x)) return true;
            }
            return false;
        }

        public static List<double[]>[] reOrderEdgeTable(List<double[]>[] GET, int currentNode)
        {
            double yVAL1 = GET[currentNode][0][0];
            double yVal2 = GET[currentNode][1][0];
            if (yVAL1 != -1 && yVal2 != -1)
            {
                if (yVAL1 > yVal2)
                {
                    double[] temp = GET[currentNode][0];
                    GET[currentNode][0] = GET[currentNode][1];
                    GET[currentNode][1] = temp;
                }
            }
            return GET;
        }
        public static List<double[]>[] createGlobalEdgeTable(Polygon polygon)
        {
            //we start by creating global edge which ranges from min y to max y
            double minY = Drawing.RoundToSignificantDigits(polygon.getShape().Min(y => y.pointsI[1]),3);
            double maxY = Drawing.RoundToSignificantDigits(polygon.getShape().Max(y => y.pointsF[1]),3);
            //Console.WriteLine($"Max is: {maxY} and minY is {minY}"});
            List<double[]>[] GET = new List<double[]>[(int)maxY + 1];
            List<line> sortedLines = polygon.getShape();
            List<int[]> alphabetPoints = new List<int[]>();
            //Console.WriteLine(sortedLines.Count);
            for (int i = 0; i <= maxY; i++)
            {
                GET[i] = new List<double[]>();
            }

            int nextPoint = 0;
            // int currentLine = 0;
            //foreach(Line l in sortedLines) Console.WriteLine($"({l.pointsI[0]}, {l.pointsI[1]})");

            for (int i = 0; i <= maxY; i++)
            {
                int processed = 0;
                foreach (int[] letter in alphabetPoints) Console.WriteLine(letter[0] + " " + letter[1]);
                //Console.WriteLine("done");
                foreach (line l in sortedLines)
                {
                    //if (processed == 1) break;

                    // processed = 1;
                    for (int m = 0; m < 2; m++)
                    {
                        int index = sortedLines.IndexOf(l);
                        if (index == 0)
                        {
                            if (m == 0) nextPoint = sortedLines.Count - 1;
                            if (m == 1) nextPoint = index + 1;
                        }
                        else if (index > 0 && index < sortedLines.Count - 1)
                        {
                            if (m == 0) nextPoint = index - 1;
                            if (m == 1) nextPoint = index + 1;
                        }
                        else if (index == sortedLines.Count - 1)
                        {
                            if (m == 0) nextPoint = index - 1;
                            if (m == 1) nextPoint = 0;
                        }
                        if (l.pointsI[1] == i && pointAlreadyExists(alphabetPoints, sortedLines.IndexOf(l), nextPoint) == false)
                        {
                            if (l.pointsI[1] == i && pointAlreadyExists(alphabetPoints, index, nextPoint) == false)
                            {
                                processed++;
                                int[] letter = { index, nextPoint };
                                alphabetPoints.Add(letter);
                                GET[i].Add(globalEdgeTableInitializing(l, sortedLines[nextPoint], 1));
                            }
                        }

                    }
                }
                if (processed < 2)
                {
                    if (processed == 0)
                    {
                        GET[i].Add(globalEdgeTableInitializing(sortedLines[0], sortedLines[nextPoint], -1));
                        GET[i].Add(globalEdgeTableInitializing(sortedLines[0], sortedLines[nextPoint], -1));
                    }
                    else if (processed == 1) GET[i].Add(globalEdgeTableInitializing(sortedLines[0], sortedLines[nextPoint], -1));

                }
                GET = reOrderEdgeTable(GET, i);
            }
            return GET;
        }


        public static double getMaxYOfAll(List<line> lines)
        {
            double maxY = -1;
            foreach (line l in lines)
            {
                if (l.getInitialPoints()[1] > maxY) maxY = l.getInitialPoints()[1];
            }
            return maxY;
        }

        public static double getMinYOfAll(List<line> lines)
        {
            double minY = 2000;
            foreach (line l in lines)
            {
                if (l.getInitialPoints()[1] < minY) minY = l.getInitialPoints()[1];
            }
            return minY;
        }

        public static int nodeToDelete(List<double[]> AET, int currentIndex)
        {
            if (AET[0][0] == currentIndex)
            {
                return 0;
            }
            else if (AET[1][0] == currentIndex) return 1;
            Console.WriteLine("well well what do we have here");
            return -1;
        }

        public static List<double[]> reshiftArray(List<double[]> AET, double[] nodeToAdd, int i)
        {
            if (nodeToDelete(AET, i) == 0)
            {
                AET[0] = nodeToAdd;
            }
            else if (nodeToDelete(AET, i) == 1)
            {
                double[] tempNode = AET[0];
                AET[0] = nodeToAdd;
                AET[1] = tempNode;
            }
            return AET;
        }
        public static (List<double[]>, List<double[]>, List<double[]>) createActiveEdgeTable(List<double[]>[] GET, double minYToStartReadingFrom, double yMax)
        {
            List<double[]> initialPoints = new List<double[]>();
            List<double[]> finalPoints = new List<double[]>();
            List<double[]> AET = new List<double[]>();
            AET.Add(GET[(int)minYToStartReadingFrom][0]);
            AET.Add(GET[(int)minYToStartReadingFrom][1]);
            double[] initialPoint = new double[2];
            double[] finalPoint = new double[2];
            (initialPoint[0], initialPoint[1]) = (AET[0][1], minYToStartReadingFrom);
            (finalPoint[0], finalPoint[1]) = (AET[1][1], minYToStartReadingFrom);
            initialPoints.Add(initialPoint);
            finalPoints.Add(finalPoint);
            //foreach (int[] n in AET)
            //{
            //    Console.WriteLine($"[{n[0]}|{n[1]}|{n[2]}]");
            //}

            Console.WriteLine(AET[0][0]);
            Console.WriteLine(AET[1][0]);
            Console.WriteLine(AET[0][1]);
            Console.WriteLine(AET[1][1]);

            for (int i = (int)(minYToStartReadingFrom + 1); i <= yMax; i++)
            {
                if (AET[0][0] == AET[1][0] && AET[0][1] == AET[1][1]) break;               
                for (int j = 0; j < 2; j++)
                {
                    // int dont = -1;
                    if (GET[i][j][0] != -1)
                    {
                        // dont = 0;
                        // AET.RemoveAt(nodeToDelete(AET, i));                                 
                        AET = reshiftArray(AET, GET[i][j], i);
                        foreach (double[] n in AET)
                        {
                            Console.WriteLine($"{i}[{n[0]}|{n[1]}|{n[2]}]");
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            AET[0][1] += AET[0][2];
                            AET[1][1] += AET[1][2];
                            //Console.WriteLine(AET[0][1] + " " + AET[0][2]);
                            foreach (double[] n in AET)
                            {

                                Console.WriteLine($"{i}[{n[0]}|{n[1]}|{n[2]}]");
                               
                            }
                        }
                    }
                    if (GET[i][j][0] != -1 || j == 0)
                    {
                        initialPoint = new double[2];
                        finalPoint = new double[2];
                        (initialPoint[0], initialPoint[1]) = (AET[0][1], i);
                        (finalPoint[0], finalPoint[1]) = (AET[1][1], i);
                        initialPoints.Add(initialPoint);
                        finalPoints.Add(finalPoint);
                        //foreach (int[] n in AET)
                        //{

                        //    Console.WriteLine($"{i}[{n[0]}|{n[1]}|{n[2]}]");
                        //}
                    }

                }
            }

            return (initialPoints, finalPoints, AET);
        }



        public static void fillPolygon(Canvas canvas, Polygon p, System.Windows.Media.Brush b)
        {
            
            List<line> lines = p.getShape();
            foreach (line l in lines)
            {
               // for (int i = 0; i < l.lineCords.Length; i++)
               // {
                    Drawing.DDA(canvas,l.pointsI[0],l.pointsI[1],l.pointsF[0],l.pointsF[1], b);
                //}
            }
            
        }

        public static int pixelReCalibrate(int pixelVal, Bitmap img, int hOrW)
        {
            if (hOrW == 1) //means x axis
                if (pixelVal > img.Width - 1) return img.Width - 1;
                else if (pixelVal < 0) return 0;
                else return pixelVal;
            else
            {
                if (pixelVal > img.Height - 1) return img.Height - 1;
                else if (pixelVal < 0) return 0;
                else return pixelVal;
            }
        }

        public static void fillPolygonWithImg(Canvas canvas, Polygon p, BitmapImage imageToFill)
        {
            //  int xi = 0;
            //  int yi = 0;
           // Bitmap imgOriginal = Image.BitmapImage2Bitmap(img);
            Bitmap imgF = Image.BitmapImage2Bitmap(imageToFill);
            List<line> lines = p.getShape();
            //hanym
            foreach (line l in lines)
            {
                //l.lineCordsColors = new Color[l.lineCords.Length][];

                
                    //l.lineCordsColors[i] = new Color[1];
                    int x = pixelReCalibrate((int)l.pointsI[0], imgF, 1);
                    int y = pixelReCalibrate((int)l.pointsF[1], imgF, 0);
                    System.Drawing.Color c = imgF.GetPixel(x, y);
                    //l.lineCordsColors[i][0] = imgF.GetPixel(x, y);
                    System.Windows.Media.Brush fillColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B));

                    Drawing.DDA(canvas, l.pointsI[0], l.pointsI[1], l.pointsF[0], l.pointsF[1],fillColor);
                    //imgOriginal.SetPixel((int)l.lineCords[i][0], (int)l.lineCords[i][1], imgF.GetPixel(x, y));
                    //img = DDAForFillingImages(img, initialPoints[i][0], initialPoints[i][1], finalPoints[i][0], finalPoints[i][1], imgF);                 
                

            }
            
        }


    }
}
