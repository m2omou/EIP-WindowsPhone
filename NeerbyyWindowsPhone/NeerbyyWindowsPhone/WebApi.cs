﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Class to interface with the API
    /// </summary>
    public sealed class WebApi
    {
        private class Request
        {
            public HttpWebRequest request { get; set; }

            public String data { get; set; }

            public JResultDelegate jResultDelegate { get; set; }

            public ErrorDelegate errorDelegate { get; set; }
        }

        private static readonly String webServiceUrl = "http://api.neerbyy.com";

        private static readonly WebApi singleton = new WebApi();

        private static User user;

        /// <summary>
        /// The Authenticated User
        /// </summary>
        public static User User
        {
            get
            {
                return user;
            }
        }

        public static void LogOut()
        {
            WebApi.user = null;
        }

        /// <summary>
        /// Delegate type for handling Results
        /// </summary>
        public delegate void ResultDelegate(Object result);

        /// <summary>
        /// Delegate type for handling Jobject Results
        /// </summary>
        public delegate void JResultDelegate(JObject result);


        /// <summary>
        /// Delegate type for handling User Results
        /// </summary>
        public delegate void UserResultDelegate(User result);

        /// <summary>
        /// Delegate type for handling Places Results
        /// </summary>
        /// <param name="results"></param>
        public delegate void PlacesResultDelegate(List<Place> results);

        /// <summary>
        /// Delegate type for handling Errors
        /// </summary>
        public delegate void ErrorDelegate(WebException error);

        private WebApi()
        {
        }

        /// <summary>
        /// Allow access to the class
        /// </summary>
        public static WebApi Singleton
        {
            get
            {
                return singleton;
            }
        }

        /// <summary>
        /// Post Data to the Webservice
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data"></param>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        private void Post(String controller, String data, JResultDelegate resultDelegate, ErrorDelegate errorDelegate)
        {
            Uri uri = new Uri(string.Format("{0}/{1}", WebApi.webServiceUrl, controller));

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            if (WebApi.user != null)
                request.Headers[HttpRequestHeader.Authorization] = WebApi.User.auth_token;

            Request apiRequest = new Request();
            apiRequest.request = request;
            apiRequest.data = data;
            apiRequest.jResultDelegate = resultDelegate;
            apiRequest.errorDelegate = errorDelegate;

            request.BeginGetRequestStream(new AsyncCallback(BeginGetRequestStream), apiRequest);
        }

        /// <summary>
        /// Get a Webpage with some variables
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data"></param>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        private void Get(String controller, String data, JResultDelegate resultDelegate, ErrorDelegate errorDelegate)
        {
            Uri uri = new Uri(string.Format("{0}/{1}?{2}", WebApi.webServiceUrl, controller, data));

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = "GET";
            if (WebApi.user != null)
                request.Headers[HttpRequestHeader.Authorization] = WebApi.User.auth_token;

            Request apiRequest = new Request();
            apiRequest.request = request;
            apiRequest.jResultDelegate = resultDelegate;
            apiRequest.errorDelegate = errorDelegate;

            request.BeginGetResponse(new AsyncCallback(BeginGetResponse), apiRequest);
        }

        /// <summary>
        /// Asynchronous Request Stream so as not to block the main thread
        /// </summary>
        /// <param name="result"></param>
        private void BeginGetRequestStream(IAsyncResult result)
        {
            Request apiRequest = (Request)result.AsyncState;
            HttpWebRequest request = apiRequest.request;

            using (Stream postStream = request.EndGetRequestStream(result))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(apiRequest.data);
                postStream.Write(byteArray, 0, byteArray.Length);
                postStream.Close();
            }

            request.BeginGetResponse(new AsyncCallback(BeginGetResponse), apiRequest);
        }

        /// <summary>
        /// Asynchronous Response
        /// </summary>
        /// <param name="result"></param>
        private void BeginGetResponse(IAsyncResult result)
        {
            Request apiRequest = (Request)result.AsyncState;
            HttpWebRequest request = apiRequest.request;

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);

                this.HandleWebResponse(apiRequest, response);
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        apiRequest.errorDelegate(e);
                    });
                }
                else
                {
                    this.HandleWebResponse(apiRequest, (HttpWebResponse)e.Response);
                }
            }
            catch (Exception e)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    apiRequest.errorDelegate(new WebException(e.Message, WebExceptionStatus.ServerProtocolViolation));
                });
            }
        }


        /// <summary>
        /// This method handles the Web Response object that contains the server's response.
        /// </summary>
        /// <param name="apiRequest"></param>
        /// <param name="response"></param>
        private void HandleWebResponse(Request apiRequest, HttpWebResponse response)
        {
            String responseString;
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                responseString = streamReader.ReadToEnd();
            }
            Debug.WriteLine(responseString);

            JObject jobject = JObject.Parse(responseString);

            int responseCode = Convert.ToInt32((String)jobject["responseCode"]);
            if (responseCode == 0)
            {
                apiRequest.jResultDelegate(jobject);
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    apiRequest.errorDelegate(new WebException((String)jobject["responseMessage"], WebExceptionStatus.ServerProtocolViolation));
                });
            }
        }

        /// <summary>
        /// Send Authentication params to server
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        public void Authenticate(String email, String password, UserResultDelegate resultDelegate, ErrorDelegate errorDelegate)
        {
            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "connection[email]", HttpUtility.UrlEncode(email));
            postData.AppendFormat("&{0}={1}", "connection[password]", HttpUtility.UrlEncode(password));

            JResultDelegate jResultDelegate = delegate(JObject result)
            {
                JObject jUser = (JObject)result["result"]["user"];
                User user = jUser.ToObject<User>();
                WebApi.user = user;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    resultDelegate(user);
                });
            };

            this.Post("sessions.json", postData.ToString(), jResultDelegate, errorDelegate);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        public void CreateUser(String email, String username, String password, UserResultDelegate resultDelegate, ErrorDelegate errorDelegate)
        {
            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "user[email]", HttpUtility.UrlEncode(email));
            postData.AppendFormat("&{0}={1}", "user[username]", HttpUtility.UrlEncode(username));
            postData.AppendFormat("&{0}={1}", "user[password]", HttpUtility.UrlEncode(password));

            JResultDelegate jResultDelegate = delegate(JObject result)
            {
                JObject jUser = (JObject)result["result"]["user"];
                User user = jUser.ToObject<User>();
                WebApi.user = user;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    resultDelegate(user);
                });
            };

            this.Post("users.json", postData.ToString(), jResultDelegate, errorDelegate);
        }

        /// <summary>
        /// Get a list of places
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        public void Places(double latitude, double longitude, PlacesResultDelegate resultDelegate, ErrorDelegate errorDelegate)
        {
            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "latitude", HttpUtility.UrlEncode(latitude.ToString()));
            postData.AppendFormat("&{0}={1}", "longitude", HttpUtility.UrlEncode(longitude.ToString()));

            JResultDelegate jResultDelegate = delegate(JObject result)
            {
                JToken jToken = result["result"]["places"];
                List<Place> places = jToken.ToObject<List<Place>>();
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    resultDelegate(places);
                });
            };

            this.Get("places.json", postData.ToString(), jResultDelegate, errorDelegate);
        }
    }
}