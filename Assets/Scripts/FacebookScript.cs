using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FacebookScript : MonoBehaviour
{
    public Text FriendsText;

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
                {
                    if (FB.IsInitialized)
                        FB.ActivateApp();
                    else
                        Debug.LogError("Couldn't initialize");
                },
                isGameShown =>
                {
                    if (!isGameShown)
                        Time.timeScale = 0;
                    else
                        Time.timeScale = 1;
                });
        }
        else
            FB.ActivateApp();
    }

    #region Login / Logout

    public void FacebookLogin()
    {
        var permissions = new List<string>() {"public_profile", "email", "user_friends"};
        FB.LogInWithReadPermissions(permissions);
    }

    public void FacebookLogout()
    {
        FB.LogOut();
    }

    #endregion

    public void FacebookShare()
    {
        FB.ShareLink(new System.Uri("https://www.facebook.com/nablagame/"), "Check it out!",
            "Good programming tutorials lol!",
            new System.Uri(
                "https://scontent-icn1-1.xx.fbcdn.net/v/t1.0-1/p480x480/23517997_1520327648017126_7208423714345216161_n.png?oh=df8c4863d0363d81014aca3eef09eb4e&oe=5A9D4344"));
    }

    #region Inviting

    public void FacebookGameRequest()
    {
        FB.AppRequest("Play Nabla!!", title: "Nabla!!");
    }

    public void FacebookInvite()
    {
        FB.Mobile.AppInvite(new System.Uri("https://play.google.com/store/apps/details?id=com.tappybyte.byteaway"));
    }

    #endregion

//    public void GetFriendsPlayingThisGame()
//    {
//        string query = "/me/friends";
//        FB.API(query, HttpMethod.GET, result =>
//            {
//                var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
//                var friendsList = (List<object>)dictionary["data"];
//                FriendsText.text = string.Empty;
//                foreach (var dict in friendsList)
//                    FriendsText.text += ((Dictionary<string, object>)dict)["name"];
//            });
//    }
}