using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSkinController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _followCamera;
    [SerializeField] private List<GameObject> _playerSkin;
    [SerializeField] private int _playerSkinIndex;

    private void Awake()
    {
        _followCamera = GameObject.Find("CM Follow Cam").GetComponent<CinemachineVirtualCamera>();
        _playerSkinIndex = PlayerPrefs.GetInt("PlayerSkinIndex", 0);
    }

    private void Start()
    {
        _playerSkin[PlayerPrefs.GetInt("PlayerSkinIndex")].SetActive(true);
        _followCamera.Follow = _playerSkin[_playerSkinIndex].transform;
    }
}
