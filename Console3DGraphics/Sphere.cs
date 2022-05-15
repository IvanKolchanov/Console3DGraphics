using System.Collections.Generic;

namespace Console3DGraphics
{
    class Sphere
    {
        public Point center { get; set; }
        public double radius { get; set; }
        public Sphere(Point center, double radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public Point[] intersect(Direct direct)
        {
            List<Point> points = new List<Point>();
            double vx = direct.coefficient.x, vy = direct.coefficient.y, vz = direct.coefficient.z;
            double px = direct.margin.x, py = direct.margin.y, pz = direct.margin.z;
            double cx = center.x, cy = center.y, cz = center.z;
            double a = vx * vx + vy * vy + vz * vz;
            double b = 2.0 * (px * vx + py * vy + pz * vz - vx * cx - vy * cy - vz * cz);
            double c = px * px - 2 * px * cx + cx * cx + py * py - 2 * py * cy + cy * cy + pz * pz - 2 * pz * cz + cz * cz - radius * radius;
            double d = b * b - 4 * a * c;
            if (d == 0)
            {
                double t = (-b / 2.0 * a);
                points.Add(new Point(px + t * vx, py + t * vy, pz + t * vz));
                return points.ToArray();
            }
            if (d > 0)
            {
                double t1 = (-b - System.Math.Sqrt(d)) / (2.0 * a);
                double t2 = (-b + System.Math.Sqrt(d)) / (2.0 * a);
                points.Add(new Point(px + t1 * vx, py + t1 * vy, pz + t1 * vz));
                points.Add(new Point(px + t2 * vx, py + t2 * vy, pz + t2 * vz));
                return points.ToArray();
            }
            return null;
        }
    }
}
