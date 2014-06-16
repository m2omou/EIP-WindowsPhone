using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Diagnostics;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// A Generic Result
    /// </summary>
    public class Result
    {
    }

    /// <summary>
    /// A User Result
    /// </summary>
    public class UserResult
    {
        /// <summary>
        /// A User
        /// </summary>
        public User user { get; set; }
    }

    /// <summary>
    /// A List of Users Result
    /// </summary>
    public class UserListResult
    {
        /// <summary>
        /// A List of Users
        /// </summary>
        public List<User> users { get; set; }
    }

    /// <summary>
    /// A Place Result
    /// </summary>
    public class PlaceResult
    {
        /// <summary>
        /// A Place
        /// </summary>
        public Place place { get; set; }
    }

    /// <summary>
    /// A List of Places Result
    /// </summary>
    public class PlaceListResult
    {
        /// <summary>
        /// A List of Places
        /// </summary>
        public List<Place> places { get; set; }
    }

    /// <summary>
    /// A Post Result
    /// </summary>
    public class PostResult
    {
        /// <summary>
        /// A Post
        /// </summary>
        public Post publication { get; set; }
    }

    /// <summary>
    /// A List of Posts Result
    /// </summary>
    public class PostListResult
    {
        /// <summary>
        /// A List of Posts
        /// </summary>
        public List<Post> publications  { get; set; }
    }

    /// <summary>
    /// A Comment Result
    /// </summary>
    public class CommentResult
    {
        /// <summary>
        /// A Comment
        /// </summary>
        public Comment comment { get; set; }
    }

    /// <summary>
    /// A List of Comments Result
    /// </summary>
    public class CommentListResult
    {
        /// <summary>
        /// A List of Comments
        /// </summary>
        public List<Comment> comments { get; set; }
    }

    /// <summary>
    /// A Vote Result
    /// </summary>
    public class VoteResult
    {
        /// <summary>
        /// A Vote
        /// </summary>
        public Vote vote { get; set; }

        /// <summary>
        /// The Post linked to the Vote
        /// </summary>
        public Post publication { get; set; }
    }

    /// <summary>
    /// A List of Conversations
    /// </summary>
    public class ConversationListResult
    {
        /// <summary>
        /// The List of Conversations
        /// </summary>
        public List<Conversation> conversations { get; set; }
    }

    /// <summary>
    /// A Message Result
    /// </summary>
    public class MessageResult
    {
        /// <summary>
        /// A Message
        /// </summary>
        public Message message { get; set; }

        /// <summary>
        /// The Conversation that the Message is in
        /// </summary>
        public Conversation conversation { get; set; }
    }

    /// <summary>
    /// A List of Messages
    /// </summary>
    public class MessageListResult
    {
        /// <summary>
        /// The List of Messages
        /// </summary>
        public List<Message> messages { get; set; }
    }

    /// <summary>
    /// A Settings Result
    /// </summary>
    public class SettingsResult
    {
        /// <summary>
        /// Settings
        /// </summary>
        public Settings settings { get; set; }
    }

    /// <summary>
    /// The type for Reports
    /// </summary>
    public enum ReportType
    {
        /// <summary>
        /// A Report Type that doesn't match the others
        /// </summary>
        Custom,
        /// <summary>
        /// A Report for Copyright infringment
        /// </summary>
        Copyright,
        /// <summary>
        /// A Report for not respecting a person's right the their image
        /// </summary>
        ImageRights,
        /// <summary>
        /// A Report for Inappropriate Sexual Content
        /// </summary>
        SexualContent
    };

    /// <summary>
    /// Class to interface with the API
    /// </summary>
    public sealed class WebApi
    {
        private static readonly string webApiUrl = "http://api.neerbyy.com";

        private static readonly string usersPath = "users";
        private static readonly string sessionsPath = "sessions";
        private static readonly string passwordResetsPath = "password_resets";
        private static readonly string placesPath = "places";
        private static readonly string postsPath = "publications";
        private static readonly string commentsPath = "comments";
        private static readonly string votesPath = "votes";
        private static readonly string followedPlacesPath = "followed_places";
        private static readonly string reportPublicationsPath = "report_publications";
        private static readonly string reportCommentsPath = "report_comments";
        private static readonly string feedPath = "feed";
        private static readonly string conversationsPath = "conversations";
        private static readonly string messagesPath = "messages";
        private static readonly string searchUsersPath = "search/users";
        private static readonly string settingsPath = "settings";

        private static readonly string usersKey = "user";
        private static readonly string sessionsKey = "connection";
        private static readonly string passwordResetsKey = "";
        //private static readonly string placesKey = "place";
        private static readonly string postsKey = "publication";
        private static readonly string commentsKey = "comment";
        private static readonly string votesKey = "vote";
        private static readonly string followedPlacesKey = "followed_place";
        private static readonly string reportPublicationsKey = "report_publication";
        private static readonly string reportCommentsKey = "report_comment";
        //private static readonly string feedKey = "feed";
        //private static readonly string conversationsKey = "conversation";
        private static readonly string messagesKey = "message";
        //private static readonly string searchUsersKey = "search";
        private static readonly string settingsKey = "setting";

        private static readonly string webApiResultType = ".json";

        /// <summary>
        /// Generic Result delegate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseMessage"></param>
        /// <param name="result"></param>
        public delegate void ResultDelegate<T>(string responseMessage, T result);

        /// <summary>
        /// Delegate type for handling Errors
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="error"></param>
        public delegate void ErrorDelegate(string responseMessage, Exception error);

        private User authenticatedUser = null;

        /// <summary>
        /// The Authenticated User
        /// </summary>
        public User User
        {
            get
            {
                return authenticatedUser;
            }
            private set
            {
                authenticatedUser = value;

                if (authenticatedUser != null)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", "token=\"" + authenticatedUser.auth_token + "\"");
                }
                else
                {
                    client.DefaultRequestHeaders.Authorization = null;
                }
            }
        }

        private static readonly WebApi singleton = new WebApi();

        private HttpClient client;

        private WebApi()
        {
            client = new HttpClient(new HttpClientHandler { UseCookies = false });
            client.BaseAddress = new Uri(webApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
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

        private static readonly string userStateKey = "userStateKey";

        public static void SaveState(IDictionary<string, object> state)
        {
            if (WebApi.Singleton.User != null)
                state[userStateKey] = WebApi.Singleton.User;
        }

        public static void RestoreState(IDictionary<string, object> state)
        {
            User user = state[userStateKey] as User;
            if (user != null)
                WebApi.Singleton.User = user;
        }

        private string MakeUri(string path, IDictionary<string, string> args = null)
        {
            StringBuilder uriString = new StringBuilder(path + webApiResultType);
            Boolean first = true;

            if (args != null)
            {
                foreach (KeyValuePair<string, string> pair in args)
                {
                    if (first)
                    {
                        first = false;
                        uriString.Append("?");
                    }
                    else
                    {
                        uriString.Append("&");
                    }
                    uriString.Append(pair.Key + "=" + HttpUtility.UrlEncode(pair.Value));
                }
            }
            return uriString.ToString();
        }

        private IDictionary<string, string> AddKey(string mainKey, IDictionary<string, string> dict)
        {
            if (mainKey == null || mainKey == "")
                return dict;

            SortedDictionary<string, string> newDict = new SortedDictionary<string, string>();

            foreach (KeyValuePair<string, string> pair in dict)
            {
                newDict.Add(mainKey + "[" + pair.Key + "]", pair.Value);
            }
            return newDict;
        }

        private string AddKey(string mainKey, string key)
        {
            return mainKey == null || mainKey == "" ? key : mainKey + "[" + key + "]";
        }

        private IDictionary<string, string> AddListOptions(IDictionary<string, string> dict, int? since_id, int? max_id, int? count)
        {
            if (since_id != null)
                dict.Add("since_id", since_id.ToString());
            if (max_id != null)
                dict.Add("max_id", max_id.ToString());
            if (count != null)
                dict.Add("count", count.ToString());
            return dict;
        }

        private async Task<T> HandleResponseMessageAsync<T>(HttpResponseMessage httpResponseMessage, ResultDelegate<T> resultDelegate, ErrorDelegate errorDelegate)
        {
            try
            {
                httpResponseMessage.EnsureSuccessStatusCode();

                string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                Debug.WriteLine(responseString);

                JObject jobject = JObject.Parse(responseString);

                int responseCode = (int)jobject["responseCode"];
                string responseMessage = (string)jobject["responseMessage"];
                if (responseCode == 0)
                {
                    JToken jToken = jobject["result"];
                    T result = jToken.ToObject<T>();

                    resultDelegate(responseMessage, result);
                    return result;
                }
                else
                {
                    Exception e = new Exception(responseMessage);
                    errorDelegate(responseMessage, e);
                }
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
            return default(T);
        }

        /// <summary>
        /// Check if the user is Authenticated
        /// </summary>
        /// <returns></returns>
        public Boolean IsUserAuthenticated()
        {
            return User != null;
        }

        /// <summary>
        /// Send Authentication params to server
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task AuthenticateAsync(ResultDelegate<UserResult> resultDelegate, ErrorDelegate errorDelegate,
            string email, string password)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("email", email);
                args.Add("password", password);

                FormUrlEncodedContent content = new FormUrlEncodedContent(AddKey(sessionsKey, args));
            
                HttpResponseMessage responseMessage = await client.PostAsync(MakeUri(sessionsPath), content);

                UserResult result = await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
                User = result.user;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Log the User out
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <returns></returns>
        public async Task LogOutAsync(ResultDelegate<Result> resultDelegate, ErrorDelegate errorDelegate)
        {
            try
            {
                HttpResponseMessage responseMessage = await client.DeleteAsync(MakeUri(sessionsPath + "/" + User.auth_token));

                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
                User = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task CreateUserAsync(ResultDelegate<UserResult> resultDelegate, ErrorDelegate errorDelegate,
            string email, string username, string password)
        {
            try
            {
                if (this.User == null)
                    return;

                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("email", email);
                args.Add("username", username);
                args.Add("password", password);

                FormUrlEncodedContent content = new FormUrlEncodedContent(AddKey(usersKey, args));

                HttpResponseMessage responseMessage = await client.PostAsync(MakeUri(usersPath), content);

                UserResult result = await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
                User = result.user;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Update a user's account
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="imageStream"></param>
        /// <param name="imagename"></param>
        /// <returns></returns>
        public async Task UpdateUserAsync(ResultDelegate<UserResult> resultDelegate, ErrorDelegate errorDelegate,
            string email = null, string username = null, string password = null, string firstname = null, string lastname = null, Stream imageStream = null, string imagename = null)
        {
            try
            {
                HttpContent httpContent = null;

                if (imageStream != null && imagename != null)
                {
                    MultipartFormDataContent dataContent = new MultipartFormDataContent();

                    StreamContent streamContent = new StreamContent(imageStream);
                    dataContent.Add(streamContent, AddKey(postsKey, "avatar"), imagename);

                    if (email != null)
                        dataContent.Add(new StringContent(email), AddKey(postsKey, "email"));
                    if (username != null)
                        dataContent.Add(new StringContent(username), AddKey(postsKey, "username"));
                    if (password != null)
                        dataContent.Add(new StringContent(password), AddKey(postsKey, "password"));
                    if (firstname != null)
                        dataContent.Add(new StringContent(firstname), AddKey(postsKey, "firstname"));
                    if (lastname != null)
                        dataContent.Add(new StringContent(lastname), AddKey(postsKey, "lastname"));

                    httpContent = dataContent;
                }
                else
                {
                    FormUrlEncodedContent formContent = null;

                    SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                    if (email != null)
                        args.Add("email", email);
                    if (username != null)
                        args.Add("username", username);
                    if (password != null)
                        args.Add("password", password);
                    if (firstname != null)
                        args.Add("firstname", firstname);
                    if (lastname != null)
                        args.Add("lastname", lastname);

                    formContent = new FormUrlEncodedContent(AddKey(usersKey, args));

                    httpContent = formContent;
                }

                HttpResponseMessage responseMessage = await client.PutAsync(MakeUri(usersPath + "/" + User.id), httpContent);

                UserResult result = await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
                result.user.auth_token = User.auth_token;
                result.user.settings_id = User.settings_id;
                User = result.user;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Delete the User's account
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <returns></returns>
        public async Task DeleteUserAsync(ResultDelegate<Result> resultDelegate, ErrorDelegate errorDelegate)
        {
            try
            {
                HttpResponseMessage responseMessage = await client.DeleteAsync(MakeUri(usersPath + "/" + User.id));

                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Get the User's information from an ID
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public async Task UserAsync(ResultDelegate<UserResult> resultDelegate, ErrorDelegate errorDelegate,
            int user_id)
        {
            try
            {
                HttpResponseMessage responseMessage = await client.GetAsync(MakeUri(usersPath + "/" + user_id));

                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Send request to restore the User's password
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task RestorePasswordAsync(ResultDelegate<Result> resultDelegate, ErrorDelegate errorDelegate,
            string email)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("email", email);

                FormUrlEncodedContent content = new FormUrlEncodedContent(AddKey(passwordResetsKey, args));

                HttpResponseMessage responseMessage = await client.PostAsync(MakeUri(passwordResetsPath), content);

                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Get a list of places
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public async Task PlacesAsync(ResultDelegate<PlaceListResult> resultDelegate, ErrorDelegate errorDelegate,
            double latitude, double longitude)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("latitude", latitude.ToString());
                args.Add("longitude", longitude.ToString());

                HttpResponseMessage responseMessage = await client.GetAsync(MakeUri(placesPath, args));

                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Get a list of publications for a Place or a User
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="place"></param>
        /// <param name="userQuery"></param>
        /// <param name="since_id"></param>
        /// <param name="max_id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task PostsForPlaceAsync(ResultDelegate<PostListResult> resultDelegate, ErrorDelegate errorDelegate,
            Place place = null, User userQuery = null, int? since_id = null, int? max_id = null, int? count = null)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                if (place != null)
                    args.Add("place_id", place.id.ToString());
                if (userQuery != null)
                    args.Add("user_id", userQuery.id.ToString());
                AddListOptions(args, since_id, max_id, count);
            
                HttpResponseMessage responseMessage = await client.GetAsync(MakeUri(postsPath, args));

                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Create a Post with a URL
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="place"></param>
        /// <param name="content"></param>
        /// <param name="url"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public async Task CreatePostWithUrlAsync(ResultDelegate<PostResult> resultDelegate, ErrorDelegate errorDelegate,
            Place place, string content, string url, double latitude, double longitude)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("place_id", place.id.ToString());
                args.Add("content", content);
                args.Add("link", url);
                args.Add("latitude", latitude.ToString());
                args.Add("longitude", longitude.ToString());

                FormUrlEncodedContent formContent = new FormUrlEncodedContent(AddKey(postsKey, args));

                HttpResponseMessage responseMessage = await client.PostAsync(MakeUri(postsPath), formContent);
                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Create a Post with a file
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="place"></param>
        /// <param name="content"></param>
        /// <param name="fileStream"></param>
        /// <param name="filename"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public async Task CreatePostWithFileAsync(ResultDelegate<PostResult> resultDelegate, ErrorDelegate errorDelegate,
            Place place, string content, Stream fileStream, string filename, double latitude, double longitude)
        {
            try
            {
                MultipartFormDataContent dataContent = new MultipartFormDataContent();
                
                StreamContent streamContent = new StreamContent(fileStream);
                dataContent.Add(streamContent, AddKey(postsKey, "file"), filename);

                dataContent.Add(new StringContent(place.id), AddKey(postsKey, "place_id"));
                dataContent.Add(new StringContent(content), AddKey(postsKey, "content"));
                dataContent.Add(new StringContent(latitude.ToString()), AddKey(postsKey, "latitude"));
                dataContent.Add(new StringContent(longitude.ToString()), AddKey(postsKey, "longitude"));

                HttpResponseMessage responseMessage = await client.PostAsync(MakeUri(postsPath), dataContent);
                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Delete a Post
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task DeletePostAsync(ResultDelegate<Result> resultDelegate, ErrorDelegate errorDelegate,
            Post post)
        {
            try
            {
                HttpResponseMessage responseMessage = await client.DeleteAsync(MakeUri(postsPath + "/" + post.id));
                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Get the list of Comments for a Post
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="post"></param>
        /// <param name="since_id"></param>
        /// <param name="max_id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task CommentsForPostAsync(ResultDelegate<CommentListResult> resultDelegate, ErrorDelegate errorDelegate,
            Post post, int? since_id = null, int? max_id = null, int? count = null)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("publication_id", post.id.ToString());
                AddListOptions(args, since_id, max_id, count);

                HttpResponseMessage responseMessage = await client.GetAsync(MakeUri(commentsPath, args));

                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Add a Comment to a Post
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="post"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task AddCommentToPostAsync(ResultDelegate<CommentResult> resultDelegate, ErrorDelegate errorDelegate,
            Post post, string content)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("publication_id", post.id.ToString());
                args.Add("content", content);

                FormUrlEncodedContent formContent = new FormUrlEncodedContent(AddKey(commentsKey, args));

                HttpResponseMessage responseMessage = await client.PostAsync(MakeUri(commentsPath), formContent);
                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Delete a Comment
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task DeleteCommentAsync(ResultDelegate<Result> resultDelegate, ErrorDelegate errorDelegate,
            Comment comment)
        {
            try
            {
                HttpResponseMessage responseMessage = await client.DeleteAsync(MakeUri(commentsPath + "/" + comment.id));
                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Vote on a Post
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="post"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetVoteOnPostAsync(ResultDelegate<VoteResult> resultDelegate, ErrorDelegate errorDelegate,
            Post post, Boolean value)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("publication_id", post.id.ToString());
                args.Add("value", value ? "true" : "false");

                FormUrlEncodedContent formContent = new FormUrlEncodedContent(AddKey(votesKey, args));

                HttpResponseMessage responseMessage = await client.PostAsync(MakeUri(votesPath), formContent);
                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Cancel the Vote on a Post
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="vote"></param>
        /// <returns></returns>
        public async Task CancelVoteAsync(ResultDelegate<VoteResult> resultDelegate, ErrorDelegate errorDelegate,
            Vote vote)
        {
            try
            {
                HttpResponseMessage responseMessage = await client.DeleteAsync(MakeUri(votesPath + "/" + vote.id));
                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Report a Post
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="post"></param>
        /// <param name="reportType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task ReportPostAsync(ResultDelegate<Result> resultDelegate, ErrorDelegate errorDelegate,
            Post post, ReportType reportType, string content)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("post_id", post.id.ToString());
                args.Add("reason", reportType.ToString());
                args.Add("content", content);

                FormUrlEncodedContent formContent = new FormUrlEncodedContent(AddKey(reportPublicationsKey, args));

                HttpResponseMessage responseMessage = await client.PostAsync(MakeUri(reportPublicationsPath), formContent);
                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Report a Comment
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="comment"></param>
        /// <param name="reportType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task ReportCommentAsync(ResultDelegate<Result> resultDelegate, ErrorDelegate errorDelegate,
            Comment comment, ReportType reportType, string content)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("comment_id", comment.id.ToString());
                args.Add("reason", reportType.ToString());
                args.Add("content", content);

                FormUrlEncodedContent formContent = new FormUrlEncodedContent(AddKey(reportCommentsKey, args));

                HttpResponseMessage responseMessage = await client.PostAsync(MakeUri(reportCommentsPath), formContent);
                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Get the list of Followed Places for the current user or another user
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="user_id"></param>
        /// <param name="since_id"></param>
        /// <param name="max_id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task FollowedPlacesAsync(ResultDelegate<PlaceListResult> resultDelegate, ErrorDelegate errorDelegate,
            int? user_id = null, int? since_id = null, int? max_id = null, int? count = null)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("user_id", user_id.ToString());
                AddListOptions(args, since_id, max_id, count);

                HttpResponseMessage responseMessage = await client.GetAsync(MakeUri(followedPlacesPath, args));

                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Follow a Place
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        public async Task FollowPlaceAsync(ResultDelegate<PlaceResult> resultDelegate, ErrorDelegate errorDelegate,
            Place place)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("place_id", place.id);

                FormUrlEncodedContent formContent = new FormUrlEncodedContent(AddKey(followedPlacesKey, args));

                HttpResponseMessage responseMessage = await client.PostAsync(MakeUri(followedPlacesPath), formContent);
                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Stop following a Place
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        public async Task StopFollowingPlaceAsync(ResultDelegate<Result> resultDelegate, ErrorDelegate errorDelegate,
            Place place)
        {
            try
            {
                HttpResponseMessage responseMessage = await client.DeleteAsync(MakeUri(followedPlacesPath + "/" + place.followed_place_id));
                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Get the Feed
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="since_id"></param>
        /// <param name="max_id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task FeedAsync(ResultDelegate<PostListResult> resultDelegate, ErrorDelegate errorDelegate,
            int? since_id = null, int? max_id = null, int? count = null)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                AddListOptions(args, since_id, max_id, count);

                HttpResponseMessage responseMessage = await client.GetAsync(MakeUri(feedPath, args));

                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Get the list of Conversations for a User
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="since_id"></param>
        /// <param name="max_id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task ConversationsAsync(ResultDelegate<ConversationListResult> resultDelegate, ErrorDelegate errorDelegate,
            int? since_id = null, int? max_id = null, int? count = null)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                AddListOptions(args, since_id, max_id, count);

                HttpResponseMessage responseMessage = await client.GetAsync(MakeUri(conversationsPath, args));

                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Get the List of Messages for a Conversation
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="conversation"></param>
        /// <param name="since_id"></param>
        /// <param name="max_id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task MessagesAsync(ResultDelegate<MessageListResult> resultDelegate, ErrorDelegate errorDelegate,
            Conversation conversation, int? since_id = null, int? max_id = null, int? count = null)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("conversation_id", conversation.id.ToString());
                AddListOptions(args, since_id, max_id, count);

                HttpResponseMessage responseMessage = await client.GetAsync(MakeUri(conversationsPath, args));

                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Send a Message to a User
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="recipient"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(ResultDelegate<MessageResult> resultDelegate, ErrorDelegate errorDelegate,
            User recipient, string content)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("recipient_id", recipient.id.ToString());
                args.Add("content", content);

                FormUrlEncodedContent formContent = new FormUrlEncodedContent(AddKey(messagesKey, args));

                HttpResponseMessage responseMessage = await client.PostAsync(MakeUri(messagesPath), formContent);
                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Search for Users using a query string
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task SearchUsersAsync(ResultDelegate<UserListResult> resultDelegate, ErrorDelegate errorDelegate,
            string query)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                args.Add("query", query);

                HttpResponseMessage responseMessage = await client.GetAsync(MakeUri(searchUsersPath, args));

                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Get the User's Settings
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <returns></returns>
        public async Task SettingsAsync(ResultDelegate<SettingsResult> resultDelegate, ErrorDelegate errorDelegate)
        {
            try
            {
                HttpResponseMessage responseMessage = await client.GetAsync(MakeUri(settingsPath + "/" + User.settings_id));

                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }

        /// <summary>
        /// Change the Settings
        /// </summary>
        /// <param name="resultDelegate"></param>
        /// <param name="errorDelegate"></param>
        /// <param name="allowMessages"></param>
        /// <param name="sendNotificationForComments"></param>
        /// <param name="sendNotificationForMessages"></param>
        /// <returns></returns>
        public async Task SetSettingsAsync(ResultDelegate<SettingsResult> resultDelegate, ErrorDelegate errorDelegate,
            bool? allowMessages = null, bool? sendNotificationForComments = null, bool? sendNotificationForMessages = null)
        {
            try
            {
                SortedDictionary<string, string> args = new SortedDictionary<string, string>();
                if (allowMessages != null)
                    args.Add("allow_messages", allowMessages.Value ? "true" : "false");
                if (sendNotificationForComments != null)
                    args.Add("send_notification_for_comments", sendNotificationForComments.Value ? "true" : "false");
                if (sendNotificationForMessages != null)
                    args.Add("send_notification_for_messages", sendNotificationForMessages.Value ? "true" : "false");

                FormUrlEncodedContent formContent = new FormUrlEncodedContent(AddKey(settingsKey, args));

                HttpResponseMessage responseMessage = await client.PutAsync(MakeUri(settingsPath + "/" + User.settings_id), formContent);
                await HandleResponseMessageAsync(responseMessage, resultDelegate, errorDelegate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                string errorMessage = "Server Error";
                errorDelegate(errorMessage, e);
            }
        }
    }
}