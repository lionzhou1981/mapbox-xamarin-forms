using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Naxam.Mapbox.Annotations;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapBoxQs.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PluginSymbolListener : ContentPage
    {
        const string MAKI_ICON_CAFE = "cafe-15";
        const string MAKI_ICON_HARBOR = "harbor-15";
        const string MAKI_ICON_AIRPORT = "airport-15";

        private IconImageSource _iconRed;
        public ObservableCollection<SymbolAnnotation> Annotations { get; set; } = new ObservableCollection<SymbolAnnotation>();
        public PluginSymbolListener()
        {
            InitializeComponent();

            map.Center = new LatLng(40.7135, -74.0066);
            map.MapStyle = MapStyle.DARK;
            //map.ZoomLevel = 16;
            //map.Pitch = 45;

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
            map.DidTapOnMarkerCommand = new Command<string>(HandleMarkerClicked);
            map.DidBoundariesChangedCommand = new Command<LatLngBounds>(DidBoundariesChanged);
            map.DidTapOnMapCommand = new Command<(LatLng, Point)>(HandleTap);
        }

        private void HandleTap((LatLng, Point) obj)
        {
            Title = "Tapped: " + obj.Item1.ToString() + " " + obj.Item2.ToString();
        }

        private void HandleMarkerClicked(string id)
        {
            var symbol = map.Annotations.FirstOrDefault(x => x.Id == id) as SymbolAnnotation;

            if (symbol != null)
            {
                //symbol.IconImage = _iconRed;
                //map.Functions.UpdateAnnotation(symbol);
                Annotations.Remove(symbol);
            }
        }

        private void DidBoundariesChanged(LatLngBounds obj)
        {
            Title = obj.NorthEast.Lat.ToString() + " " + obj.NorthEast.Long.ToString();
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            IconImageSource icon = (ImageSource)"blue_marker_view.png";
            _iconRed = (ImageSource)"red_marker.png";
            
            map.Functions.AddStyleImage(icon);
            map.Functions.AddStyleImage(_iconRed);

            var symbol = new SymbolAnnotation {
                Coordinates = new LatLng(60.169091, 24.939876),
                IconImage = Device.RuntimePlatform == Device.Android ?  icon : null,
                IconSize = 2.0f,
                IsDraggable = true,
                Id = Guid.NewGuid().ToString()
            };

            Annotations.Add(symbol);

            symbol = new SymbolAnnotation
            {
                Coordinates = new LatLng(61.169091, 24.939876),
                IconImage = Device.RuntimePlatform == Device.Android ? icon : null,
                IconSize = 2.0f,
                IsDraggable = true,
                Id = Guid.NewGuid().ToString()
            };
            
            Annotations.Add(symbol);
            map.Annotations = Annotations;
        }
    }
}