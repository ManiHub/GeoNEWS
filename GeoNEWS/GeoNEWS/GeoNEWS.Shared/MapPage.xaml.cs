using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Location;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Portal;
using Esri.ArcGISRuntime.Security;
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
        // Constants for OAuth-related values ...
        // URL of the server to authenticate with
        private const string ServerUrl = "https://summerinterns.maps.arcgis.com/sharing/rest";
        // TODO: Provide the client ID for your app (registered with the server)
        private const string ClientId = "o4mlpsRN6IJARJSK";
        // TODO: [optional] Provide the client secret for the app (only needed for the OAuthAuthorizationCode auth type)
        private const string ClientSecret = "43b4d51db14d4100b7d14d4d917d4a3e";
        // TODO: Provide a URL registered for the app for redirecting after a successful authorization
        private const string RedirectUrl = "https://developers.arcgis.com";
        // TODO: Provide an ID for a secured web map item hosted on the server
        private const string WebMapId = "6291995bb09f4f989148b6f58e491ff3";

        public MapPage()
        {
            InitializeComponent();

            Title = "The Mapsetters";
            Initialize();
        }

        private void Initialize()
        {
            LoadUIElements();
            // Set up the AuthenticationManager to use OAuth for secure ArcGIS Online requests
            UpdateAuthenticationManager();

            // Display a secured web map from ArcGIS Online (will be challenged to log in)
            DisplayWebMap();

            MyMapView.LocationDisplay.IsEnabled = true;
            //MyMapView.LocationDisplay.AutoPanMode = LocationDisplayAutoPanMode.Navigation;
        }

        private void LoadUIElements()
        {
            imgLocation.Source = "user.jpg";
            imgLocation.GestureRecognizers.Add(new TapGestureRecognizer(OnlocationimgTap));
            imglistview.Source = "earth.jpg";
            imgprofile.Source = "user.jpg";
        }

        private void OnlocationimgTap(View arg1, object arg2)
        {
            MyMapView.LocationDisplay.AutoPanMode = LocationDisplayAutoPanMode.Navigation;
        }

        private void UpdateAuthenticationManager()
        {
            // Register the server information with the AuthenticationManager
            Esri.ArcGISRuntime.Security.ServerInfo serverInfo = new ServerInfo
            {
                ServerUri = new Uri(ServerUrl),
                OAuthClientInfo = new OAuthClientInfo
                {
                    ClientId = ClientId,
                    RedirectUri = new Uri(RedirectUrl)
                }
            };

            // If a client secret has been configured, set the authentication type to OAuthAuthorizationCode
            if (!string.IsNullOrEmpty(ClientSecret))
            {
                // Use OAuthAuthorizationCode if you need a refresh token (and have specified a valid client secret)
                serverInfo.TokenAuthenticationType = TokenAuthenticationType.OAuthAuthorizationCode;
                serverInfo.OAuthClientInfo.ClientSecret = ClientSecret;
            }
            else
            {
                // Otherwise, use OAuthImplicit
                serverInfo.TokenAuthenticationType = TokenAuthenticationType.OAuthImplicit;
            }

            // Register this server with AuthenticationManager
            AuthenticationManager.Current.RegisterServer(serverInfo);

            // Use a function in this class to challenge for credentials
            AuthenticationManager.Current.ChallengeHandler = new ChallengeHandler(CreateCredentialAsync);

            // Note: In a WPF app, you need to associate a custom IOAuthAuthorizeHandler component with the AuthenticationManager to 
            //     handle showing OAuth login controls (AuthenticationManager.Current.OAuthAuthorizeHandler = new MyOAuthAuthorize();).
            //     The UWP AuthenticationManager, however, uses a built-in IOAuthAuthorizeHandler (based on WebAuthenticationBroker).
        }

        private async void DisplayWebMap()
        {
            // Display a web map hosted in a portal. If the web map item is secured, AuthenticationManager will
            // challenge for credentials
            try
            {
                // Connect to a portal (ArcGIS Online, for example)
                ArcGISPortal arcgisPortal = await ArcGISPortal.CreateAsync(new Uri(ServerUrl));

                // Get a web map portal item using its ID
                // If the item is secured (not shared publicly) the user will be challenged for credentials at this point
                PortalItem portalItem = await PortalItem.CreateAsync(arcgisPortal, WebMapId);

                // Create a new map with the portal item
                Map myMap = new Map(portalItem);

                // Assign the map to the MapView.Map property to display it in the app
                MyMapView.Map = myMap;
                await myMap.RetryLoadAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<Credential> CreateCredentialAsync(CredentialRequestInfo info)
        {
            // ChallengeHandler function for AuthenticationManager that will be called whenever access to a secured
            // resource is attempted
            OAuthTokenCredential credential = null;

            try
            {
                // Create generate token options if necessary
                if (info.GenerateTokenOptions == null)
                {
                    info.GenerateTokenOptions = new GenerateTokenOptions();
                }

                // AuthenticationManager will handle challenging the user for credentials
                credential = await AuthenticationManager.Current.GenerateCredentialAsync
                    (
                            info.ServiceUri,
                            info.GenerateTokenOptions
                    ) as OAuthTokenCredential;
            }
            catch (Exception ex)
            {
                // Exception will be reported in calling function
                throw (ex);
            }

            return credential;
        }

    }
}
