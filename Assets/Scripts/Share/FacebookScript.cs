﻿using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FacebookScript : MonoBehaviour
{
//    public Text FriendsText;

    void Awake()
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
        Debug.Log("FeedShare");
        FB.FeedShare(
            toId: "",
            link: null,
            linkName: "Nabla",
            linkCaption: "Please Download in PlayStore",
            linkDescription: "desc",
            picture: new System.Uri ("https://www.facebook.com/nablagame/"),
            mediaSource: "",
            callback: shareCallBack
        );
    }

    void shareCallBack(){
    
    }

    #region Inviting

    public void FacebookGameRequest()
    {
        FB.AppRequest("Hey! Come and play this awesome game!", title: "Reso Coder Tutorial");
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