using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] characters;
    public GameObject cursor;
    public void SelectCharacter(int index)
    {
        GameState.SelectedCharacter = (CharacterType)index;
        cursor.transform.DOMove(characters[index].transform.position, 0.3f, true).SetEase(Ease.OutBack);
    }

}
