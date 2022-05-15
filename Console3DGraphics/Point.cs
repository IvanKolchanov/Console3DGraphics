using System;
namespace Console3DGraphics
{
    
    class Point
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static double abs(Point A, Point B)
        {
            Point C = A.getSubstracted(B);
            double length = Math.Sqrt(C.x * C.x + C.y * C.y + C.z * C.z);
            return length;
        }

        public void substract(Point B)
        {
            x -= B.x;
            y -= B.y;
            z -= B.z;
        }

        public Point getSubstracted(Point B)
        {
            return new Point(x - B.x, y - B.y, z - B.z);
        }

        public bool isEqual(Point B)
        {
            if (Math.Round(x, 4) != Math.Round(B.x, 4)) return false;
            if (Math.Round(y, 4) != Math.Round(B.y, 4)) return false;
            if (Math.Round(z, 4) != Math.Round(B.z, 4)) return false;
            return true;
        }
    }
}
