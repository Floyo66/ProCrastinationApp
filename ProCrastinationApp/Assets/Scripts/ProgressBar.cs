using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    [SerializeField]
    private Text text_1;

    [SerializeField]
    private Text text_2;

    [SerializeField]
    private Image bar_fill;
    
    [SerializeField]
    private Image bar_outline;

    
    [SerializeField]
    private Image circle_1;

    [SerializeField]
    private Image circle_2;

    [SerializeField]
    private Color color;

     [SerializeField]
    private Color background_color;

    private int level = 0;

    private float currentAmount = 0;

    private Coroutine routine;


    void Start()
    {
        InitColor();
        level = 0;
        currentAmount = 0;
        bar_fill.fillAmount = currentAmount;
        UpdateLevel(level);
    }

    void InitColor()
    {
        circle_1.color = color;
        circle_2.color = color;

        bar_fill.color = color;
        bar_outline.color = color;

        text_1.color = background_color;
        text_2.color = color;
    }

    public void UpdateProgress(float amount, float duration = 0.1f)
    {
        if (routine != null)
            StopCoroutine(routine);

        float target = currentAmount + amount;
        routine = StartCoroutine(FillRoutine(target, duration));
    }

    private IEnumerator FillRoutine(float target, float duration)
    {
        float time = 0;
        float tempAmount = currentAmount;
        float diff = target - tempAmount;
        currentAmount = target;

        while (time < duration)
        {
            time += Time.deltaTime;
            float percent = time / duration;
            bar_fill.fillAmount = tempAmount + diff * percent;
            yield return null;
        }

        if (currentAmount >= 1)
        LevelUp();
    }

    private void LevelUp()
    {
        UpdateLevel(level + 1);
        UpdateProgress(-1f, 0.2f);
    }

    private void UpdateLevel(int level)
    {
        this.level = level;
        text_1.text = this.level.ToString();
        text_2.text = (this.level + 1).ToString();
    }

}
