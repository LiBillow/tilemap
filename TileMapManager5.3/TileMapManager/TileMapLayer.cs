using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;

namespace TileMapManager
{
    /// <summary>
    /// ���ģ�͵Ĳ���Ϣ
    /// </summary>
    public struct LodInfor 
    {
        public int LevelID;//��μ���
        public double Scale;//���ű���
        public double Resolution;//ʵ�ʾ�����ͼƬ���صı�ֵ ��λ�� ʵ�ʾ���/����
    };
    class TileMapLayer
    {
        MapRect m_TotalTileMapRect=new MapRect();//��Ƭͼ��ȫͼ��Χ
        public MapRect TotalTileMapRect
        {
            get { return m_TotalTileMapRect; }
            set { m_TotalTileMapRect = value; }
        }
        double m_TileOriginX = 0;//��Ƭ�и�ʱ����ԭ��X
        double m_TileOriginY = 0;//��Ƭ�и�ʱ����ԭ��Y
        int m_TileRows = 0;//������ƬͼƬ������
        int m_TileCols = 0;//������ƬͼƬ������
        List<LodInfor> m_LodInfors = new List<LodInfor>();//���ģ����Ϣ
        int m_Dpi = 96;//DPI���ڵ�ͼ����м���Resolution 
        public int Dpi
        {
            get { return m_Dpi; }
        }
        
        string m_layerPath = "";//��ͼ����·��
        Bitmap m_TileImage = null;//�������Ƭͼ

        private Bitmap nonImg = null;

        //�����ʽ ��ʼ��Ϊ������ ���֣�������esriMapCacheStorageModeCompact�� ��ɢ��esriMapCacheStorageModeExploded
        String m_StorageFormat = "esriMapCacheStorageModeCompact";
        String m_CacheTileFormat = "jpg";//��ɢ����Ƭ��ʽ Ŀǰֻ֧�� BMP PNG JEPG

