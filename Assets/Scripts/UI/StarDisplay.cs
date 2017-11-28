using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StarDisplay : MonoBehaviour
{
    public Text starCount;
    int prevSpentStars;
    int earnedStars;
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        earnedStars = SaveDataManager.clearRecord.Values.Sum(c => c.stars);
        prevSpentStars = SaveDataManager.data.spentStars;
        starCount.text = (earnedStars - prevSpentStars).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (prevSpentStars == SaveDataManager.data.spentStars) return;
        starCount.text = (earnedStars - SaveDataManager.data.spentStars).ToString();
        prevSpentStars = SaveDataManager.data.spentStars;
    }
}
