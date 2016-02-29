using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace TileMapManager
{
    public partial class TileMapViewCtrl : UserControl
    {
        TileMapLayer layer = new TileMapLayer();
        double mapScale = 1;//������
        double mapCenX = 100;
        double mapCenY = 100;
        private bool showGrid=false;//�Ƿ���ʾ����
        MapRect mapRect = new MapRect();//��ǰ��ʾ��Χ
        //��ʱ����
        //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        public TileMapViewCtrl()
        {
            InitializeComponent();
            mapScale = 2000;
            mapCenX = 1000;
            mapCenY = 985;
            //watch.Reset();
        }
        /// <����ͼ��·��>
        /// ����ͼ��·��
        /// </����ͼ��·��>
        /// <param name="layerPath">��ͼ·��</param>
        /// <returns>ͼ�������Ƿ�ɹ�</returns>
        public bool SetLayerPath(String layerPath)
        {
            layer = new TileMapLayer();
            return layer.ReadTileInfor(layerPath);
        }
        /// <��ȡ��ͼ�� ��ǰ����ͱ�������Ϣ>
        /// ��ȡ��ͼ�� ��ǰ����ͱ�������Ϣ
        /// </��ȡ��ͼ�� ��ǰ����ͱ�������Ϣ>
        /// <param name="valx"></param>
        /// <param name="valy"></param>
        /// <param name="valscale"></param>
        public void GetCoordsAndScaleInfors(out double valx, out double valy, out double valscale)
        {
            valx = currentMapPoint.x;
            valy = currentMapPoint.y;
            valscale = mapScale;
        }

        #region ����ת������ʾ��Χ ����Ҫ���㷨

        /// <summary>
        /// ���¼��㵱ǰ��ͼ�ؼ���ʾ�ĵ���Χ
        /// </summary>
        private void ReCalculateMapRect()
        {
            double resolution = (25.39999918 / layer.Dpi) * mapScale / 1000;
            mapRect.xMin = mapCenX - resolution * mapPicBox.Width / 2;
            mapRect.yMin = mapCenY - resolution * mapPicBox.Height / 2;
            mapRect.xMax = mapCenX + resolution * mapPicBox.Width / 2;
            mapRect.yMax = mapCenY + resolution * mapPicBox.Height / 2;
        }

        /// <summary>
        /// ����������תΪ��ͼ������
        /// </summary>
        /// <param name="vx">�����е�x</param>
        /// <param name="vy">�����е�y</param>
        /// <param name="wx">��Ӧ��ͼ��x</param>
        /// <param name="wy">��Ӧ��ͼ��y</param>
        /// <returns></returns>
        public void CoordViewToMap(int vx, int vy, out double wx, out double wy)
        {
            double resolution = 0;
            if (layer == null)
                resolution = (25.39999918 / 96) * mapScale / 1000;
            else 
                resolution = (25.39999918 / layer.Dpi) * mapScale / 1000;
            wx = mapCenX - (mapPicBox.Width / 2 - vx) * resolution;
            wy = mapCenY + (mapPicBox.Height / 2 - vy) * resolution;
        }
        private List<Point> CoordMapToViewPointList(List<MapPoint> mapListP)
        {
            double resolution = (25.39999918 / layer.Dpi) * mapScale / 1000;
            List<Point> viewListP = new List<Point>();
            Point tmpViewP = new Point();
            foreach (MapPoint mapP in mapListP)
            {
                tmpViewP.X = Convert.ToInt32((mapP.x - mapCenX) / resolution + mapPicBox.Width / 2);
                tmpViewP.Y = Convert.ToInt32(mapPicBox.Height / 2-(mapP.y - mapCenY) / resolution);
                viewListP.Add(tmpViewP);
            }
            return viewListP;
        }
        /// <summary>
        /// λ��ת�� �ɴ�����λ��תΪ��ͼλ��
        /// </summary>
        /// <param name="orgP">��ʼ��</param>
        /// <param name="desP">��ֹ��</param>
        /// <param name="dx">xƫ��</param>
        /// <param name="dy">yƫ��</param>
        public void TransWindToMap(Point orgP, Point desP, out double dx, out double dy)
        {
            double resolution = (25.39999918 / layer.Dpi) * mapScale / 1000;
            dx = (orgP.X - desP.X) * resolution;
            dy = (desP.Y - orgP.Y) * resolution;
        }

        /// <summary>
        /// ͼƬ�ƶ� ����϶���Ƭʱ ����ͼ�϶���ʾ ���ⲻ��ȥ��ȡ��Ӧ����Ƭ ͬʱ�ӳ�
        /// δ���PictureBoxʱ�÷�
        /// </summary>
        /// <param name="orgP">��ʼ��</param>
        /// <param name="desP">��ֹ��</param>
        /// <param name="imgRectToShow">�ӵ�ͼ�Ͻ�ȡ�ľ��η�Χ</param>
        /// <param name="viewRectToShow">��ʾ�ڴ����еľ��η�Χ</param>
        private void ImgTransImgToView(Point orgP, Point desP, out Rectangle imgRectToShow, out Rectangle viewRectToShow)
        {
            //��ͼ�ϵ�λ��
            int vdx = desP.X - orgP.X;
            int vdy = desP.Y - orgP.Y;
            //��ͼ�е�λ�� ���ڵ�ģ���� ��ͼ��λͼһ���󣨼��ٴ洢�ռ� ������ٶȣ�
            int idx = vdx;
            int idy = vdy;
            //ͼ��ü���
            int istartx = 0, istarty = 0;//�ü��� ��ʼ��
            int imgW = 0, imgH = 0;//�ü��� �߿�
            if (idx > 0)
            {
                istartx = 0;
                imgW = mapImg.Width - idx;
            }
            else
            {
                istartx = -idx;
                imgW = mapImg.Width + idx;
            }
            if (idy > 0)
            {
                istarty = 0;
                imgH = mapImg.Height - idy;
            }
            else
            {
                istarty = -idy;
                imgH = mapImg.Height + idy;
            }
            imgRectToShow = new Rectangle(istartx, istarty, imgW, imgH);

            //ͼƬ�� ��ͼ�д�� ��ʾ����
            int vstartx = 0, vstarty = 0;//��ʾ�� ��ʼ��
            int viewW = 0, viewH = 0;//��ʾ�� �߿�
            if (vdx > 0)
            {
                vstartx = vdx;
                viewW = mapPicBox.Width - vdx;
            }
            else
            {
                vstartx = 0;
                viewW = mapPicBox.Width + vdx;
            }
            if (vdy > 0)
            {
                vstarty = vdy;
                viewH = mapPicBox.Height - vdy;
            }
            else
            {
                vstarty = 0;
                viewH = mapPicBox.Height + vdy;
            }
            viewRectToShow = new Rectangle(vstartx, vstarty, viewW, viewH);

        }
        /// <summary>
        /// ��ȡ��ǰ��Ļ��ʼ��֮���Ӧ��ͼƬ
        /// </summary>
        /// <param name="startP">���</param>
        /// <param name="endP">�յ�</param>
        /// <returns>��Ӧ�����ͼƬ</returns>
        private Image ImgAreaImgToView(Rectangle viewRect)
        {
            if (viewRect.Width == 0 || viewRect.Height == 0) return null;
            Image tmpImg = new Bitmap(viewRect.Width, viewRect.Height);
            Graphics imgGraphic = Graphics.FromImage(tmpImg);
            imgGraphic.DrawImage(mapImg, new Rectangle(0, 0, tmpImg.Width, tmpImg.Height), viewRect.X, viewRect.Y, viewRect.Width, viewRect.Height, GraphicsUnit.Pixel);
            imgGraphic.Dispose();
            return tmpImg;
        }
        /// <summary>
        /// ��ȡ��Ļ�� �������㹹�ɵ� ����
        /// </summary>
        /// <param name="startxP">���</param>
        /// <param name="endP">�յ�</param>
        /// <returns></returns>
        private Rectangle GetRectFromTwoPoint(Point startxP, Point endP)
        {
            int rectX = 0;
            int rectY = 0;
            int rectW = 0;
            int rectH = 0;
            if (startxP.X > endP.X)
            {
                rectX = endP.X;
                rectW = startxP.X - endP.X;
            }
            else
            {
                rectX = startxP.X;
                rectW = endP.X - startxP.X;
            }

            if (startxP.Y > endP.Y)
            {
                rectY = endP.Y;
                rectH = startxP.Y - endP.Y;
            }
            else
            {
                rectY = startxP.Y;
                rectH = endP.Y - startxP.Y;
            }
            return new Rectangle(rectX, rectY, rectW, rectH);
        }


          // ��Ĳ��: AB * AC
        double cross(MapPoint A, MapPoint B, MapPoint C)
        {
            return (B.x - A.x) * (C.y - A.y) - (B.y - A.y) * (C.x - A.x);
        }
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="ployPointsList">����ε������</param>
        /// <returns>���</returns>
        private double GetPloygonArea(List<MapPoint> ployPointsList)
        {
            if (ployPointsList.Count < 3) return 0;

            double area = 0.0;

            int i;
            MapPoint temp;
            temp.x = temp.y = 0;//ԭ��
            for (i = 0; i < ployPointsList.Count - 1; ++i)
            {
                area += cross(temp, ployPointsList[i], ployPointsList[i + 1]);
            }
            area += cross(temp, ployPointsList[ployPointsList.Count - 1], ployPointsList[0]);//��β����
            area = area / 2.0;        //ע��Ҫ����2
            return area > 0 ? area : -area;    //���طǸ���
        }
        #endregion

        #region ��Ƭͼ�����ʾ����
        private Bitmap mapImg = null;//��ǰ��ʾ�ĵ�ͼ
        /// <summary>
        /// ˢ�µ�ͼ
        /// </summary>
        public void RefreshMap()
        {
            ReCalculateMapRect();
            mapImg = layer.GetLayerImage(mapScale, mapRect, mapPicBox.Size, showGrid);
            if (mapImg != null)
            {
                mapPicBox.BackgroundImage = mapImg;
            }
        }
        /// <summary>
        /// ��λ��ͼ
        /// </summary>
        public void RestoreMap()
        {
            MapRect totalRect = layer.TotalTileMapRect;
            mapCenX = (totalRect.xMax + totalRect.xMin) / 2;
            mapCenY = (totalRect.yMax + totalRect.yMin) / 2;
            double wk = totalRect.GetWidth() / mapPicBox.Width;
            double hk = totalRect.GetHeigh() / mapPicBox.Height;
            if (wk > hk)
                mapScale = wk * 1000 * layer.Dpi / 25.39999918;
            else
                mapScale = hk * 1000 * layer.Dpi / 25.39999918;
            RefreshMap();
        }

        /// <summary>
        /// ���̶�������С
        /// </summary>
        public void ZoomOut()
        {
            mapScale = mapScale * 1.25;
            double maxScale = layer.GetMaxScale();
            if (mapScale > maxScale * 4)//��С̫С ��ȡ�õ�minscale/4��Ϊ��ǰ������
            {
                mapScale = maxScale * 4;
            }
            ReCalculateMapRect();
            mapImg = layer.GetLayerImage(mapScale, mapRect, mapPicBox.Size, showGrid);
            if (mapImg != null)
            {
                mapPicBox.BackgroundImage = mapImg;
            }
        }
        /// <summary>
        /// ���̶������Ŵ�
        /// </summary>
        public void ZoomIn()
        {
            mapScale = mapScale * 0.8;
            double minScale = layer.GetMinScale();
            if (mapScale < minScale / 4)//��С̫С ��ȡ�õ�minscale/4��Ϊ��ǰ������
            {
                mapScale = minScale / 4;
            }
            ReCalculateMapRect();
            mapImg = layer.GetLayerImage(mapScale, mapRect, mapPicBox.Size, showGrid);
            if (mapImg != null)
            {
                mapPicBox.BackgroundImage = mapImg;
            }
        }
        /// <summary>
        /// ����ָ����������ʾ
        /// </summary>
        /// <param name="scale">ָ���ı�����</param>
        public void SetScale(double scale)
        {
            mapScale = scale;

            double minScale = layer.GetMinScale();
            if (mapScale < minScale / 4)//��С̫С ��ȡ�õ�minscale/4��Ϊ��ǰ������
            {
                mapScale = minScale / 4;
            }
            double maxScale = layer.GetMaxScale();
            if (mapScale > maxScale * 4)//��С̫С ��ȡ�õ�minscale/4��Ϊ��ǰ������
            {
                mapScale = maxScale * 4;
            }
            ReCalculateMapRect();
            mapImg = layer.GetLayerImage(mapScale, mapRect, mapPicBox.Size, showGrid);
            if (mapImg != null)
            {
                mapPicBox.BackgroundImage = mapImg;
            }
        }
        /// <summary>
        /// ����ָ����������ʾ
        /// </summary>
        /// <param name="scale">ָ���ı�����</param>
        public void SetScaleForMouseWeel(double scale, double curCenX, double curCenY)
        {
            double tmpMapScale = scale;
            double minScale = layer.GetMinScale();
            if (tmpMapScale < minScale / 4)//��С̫С ��ȡ�õ�minscale/4��Ϊ��ǰ������
            {
                tmpMapScale = minScale / 4;
            }
            double maxScale = layer.GetMaxScale();
            if (tmpMapScale > maxScale * 4)//��С̫С ��ȡ�õ�minscale/4��Ϊ��ǰ������
            {
                tmpMapScale = maxScale * 4;
            }
            mapCenX = curCenX - (curCenX - mapCenX) * scale / mapScale;
            mapCenY = curCenY - (curCenY - mapCenY) * scale / mapScale;
            mapScale = tmpMapScale;
            ReCalculateMapRect();
            mapImg = layer.GetLayerImage(mapScale, mapRect, mapPicBox.Size, showGrid);
            if (mapImg != null)
            {
                mapPicBox.BackgroundImage = mapImg;
            }
        }
        /// <summary>
        /// ����ͼ�����Ƶ�ָ��Ŀ��λ��
        /// </summary>
        /// <param name="desX">Ŀ���x</param>
        /// <param name="desY">Ŀ���y</param>
        public void MoveTo(double desX, double desY)
        {
            mapCenX = desX;
            mapCenY = desY;
            ReCalculateMapRect();
            mapImg = layer.GetLayerImage(mapScale, mapRect, mapPicBox.Size, showGrid);
            if (mapImg != null)
            {
                mapPicBox.BackgroundImage = mapImg;
            }
        }

        /// <summary>
        /// ��ʾ��ȡ��������ʾ
        /// </summary>
        public void setShowGrid()
        {
            showGrid = !showGrid;
        }
        #endregion

        #region ��Ƭ��ͼ���״̬���� ��Ϊָ�� �ƶ� �Ŵ� ��С״̬
        public enum BrowserStyle
        {
            MapDefaultPiont,//Ĭ�ϵ�ָ��״̬
            MapMove,//�ƶ�
            MapZoomIn,//�Ŵ�
            MapZoomOut//��С
        }
        private BrowserStyle mapBrowserStyle = BrowserStyle.MapDefaultPiont;
        private Cursor CursorZoomIn = new Cursor(Properties.Resources.ZoomIn.ToBitmap().GetHicon());
        private Cursor CursorZoomOut = new Cursor(Properties.Resources.ZoomOut.ToBitmap().GetHicon());
        public void SetMapBrowserStyle(BrowserStyle style)
        {
            mapBrowserStyle = style;
            switch (mapBrowserStyle)
            {
                case BrowserStyle.MapDefaultPiont:
                    {
                        mapPicBox.Cursor = Cursors.Arrow;
                    }
                    break;
                case BrowserStyle.MapMove:
                    {
                        mapPicBox.Cursor = Cursors.Hand;
                    }
                    break;
                case BrowserStyle.MapZoomIn:
                    {
                        mapPicBox.Cursor = CursorZoomIn;
                    }
                    break;
                case BrowserStyle.MapZoomOut:
                    {
                        mapPicBox.Cursor = CursorZoomOut;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region �¼���Ӧ ��� �� ��ͼ

        /// <summary>
        /// �����С�����ı�ʱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TileMapPictureBox_SizeChanged(object sender, EventArgs e)
        {
            if (mapPicBox.Width > 0 && mapPicBox.Height > 0)
            {
                if (layer != null)
                    RefreshMap();
            }
        }

        private Point dragOldPoint;//�϶�ǰ�����λ��
        bool dragTag = false;
        private void TileMapPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mapBrowserStyle == BrowserStyle.MapMove)//�ƶ���ͼ
                {
                    dragTag = true;
                    dragOldPoint = e.Location;
                    moveLastPoint = e.Location;
                }
                else if (mapBrowserStyle == BrowserStyle.MapZoomOut)//��С
                {
                    dragOldPoint = e.Location;
                }
                else if (mapBrowserStyle == BrowserStyle.MapZoomIn)//�Ŵ�
                {
                    dragTag = true;
                    dragOldPoint = e.Location;
                    //rectLastPoint = e.Location;
                }

                if (m_MeasureState == MeasureStyle.LinesMeasure && mapBrowserStyle == BrowserStyle.MapDefaultPiont)
                {
                    MapPoint tmpMapP;
                    CoordViewToMap(e.X, e.Y, out tmpMapP.x, out tmpMapP.y);
                    m_MeasureListPoint.Add(tmpMapP);
                    DrawFrontPage();
                }
                if (m_MeasureState == MeasureStyle.PloysMeasure && mapBrowserStyle == BrowserStyle.MapDefaultPiont)
                {
                    MapPoint tmpMapP;
                    CoordViewToMap(e.X, e.Y, out tmpMapP.x, out tmpMapP.y);
                    m_MeasureListPoint.Add(tmpMapP);
                    DrawFrontPage();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                SetMapBrowserStyle(BrowserStyle.MapDefaultPiont);
            }
        }

        private void TileMapPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mapBrowserStyle == BrowserStyle.MapMove)//�ƶ�
                {
                    if (dragTag == true)
                    {
                        double dx = 0; double dy = 0;
                        TransWindToMap(dragOldPoint, e.Location, out dx, out dy);
                        MoveTo(mapCenX + dx, mapCenY + dy);
                        dragTag = false;

                    }
                }
                else if (mapBrowserStyle == BrowserStyle.MapZoomOut)//��С
                {
                    //��Сʱ ����ͼ�����ƶ�����ǰ�� ������һ��������С
                    CoordViewToMap(e.X, e.Y, out mapCenX, out mapCenY);
                    ZoomOut();
                    dragTag = false;

                }
                else if (mapBrowserStyle == BrowserStyle.MapZoomIn)//�Ŵ�
                {
                    //��Ϊ����Ŵ� ������Ŵ�
                    if (e.X == dragOldPoint.X && e.Y == dragOldPoint.Y)//��������� ��ʾ����Ŵ�
                    {
                        CoordViewToMap(e.X, e.Y, out mapCenX, out mapCenY);
                        ZoomIn();
                    }
                    else
                    {
                        //lastImg = null;//�������� �������õĸ�Ϊ��
                        Rectangle zoomRect = GetRectFromTwoPoint(dragOldPoint, e.Location);
                        if (zoomRect.Width != 0 && zoomRect.Height != 0)
                        {
                            int cenx = (dragOldPoint.X + e.Location.X) / 2;
                            int ceny = (dragOldPoint.Y + e.Location.Y) / 2;
                            CoordViewToMap(cenx, ceny, out mapCenX, out mapCenY);
                            double tmpScale = 1;
                            if (mapPicBox.Width / zoomRect.Width < mapPicBox.Height / zoomRect.Height)
                            {
                                tmpScale = mapScale * zoomRect.Width / mapPicBox.Width;
                            }
                            else tmpScale = mapScale * zoomRect.Height / mapPicBox.Height;
                            SetScale(tmpScale);
                            mapPicBox.Image = null;//ǰͼ�㲻�ɼ�
                        }
                    }
                    dragTag = false;
                }

                DrawFrontPage();
            }

        }

        Point moveLastPoint;//�����ƶ�
        //Point rectLastPoint;//��������
        //Image lastImg = null;
        MapPoint currentMapPoint=new MapPoint();
        private void TileMapPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            CoordViewToMap(e.X, e.Y, out currentMapPoint.x, out currentMapPoint.y);

            if (mapBrowserStyle == BrowserStyle.MapMove)
            {
                if (mapImg != null && dragTag == true)
                {
                    Rectangle imgRect = new Rectangle();
                    Rectangle viewRect = new Rectangle();
                    ImgTransImgToView(moveLastPoint, e.Location, out imgRect, out viewRect);
                    Bitmap moveImg = new Bitmap(mapPicBox.Width, mapPicBox.Height);
                    Graphics imgGraphic = Graphics.FromImage(moveImg);
                    //imgGraphic.Clear(Color.White);//���������ɫ
                    imgGraphic.DrawImage(mapImg, viewRect, imgRect, GraphicsUnit.Pixel);
                    imgGraphic.Dispose();
                    mapPicBox.BackgroundImage = moveImg;
                }
            }
            else if (mapBrowserStyle == BrowserStyle.MapZoomIn)
            {
                if (dragTag == true)
                {
                    //������Ӧ�ľ��ο�

                    //����һ��ͨ��ÿ�λ�ȡ���η�Χ�� ԭͼ ���вü� ����picturebox�� ��ʵ�������ϴεĺۼ� Ч������ ������λ
                    //Graphics zoomRectGraphic = mapPicBox.CreateGraphics();
                    //Rectangle viewRect = GetRectFromTwoPoint(dragOldPoint, rectLastPoint);
                    //Image lastImg = ImgAreaImgToView(viewRect);
                    //if (lastImg != null)
                    //    zoomRectGraphic.DrawImage(lastImg, viewRect.X, viewRect.Y, viewRect.Width, viewRect.Height);
                    //zoomRectGraphic.DrawRectangle(Pens.Green, GetRectFromTwoPoint(dragOldPoint, e.Location));
                    //rectLastPoint = e.Location;

                    ////������  ÿ�ζ���ԭ�׻��Ļ����ϻ� ʵ������ԭ�кۼ�
                    ////Image lastImg = (Image)mapImg.Clone();
                    ////watch.Reset();
                    ////watch.Start();
                    //if (lastImg == null)
                    //{
                    //    lastImg = new Bitmap(mapPicBox.Width, mapPicBox.Height);
                    //}
                    //Graphics zoomRectGraphic = Graphics.FromImage(lastImg);
                    //if (mapImg != null)
                    //    zoomRectGraphic.DrawImage(mapImg, 0, 0);//�ѵ�ͼ������ʱͼƬ��
                    //Rectangle dragRect = GetRectFromTwoPoint(dragOldPoint, e.Location);
                    ////ע�⣺���÷�Χת��ΪͼƬ�ϵķ�Χ
                    //RectangleF dragImgRectF = new RectangleF(dragRect.X * lastImg.Width / mapPicBox.Width, dragRect.Y * lastImg.Width / mapPicBox.Width, dragRect.Width * lastImg.Width / mapPicBox.Width, dragRect.Height * lastImg.Width / mapPicBox.Width);
                    //zoomRectGraphic.DrawRectangle(Pens.Green, dragImgRectF.X, dragImgRectF.Y, dragImgRectF.Width, dragImgRectF.Height);
                    //zoomRectGraphic.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Blue)), dragImgRectF);
                    ////watch.Stop();
                    ////zoomRectGraphic.DrawString(watch.ElapsedMilliseconds.ToString(), Font, Brushes.Red, 30, 30, StringFormat.GenericDefault);
                    ////mapPicBox.BackgroundImage.Save("D:\\360data\\��Ҫ����\\����\\testshp\\TitleDataCompact\\ͼ��\\123.bmp");
                    //zoomRectGraphic.Dispose();
                    //mapPicBox.BackgroundImage = lastImg;

                    //������ ͨ�����µ�λͼ�ϲ��� ����λͼ����Ϊ͸�� Ȼ����ֵ���� picturebox��image
                    Bitmap zoomImg = new Bitmap(mapPicBox.Width, mapPicBox.Height);
                    Graphics zoomRectGraphic = Graphics.FromImage(zoomImg);
                    Rectangle dragRect = GetRectFromTwoPoint(dragOldPoint, e.Location);
                    zoomRectGraphic.DrawRectangle(Pens.Green, dragRect.X, dragRect.Y, dragRect.Width, dragRect.Height);
                    zoomRectGraphic.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Blue)), dragRect);
                    zoomRectGraphic.Dispose();
                    zoomImg.MakeTransparent();
                    mapPicBox.Image = zoomImg;
                }
            }
            else if (mapBrowserStyle == BrowserStyle.MapDefaultPiont)
            {
                if (m_MeasureState == MeasureStyle.LinesMeasure)
                {
                    if (m_MeasureListPoint.Count > 0)
                    {
                        if (m_MeasureListPoint.Count > 1)
                            m_MeasureListPoint.RemoveAt(m_MeasureListPoint.Count - 1);
                        MapPoint tmpMapP;
                        CoordViewToMap(e.X, e.Y, out tmpMapP.x, out tmpMapP.y);
                        m_MeasureListPoint.Add(tmpMapP);
                        DrawFrontPage();
                    }
                }
                else if (m_MeasureState == MeasureStyle.PloysMeasure)
                {
                    if (m_MeasureListPoint.Count > 0)
                    {
                        if (m_MeasureListPoint.Count > 1)
                            m_MeasureListPoint.RemoveAt(m_MeasureListPoint.Count - 1);
                        MapPoint tmpMapP;
                        CoordViewToMap(e.X, e.Y, out tmpMapP.x, out tmpMapP.y);
                        m_MeasureListPoint.Add(tmpMapP);
                        DrawFrontPage();
                    }
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (mapBrowserStyle == BrowserStyle.MapMove || mapBrowserStyle == BrowserStyle.MapZoomIn || mapBrowserStyle == BrowserStyle.MapZoomOut)
            {
                //ԭ�طŴ� �Ŵ�ص㲻�� ���ַ�������һ����� ��Ҫ�Ǽ����ȡʱ ����һ����ƫ��
                double tmpScale = 1;
                if (e.Delta > 0)
                    tmpScale = mapScale * 1.2;
                else if (e.Delta < 0)
                    tmpScale = mapScale / 1.2;
                double curCenX = 0;
                double curCenY = 0;
                CoordViewToMap(e.X, e.Y, out curCenX, out curCenY);
                SetScaleForMouseWeel(tmpScale, curCenX, curCenY);
                DrawFrontPage();
            }
            base.OnMouseWheel(e);
        }
        /// <summary>
        /// ������ʱ ��ȡ���� �����޷��ù���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mapPicBox_MouseEnter(object sender, EventArgs e)
        {
            if (!this.Focused)
                this.Focus();//Ϊ���ֲ������� ��ȡ����
        }
        /// <summary>
        /// ����뿪ʱ ʧȥ���� ������������Ҳ����Ӧ�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mapPicBox_MouseLeave(object sender, EventArgs e)
        {
            if (this.Focused)
                this.Parent.Focus();//�뿪ʱ ��ʧȥ����
        }
        #endregion

        #region �Ҽ��˵����� ��ͼ�����ʽ �Ŵ� ��С �ƶ� ��λ ˢ�� �������� �������

        private void �Ŵ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMapBrowserStyle(BrowserStyle.MapZoomIn);
        }

        private void ��СToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMapBrowserStyle(BrowserStyle.MapZoomOut);
        }

        private void �ƶ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMapBrowserStyle(BrowserStyle.MapMove);
        }

        private void ��λToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreMap();
        }

        private void ˢ��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshMap();
        }
        
        public enum MeasureStyle
        {
            NoMeasure,//û���κβ�������
            LinesMeasure,//���Ȳ���
            PloysMeasure//�������
        }
        private MeasureStyle m_MeasureState = MeasureStyle.NoMeasure;
        private List<MapPoint> m_MeasureListPoint = new List<MapPoint>();
        private Bitmap FrontImgMap = null;//���ڲ�����ͼ�� ǰͼ�����ս� ǰͼ��Ϊ͸�� ǰ��ͼ�ϲ� ��ʵ�ֽ����滭��Ч����
        private void ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_MeasureState == MeasureStyle.NoMeasure)//��ǰû�н��в�������
            {
                LinesMeasureToolStripMenuItem.Text = "��������(����)";
                m_MeasureState = MeasureStyle.LinesMeasure;
            }
            else if (m_MeasureState == MeasureStyle.LinesMeasure)//���ڽ��г��Ȳ�������
            {
                m_MeasureListPoint.Clear();
                DrawFrontPage();

                LinesMeasureToolStripMenuItem.Text = "��������(��ʼ)";
                m_MeasureState = MeasureStyle.NoMeasure;
            }
            else if (m_MeasureState == MeasureStyle.PloysMeasure)//���ڽ��������������
            {
                LinesMeasureToolStripMenuItem.Text = "��������(����)";
                PolysMeasureToolStripMenuItem.Text = "�������(��ʼ)";
                m_MeasureState = MeasureStyle.LinesMeasure;
                m_MeasureListPoint.Clear();
                DrawFrontPage();
            }       
        }
        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_MeasureState == MeasureStyle.NoMeasure)//��ǰû�н��в�������
            {
                PolysMeasureToolStripMenuItem.Text = "�������(����)";
                m_MeasureState = MeasureStyle.PloysMeasure;
            }
            else if (m_MeasureState == MeasureStyle.LinesMeasure)//���ڽ��г��Ȳ�������
            {
                LinesMeasureToolStripMenuItem.Text = "��������(��ʼ)";
                PolysMeasureToolStripMenuItem.Text = "�������(����)";
                m_MeasureState = MeasureStyle.PloysMeasure;
                m_MeasureListPoint.Clear();
                DrawFrontPage();
            }
            else if (m_MeasureState == MeasureStyle.PloysMeasure)//���ڽ��������������
            {
                m_MeasureListPoint.Clear();
                DrawFrontPage();
                PolysMeasureToolStripMenuItem.Text = "�������(��ʼ)";
                m_MeasureState = MeasureStyle.NoMeasure;
            }
        }
        private void DrawFrontPage()
        {
            FrontImgMap = new Bitmap(mapPicBox.Width, mapPicBox.Height);
            Graphics frontImgGraphic = Graphics.FromImage(FrontImgMap);
            if (m_MeasureState == MeasureStyle.LinesMeasure)//�����е��� ������
            {
                if (m_MeasureListPoint.Count > 0)
                {
                    List<Point> tmpViewPoints = CoordMapToViewPointList(m_MeasureListPoint);
                    double lenth = 0;
                    Point []LinesPoints=new Point [m_MeasureListPoint.Count];
                    for(int i=0;i<tmpViewPoints.Count;i++)
                    {
                        LinesPoints[i]=tmpViewPoints[i];
                        if(i>0)
                        {
                            lenth += Math.Sqrt((m_MeasureListPoint[i].x - m_MeasureListPoint[i - 1].x) * (m_MeasureListPoint[i].x - m_MeasureListPoint[i - 1].x) + (m_MeasureListPoint[i].y - m_MeasureListPoint[i - 1].y) * (m_MeasureListPoint[i].y - m_MeasureListPoint[i - 1].y));
                        }
                        frontImgGraphic.DrawString(lenth.ToString("0.00"),this.Font, new SolidBrush(Color.Red), LinesPoints[i].X+5, LinesPoints[i].Y-10);
                        frontImgGraphic.DrawEllipse(new Pen(Color.Red, 2), LinesPoints[i].X-4, LinesPoints[i].Y-4,8,8);
                    }
                    if (tmpViewPoints.Count>1)
                        frontImgGraphic.DrawLines(new Pen(Color.RoyalBlue, 1),LinesPoints);
                    
                }
                
            }
            else if (m_MeasureState == MeasureStyle.PloysMeasure)//�����������
            {
                if (m_MeasureListPoint.Count > 0)
                {
                    List<Point> tmpViewPoints = CoordMapToViewPointList(m_MeasureListPoint);
                    Point[] PloysPoints = new Point[m_MeasureListPoint.Count];
                    for (int i = 0; i < tmpViewPoints.Count; i++)
                    {
                        PloysPoints[i] = tmpViewPoints[i];
                        frontImgGraphic.DrawEllipse(new Pen(Color.Red, 2), PloysPoints[i].X - 4, PloysPoints[i].Y - 4, 8, 8);
                    }
                    if (tmpViewPoints.Count > 1)
                    {
                        frontImgGraphic.DrawPolygon(new Pen(Color.Green, 1), PloysPoints);
                        if (tmpViewPoints.Count > 2)
                        {
                            double area = GetPloygonArea(m_MeasureListPoint);
                            frontImgGraphic.FillPolygon(new SolidBrush(Color.FromArgb(100, Color.Blue)), PloysPoints);
                            frontImgGraphic.DrawString(area.ToString("0.00")+"ƽ����", this.Font, new SolidBrush(Color.Red), tmpViewPoints[tmpViewPoints.Count - 1].X + 5, tmpViewPoints[tmpViewPoints.Count - 1].Y - 10);
                        }
                    }
                }
            }
            frontImgGraphic.Dispose();
            FrontImgMap.MakeTransparent();
            mapPicBox.Image = FrontImgMap;
            
        }
        #endregion
    }
}
