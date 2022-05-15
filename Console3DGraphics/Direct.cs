namespace Console3DGraphics
{
    class Direct
    {
        public Point margin { get; set; }
        public Point coefficient { get; set; }
        public Direct(Point A, Point B)
        {
            margin = A;
            coefficient = B.getSubstracted(A);
        }
    }
}
