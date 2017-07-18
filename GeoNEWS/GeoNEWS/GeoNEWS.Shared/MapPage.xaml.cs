using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GeoNEWS
{
	public partial class MapPage : ContentPage
    {
        private Dictionary<string, Uri> _iconUris;
        private Dictionary<string, Uri> _buttonUris;

        public MapPage()
        {
            InitializeComponent();

            Title = "Show Callout";
            InitMap();

            FillComboBoxes();
        }


        private void InitMap()
        {
            var map = new Esri.ArcGISRuntime.Mapping.Map(Basemap.CreateLightGrayCanvas());
            var uriCity = new Uri("https://sampleserver6.arcgisonline.com/arcgis/rest/services/USA/MapServer/0");
            var uriHwy = new Uri("https://sampleserver6.arcgisonline.com/arcgis/rest/services/USA/MapServer/1");
            var uriCounty = new Uri("https://sampleserver6.arcgisonline.com/arcgis/rest/services/USA/MapServer/3");

            var cityLayer = new FeatureLayer(uriCity);
            cityLayer.Id = "city";
            var hwyLayer = new FeatureLayer(uriHwy);
            hwyLayer.Id = "hwy";
            var countyLayer = new FeatureLayer(uriCounty);
            countyLayer.Id = "county";

            map.OperationalLayers.Add(countyLayer);
            map.OperationalLayers.Add(hwyLayer);
            map.OperationalLayers.Add(cityLayer);

            MyMapView.Map = map;
            MyMapView.GeoViewTapped += MyMapView_GeoViewTapped;

            MyMapView.SetViewpointAsync(new Viewpoint(34.056077, -117.198855, 15000));
        }

        private void FillComboBoxes()
        {
            _iconUris = new Dictionary<string, Uri>();
            _iconUris.Add("Maple Leaf", new Uri("http://static.arcgis.com/images/Symbols/Business/esriBusinessMarker_236.png"));
            _iconUris.Add("Earth", new Uri("http://static.arcgis.com/images/Symbols/PeoplePlaces/Globe.png"));
            _iconUris.Add("Car", new Uri("http://static.arcgis.com/images/Symbols/Transportation/CarYellowFront.png"));
            _iconUris.Add("None", null);

            //foreach (var icon in _iconUris.Keys)
            //{
            //    CalloutIconPicker.Items.Add(icon);
            //}

            //CalloutIconPicker.SelectedIndex = 0;

            _buttonUris = new Dictionary<string, Uri>();
            _buttonUris.Add("Star", new Uri("http://static.arcgis.com/images/Symbols/Shapes/BlueStarLargeB.png"));
            _buttonUris.Add("Question", new Uri("http://static.arcgis.com/images/Symbols/PeoplePlaces/InformationQuestion.png"));
            _buttonUris.Add("Exclaim", new Uri("http://static.arcgis.com/images/Symbols/Transportation/WarningRed.png"));
            _buttonUris.Add("None", null);

            //foreach (var btn in _buttonUris.Keys)
            //{
            //    CalloutButtonPicker.Items.Add(btn);
            //}

            //CalloutButtonPicker.SelectedIndex = 0;
        }

        private async void MyMapView_GeoViewTapped(object sender, GeoViewInputEventArgs e)
        {
            //if (!UseCustomToggleButton.IsToggled) // Identify Features
            //{
            //    var iconName = CalloutIconPicker.Items[CalloutIconPicker.SelectedIndex];
            //    var buttonName = CalloutButtonPicker.Items[CalloutButtonPicker.SelectedIndex];
            //    var iconImageUrl = _iconUris[iconName]; // CalloutIconPicker.SelectedValue as Uri;
            //    var buttonImageUrl = _buttonUris[buttonName]; // CalloutButtonPicker.SelectedValue as Uri;

            //    var feature = await FindFeatureNearClickAsync(e.Position);
            //    CalloutDefinition calloutDef = null;

            //    if (feature != null)
            //    {
            //        calloutDef = new CalloutDefinition(feature);
            //        if (iconImageUrl != null)
            //        {
            //            var mapleMarker = new PictureMarkerSymbol(iconImageUrl);
            //            await calloutDef.SetIconFromSymbolAsync(mapleMarker);
            //        }

            //        if (buttonImageUrl != null)
            //        {
            //            var buttonImg = new RuntimeImage(buttonImageUrl);
            //            calloutDef.ButtonImage = buttonImg;
            //            calloutDef.OnButtonClick += OnCalloutButtonClick;
            //        }

            //        calloutDef.Tag = feature;
            //        if (feature.Attributes.ContainsKey("pop2000"))
            //        {
            //            calloutDef.DetailText = "Here's another value: " + feature.Attributes["pop2000"];
            //        }

            //        MyMapView.ShowCalloutForGeoElement(feature, e.Position, calloutDef);
            //    }
            //    else
            //    {
            //        calloutDef = new CalloutDefinition("You missed ...", "no features here");
            //        var skullMarkerSym = new PictureMarkerSymbol(new Uri("http://static.arcgis.com/images/Symbols/Transportation/SkullandCrossbones.png"));
            //        await calloutDef.SetIconFromSymbolAsync(skullMarkerSym);
            //        MyMapView.ShowCalloutAt(e.Location, calloutDef);
            //    }
        }
           
    
    }
}
