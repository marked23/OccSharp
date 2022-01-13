﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace OccView
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string thePropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(thePropertyName));
            }
        }

        public D3dViewer mViewer;
        public MainWindow()
        {
            InitializeComponent();
            InitViewer();
        }

        public void InitViewer()
        {
            mViewer = new D3dViewer();
            Grid g = new Grid();
            Map.Add(g, mViewer);
            ImageBrush imgBrush = new ImageBrush(mViewer.Image);

            g.Background = imgBrush;
            //g.MouseMove += new MouseEventHandler(g_MouseMove);
            //g.MouseDown += new MouseButtonEventHandler(g_MouseDown);
            //g.MouseUp += new MouseButtonEventHandler(g_MouseUp);

            g.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;


            TabItem aNewTab = new TabItem();
            aNewTab.Content = g;

            aNewTab.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            aNewTab.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
            aNewTab.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;

            g.SizeChanged += new SizeChangedEventHandler(g_SizeChanged);

            aNewTab.IsSelected = true;
            aNewTab.Header = "View " + mViewCounter.ToString();
            mViewCounter++;

            ViewPanel.Items.Add(aNewTab);

            ViewPanel.Focus();

            // update XAML property
            RaisePropertyChanged("IsDocumentOpen");
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            if (ActViewer != null)
            {
                ActViewer.ImportModel(ModelFormat.STEP);
            }
        }

        private void Tool_Click(object sender, RoutedEventArgs e)
        {

        }

        private OCCViewer ActViewer
        {
            get
            {
                if (!IsDocumentOpen)
                {
                    return null;
                }
                Grid grid = (ViewPanel.SelectedContent) as Grid;
                if (grid == null)
                {
                    return null;
                }
                return Map[grid].Viewer;
            }
        }

        public Boolean IsDocumentOpen
        {
            get
            {
                return ViewPanel.Items.Count > 0;
            }
        }

        void g_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!IsDocumentOpen)
                return;
            Grid aGrid = (ViewPanel.SelectedContent) as Grid;

            if (aGrid == null)
                return;

            Map[aGrid].Resize(Convert.ToInt32(e.NewSize.Width),
                               Convert.ToInt32(e.NewSize.Height));
        }

        private int mViewCounter = 1;
        Dictionary<Grid, D3dViewer> Map = new Dictionary<Grid, D3dViewer>();
    }
}