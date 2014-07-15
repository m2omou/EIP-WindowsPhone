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
        public static string userLogin = "CallumOz";
        public static string userPassword = "toto";
        public static string userEmail = "callum.henshall@me.com";

        public static string testLogin = "TestToto";
        public static string testPassword = "toto";
        public static string testEmail = "testtoto@testtoto.com";

        public static double userLatitude = 48.8581646494056;
        public static double userLongitude = 2.294425964355468;

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
        }

        [TestMethod]
        public async Task CreateAndDeleteUserAsync()
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
                },
                (string responseMessage, Exception exception) =>
                {
                    Assert.Fail(responseMessage);
                }, testEmail, testLogin, testPassword);

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
        }

        [TestMethod]
        public async Task SetSettings()
        {
            await this.AuthenticateAsync();

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
        }

        [TestMethod]
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
        }
    }
}
