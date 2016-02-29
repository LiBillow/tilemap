using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using System.Collections.Generic;
using TileMapManager.EditTools;

namespace TileMapManager
{
    /// <summary>
    /// Summary description for TileMapDragClipTool.
    /// ��������ѡȡ��Χ ͬʱ�ı������� ֧�ֶ��������
    /// </summary>
    [Guid("a3f059df-f8db-409e-873f-36eece70127a")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TileMapManager.TileMapPartCutTool")]
    public sealed class TileMapDragClipTool : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        public TileMapDragClipTool()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "CustomTools"; //localizable text 
            base.m_caption = "����ü�����";  //localizable text 
            base.m_message = "ͨ������ȷ���ü���Χ";  //localizable text
            base.m_toolTip = "ѡȡ�ü���Χ";  //localizable text
            base.m_name = "CustomTools_GetClipEnvelop";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }


        #region Overridden Class Methods

        public override bool Enabled
        {
            get
            {
                if (m_hookHelper.ActiveView != null) 
                    return true;
                else return false;
            }
        } 

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            // TODO:  Add TileMapDragClipTool.OnCreate implementation
            m_hookHelper.Hook = hook;
            IntPtr pHandle = new IntPtr(m_hookHelper.ActiveView.ScreenDisplay.hWnd);
            m_axMapControl = System.Windows.Forms.Form.FromHandle(pHandle) as AxMapControl;//�������ͼ�ؼ������������ֱ�ӷ�Ӧ��������ĵ�ͼ�ؼ��� 
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add TileMapDragClipTool.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add TileMapDragClipTool.OnMouseDown implementation
            if (memRedy)
            {
                IEnvelope pEnv = m_axMapControl.TrackRectangle();//�������
                if (pEnv.XMin == pEnv.XMax || pEnv.YMin == pEnv.YMax) return;//�����Ĳ��Ǿ��� �Ͳ����洢
                m_clipAreas.Add(pEnv);//��¼���������õľ���
                dragTimesTag++;

                IElement element = ElementFromEnveLop(pEnv);
                pGra.AddElement(element, 0);
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                ///��������Ϣ���������һ����ѡ��Areaֵ
                m_partDragCtrl.InsertRect(pEnv, dragTimesTag);
            }
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add TileMapDragClipTool.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add TileMapDragClipTool.OnMouseUp implementation
        }
        #endregion

        bool memRedy = false;
        IGraphicsContainer pGra = null;
        IActiveView pActiveView = null;
        IScreenDisplay screenDisp = null;
        private IHookHelper m_hookHelper = null;
        private AxMapControl m_axMapControl = null;
        public List<IEnvelope> m_clipAreas = new List<IEnvelope>();
        private PartTileDragXtraUserCtrl m_partDragCtrl = null;
        int dragTimesTag = 0;//���ڱ�ǵ�ǰ���ľ��ε����
        /// <summary>
        /// ��������ü�����
        /// ��Ҫ���ڴ��ݽ������
        /// </summary>
        /// <param name="dragInforView"></param>
        public void StartTool(PartTileDragXtraUserCtrl dragInforView)
        {
            //����������ݽ���
            screenDisp = m_axMapControl.ActiveView.ScreenDisplay;
            IMap pMap = m_axMapControl.Map;
            pActiveView = pMap as IActiveView;
            pGra = pMap as IGraphicsContainer;
            memRedy = true;
            m_partDragCtrl = dragInforView;
            //����ǰ���󴫵� ������Ϣ�ؼ��� ����ʼ������
            m_partDragCtrl.SetToolsObject(this,m_axMapControl);
        }
        /// <summary>
        /// ��ֹ����ü�����
        /// </summary>
        public void StopTool()
        {
            //��ֹ���߻�ԭ���� ���и���
            pGra.DeleteAllElements();
            m_clipAreas.Clear();
            pActiveView.Refresh();
            memRedy = false;

            m_partDragCtrl.ClearRect();
            dragTimesTag = 0;
            m_axMapControl.CurrentTool = null;
        }

        public IElement ElementFromEnveLop(IEnvelope penv)
        {
            IRectangleElement pRectangleEle = new RectangleElementClass();
            IElement pEle = pRectangleEle as IElement;
            pEle.Geometry = penv;

            IRgbColor pColor = new RgbColorClass();
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            pColor.Transparency = 50;
            // ����һ���߷��Ŷ���
            ILineSymbol pOutline = new SimpleLineSymbolClass();
            pOutline.Width = 1.5;
            pOutline.Color = pColor;
            // ������ɫ����(���TransparencyΪ0 ��ʾ͸�� ����Ϊ��͸�� û�а�͸����Ч��)
            //pColor = new RgbColorClass();
            //pColor.Red = 255;
            //pColor.Green = 0;
            //pColor.Blue = 0;
            pColor.Transparency = 0;
            // ���������ŵ�����
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = pColor;
            pFillSymbol.Outline = pOutline;
            IFillShapeElement pFillShapeEle = pEle as IFillShapeElement;
            pFillShapeEle.Symbol = pFillSymbol;
            pFillShapeEle.Symbol.Color.Transparency = 0;
            return pFillShapeEle as IElement;
        }
        /// <summary>
        /// ����graphics��
        /// </summary>
        public void setGraphicsUnVisiable()
        {
            m_axMapControl.Map.ActiveGraphicsLayer.Visible = false;
        }
        /// <summary>
        /// ��ʾgraphics��
        /// </summary>
        public void setGraphicsVisiable()
        {
            m_axMapControl.Map.ActiveGraphicsLayer.Visible = true;
        }
    }
}
