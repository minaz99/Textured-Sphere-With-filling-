using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace project5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BitmapImage imageToFillShapeWith;
        double alphaX = 0;
        double alphaY = 0;
        double cameraZ = 20;
        BitmapImage img;
        System.Drawing.Color c = System.Drawing.Color.FromArgb(255, 255, 255);
        List<Polygon> FilledPolygons = new List<Polygon>();
        int subD = 15;
        int r = 10;
        int fillingMode = 0;
        Brush fillColor;
        public MainWindow()
        {
            InitializeComponent();
            foreach (PropertyInfo prop in typeof(Colors).GetProperties())
            {
                colorSelection.Items.Add(prop.Name);
            }
            Drawing.DDA(canvas, 50, 50, 51, 50, Brushes.Red);
            canvas.Measure(new Size(1920, 1080));
            canvas.Arrange(new Rect(0, 0, canvas.DesiredSize.Width, canvas.DesiredSize.Height));
            //imageToStore = Image.initializeImage(1920, 1080);
            // image.Source = img;
            img = Image.initializeImage(1920, 1080);
            int m = subD;
            int n = subD;
            int radius = r;
            vertex[] vertices = Drawing.generateVertices(m, n, radius);
            double[,] transformations = Drawing.transformations(alphaX, alphaY, cameraZ);

            Drawing.project_vertices(vertices, transformations);
            var items2 = Drawing.createTriangles(vertices, m, n);
            triangle[] triangles = items2.Item1;
            List<Polygon> polygons = items2.Item2;
            Drawing.drawMesh(triangles, canvas);





            /////////////////////////////
            //foreach (Polygon p in polygons)
            // {

           
          
            foreach(Polygon p in polygons) { 
                    //p = polygons[j];

                //Console.WriteLine(j);

                double maxY = Filling.getMaxYOfAll(p.getShape());
                double minY = Filling.getMinYOfAll(p.getShape());

                List<double[]>[] GET = new List<double[]>[(int)maxY + 1];
                GET = Filling.createGlobalEdgeTable(p);
                foreach (List<double[]> li in GET)
                {
                    if (li[0][0] != -1 && li[1][0] != -1)
                        Console.WriteLine($"[{li[0][0]}|{li[0][1]}|{li[0][2]}]->[{li[1][0]}|{li[1][1]}|{li[1][2]}]");
                    else if (li[1][0] == -1 && li[0][1] != -1)
                        Console.WriteLine($"[{li[0][0]}|{li[0][1]}|{li[0][2]}]");
                    else if (li[1][0] != -1 && li[0][1] == -1)
                        Console.WriteLine($"[{li[1][0]}|{li[1][1]}|{li[1][2]}]");

                    else
                        Console.WriteLine(li[0][0]);
                }

                var items = Filling.createActiveEdgeTable(GET, minY, maxY);
                List<double[]> initialPoints = items.Item1;
                List<double[]> finalPoints = items.Item2;

                Polygon filledPolygon = new Polygon(2, c);
                for (int i = 0; i < finalPoints.Count - 1; i++)
                {
                    //Console.WriteLine($"({initialPoints[i][0]},{initialPoints[i][1]}) , ({finalPoints[i][0]},{finalPoints[i][1]})");
                    double dx = finalPoints[i][0] - initialPoints[i][0];
                    double dy = finalPoints[i][1] - initialPoints[i][1];

                    int steps = (int)(Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy));
                    line filledLine = new line(0, initialPoints[i][0], initialPoints[i][1], finalPoints[i][0], finalPoints[i][1], c, Drawing.getLinePoints(initialPoints[i][0], initialPoints[i][1], finalPoints[i][0], finalPoints[i][1]), 1);
                    filledPolygon.addLine(filledLine);
                }
                FilledPolygons.Add(filledPolygon);             
            }
           
        }

        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            imageToFillShapeWith = null;
        }

        private void showCurrentColor(Object sender, RoutedEventArgs e)
        {
            //var values = typeof(Brushes).GetProperties().Select(b => new { Name = b.Name, Brush = b.GetValue(null) as Brush }).ToArray();
            fillingMode = 1;
            string value = colorSelection.SelectedItem.ToString();
            PropertyInfo prop = typeof(Colors).GetProperty(value);
            c = System.Drawing.Color.FromName(prop.Name);
            fillColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(c.A,c.R, c.G, c.B));
            //fillColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(c.R,c.G,c.B));
            //System.Windows.Media.SolidColorBrush brush  = (System.Windows.Media.SolidColorBrush)prop.GetValue(null, null); 

        }

        private void renderSphere()
        {
            InitializeComponent();
            int m = subD;
            int n = subD;
            int radius = r;
            vertex[] vertices = Drawing.generateVertices(m, n, radius);
            double[,] transformations = Drawing.transformations(alphaX, alphaY, cameraZ);
            Drawing.project_vertices(vertices, transformations);
            var items2 = Drawing.createTriangles(vertices, m, n);
            triangle[] triangles = items2.Item1;
            List<Polygon> polygons = items2.Item2;
            Drawing.drawMesh(triangles, canvas);

            if (fillingMode == 1)
            {
                foreach (Polygon p in FilledPolygons)
                {
                    Filling.fillPolygonWithImg(canvas, p,imageToFillShapeWith);
                }              
            }
        }


        private void loadImg(Object sender, RoutedEventArgs e)
        {
            string imageName = "";
            if (imageToFillShapeWith == null)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.* ";
                if (dlg.ShowDialog() == true)
                {
                    Stream stream = File.Open(dlg.FileName, FileMode.Open);
                    BitmapImage imgsrc = new BitmapImage();
                    imgsrc.BeginInit();
                    imgsrc.StreamSource = stream;
                    imgsrc.EndInit();
                    //image.Source = imgsrc;
                    imageToFillShapeWith = imgsrc;
                    imageName = dlg.FileName;
                    //image.Source = imageToFillShapeWith;
                    Console.WriteLine(dlg.FileName);
                }
            }
            fillingMode = 1;
            foreach (Polygon p in FilledPolygons)
            {
                Filling.fillPolygonWithImg(canvas, p, imageToFillShapeWith);
            }
            //renderSphere();
           
        }

        private void window1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                alphaY -= 0.01;
            }
            if (e.Key == Key.Right)
            {
                alphaY += 0.01;
            }
            if (e.Key == Key.Up)
            {
                alphaX -= 0.01;
            }
            if (e.Key == Key.Down)
            {
                alphaX += 0.01;
            }
            if (e.Key == Key.OemPlus)
            {
                if (cameraZ > 13)
                    cameraZ -= 0.2;
            }
            if (e.Key == Key.OemMinus)
            {
                if (cameraZ < 80)
                    cameraZ += 0.2;
            }
            canvas.Children.Clear();
            renderSphere();
        }

        private void subdivisions_TextChanged(object sender, TextChangedEventArgs e)
        {
            // result;
            int result;
            if (int.TryParse(subdivisions.Text.ToString(), out result)) subD = result;
            
        }

        private void radius_TextChanged(object sender, TextChangedEventArgs e)
        {
            int result;
            if (int.TryParse(rads.Text.ToString(), out result)) r = result;
           
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (canvas != null)
            {
                canvas.Children.Clear();
                renderSphere();
            }
        }
    }
}
