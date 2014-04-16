using System;
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

            public Stream bodyStream { get; set; }

            public string data { get; set; }

            public SortedDictionary<string, string> args;

            public SortedDictionary<string, string> files;

            public JObjectResultDelegate jObjectResultDelegate { get; set; }

            public ErrorDelegate errorDelegate { get; set; }
        }

        private static readonly string webServiceUrl = "http://api.neerbyy.com";

        private static readonly WebApi singleton = new WebApi();

        private static readonly string boundary = "0xKhTmLbOuNdArY";

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

        /// <summary>
        /// Log the User out
        /// </summary>
        public static void LogOut()
        {
            WebApi.user = null;
        }

        /// <summary>
        /// Delegate type for handling Results
        /// </summary>
        private delegate void ResultDelegate(object result);

        /// <summary>
        /// Delegate type for handling Jobject Results
        /// </summary>
        private delegate void JResultDelegate(JObject result);

        /// <summary>
        /// Callback used inside Request
        /// </summary>
        /// <param name="request"></param>
        private delegate void RequestCallBack(Request request);

        /// <summary>
        /// Generic result delegate
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="result"></param>
        public delegate void ObjectResultDelegate(string responseMessage, Object result);

        /// <summary>
        /// Generic result delegate for JObject
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="result"></param>
        public delegate void JObjectResultDelegate(string responseMessage, JObject result);

        /// <summary>
        /// Delegate type for handling User Results
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="result"></param>
        public delegate void UserResultDelegate(string responseMessage, User result);
        
        /// <summary>
        /// Delegate type for handling Places Results
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="results"></param>
        public delegate void PlacesResultDelegate(string responseMessage, List<Place> results);
        
        /// <summary>
        /// Delegate type for handling a Post reuslt
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="result"></param>
        public delegate void PostResultDelegate(string responseMessage, Post result);

        /// <summary>
        /// Delegate type for handling Posts Results
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="results"></param>
        public delegate void PostsResultDelegate(string responseMessage, List<Post> results);

        /// <summary>
        /// Delegate type for handling Errors
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="error"></param>
        public delegate void ErrorDelegate(string responseMessage, WebException error);

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

        private void AddAuthenticationHeaders(HttpWebRequest request)
        {
            request.Headers[HttpRequestHeader.Authorization] = "Token token=\"" + WebApi.User.auth_token + "\"";
        }

        private Uri UriForController(String controller, String arguments)
        {
            if (arguments == null || arguments.Length == 0)
                return new Uri(string.Format("{0}/{1}", WebApi.webServiceUrl, controller));
            else
                return new Uri(string.Format("{0}/{1}?{2}", WebApi.webServiceUrl, controller, arguments));
        }

        /// <summary>
        /// Post Data to the Webservice
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data"></param>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        private void Post(String controller, String data, JObjectResultDelegate resultDelegate, ErrorDelegate errorDelegate)
        {
            Uri uri = this.UriForController(controller, null);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            if (WebApi.user != null)
                this.AddAuthenticationHeaders(request);

            Request apiRequest = new Request();
            apiRequest.request = request;
            apiRequest.data = data;
            apiRequest.jObjectResultDelegate = resultDelegate;
            apiRequest.errorDelegate = errorDelegate;


            //RequestCallBack callBack = new RequestCallBack(this.BeginCreateBodyStream);
            //callBack.BeginInvoke(apiRequest, null, null);

            this.BeginCreateBodyStream(apiRequest);
        }

        private void PostFile(String controller, SortedDictionary<string, string> args, SortedDictionary<string, string> files, JObjectResultDelegate resultDelegate, ErrorDelegate errorDelegate)
        {
            Uri uri = this.UriForController(controller, null);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = "POST";
            if (files.Count > 0)
                request.ContentType = String.Format("multipart/form-data; boundary={0}", WebApi.boundary);
            else
                request.ContentType = "application/x-www-form-urlencoded";
            if (WebApi.user != null)
                this.AddAuthenticationHeaders(request);

            Request apiRequest = new Request();
            apiRequest.request = request;
            apiRequest.args = args;
            apiRequest.files = files;
            apiRequest.jObjectResultDelegate = resultDelegate;
            apiRequest.errorDelegate = errorDelegate;

            RequestCallBack callBack = new RequestCallBack(this.BeginCreateBodyStream);
            callBack.BeginInvoke(apiRequest, null, null);
        }

        /// <summary>
        /// Get a Webpage with some variables
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data"></param>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        private void Get(String controller, String data, JObjectResultDelegate resultDelegate, ErrorDelegate errorDelegate)
        {
            Uri uri = this.UriForController(controller, data);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = "GET";
            if (WebApi.user != null)
                this.AddAuthenticationHeaders(request);

            Request apiRequest = new Request();
            apiRequest.request = request;
            apiRequest.jObjectResultDelegate = resultDelegate;
            apiRequest.errorDelegate = errorDelegate;

            request.BeginGetResponse(new AsyncCallback(BeginGetResponse), apiRequest);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiRequest"></param>
        private void BeginCreateBodyStream(Request apiRequest)
        {
            HttpWebRequest request = apiRequest.request;

            Stream bodyStream = new System.IO.MemoryStream();

            if (apiRequest.files != null && apiRequest.files.Count > 0)
            {
                byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");


                string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

                foreach (KeyValuePair<string, string> kvp in apiRequest.args)
                {
                    string formitem = string.Format(formdataTemplate, kvp.Key, kvp.Value);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    bodyStream.Write(formitembytes, 0, formitembytes.Length);
                }

                string headerTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";

                foreach (KeyValuePair<string, string> kvp in apiRequest.files)
                {

                    string header = string.Format(headerTemplate, kvp.Key, kvp.Value);

                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                    bodyStream.Write(headerbytes, 0, headerbytes.Length);

                    FileStream fileStream = new FileStream(kvp.Value, FileMode.Open, FileAccess.Read);
                    byte[] buffer = new byte[1024];

                    int bytesRead = 0;

                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        bodyStream.Write(buffer, 0, bytesRead);
                    }
                    fileStream.Close();
                }

                bodyStream.Write(boundarybytes, 0, boundarybytes.Length);
            }
            else if (apiRequest.args != null && apiRequest.args.Count > 0)
            {
                byte[] seperatorBytes = System.Text.Encoding.UTF8.GetBytes("&");

                int index = 0;
                foreach (KeyValuePair<string, string> kvp in apiRequest.args)
                {
                    if (index > 0)
                        bodyStream.Write(seperatorBytes, 0, seperatorBytes.Length);

                    string formitem = string.Format("{0}={1}", HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value));
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    bodyStream.Write(formitembytes, 0, formitembytes.Length);

                    index += 1;
                }
            }
            else
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(apiRequest.data);
                bodyStream.Write(byteArray, 0, byteArray.Length);
            }

            request.ContentLength = bodyStream.Length;
            apiRequest.bodyStream = bodyStream;

            request.BeginGetRequestStream(new AsyncCallback(BeginGetRequestStream), apiRequest);
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
                Stream bodyStream = apiRequest.bodyStream;

                bodyStream.Position = 0;
                byte[] tempBuffer = new byte[bodyStream.Length];
                bodyStream.Read(tempBuffer, 0, tempBuffer.Length);
                bodyStream.Close();
                postStream.Write(tempBuffer, 0, tempBuffer.Length);
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
                        apiRequest.errorDelegate(e.Message, e);
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
                    apiRequest.errorDelegate(e.Message, new WebException(e.Message, WebExceptionStatus.ServerProtocolViolation));
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
            Debug.WriteLine("Test");
            Debug.WriteLine(responseString);

            JObject jobject = JObject.Parse(responseString);

            int responseCode = Convert.ToInt32((String)jobject["responseCode"]);
            String responseMessage = (String)jobject["responseMessage"];
            if (responseCode == 0)
            {
                apiRequest.jObjectResultDelegate(responseMessage, jobject);
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    apiRequest.errorDelegate(responseMessage, new WebException(responseMessage, WebExceptionStatus.ServerProtocolViolation));
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

            JObjectResultDelegate jObjectResultDelegate = delegate(String responseMessage, JObject result)
            {
                JObject jUser = (JObject)result["result"]["user"];
                User user = jUser.ToObject<User>();
                WebApi.user = user;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    resultDelegate(responseMessage, user);
                });
            };

            this.Post("sessions.json", postData.ToString(), jObjectResultDelegate, errorDelegate);
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

            JObjectResultDelegate jObjectResultDelegate = delegate(String responseMessage, JObject result)
            {
                JObject jUser = (JObject)result["result"]["user"];
                User user = jUser.ToObject<User>();
                WebApi.user = user;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    resultDelegate(responseMessage, user);
                });
            };

            this.Post("users.json", postData.ToString(), jObjectResultDelegate, errorDelegate);
        }

        /// <summary>
        /// Send request to restore the User's password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        public void RestorePassword(String email, ObjectResultDelegate resultDelegate, ErrorDelegate errorDelegate)
        {
            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "email", HttpUtility.UrlEncode(email));

            JObjectResultDelegate jObjectResultDelegate = delegate(String responseMessage, JObject result)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    resultDelegate(responseMessage, null);
                });
            };

            this.Post("password_resets.json", postData.ToString(), jObjectResultDelegate, errorDelegate);
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

            JObjectResultDelegate jObjectResultDelegate = delegate(String responseMessage, JObject result)
            {
                JToken jToken = result["result"]["places"];
                List<Place> places = jToken.ToObject<List<Place>>();
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    resultDelegate(responseMessage, places);
                });
            };

            this.Get("places.json", postData.ToString(), jObjectResultDelegate, errorDelegate);
        }

        /// <summary>
        /// Get a list of publications for a Place
        /// </summary>
        /// <param name="place"></param>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        public void PublicationsForPlace(Place place, PostsResultDelegate resultDelegate, ErrorDelegate errorDelegate)
        {
            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "place_id", HttpUtility.UrlEncode(place.id.ToString()));

            JObjectResultDelegate jObjectResultDelegate = delegate(String responseMessage, JObject result)
            {
                try
                {
                    JToken jToken = result["result"]["publications"];
                    List<Post> posts = jToken.ToObject<List<Post>>();
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        resultDelegate(responseMessage, posts);
                    });
                }
                catch (Exception)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        string errorMessage = "Server Error";
                        errorDelegate(errorMessage, new WebException(errorMessage, WebExceptionStatus.ServerProtocolViolation));
                    });
                }
            };

            this.Get("publications.json", postData.ToString(), jObjectResultDelegate, errorDelegate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="place"></param>
        /// <param name="url"></param>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        public void PublishURL(Place place, String url, PostResultDelegate resultDelegate, ErrorDelegate errorDelegate)
        {
            //SortedDictionary<string, string> args = new SortedDictionary<string, string>();
            //args.Add("publication[user_id]", user.id.ToString());
            //args.Add("publication[place_id]", place.id.ToString());
            //args.Add("publication[link]", url);

            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "publication[user_id]", HttpUtility.UrlEncode(user.id.ToString()));
            postData.AppendFormat("&{0}={1}", "publication[place_id]", HttpUtility.UrlEncode(place.id.ToString()));
            postData.AppendFormat("&{0}={1}", "publication[link]", HttpUtility.UrlEncode(url));

            JObjectResultDelegate jObjectResultDelegate = delegate(String responseMessage, JObject result)
            {
                try
                {
                    JToken jToken = result["result"]["publication"];
                    Post post = jToken.ToObject<Post>();
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        resultDelegate(responseMessage, post);
                    });
                }
                catch (Exception)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        string errorMessage = "Server Error";
                        errorDelegate(errorMessage, new WebException(errorMessage, WebExceptionStatus.ServerProtocolViolation));
                    });
                }
            };

            this.Post("publications.json", postData.ToString(), jObjectResultDelegate, errorDelegate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="place"></param>
        /// <param name="filePath"></param>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        public void PublishFile(Place place, String filePath, PostResultDelegate resultDelegate, ErrorDelegate errorDelegate)
        {

        }
    }
}