using System.Drawing;
using Environments;

namespace Presenters
{
    public abstract class Presenter
    {
        protected Presenter()
        {
            this.minX = 0;
            this.minY = 0;
            this.maxX = 1;
            this.maxY = 1;
        }

        protected Presenter(double minX, double minY, double maxX, double maxY)
        {
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
        }

        public abstract void Draw();

        public void DrawLine(Pen pen, double fromX, double fromY, double toX, double toY)
        {
            lastUsedPosX = toX;
            lastUsedPosY = toY;
            Move(ref fromX, ref fromY);
            Move(ref toX, ref toY);
            Graphics.DrawLine(pen, (int)fromX, (int)fromY, (int)toX, (int)toY);
        }

        public void DrawLine(Pen pen, double toX, double toY)
        {
            DrawLine(pen, lastUsedPosX, lastUsedPosY, toX, toY);
        }

        public void FillCircle(Brush brush, double whereX, double whereY, double radius)
        {
            lastUsedPosX = whereX;
            lastUsedPosY = whereY;
            int radiusX = (int)(radius * scalarX);
            int radiusY = (int)(radius * scalarY);
            Move(ref whereX, ref whereY);
            Graphics.FillEllipse(brush, (int)(whereX - radiusX / 2), (int)(whereY - radiusY / 2), radiusX + 1, radiusY + 1);
        }

        public void FillRectangle(Brush brush, double whereX, double whereY, double width, double height)
        {
            lastUsedPosX = whereX;
            lastUsedPosY = whereY;
            width *= scalarX;
            height *= scalarY;
            Move(ref whereX, ref whereY);
            Graphics.FillRectangle(brush, (int)whereX, (int)whereY, (int)width, (int)height);
        }

        public void MoveTo(double toX, double toY)
        {
            lastUsedPosX = toX;
            lastUsedPosY = toY;
        }

        public void Resize(Graphics graphics, int width, int height)
        {
            this.Graphics = graphics;
            int scalar = width > height ? height : width;
            scalarX = (int)(scalar / (maxX - minX));
            scalarY = (int)(scalar / (maxY - minY));
            startX = width > height ? (width - height) / 2 : 0;
            startY = width > height ? height : (height + width) / 2;
        }

        protected void Move(ref double posX, ref double posY)
        {
            posX -= minX;
            posY -= minY;
            posX *= scalarX;
            posY *= -scalarY;
            posX += startX;
            posY += startY;
        }

        protected Graphics Graphics { get; private set; }

        private double lastUsedPosX;
        private double lastUsedPosY;

        private int scalarX;
        private int scalarY;
        private int startX;
        private int startY;
        private double minX;
        private double minY;
        private double maxX;
        private double maxY;
    }
}
