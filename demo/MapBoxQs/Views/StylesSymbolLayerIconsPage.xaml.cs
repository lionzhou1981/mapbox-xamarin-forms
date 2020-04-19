﻿using Acr.UserDialogs;
using GeoJSON.Net.Feature;
using MapBoxQs.Services;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Annotations;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Naxam.Mapbox.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MapBoxQs.Views
{
    public partial class StylesSymbolLayerIconsPage : ContentPage
    {
        public StylesSymbolLayerIconsPage()
        {
            InitializeComponent();

            Title = "Symbol layer icons";

            map.Center = new LatLng(-32.557013, -56.149056);
            map.ZoomLevel = 5.526846;
            map.MapStyle = new MapStyle("mapbox://styles/mapbox/cjf4m44iw0uza2spb3q0a7s41");

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
            map.DidTapOnMarkerCommand = new Command<string>(HandleMarkerClicked);
            map.DidBoundariesChangedCommand = new Command<LatLngBounds>(DidBoundariesChanged);

        }

        void HandleStyleLoaded(MapStyle obj)
        {
            IconImageSource iconImageSource = ImageSource.FromFile("red_marker.png");
            /*map.Functions.AddStyleImage(iconImageSource);

            var symbolLayerIconFeatureList = new List<Feature>();
            symbolLayerIconFeatureList.Add(
                new Feature(new GeoJSON.Net.Geometry.Point(
                    new GeoJSON.Net.Geometry.Position(-33.213144, -57.225365))));
            symbolLayerIconFeatureList.Add(
                new Feature(new GeoJSON.Net.Geometry.Point(
                    new GeoJSON.Net.Geometry.Position(-33.981818, -54.14164))));
            symbolLayerIconFeatureList.Add(
                new Feature(new GeoJSON.Net.Geometry.Point(
                    new GeoJSON.Net.Geometry.Position(-30.583266, -56.990533))));

            var featureCollection = new FeatureCollection(symbolLayerIconFeatureList);

            var source = new GeoJsonSource
            {
                Id = "feature.memory.src",
                Data = featureCollection
            };
            map.Functions.AddSource(source);

            var symbolLayer = new SymbolLayer("feature.symbol.layer", source.Id)
            {
                IconAllowOverlap = Expression.Literal(true),
                IconImage = Expression.Literal(iconImageSource.Id),
                IconOffset = Expression.Literal(new[] { 0.0, -9.0 })
            };
            map.Functions.AddLayer(symbolLayer);*/

            var symbol = new SymbolAnnotation
            {
                Coordinates = new LatLng(60.169091, 24.939876),
                IconImage = iconImageSource,
                IconSize = 2.0f,
                IsDraggable = true,
                Id = Guid.NewGuid().ToString()
            };

            map.Annotations = new[] { symbol };
        }

        private void HandleMarkerClicked(string id)
        {
            var symbol = map.Annotations.FirstOrDefault(x => x.Id == id) as SymbolAnnotation;

            if (symbol != null)
            {
                symbol. IconImage = (ImageSource)"icon.png";
                map.Functions.UpdateAnnotation(symbol);
            }
        }

        private void DidBoundariesChanged(LatLngBounds obj)
        {
            Title = obj.NorthEast.Lat.ToString() + " " + obj.NorthEast.Long.ToString();
        }


        async void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            var buttons = MapBoxService.DefaultStyles.Select((arg) => arg.Name).ToArray();
            var choice = await UserDialogs.Instance.ActionSheetAsync("Change style", "Cancel", "Reload current style", buttons: buttons);

            if (buttons.Contains(choice))
            {
                map.MapStyle = MapBoxService.DefaultStyles.FirstOrDefault((arg) => arg.Name == choice);
            }
        }
    }
}
