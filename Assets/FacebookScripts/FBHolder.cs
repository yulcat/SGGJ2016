using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FBHolder : MonoBehaviour {

	void Awake ()
	{
		FB.Init(OnInitComplete, OnHideUnity);
	}

	private void OnInitComplete()
	{
		if (FB.IsInitialized)
			FB.ActivateApp ();
		else
			Debug.Log ("FB Init Fail");
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			Time.timeScale = 0f;
		}
		else {
			Time.timeScale = 1f;
		}
	}

	public void ShareLink()
	{
		if (!FB.IsLoggedIn) {
			FBlogin ();
		}
		else {
			FB.FeedShare (
				toId: "",
				link: null,
				linkName: "EC",
				linkCaption: "I scored " + 1111.ToString () + " in EC.",
				linkDescription: "I scored " + 1111.ToString () + " in EC.",
				picture: new System.Uri ("https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcQydu2Si199sCVGdx6mXZNwgLcnUN6okX4RAEa-cSMViKkN3YtgYg"),
				mediaSource: "",
				callback: ShareCallback
			);
		}
	}

	public void FBloginforpublish()
	{
		FB.LogInWithPublishPermissions (new List<string> () { "public_actions" }, AuthCallback);
	}

	public void FBlogin()
	{
		FB.LogInWithReadPermissions (new List<string> () { "email", "public_profile", "user_friends" }, null);
	}

	private void AuthCallback(ILoginResult result)
	{
		if (FB.IsLoggedIn) {
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
		}
	}
		
	private void ShareCallback(IShareResult result)
	{
	}
}
