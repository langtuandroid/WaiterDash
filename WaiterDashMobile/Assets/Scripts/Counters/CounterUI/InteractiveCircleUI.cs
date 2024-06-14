using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveCircleUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image centerImage;
    // Start is called before the first frame update
    private void Start()
    {
        timerImage.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
        centerImage.gameObject.SetActive(false);

    }
    private void Update()
    {
        timerImage.fillAmount = Player.Instance.GetInteractiveTimerNormalized();
        if (Player.Instance.GetInteractiveTimerNormalized() <= 0f || Player.Instance.GetInteractiveTimerNormalized() >= 1f)
        {
            timerImage.gameObject.SetActive(false);
            backgroundImage.gameObject.SetActive(false);
            centerImage.gameObject.SetActive(false);
        }
        else
        {
            timerImage.gameObject.SetActive(true);
            backgroundImage.gameObject.SetActive(true);
            centerImage.gameObject.SetActive(true);
        }
    }
}
