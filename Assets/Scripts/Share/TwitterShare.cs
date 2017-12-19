using UnityEngine;
using UnityEngine.UI;   
using System.Collections;

public class TwitterShare : MonoBehaviour
{
//    public Text stageNumber;
//    public Text stageRanking;
    /* TWITTER VARIABLES*/

    //Twitter Share Link
    string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";

    //Language
    string TWEET_LANGUAGE = "en";

    //This is the text which you want to show
    string textToDisplaybeg="[Playing Nabla] https://www.facebook.com/nablagame // I scored ";
    string textToDisplayend="% at stage";

    /*END OF TWITTER VARIABLES*/

    // Twitter Share Button 
    public void shareScoreOnTwitter (Text stageNumber, Text stageRanking) 
//    public void shareScoreOnTwitter () 
    {
        Debug.Log("Tweet");
        Application.OpenURL (TWITTER_ADDRESS + "?text=" + WWW.EscapeURL(textToDisplaybeg) + stageNumber + WWW.EscapeURL(textToDisplayend) + stageRanking + "&amp;lang=" + WWW.EscapeURL(TWEET_LANGUAGE));
    }
}
