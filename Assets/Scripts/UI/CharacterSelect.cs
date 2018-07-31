using UnityEngine;
using DG.Tweening;
using System;
using JetBrains.Annotations;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] characters;
    public GameObject cursor;
    Action unlockResult;

    void OnEnable()
    {
        for (var i = 1; i < characters.Length; i++)
        {
            var available = SaveDataManager.IsCharacterAvailable(i);
            var charImg = characters[i].transform.GetChild(0).GetComponent<Image>();
            var lockImg = characters[i].transform.GetChild(1).gameObject;
            charImg.color = available ? Color.white : Color.black;
            lockImg.SetActive(!available);
        }

        SelectCharacter((int) GameState.selectedCharacter);
    }

    void Update()
    {
        if (unlockResult == null) return;
        unlockResult();
        unlockResult = null;
    }

    [UsedImplicitly]
    public void SelectCharacter(int index)
    {
        if (!SaveDataManager.IsCharacterAvailable(index))
        {
            TryUnlockCharacter(index);
            return;
        }

        GameState.selectedCharacter = (CharacterType) index;
        var targetPosition = transform.parent.InverseTransformPoint(characters[index].transform.position);
        cursor.transform
            .DOLocalMove(targetPosition, 0.3f, true)
            .SetEase(Ease.OutBack);
    }

    void TryUnlockCharacter(int index)
    {
        WindowYNPop.OpenYNPop(
            string.Format(DB.MessageDB["charBuy_confirm"], DB.characterDB[index].name, DB.characterDB[index].price),
            () => UnlockSuccess(index));
    }

    void UnlockSuccess(int index)
    {
        var success = SaveDataManager.BuyCharacter(index);
        unlockResult = () => WindowPop.Open(success ? DB.MessageDB["charBuy_success"] : DB.MessageDB["charBuy_failed"]);
    }
}