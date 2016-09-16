#region File Info
// File       : VisualPrintDialog.cs
// Description: 
// Package    : VisualPrint
//
// Authors    : Fred Song
//
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Printing;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace VisualPrint
{
    public class VisualPrintDialog
    {
        //private PrintWindow m_Window;
        private Visual m_Visual;
        private PrintDialog m_PrintDialog;

        public VisualPrintDialog(Visual visual)
        {
            //m_Window = new PrintWindow(visual);
            m_Visual = visual;
            m_PrintDialog = new PrintDialog();
            m_PrintDialog.UserPageRangeEnabled = false;
        }

        #region 'Public Properties'

        public Visual Visual
        {
            get { return m_Visual; }
            set { m_Visual = value; }
        }

        #endregion

        #region 'Public Methods'

        public void PrintPreview()
        {
            FlowDocument flowDocument = null;

            System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                flowDocument = Helper.CreateFlowDocument(m_Visual, new Size(m_PrintDialog.PrintableAreaWidth, m_PrintDialog.PrintableAreaHeight));
            }
            finally
            {
                System.Windows.Input.Mouse.OverrideCursor = null;
            }
            if (flowDocument != null)
            {
                PrintPreview(flowDocument);
            }
        }

        public void ShowDialog()
        {
            try
            {
                bool? result = m_PrintDialog.ShowDialog();
                if (result == true)
                {
                    this.PrintPreview();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 'Private Methods/Events'
        
        private void OnPrint(object sender, RoutedEventArgs e)
        {
            FlowDocument document = (e.Source as PreviewWindow).Document;
            if (document != null)
                Print(document);
        }

        private void Print(FlowDocument document)
        {
            DocumentPaginator paginator = ((IDocumentPaginatorSource)document).DocumentPaginator;
            m_PrintDialog.PrintDocument(paginator, "Printing");
        }

        private void PrintPreview(FlowDocument document)
        {
            PreviewWindow win = new PreviewWindow(document);
            win.Print += new RoutedEventHandler(OnPrint);
            win.ShowDialog();
        }
 
        #endregion
    }
}