        public TileMapLayer()
        {
            nonImg = new Bitmap(Properties.Resources.nonImg);
        }
        //��ȡ��������
        public double GetMaxScale()
        {
            return m_LodInfors[0].Scale;
        }
        //��ȡ��С������
        public double GetMinScale()
        {
            return m_LodInfors[m_LodInfors.Count - 1].Scale;
        }
        /// <summary>
        /// ��ȡ��Ƭ���ݵ���Ϣ
        /// </summary>
        /// <param name="tileMapPath">��Ƭ����·��</param>
        /// <returns>�Ƿ�ɹ���ȡ</returns>
        public bool ReadTileInfor(string tileMapPath)
        {
            m_layerPath = tileMapPath;
            try
            {
                //�ṹ˵��
                //·��ʾ����"D:\\360data\\��Ҫ����\\����\\testshp\\TitleDataCompact\\ͼ��";
                //1.��ȡconf.cdi
                //2.��ȡconf.xml

                XmlDocument doc = new XmlDocument();

                //1.��ȡconf.cdi
                string configCdi = tileMapPath + "\\conf.cdi";
                doc.Load(configCdi);
                XmlNode envelopeNode=doc.SelectSingleNode("//EnvelopeN");
                XmlNodeList envelopeChildnodeList = envelopeNode.ChildNodes;
                foreach (XmlNode node in envelopeChildnodeList)
                {
                    if (node.Name == "XMin") m_TotalTileMapRect.xMin = Convert.ToDouble(node.InnerText);
                    else if (node.Name == "YMin") m_TotalTileMapRect.yMin = Convert.ToDouble(node.InnerText);
                    else if (node.Name == "XMax") m_TotalTileMapRect.xMax = Convert.ToDouble(node.InnerText);
                    else if (node.Name == "YMax") m_TotalTileMapRect.yMax = Convert.ToDouble(node.InnerText);
                }

                //2.��ȡconf.xml
                string configXml = tileMapPath + "\\conf.xml";
                doc.Load(configXml);
                //��ȡTileOrigin
                XmlNode tileOriginNode = doc.SelectSingleNode("//TileOrigin");
                XmlNodeList tileOriginChildnodeList = tileOriginNode.ChildNodes;
                foreach (XmlNode node in tileOriginChildnodeList)
                {
                    if (node.Name == "X") m_TileOriginX = Convert.ToDouble(node.InnerText);
                    else if (node.Name == "Y") m_TileOriginY = Convert.ToDouble(node.InnerText);
                }
                //��ȡTileRows��TileCols
                XmlNode tileColsNode = doc.SelectSingleNode("//TileCols");
                m_TileCols = Convert.ToInt32(tileColsNode.InnerText);
                XmlNode tileRowsNode = doc.SelectSingleNode("//TileRows");
                m_TileRows = Convert.ToInt32(tileRowsNode.InnerText);
                XmlNode dpiNode = doc.SelectSingleNode("//DPI");
                m_Dpi = Convert.ToInt32(dpiNode.InnerText);
                //��ȡLodInfors
                XmlNode lodInforsNode = doc.SelectSingleNode("//LODInfos");
                XmlNodeList lodInforsChildnodeList = lodInforsNode.ChildNodes;
                foreach (XmlNode node in lodInforsChildnodeList)
                {
                    if (node.Name == "LODInfo")
                    {
                        XmlNodeList lodInforNodeList = node.ChildNodes;
                        LodInfor lodInfor=new LodInfor();
                        foreach (XmlNode lodInfornode in lodInforNodeList)
                        {
                            if (lodInfornode.Name == "LevelID") lodInfor.LevelID = Convert.ToInt32(lodInfornode.InnerText);
                            else if (lodInfornode.Name == "Scale") lodInfor.Scale = Convert.ToDouble(lodInfornode.InnerText);
                            else if (lodInfornode.Name == "Resolution") lodInfor.Resolution = Convert.ToDouble(lodInfornode.InnerText);
                        }
                        m_LodInfors.Add(lodInfor);
                      
                    }
                }

                //��ȡ��Ƭ��ʽ
                XmlNode StorageFormatNode = doc.SelectSingleNode("//StorageFormat");
                m_StorageFormat = StorageFormatNode.InnerText;
                XmlNode CacheTileFormatNode = doc.SelectSingleNode("//CacheTileFormat");
                if (CacheTileFormatNode.InnerText == "BMP")
                    m_CacheTileFormat = "bmp";
                else if (CacheTileFormatNode.InnerText == "JPEG")
                    m_CacheTileFormat = "jpg";
                else if (CacheTileFormatNode.InnerText == "PNG8" || CacheTileFormatNode.InnerText == "PNG24" || CacheTileFormatNode.InnerText == "PNG32")
                    m_CacheTileFormat = "png";
                else
                {
                    MessageBox.Show("�ݲ�֧�ֻ���ͻ������ݣ�");
                    return false;
                }

                return true;
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message, "��ȡ��Ƭ���ݳ���");
                return false;
            }
            
        }
        /// <summary>
        /// ���ݱ����ߺ���ʾ��Χ ��ȡ��ʾ��ͼƬ
        /// </summary>
        /// <param name="mapScale">��ǰ��ͼ�ı�����</param>
        /// <param name="mapRect">��ǰ��ͼ����ʾ��Χ</param>
        /// /// <param name="imgSize">��Ҫ��ȡͼƬ�Ĵ�С</param>
        /// <returns>��Ӧ��Χ��ͼƬ</returns>
        /// <param name="showGrid">�Ƿ���ʾ����</param>
        /// <returns></returns>
        public Bitmap GetLayerImage(double mapScale, MapRect mapRect,Size imgSize,bool showGrid)
        {
            if(m_LodInfors.Count<=0) return null;
            //ȷ����Ӧ��Χ���ڵĲ��
            int curLevel = m_LodInfors[m_LodInfors.Count - 1].LevelID;
            for (int i = 0; i < m_LodInfors.Count; i++)
            {
                if (mapScale >= m_LodInfors[i].Scale)
                {
                    curLevel = m_LodInfors[i].LevelID;
                    break;
                }
            }
            //������������½� ���Ͻ� ��Ӧ������ֵ
            int mapColsStart = Convert.ToInt32(Math.Floor((mapRect.xMin - m_TileOriginX) / (m_LodInfors[curLevel].Resolution * m_TileCols)));//��ʼ��
            int mapRowsEnd = Convert.ToInt32(Math.Floor((m_TileOriginY - mapRect.yMin) / (m_LodInfors[curLevel].Resolution * m_TileRows)));//��ʼ��
            int mapColsEnd = Convert.ToInt32(Math.Floor((mapRect.xMax - m_TileOriginX) / (m_LodInfors[curLevel].Resolution * m_TileCols)));//��ֹ��
            int mapRowsStart = Convert.ToInt32(Math.Floor((m_TileOriginY - mapRect.yMax) / (m_LodInfors[curLevel].Resolution * m_TileRows)));//��ֹ��
            //��ȡ��Ӧ���е���Ƭ����ƴ������ ��Ϊһ����ͼ
            m_TileImage = new Bitmap(m_TileCols * (mapColsEnd - mapColsStart + 1), m_TileRows * (mapRowsEnd - mapRowsStart + 1));
            Graphics backImgGraphic = Graphics.FromImage(m_TileImage);
            //backImgGraphic.Clear(Color.Pink);//�����ɫ
            //����ͼ ��ˮӡͼƬ���ͼ    
            TextureBrush texture = new TextureBrush(nonImg,System.Drawing.Drawing2D.WrapMode.Tile);//ƽ��
            backImgGraphic.FillRectangle(texture,0,0,m_TileImage.Width,m_TileImage.Height);
            
            //m_TileImage.Save("D:\\360data\\��Ҫ����\\����\\testshp\\TitleDataCompact\\ͼ��\\test\\total.bmp");
            
            //��ȡ���������� ������Χ����Բ�ȥ��ȡ��Ӧλ�õ���Ƭ���ݣ�һ�ι��ˣ�
            int maxRow = Convert.ToInt32(Math.Floor((m_TileOriginY - m_TotalTileMapRect.yMin) / (m_LodInfors[curLevel].Resolution * m_TileRows)));//��ʼ��
            int maxCol = Convert.ToInt32(Math.Floor((m_TotalTileMapRect.xMax - m_TileOriginX) / (m_LodInfors[curLevel].Resolution * m_TileCols)));//��ֹ��

            if (m_StorageFormat == "esriMapCacheStorageModeCompact")//���������ݶ���
            {
                for (int j = mapRowsStart; j <= mapRowsEnd; j++)
                {
                    for (int i = mapColsStart; i <= mapColsEnd; i++)
                    {
                        //����Ƭ��ͼ (������Ƭ��Χ �Ͳ�ȥ��ȡ���� ����)
                        if (!(i < 0 || j < 0 || i > maxCol || j > maxRow))
                        {
                            Image RowColImage = getCompactMapTile(curLevel, j, i);
                            if (RowColImage != null)
                                backImgGraphic.DrawImage(RowColImage, (i - mapColsStart) * m_TileCols, (j - mapRowsStart) * m_TileRows, m_TileCols, m_TileRows);//��ͼƬ��������ͼ��

                        }
                        //����������
                        if(showGrid)
                            backImgGraphic.DrawRectangle(Pens.Blue, (i - mapColsStart) * m_TileCols, (j - mapRowsStart) * m_TileRows, m_TileCols, m_TileRows);
                        //RowColImage.Save("D:\\360data\\��Ҫ����\\����\\testshp\\TitleDataCompact\\ͼ��\\test\\r" + j + "c" + i + ".bmp");
                    }
                }
            }
            else if (m_StorageFormat == "esriMapCacheStorageModeExploded")//��ɢ�����ݶ���
            {
                for (int j = mapRowsStart; j <= mapRowsEnd; j++)
                {
                    for (int i = mapColsStart; i <= mapColsEnd; i++)
                    {
                        //����Ƭ��ͼ (������Ƭ��Χ �Ͳ�ȥ��ȡ���� ����)
                        if (!(i < 0 || j < 0 || i > maxCol || j > maxRow))
                        {
                            Image RowColImage = getExplodedMapTile(curLevel, j, i);
                            if (RowColImage != null)
                                backImgGraphic.DrawImage(RowColImage, (i - mapColsStart) * m_TileCols, (j - mapRowsStart) * m_TileRows, m_TileCols, m_TileRows);//��ͼƬ��������ͼ��
                        }
                        //����������
                        if (showGrid)
                            backImgGraphic.DrawRectangle(Pens.Blue, (i - mapColsStart) * m_TileCols, (j - mapRowsStart) * m_TileRows, m_TileCols, m_TileRows);
                        //RowColImage.Save("D:\\360data\\��Ҫ����\\����\\testshp\\TitleDataCompact\\ͼ��\\test\\r" + j + "c" + i + ".bmp");
                    }
                }
            }

            //backImgGraphic.Dispose();
            //m_TileImage.Save("D:\\360data\\��Ҫ����\\����\\testshp\\TitleDataCompact\\ͼ��\\test\\total.bmp");
            //����ʵ�ʷ�Χ�� ͨ���ü�����ͼ ��ȡʵ����Ҫ��ͼƬ
            double startx = mapColsStart * m_LodInfors[curLevel].Resolution * m_TileCols + m_TileOriginX;
            double endy = m_TileOriginY - mapRowsStart * m_LodInfors[curLevel].Resolution * m_TileRows;
            double endx = (mapColsEnd+1) * m_LodInfors[curLevel].Resolution * m_TileCols + m_TileOriginX;
            double starty = m_TileOriginY - (mapRowsEnd + 1) * m_LodInfors[curLevel].Resolution * m_TileRows;

            int imgRectX = Convert.ToInt32((mapRect.xMin - startx) / (endx - startx) * m_TileImage.Width);
            int imgRectY = Convert.ToInt32((endy - mapRect.yMax) / (endy - starty) * m_TileImage.Height);
            int imgRectWid = Convert.ToInt32(mapRect.GetWidth() / (endx - startx) * m_TileImage.Width);
            int imgRectHeight = Convert.ToInt32(mapRect.GetHeigh() / (endy - starty) * m_TileImage.Height);
            //Bitmap mapImg = m_TileImage.Clone(new Rectangle(imgRectX, imgRectY, imgRectWid, imgRectHeight), m_TileImage.PixelFormat);
            Bitmap mapImg=new Bitmap(imgSize.Width,imgSize.Height);
            backImgGraphic = Graphics.FromImage(mapImg);
            backImgGraphic.DrawImage(m_TileImage, new Rectangle(0, 0, imgSize.Width, imgSize.Height), new Rectangle(imgRectX, imgRectY, imgRectWid, imgRectHeight),GraphicsUnit.Pixel);
            backImgGraphic.Dispose();
            return mapImg;
        }

