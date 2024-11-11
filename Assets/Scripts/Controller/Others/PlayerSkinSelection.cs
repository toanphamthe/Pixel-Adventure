using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSkinSelection : MonoBehaviour
{
    [SerializeField] private List<GameObject> _playerSkin;
    [SerializeField] private int _playerSkinIndex;
    [SerializeField] private TextMeshProUGUI _selectText;

    private void Awake()
    {
        _playerSkinIndex = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerSkinIndex, 0);
    }

    private void Update()
    {
        UpdateSkin();
        UpdateSelectSkinText();
    }

    private void UpdateSkin()
    {
        for (int i = 0; i < _playerSkin.Count; i++)
        {
            if (i == _playerSkinIndex)
            {
                _playerSkin[i].SetActive(true);
            }
            else
            {
                _playerSkin[i].SetActive(false);
            }
        }
    }

    private void UpdateSelectSkinText()
    {
        if (_playerSkinIndex == PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerSkinIndex))
        {
            _selectText.text = "Selected";
        }
        else
        {
            _selectText.text = "Select";
        }
    }

    public void SelectSkin()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.PlayerSkinIndex, _playerSkinIndex);
        _selectText.text = "Selected";
    }    

    public void ChangeSkin(bool left)
    {
        if (left)
        {
            if (_playerSkinIndex == 0)
            {
                _playerSkinIndex = _playerSkin.Count - 1;
            }
            else
            {
                _playerSkinIndex--;
            }
        }
        else
        {
            if (_playerSkinIndex == _playerSkin.Count - 1)
            {
                _playerSkinIndex = 0;
            }
            else
            {
                _playerSkinIndex++;
            }
        }
    }
}
