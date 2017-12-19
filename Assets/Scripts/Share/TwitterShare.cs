using UnityEngine;

public class TwitterShare : MonoBehaviour
{
    /* TWITTER VARIABLES*/

    //Twitter Share Link
    const string TwitterAddress = "http://twitter.com/intent/tweet";

    //Language
    const string TweetLanguage = "en";

    //This is the text which you want to show
    const string TextToDisplaybeg = "[Playing Nabla] https://www.facebook.com/nablagame // I scored ";

    const string TextToDisplayend = "% at stage";

    /*END OF TWITTER VARIABLES*/

    // Twitter Share Button 
    public void ShareScoreOnTwitter(string stageNumber, string stageRanking)
    {
        Debug.Log("Tweet");
        Application.OpenURL(TwitterAddress + "?text=" + WWW.EscapeURL(TextToDisplaybeg) + stageNumber +
                            WWW.EscapeURL(TextToDisplayend) + stageRanking + "&amp;lang=" +
                            WWW.EscapeURL(TweetLanguage));
    }
}