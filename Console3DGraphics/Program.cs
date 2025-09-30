
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Console3DGraphics
{
    class Program
    {
        public static int width = Console.LargestWindowWidth / 3, height = Console.LargestWindowHeight / 3;
        public static double aspect = (double)width / height, pixelAspect = 0.5;
        private static Point camera;
        public static Point lightSource = new Point(0, -1, 2);
        private static int fov = 150;
        private static List<Direct> screenDirects = new List<Direct>();
        private static Sphere body1 = new Sphere(new Point(0, 1, 0), 1.1);
        private static char[] gradient = new char[] {' ', '.', ':', '!', '/', 'r', '(', 'l', '1', 'Z', '4', 'H', '9', 'W', '8', '$', '@' };
        private static List<PointOnScreen> pointOnScreen = new List<PointOnScreen>();
        private static bool firstIteraction = true;

        class PointOnScreen
        {
            public int i { get; set; }
            public int j { get; set; }
            public PointOnScreen(int i, int j)
            {
                this.i = i;
                this.j = j;
            }
        }

        private static void findScreenPosition()
        {
            double fovToRads = Math.PI * fov / 180;
            double w = 2 * aspect * pixelAspect;
            double a = w / Math.Sqrt(2 - 2 * Math.Cos(fovToRads));
            double distanceToCamera = Math.Sqrt(a * a - w * w * 0.25);
            camera = new Point(0, -distanceToCamera, 0);
            //Screen's center is located in (0, 0, 0)
            //Screen's rightes x is 1.777, leftest is -1.777
            //Screen's highest z is 1 lowest is -1
        }

        private static void findScreenDirects()
        {
            double x0, z0;
            for (int j = 0; j < height; j++)
            {
                z0 = jToZ(j);
                for (int i = 0; i < width; i++)
                {
                    x0 = iToX(i);
                    screenDirects.Add(new Direct(camera, new Point(x0, 0, z0)));
                }
            }
        }

        private static void traceRays()
        {
            int z = -1;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    z++;
                    if (!pointOnScreen.Exists(point => point.i == i && point.j == j) && !firstIteraction) continue;
                    Direct direct = screenDirects[z];
                    Point[] points = body1.intersect(direct);
                    if (points != null)
                    {
                        Point intersectionPoint = null;
                        if (points.Length == 1) intersectionPoint = points[0];
                        else
                        {
                            if (points[0].y < points[1].y) intersectionPoint = points[0];
                            else intersectionPoint = points[1];
                        }
                        Direct direct1 = new Direct(intersectionPoint, lightSource);
                        Point[] lightPoints = body1.intersect(direct1);
                        if (lightPoints.Length == 0)
                        {
                            Console.Write("@");
                        }
                        else
                        {
                            if (firstIteraction) pointOnScreen.Add(new PointOnScreen(i, j));
                            double length1 = Point.abs(lightSource, lightPoints[0]);
                            double length2 = Point.abs(lightSource, lightPoints[1]);
                            double oneColor = 0.8 / gradient.Length;
                            int color = 0;
                            if (lightPoints[0].isEqual(intersectionPoint)) color = Math.Max(0, Math.Min(gradient.Length - 1, (int)((length2 / length1 - 0.6) / oneColor)));
                            else color = Math.Max(0, Math.Min(gradient.Length - 1, (int)((length1 / length2 - 0.6) / oneColor)));
                            //Console.SetCursorPosition(i, j);
                            //Console.Write(gradient[color]);
                            FConsole.SetChar((short)i, (short)j, new PixelValue(ConsoleColor.White, ConsoleColor.Black, gradient[color]));
                        }
                    }
                    else
                    {
                        FConsole.SetChar((short)i, (short)j, new PixelValue(ConsoleColor.White, ConsoleColor.Black, ' '));
                    }
                }
            }
            if (firstIteraction) firstIteraction = false;
        }

        static void Main(string[] args)
        {
            Console.SetWindowSize(width, height);
            Console.BufferWidth = width;
            Console.Title = "Console 3D graphics";
            findScreenPosition();
            findScreenDirects();
            FConsole.Clear();
            FConsole.Initialize("Run video to symbols", ConsoleColor.White, ConsoleColor.Black);
            while (true)
            {
                Console.CursorVisible = false;
                traceRays();
                FConsole.DrawBuffer();
                lightSource = turnVector(lightSource, 15);
            }
        }

        public static double iToX(int i)
        {
            double x = ((double)i / width) * 2 - 1;
            x *= aspect * pixelAspect;
            return x;
        }

        public static double jToZ(int j)
        {
            double z = ((double)(height - 1 - j) / height) * 2 - 1;
            return z;
        }
        public static Point turnVector(Point point, int angelInDegrees)
        {
            double angelInRads = Math.PI * angelInDegrees / 180;
            double cos = Math.Cos(angelInRads), sin = Math.Sin(angelInRads);
            double newX = point.x * cos - point.y * sin;
            double newY = point.x * sin + point.y * cos;
            return new Point(newX, newY, point.z);
        }
    }
}
