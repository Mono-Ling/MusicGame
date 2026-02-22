using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : BaseUI
{
    public Button butQuit;
    public Button butAgain;
    protected override void InitUI()
    {
        butQuit.onClick.AddListener(Quit);
        butAgain.onClick.AddListener(Again);
    }
    private void Quit()
    {
        UIManager.Instance.HideUI<EndGame>(() => { SceneManager.LoadScene("Begin"); });
    }
    private void Again()
    {
        UIManager.Instance.HideUI<EndGame>(() => { SceneManager.LoadScene("Game"); });
    }
}
