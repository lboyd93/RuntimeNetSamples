using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace ClientSideGraphics
{
    /// <summary>
    /// This sample is modified from the ArcGIS Runtime SDK for .NET sample
    /// Here: https://developers.arcgis.com/net/latest/wpf/sample-code/add-graphics-with-renderer/
    /// This sample adds functionality that generates random points within
    /// the created extent and provides a button to remove the graphics overlay.
    /// </summary>
    public partial class MainWindow : Window
    {
        GraphicsOverlay _graphicsOverlay;

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        //Calcutlate method modified from  https://stackoverflow.com/questions/41342183/generate-random-coordinates-with-boundaries
        private IEnumerable< MapPoint> Calculate(MapPoint location1, MapPoint location2, MapPoint location3,
        MapPoint location4)
        {
            MapPoint[] allCoords = { location1, location2, location3, location4 };
            double minLat = allCoords.Min(x => x.Y);
            double minLon = allCoords.Min(x => x.X);
            double maxLat = allCoords.Max(x => x.Y);
            double maxLon = allCoords.Max(x => x.X);

            Random r = new Random();

            //replase 500 with your number
            MapPoint[] result = new MapPoint[300000];
            for (int i = 0; i < result.Length; i++)
            {
                MapPoint point = new MapPoint(0,0); 
                do
                {
                    double Latitude = r.NextDouble() * (maxLat - minLat) + minLat;
                    double Longitude = r.NextDouble() * (maxLon - minLon) + minLon;
                    point = new MapPoint(Longitude, Latitude);
                } while (!IsPointInPolygon(point, allCoords));
                result[i] = point;
            }
            return result;
        }

        //IsPointPolygon method modified from https://stackoverflow.com/questions/41342183/generate-random-coordinates-with-boundaries
        private bool IsPointInPolygon(MapPoint point, MapPoint[] polygon)
        {
            int polygonLength = polygon.Length, i = 0;
            bool inside = false;
            // x, y for tested point.
            double pointX = point.X, pointY = point.Y;
            // start / end point for the current polygon segment.
            double startX, startY, endX, endY;
            MapPoint endPoint = polygon[polygonLength - 1];
            endX = endPoint.X;
            endY = endPoint.Y;
            while (i < polygonLength)
            {
                startX = endX;
                startY = endY;
                endPoint = polygon[i++];
                endX = endPoint.X;
                endY = endPoint.Y;
                //
                inside ^= ((endY > pointY) ^ (startY > pointY)) /* ? pointY inside [startY;endY] segment ? */
                          && /* if so, test if it is under the segment */
                          (pointX - endX < (pointY - endY) * (startX - endX) / (startY - endY));
            }
            return inside;
        }


        private void Initialize()
        {
            // Create a map with 'Imagery with Labels' basemap.
            Map myMap = new Map(Basemap.CreateStreetsVector());

            // Assign the map to the MapView.
            MyMapView.Map = myMap;

            // Create a center point for the graphics.
            MapPoint centerPoint = new MapPoint(-117.195800, 34.056295, SpatialReferences.Wgs84);

            // Create an envelope from that center point.
            Envelope pointExtent = new Envelope(centerPoint, .5, .5);

            //get the four corners of the extent
            MapPoint location1 = new MapPoint(pointExtent.XMax, pointExtent.YMax);
            MapPoint location2 = new MapPoint(pointExtent.XMax, pointExtent.YMin);
            MapPoint location3 = new MapPoint(pointExtent.XMin, pointExtent.YMax);
            MapPoint location4 = new MapPoint(pointExtent.XMin, pointExtent.YMin);


            // Create a collection of points in the extent
            Esri.ArcGISRuntime.Geometry.PointCollection points = new Esri.ArcGISRuntime.Geometry.PointCollection(Calculate(location1, location2, location3, location4), SpatialReferences.Wgs84);

            // Create overlay to where graphics are shown.
            _graphicsOverlay = new GraphicsOverlay();

            // Add points to the graphics overlay.
            foreach (MapPoint point in points)
            {
                // Create new graphic and add it to the overlay.
                _graphicsOverlay.Graphics.Add(new Graphic(point));
            }

            // Create symbol for points.
            SimpleMarkerSymbol pointSymbol = new SimpleMarkerSymbol()
            {
                Color = System.Drawing.Color.Yellow,
                Size = 30,
                Style = SimpleMarkerSymbolStyle.Square
            };

            // Create simple renderer with symbol.
            SimpleRenderer renderer = new SimpleRenderer(pointSymbol);

            // Set renderer to graphics overlay.
            _graphicsOverlay.Renderer = renderer;

            // Add created overlay to the MapView.
            MyMapView.GraphicsOverlays.Add(_graphicsOverlay);

            // Center the MapView on the points.
            MyMapView.SetViewpointGeometryAsync(pointExtent, 50);
        }

        private void ClearGO_Click(object sender, RoutedEventArgs e)
        {
            if (_graphicsOverlay != null)
            {
                _graphicsOverlay.Graphics.Clear();
                MyMapView.GraphicsOverlays.Remove(_graphicsOverlay);

                _graphicsOverlay = null;
            }
        }

        // Map initialization logic is contained in MapViewModel.cs
    }

    

}