        /// <summary>
        /// ��ȡ��ɢ����Ƭ
        /// </summary>
        /// <param name="level">LOD</param>
        /// <param name="row">��</param>
        /// <param name="col">��</param>
        /// <returns>��Ӧ���е���ƬͼƬ</returns>
        private Image getExplodedMapTile(int level, int row, int col)
        {
            String imgPath = GetExplodedTileImageName(row, col, level, m_layerPath, m_CacheTileFormat);
            if (System.IO.File.Exists(imgPath))
            {
                Image img = new Bitmap(imgPath);
                return img;
            }
            else
                return null;
        }

        /// <summary>
        /// ��ȡ��ɢ����ƬͼƬ������ �����·�������� �򴴽�һ��·��
        /// </summary>
        /// <param name="row">������</param>
        /// <param name="col">������</param>
        /// <param name="level">���ڼ���</param>
        /// <param name="orignalPath">ԭʼ·��</param>
        /// <param name="imgType">ͼƬ��ʽ</param>
        /// <returns>ͼƬ·��</returns>
        public String GetExplodedTileImageName(int row, int col, int level, String orignalPath, String imgType)
        {
            String path = orignalPath + "\\_alllayers"
                          + "\\L" + level.ToString().PadLeft(2, '0')
                          + "\\R" + row.ToString("x").PadLeft(8, '0');
            if (!System.IO.Directory.Exists(path)) return null;//·��������
            String tileName = path + "\\C" + col.ToString("x").PadLeft(8, '0')
                          + "." + imgType;
            return tileName;
        }

