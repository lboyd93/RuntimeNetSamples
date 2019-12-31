using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Location;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Security;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using UIKit;

namespace AddLayer
{
    public partial class ViewController : UIViewController
    {
        MapViewModel _mapViewModel = new MapViewModel();
        MapView _mapView;

        string _serviceURL = "";
        Envelope _extent;

        public ViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Create a new map view, set its map, and provide the coordinates for laying it out
            _mapView = new MapView()
            {
                //Map = _mapViewModel.Map // Use the map from the view-model
                Map = new Map(Basemap.CreateTopographic())
            };

            // Add the MapView to the Subview
            View.AddSubview(_mapView);

            View.AddSubview(InputServiceURL);
            View.AddSubview(AddLayer);
            View.AddSubview(ClearButton);
        }

        public override void ViewDidLayoutSubviews()
        {
            // Fill the screen with the map
            _mapView.Frame = new CoreGraphics.CGRect(0, 0, View.Bounds.Width, View.Bounds.Height);
            base.ViewDidLayoutSubviews();
        }

        private void MapViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Update the map view with the view model's new map
            if (e.PropertyName == "Map" && _mapView != null)
                _mapView.Map = _mapViewModel.Map;
        }

        partial void AddLayer_TouchUpInside(UIButton sender)
        {
            _serviceURL = InputServiceURL.Text;
            if (_serviceURL.Length <= 4)
            {
                //Create Alert
                var okAlertController = UIAlertController.Create("No URL Input", "Please type in a URL in the text box.", UIAlertControllerStyle.Alert);
                //Add Action
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                // Present Alert
                PresentViewController(okAlertController, true, null);
            }
            else
            {
                
                if (_serviceURL.Substring(0, 4) != "http")
                {
                    //Create Alert
                    var okAlertController = UIAlertController.Create("Invalid URL", "Please input a valid URL", UIAlertControllerStyle.Alert);
                    //Add Action
                    okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    // Present Alert
                    PresentViewController(okAlertController, true, null);
                }
                else
                {
                    Uri serviceURI = new Uri(_serviceURL);

                    if (_serviceURL.Contains("MapServer"))
                    {
                        ArcGISMapImageLayer mapImageLayer = new ArcGISMapImageLayer(serviceURI);
                        mapImageLayer.LoadAsync();
                        //_extent = mapImageLayer.FullExtent;
                        _mapView.Map.OperationalLayers.Add(mapImageLayer);
                    }
                    else if (_serviceURL.Contains("FeatureServer"))
                    {

                        FeatureLayer featurelayer = new FeatureLayer(serviceURI);
                        featurelayer.LoadAsync();
                        //_extent = featurelayer.FullExtent;
                        _mapView.Map.OperationalLayers.Add(featurelayer);
                        
                    }
                    else
                    {
                        //Create Alert
                        var okAlertController = UIAlertController.Create("Invalid Service Type", "Please input a valid map service or feature service.", UIAlertControllerStyle.Alert);
                        //Add Action
                        okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                        // Present Alert
                        PresentViewController(okAlertController, true, null);
                    }

                    //_mapView.SetViewpointGeometryAsync(_extent);

                }
            }


        }

        partial void ClearButton_TouchUpInside(UIButton sender)
        {
            _mapView.Map.OperationalLayers.Clear();
        }
    }
}