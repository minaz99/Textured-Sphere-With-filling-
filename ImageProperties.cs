using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace project5
{
    public abstract class ImageProperties
    {

        // public BitmapImage img = new BitmapImage();

        public int classNo;
        public Color c;
        public ImageProperties(int classId, Color color)
        {
            c = color;
            classNo = classId;
        }

        public abstract double[] getInitialPoints();
        public abstract double[] getFinalPoints();
        public abstract List<line> getShape();
        public abstract void addLine(line line);
        public abstract float[] getTopRight();
        public abstract float[] getTopLeft();

        public abstract float[] getBottomRight();

        public abstract float[] getBottomLeft();
        public abstract double[][] getLineCords();
        public abstract int isFilled();
        public abstract Color[][] getLineCordsColrs();
        public abstract BitmapImage getImage();
        public abstract string getImageName();

    }

   
    public class line : ImageProperties
    {
        public double[] pointsI = new double[2];
        public double[] pointsF = new double[2];
        public double[][] lineCords;
        public Color[][] lineCordsColors;
        public line(int classNo, double xI, double yI, double xF, double yF, Color c, double[][] cords, int size) : base(classNo, c)
        {
            pointsI[0] = xI;
            pointsI[1] = yI;
            pointsF[0] = xF;
            pointsF[1] = yF;
            lineCords = new double[size][];
            lineCordsColors = new Color[size][];
            for (int i = 0; i < size; i++)
            {
                lineCords[i] = new double[2];
                lineCordsColors[i] = new Color[2];
            }
            lineCords = cords;
        }

        public void setColorsToPoints(Color[][] c)
        {
            c = lineCordsColors;
        }
        public Color[][] getColorOfPoints()
        {
            return lineCordsColors;
        }
        public override void addLine(line line)
        {
            throw new NotImplementedException();
        }

        public override float[] getBottomLeft()
        {
            throw new NotImplementedException();
        }

        public override float[] getBottomRight()
        {
            throw new NotImplementedException();
        }

        public override double[] getFinalPoints()
        {
            return pointsF;
        }

        public override double[] getInitialPoints()
        {
            return pointsI;
        }

        public override double[][] getLineCords()
        {
            return lineCords;
        }

        public override List<line> getShape()
        {
            throw new NotImplementedException();
        }

        public override float[] getTopLeft()
        {
            throw new NotImplementedException();
        }

        public override float[] getTopRight()
        {
            throw new NotImplementedException();
        }

        public override int isFilled()
        {
            throw new NotImplementedException();
        }

        public override Color[][] getLineCordsColrs()
        {
            return lineCordsColors;
        }

        public override BitmapImage getImage()
        {
            throw new NotImplementedException();
        }

        public override string getImageName()
        {
            throw new NotImplementedException();
        }
    }

   
    public class Polygon : ImageProperties
    {
        public List<line> linesOfPolygon;
        public int filled = 0;
        public string ImageName;
        public BitmapImage polygonImg;

        public Polygon(int classNo, Color c) : base(classNo, c)
        {
            linesOfPolygon = new List<line>();

        }
        public Polygon(int classNo, Color c, int isFilled, BitmapImage img) : base(classNo, c)
        {
            linesOfPolygon = new List<line>();
            filled = isFilled;
            polygonImg = img;
        }
        public override void addLine(line line)
        {
            linesOfPolygon.Add(line);
        }

        public override float[] getBottomLeft()
        {
            throw new NotImplementedException();
        }

        public override float[] getBottomRight()
        {
            throw new NotImplementedException();
        }

        public override double[] getFinalPoints()
        {
            throw new NotImplementedException();
        }

        public override BitmapImage getImage()
        {
            return polygonImg;
        }

        public override string getImageName()
        {
            return ImageName;
        }

        public override double[] getInitialPoints()
        {
            throw new NotImplementedException();
        }

        public override double[][] getLineCords()
        {
            throw new NotImplementedException();
        }

        public override Color[][] getLineCordsColrs()
        {
            throw new NotImplementedException();
        }

        public override List<line> getShape()
        {
            return linesOfPolygon;
        }

        public override float[] getTopLeft()
        {
            throw new NotImplementedException();
        }

        public override float[] getTopRight()
        {
            throw new NotImplementedException();
        }

        public override int isFilled()
        {
            return filled;
        }
    }
}