        /// <summary>
        /// ��ȡ��Ƭ
        /// </summary>
        /// <param name="level">LOD</param>
        /// <param name="row">��</param>
        /// <param name="col">��</param>
        /// <returns>��Ӧ���е���ƬͼƬ</returns>
        private Image getCompactMapTile(int level, int row, int col)
        {
            byte[] result = null;
            System.IO.FileStream isBundlx = null, isBundle = null;
            try
            {
                //���㲹���ļ�·��
                String bundlesDir = m_layerPath + "\\_alllayers";

                String l = "0" + level;
                int lLength = l.Length;
                if (lLength > 2)
                {
                    l = l.Substring(1);
                }
                l = "L" + l;

                int rGroup = 128 * (row / 128);
                String r = "000" + rGroup.ToString("X");//תΪ16����
                int rLength = r.Length;
                if (rLength > 7)
                {
                    r = r.Substring(3);
                }
                else if (rLength > 4)
                {
                    r = r.Substring(rLength - 4);
                }
                r = "R" + r;

                int cGroup = 128 * (col / 128);
                String c = "000" + cGroup.ToString("X");//תΪ16����
                int cLength = c.Length;
                if (cLength > 7)
                {
                    c = c.Substring(3);
                }
                else if (cLength > 4)
                {
                    c = c.Substring(cLength - 4);
                }
                c = "C" + c;

                String bundleBase = string.Format("{0}\\{1}\\{2}{3}", bundlesDir, l, r, c);
                String bundlxFileName = bundleBase + ".bundlx";
                String bundleFileName = bundleBase + ".bundle";

                //��ȡ����Ƭ����Ƭ�е�λ��
                int index = 128 * (col - cGroup) + (row - rGroup);//�����Ӧ��Ƭ�� ���ڵ�����λ�ã��൱��������Ϣ��
                isBundlx = new System.IO.FileStream(bundlxFileName,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);
                int skip = 16 + 5 * index;
                byte[] buffer = new byte[5];
                isBundlx.Seek((long)skip, System.IO.SeekOrigin.Begin);
                isBundlx.Read(buffer, 0, buffer.Length);
                //��λ����λתΪ��λ����λ��5���ֽڷ������¼���ȣ� 
                // ��ABCDE->EDCBA ������A*236^0+B*256^1+C*256^2+D*256^3+E*256^4 ����ȡ&0xff��֪����ʲô����
                long offset = (long)(buffer[0] & 0xff) + (long)(buffer[1] & 0xff)
                        * 256 + (long)(buffer[2] & 0xff) * 65536
                        + (long)(buffer[3] & 0xff) * 16777216
                        + (long)(buffer[4] & 0xff) * 4294967296L;

                isBundle = new System.IO.FileStream(bundleFileName,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);

                //��ȡ����Ƭ��ͼ������
                byte[] lengthBytes = new byte[4];
                isBundle.Seek(offset, System.IO.SeekOrigin.Begin);
                isBundle.Read(lengthBytes, 0, lengthBytes.Length);
                //��λ����λתΪ��λ����λ��4���ֽڷ������¼���ȣ�
                int length = (int)(lengthBytes[0] & 0xff)
                        + (int)(lengthBytes[1] & 0xff) * 256
                        + (int)(lengthBytes[2] & 0xff) * 65536
                        + (int)(lengthBytes[3] & 0xff) * 16777216;
                result = new byte[length];
                isBundle.Seek(offset + lengthBytes.Length, System.IO.SeekOrigin.Begin);
                isBundle.Read(result, 0, length);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(result);

                return System.Drawing.Image.FromStream(ms);
            }
            catch (Exception /*ex*/)
            {
                return null;//���û�ж���ͼƬ �ͷ���������ͼƬ
                //throw (ex);
            }
            finally
            {
                if (isBundle != null)
                {
                    isBundle.Close();
                    isBundle.Dispose();
                }
                if (isBundlx != null)
                {
                    isBundlx.Close();
                    isBundlx.Dispose();
                }
            }
        }
    }
}
