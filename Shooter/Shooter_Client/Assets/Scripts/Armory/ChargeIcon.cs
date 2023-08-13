using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargeIcon : MonoBehaviour
{
    public GameObject ResetView;
    public GameObject ActiveView;
    public Image Foreground;
    public TextMeshProUGUI Text;

    public void StartCharge()
    {
        ResetView.SetActive(true);
        ActiveView.SetActive(false);
    }

    public void StopCharge()
    {
        ResetView.SetActive(false);
        ActiveView.SetActive(true);
    }

    public void SetChargeValue(float currentCharge, float maxCharge)
    {
        Foreground.fillAmount = currentCharge / maxCharge;
        Text.text = Mathf.Ceil(maxCharge - currentCharge).ToString();
    }
}
