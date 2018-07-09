using UnityEngine;

public class TwitterShare : MonoBehaviour
{
    /* TWITTER VARIABLES*/

    //Twitter Share Link
    const string TwitterAddress = "http://twitter.com/intent/tweet";

    //Language
//    const string TweetLanguage = "en";

    /*END OF TWITTER VARIABLES*/

    // Twitter Share Button 
    public void ShareScoreOnTwitter(string stageNumber, string stageRanking)
    {
        Debug.Log("Tweet");
        Application.OpenURL(TwitterAddress + "?text=" + WWW.EscapeURL(GetShareContent(stageNumber, stageRanking)));
//                            "&amp;lang=" +
//                            WWW.EscapeURL(TweetLanguage));
    }

    string GetShareContent(string stageNumber, string stageRanking)
        => string.Format(DB.MessageDB["twitter_share_score"], stageNumber, stageRanking);
}