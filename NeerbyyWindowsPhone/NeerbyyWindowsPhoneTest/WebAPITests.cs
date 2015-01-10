using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using NeerbyyWindowsPhone;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NeerbyyWindowsPhoneTest
{
    [TestClass]
    public class WebAPITests
    {
        /*
        public static string userLogin = "Callum";
        public static string userPassword = "toto";
        public static string userEmail = "callum.henshall@gmail.com";
        */
        public static string userLogin = "CallumOz";
        public static string userPassword = "password";
        public static string userEmail = "callum.henshall@me.com";

        public static string testLogin = "TestToto";
        public static string testPassword = "toto";
        public static string testEmail = "testtoto@testtoto.com";

        public static string updateLogin = "NewTestToto";
        public static string updateEmail = "newtesttoto@testtoto.com";

        // Tour Eiffel
        public static double userLatitude = 48.8581646494056;
        public static double userLongitude = 2.294425964355468;

        // EPITECH
        //public static double userLatitude = 48.81529956035847;
        //public static double userLongitude = 2.3629510402679443;

        public static string postContent = "Test Message";
        public static string postURL = "http://google.fr";

        public static string commentContent = "Test Comment";

        public async Task AuthenticateAsync()
        {
            await WebApi.Singleton.AuthenticateAsync((string responseMessage, UserResult result) =>
            {
            },
            (string responseMessage, Exception exception) =>
            {
            }, userEmail, userPassword);

            if (WebApi.Singleton.AuthenticatedUser == null)
            {
                await WebApi.Singleton.CreateUserAsync((string responseMessage, UserResult result) =>
                {
                },
                (string responseMessage, Exception exception) =>
                {
                }, userEmail, userLogin, userPassword);
            }
        }

        [TestMethod]
        public async Task AuthenticateTestAsync()
        {
            await this.AuthenticateAsync();

            await WebApi.Singleton.AuthenticateAsync((string responseMessage, UserResult result) =>
            {
                Assert.IsNotNull(result.user.auth_token, "Authentication Token is null");
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, userEmail, userPassword);

            await WebApi.Singleton.LogOutAsync((string responseMessage, Result result) =>
            {

            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            });

            await WebApi.Singleton.AuthenticateAsync((string responseMessage, UserResult result) =>
            {
                Assert.IsNotNull(result.user.auth_token, "Authentication Token is null");
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, userEmail, userPassword);

            await WebApi.Singleton.LogOutAsync((string responseMessage, Result result) =>
            {

            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            });
        }

        [TestMethod]
        public async Task CreateDeleteAndUpdateUserAsync()
        {
            await WebApi.Singleton.AuthenticateAsync((string responseMessage, UserResult result) =>
            {
            },
            (string responseMessage, Exception exception) =>
            {
            }, testEmail, testPassword);

            if (WebApi.Singleton.AuthenticatedUser == null)
            {

                await WebApi.Singleton.CreateUserAsync((string responseMessage, UserResult result) =>
                {
                    Assert.IsNotNull(result.user.auth_token, "Authentication Token is null");

                    Assert.AreEqual(testEmail, result.user.email, "The user email is wrong");
                    Assert.AreEqual(testLogin, result.user.username, "The username is wrong");
                },
                (string responseMessage, Exception exception) =>
                {
                    Assert.Fail(responseMessage);
                }, testEmail, testLogin, testPassword);


                await WebApi.Singleton.UpdateUserAsync((string responseMessage, UserResult result) =>
                {
                    Assert.IsNotNull(result.user.auth_token, "Authentication Token is null");

                    Assert.AreEqual(updateEmail, result.user.email, "The user email is wrong");
                    Assert.AreEqual(updateLogin, result.user.username, "The username is wrong");
                },
                (string responseMessage, Exception exception) =>
                {

                }, updateEmail, updateLogin);
            }

            await WebApi.Singleton.DeleteUserAsync((string responseMessage, Result result) =>
            {

            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            });
        }

        [TestMethod]
        public async Task ListPlacesAsync()
        {
            await WebApi.Singleton.PlacesAsync((string responseMessage, PlaceListResult result) =>
            {
                Assert.AreNotEqual(0, result.places.Count, 0, "The result is empty");
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, userLatitude, userLongitude);
        }

        [TestMethod]
        public async Task SearchPlacesAsync()
        {
            await this.AuthenticateAsync();

            await WebApi.Singleton.SearchPlacesAsync((string responseMessage, PlaceListResult result) =>
            {
                Assert.AreNotEqual(0, result.places.Count, 0, "The result is empty");
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, "Effiel", userLatitude, userLongitude);

            await WebApi.Singleton.LogOutAsync((string responseMessage, Result result) =>
            {

            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            });
        }

        [TestMethod]
        public async Task PlacesCategoryAsync()
        {
            List<Category> categories = null;

            await WebApi.Singleton.CategoriesAsync((string responseMessage, CategoryListResult result) =>
            {
                Assert.AreNotEqual(0, result.categories.Count, 0, "The result is empty");

                categories = result.categories;
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            });

            await WebApi.Singleton.PlacesAsync((string responseMessage, PlaceListResult result) =>
            {
                Assert.AreNotEqual(0, result.places.Count, 0, "The result is empty");
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, userLatitude, userLongitude, userLatitude, userLongitude, categories[0]);
        }

        [TestMethod]
        public async Task PostsForPlaceAsync()
        {
            Place place = null;

            await WebApi.Singleton.PlacesAsync((string responseMessage, PlaceListResult result) =>
            {
                Assert.AreNotEqual(0, result.places.Count, 0, "The result is empty");

                place = result.places[0];
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, userLatitude, userLongitude);


            await WebApi.Singleton.PostsForPlaceAsync((string responseMessage, PostListResult result) =>
            {
                Assert.AreNotEqual(0, result.publications.Count, 0, "The result is empty");
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, place);

        }

        [TestMethod]
        public async Task PostsForUserAsync()
        {
            await this.AuthenticateAsync();

            await WebApi.Singleton.PostsForPlaceAsync((string responseMessage, PostListResult result) =>
            {
                Assert.AreNotEqual(0, result.publications.Count, 0, "The result is empty");
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, null, WebApi.Singleton.AuthenticatedUser);

        }

        [TestMethod]
        public async Task CreateAndDeletePostAsync()
        {
            await this.AuthenticateAsync();

            Place place = null;

            await WebApi.Singleton.PlacesAsync((string responseMessage, PlaceListResult result) =>
            {
                Assert.AreNotEqual(0, result.places.Count, 0, "The result is empty");

                place = result.places[0];
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, userLatitude, userLongitude, userLatitude, userLongitude);

            Post post = null;

            await WebApi.Singleton.CreatePostWithUrlAsync((string responseMessage, PostResult result) =>
            {
                Assert.AreEqual(postContent, result.publication.content, "The content is wrong");
                Assert.AreEqual(postURL, result.publication.url, "The URL is wrong");

                post = result.publication;

            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, place, postContent, postURL, userLatitude, userLongitude);


            await WebApi.Singleton.DeletePostAsync((string responseMessage, Result result) =>
            {

            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, post);
        }

        [TestMethod]
        public async Task ListComments()
        {
            await this.AuthenticateAsync();

            Place place = new Place();

            place.id = "4b48a6f6f964a520ae5126e3"; // Epita - EPITECH

            Post post = null;

            await WebApi.Singleton.PostsForPlaceAsync((string responseMessage, PostListResult result) =>
            {
                Assert.AreNotEqual(0, result.publications.Count, 0, "The Post result is empty");

                post = result.publications[0];

            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, place);

            await WebApi.Singleton.CommentsForPostAsync((string responseMessage, CommentListResult result) =>
            {
                Assert.AreNotEqual(0, result.comments.Count, 0, "The Comment result is empty");
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, post);
        }

        [TestMethod]
        public async Task CreateAndDeleteComment()
        {
            await this.AuthenticateAsync();

            Place place = null;

            await WebApi.Singleton.PlacesAsync((string responseMessage, PlaceListResult result) =>
            {
                Assert.AreNotEqual(0, result.places.Count, 0, "The result is empty");

                place = result.places[0];
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, userLatitude, userLongitude, userLatitude, userLongitude);

            Post post = null;

            await WebApi.Singleton.PostsForPlaceAsync((string responseMessage, PostListResult result) =>
            {
                Assert.AreNotEqual(0, result.publications.Count, 0, "The result is empty");

                post = result.publications[0];

            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, place);

            Comment comment = null;

            await WebApi.Singleton.AddCommentToPostAsync((string responseMessage, CommentResult result) =>
            {
                Assert.AreEqual(commentContent, result.comment.content, "The content is wrong");

                comment = result.comment;
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, post, commentContent);

            await WebApi.Singleton.DeleteCommentAsync((string responseMessage, Result result) =>
            {

            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, comment);
        }

        [TestMethod]
        public async Task SearchUsersAsync()
        {
            await this.AuthenticateAsync();

            await WebApi.Singleton.SearchUsersAsync((string responseMessage, UserListResult result) =>
            {
                Assert.AreNotEqual(0, result.users.Count, 0, "The result is empty");
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, "Callum");

            await WebApi.Singleton.LogOutAsync((string responseMessage, Result result) =>
            {

            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            });
        }

        [TestMethod]
        public async Task SetSettings()
        {
            await this.AuthenticateAsync();

            Assert.IsNotNull(WebApi.Singleton.AuthenticatedUser.setting, "The Settings are missing");

            bool expectedAllowMessages = true;

            await WebApi.Singleton.SetSettingsAsync((string responseMessage, SettingsResult result) =>
            {
                Assert.AreEqual(expectedAllowMessages, result.settings.allow_messages, "The value for Allow Messages hasn't changed");
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, expectedAllowMessages);

            await WebApi.Singleton.SetSettingsAsync((string responseMessage, SettingsResult result) =>
            {
                Assert.AreNotEqual(expectedAllowMessages, result.settings.allow_messages, "The value for Allow Messages hasn't changed");
            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, !expectedAllowMessages);

            await WebApi.Singleton.LogOutAsync((string responseMessage, Result result) =>
            {

            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            });
        }

        public async Task SetNotificationsAsync()
        {
            await this.AuthenticateAsync();

            await WebApi.Singleton.SetNotificationTokenAsync((string responseMessage, Result result) =>
            {

            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            }, "0123456789abcdef");

            await WebApi.Singleton.LogOutAsync((string responseMessage, Result result) =>
            {

            },
            (string responseMessage, Exception exception) =>
            {
                Assert.Fail(responseMessage);
            });
        }
    }
}
