using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] Slider healthSlider;
    [SerializeField] Image fillImage;
    [SerializeField] Image[] colors;
    [SerializeField] Text scoreText;

    int colorIndex;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        colorIndex = 0;
    }

    public void Init()
    {
        SetHealthSlider(1f);
        colorIndex = 0;
        foreach (Image color in colors)
            color.enabled = false;
        colors[colorIndex].enabled = true;
        SetScore(0);
    }

    public void SetHealthSlider(float value)
    {
        if (value >= 0.75f)
            fillImage.color = Color.green;
        else if (value > 0.3f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.red;

        healthSlider.value = value;
    }

    public void SetColor()
    {
        colors[colorIndex].enabled = false;
        colorIndex = (colorIndex + 1) % colors.Length;
        colors[colorIndex].enabled = true;
    }

    public void SetScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
}
